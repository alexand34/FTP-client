using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Win7FTP.Library
{
    #region FTP client class

    public class FTPclient
    {
        #region Events
        //Download Progress Changed Event
        public delegate void DownloadProgressChangedHandler(object sender, DownloadProgressChangedArgs e);
        public event DownloadProgressChangedHandler OnDownloadProgressChanged;

        //Download Completed Event
        public delegate void DownloadCompletedHandler(object sender, DownloadCompletedArgs e);
        public event DownloadCompletedHandler OnDownloadCompleted;

        //New Server Message Event
        public delegate void NewMessageHandler(object sender, NewMessageEventArgs e);
        public event NewMessageHandler OnNewMessageReceived;

        //Upload Progress Changed Event
        //Download Progress Changed Event
        public delegate void UploadProgressChangedHandler(object sender, UploadProgressChangedArgs e);
        public event UploadProgressChangedHandler OnUploadProgressChanged;

        //Upload Completed Event
        public delegate void UploadCompletedHandler(object sender, UploadCompletedArgs e);
        public event UploadCompletedHandler OnUploadCompleted;
        #endregion

        #region CONSTRUCTORS

        public FTPclient()
        {
        }

        public FTPclient(string Hostname)
        {
            _hostname = Hostname;
        }

        public FTPclient(string Hostname, string Username, string Password)
        {
            _hostname = Hostname;
            _username = Username;
            _password = Password;
        }
        #endregion

        #region Directory functions

        public List<string> ListDirectory(string directory)
        {
            System.Net.FtpWebRequest ftp = GetRequest(GetDirectory(directory));
            ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
            NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "List Directory", "NLST");
            OnNewMessageReceived(this, e);

            string str = GetStringResponse(ftp);
            str = str.Replace("\r\n", "\r").TrimEnd('\r');
            List<string> result = new List<string>();
            result.AddRange(str.Split('\r'));
            return result;
        }


        public FTPdirectory ListDirectoryDetail(string directory)
        {
            System.Net.FtpWebRequest ftp = GetRequest(GetDirectory(directory));
            ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;
            NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "List Directory Details", "LIST");
            OnNewMessageReceived(this, e);
            string str = GetStringResponse(ftp);
            str = str.Replace("\r\n", "\r").TrimEnd('\r');
            return new FTPdirectory(str, _lastDirectory);
        }

        #endregion

        #region Upload
        
        
        #region Upload Variables
        System.Net.FtpWebRequest UploadFTPRequest = null;
        FileStream UploadFileStream = null;
        Stream UploadStream = null;
        bool UploadCanceled = false;
        FileInfo UploadFileInfo = null;
        #endregion

        public bool Upload(string localFilename, string targetFilename)
        {
            if (!File.Exists(localFilename))
            {
                throw (new ApplicationException("File " + localFilename + " not found"));
            }
            FileInfo fi = new FileInfo(localFilename);
            return Upload(fi, targetFilename);
        }

        public bool Upload(FileInfo fi, string targetFilename)
        {
            //1. check target
            string target;
            if (targetFilename.Trim() == "")
            {
                //Blank target: use source filename & current dir
                target = this.CurrentDirectory + fi.Name;
            }
            else if (targetFilename.Contains("/"))
            {
                //If contains / treat as a full path
                target = AdjustDir(targetFilename);
            }
            else
            {
                //otherwise treat as filename only, use current directory
                target = CurrentDirectory + targetFilename;
            }

            string URI = Hostname + target;
            //perform copy
            UploadFTPRequest = GetRequest(URI);

            //Set request to upload a file in binary
            UploadFTPRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            UploadFTPRequest.UseBinary = true;
            //Notify FTP of the expected size
            UploadFTPRequest.ContentLength = fi.Length;
            UploadFileInfo = fi;

            //create byte array to store: ensure at least 1 byte!
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1 + 1];
            int dataRead;
            
            //open file for reading
            using (UploadFileStream = fi.OpenRead())
            {
                try
                {
                    //open request to send
                    using (UploadStream = UploadFTPRequest.GetRequestStream())
                    {
                        //Give Message of Command
                        NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Upload File", "STOR");
                        OnNewMessageReceived(this, e);

                        //Get File Size
                        Int64 TotalBytesUploaded = 0;
                        Int64 FileSize = fi.Length;        
                        do
                        {
                            if (UploadCanceled)
                            {
                                NewMessageEventArgs CancelMessage = new NewMessageEventArgs("RESPONSE","Upload Canceled.", "CANCEL");
                                OnNewMessageReceived(this, CancelMessage);
                                UploadCanceled = false;
                                return false;
                            }

                            dataRead = UploadFileStream.Read(content, 0, BufferSize);
                            UploadStream.Write(content, 0, dataRead);
                            TotalBytesUploaded += dataRead;
                            //Declare Event
                            UploadProgressChangedArgs DownloadProgress = new UploadProgressChangedArgs(TotalBytesUploaded, FileSize);

                            //Progress changed, Raise the event.
                            OnUploadProgressChanged(this, DownloadProgress);

                            System.Windows.Forms.Application.DoEvents();
                        } while (!(dataRead < BufferSize));

                        //Get Message and Raise Event
                        NewMessageEventArgs UPloadResponse = new NewMessageEventArgs("RESPONSE", "File Uploaded!", "STOR");
                        OnNewMessageReceived(this, UPloadResponse);

                        //Declare Event
                        UploadCompletedArgs Args = new UploadCompletedArgs("Successful", true);
                        //Raise Event
                        OnUploadCompleted(this, Args);

                        UploadStream.Close();
                    }

                }
                catch (Exception ex)
                {
                    //Declare Event
                    UploadCompletedArgs Args = new UploadCompletedArgs("Error: " + ex.Message, false);
                    //Raise Event
                    OnUploadCompleted(this, Args);
                }
                finally
                {
                    //ensure file closed
                    UploadFileStream.Close();
                }

            }


            UploadFTPRequest = null;
            return true;

        }

        public void CancelUpload(string UploadFileName)
        {
            if (UploadFileStream != null)
            {
                UploadFileStream.Close();
                UploadFTPRequest.Abort();
                //UploadFileInfo.Delete();
                UploadCanceled = true;
                UploadFTPRequest = null;
                this.FtpDelete(UploadFileName);
                MessageBox.Show("Upload Canceled");
            }
        }
        #endregion

        #region Download
                
        #region Download Variables
        System.Net.FtpWebRequest DownloadFTPRequest = null;
        FtpWebResponse DownloadResponse = null;
        Stream DownloadResponseStream = null;
        FileStream DownloadFileStream = null;
        FileInfo TargetFileInfo = null;
        bool DownloadCanceled = false;
        #endregion


        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
        {
            FileInfo fi = new FileInfo(localFilename);
            return this.Download(sourceFilename, fi, PermitOverwrite);
        }


        public bool Download(FTPfileInfo file, string localFilename, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFilename, PermitOverwrite);
        }


        public bool Download(FTPfileInfo file, FileInfo localFI, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFI, PermitOverwrite);
        }

       

        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
        {

            if (targetFI.Exists && !(PermitOverwrite))
            {
                throw (new ApplicationException("Target file already exists"));
            }


            string target;
            if (sourceFilename.Trim() == "")
            {
                throw (new ApplicationException("File not specified"));
            }
            else if (sourceFilename.Contains("/"))
            {

                target = AdjustDir(sourceFilename);
            }
            else
            {

                target = CurrentDirectory + sourceFilename;
            }

            string URI = Hostname + target;


            DownloadFTPRequest = GetRequest(URI);


            DownloadFTPRequest.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            DownloadFTPRequest.UseBinary = true;
            TargetFileInfo = targetFI;

            using (DownloadResponse = (FtpWebResponse)DownloadFTPRequest.GetResponse())
            {
                using (DownloadResponseStream = DownloadResponse.GetResponseStream())
                {

                    using (DownloadFileStream = new FileStream(targetFI.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        try
                        {

                            NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Download File", "RETR");
                            OnNewMessageReceived(this, e);
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            Int64 TotalBytesRead = 0;
                            Int64 FileSize = this.GetFileSize(sourceFilename);
                            DownloadCanceled = false;
                            do
                            {
                                if (DownloadCanceled)
                                {
                                    NewMessageEventArgs CancelMessage = new NewMessageEventArgs("RESPONSE", "Download Canceled.", "CANCEL");

                                    DownloadCanceled = false;
                                    OnNewMessageReceived(this, CancelMessage);
                                    return false;
                                }

                                read = DownloadResponseStream.Read(buffer, 0, buffer.Length);
                                DownloadFileStream.Write(buffer, 0, read);
                                TotalBytesRead += read;

                                DownloadProgressChangedArgs DownloadProgress = new DownloadProgressChangedArgs(TotalBytesRead, FileSize);


                                OnDownloadProgressChanged(this, DownloadProgress);

                                System.Windows.Forms.Application.DoEvents();

                            } while (!(read == 0));


                            NewMessageEventArgs NewMessageArgs = new NewMessageEventArgs("RESPONSE", DownloadResponse.StatusDescription, DownloadResponse.StatusCode.ToString());
                            OnNewMessageReceived(this, NewMessageArgs);


                            DownloadCompletedArgs Args = new DownloadCompletedArgs("Successful", true);

                            OnDownloadCompleted(this, Args);

                            DownloadResponseStream.Close();
                            DownloadFileStream.Flush();
                            DownloadFileStream.Close();
                            DownloadFileStream = null;
                            DownloadResponseStream = null;
                        }
                        catch (Exception ex)
                        {

                            DownloadFileStream.Close();

                            targetFI.Delete();

                            DownloadCompletedArgs DownloadCompleted = new DownloadCompletedArgs("Error: " + ex.Message, false);

                            OnDownloadCompleted(this, DownloadCompleted);
                        }
                    }
                    if (DownloadFileStream != null)
                        DownloadResponseStream.Close();
                }
                if (DownloadFileStream != null)
                    DownloadResponse.Close();
            }
            return true;
        }

        public void CancelDownload()
        {
            if (DownloadFileStream != null)
            {
                DownloadFileStream.Close();
                
                DownloadFTPRequest.Abort();
                
                DownloadResponse.Close();
                DownloadResponseStream.Close();

                TargetFileInfo.Delete();
                DownloadCanceled = true;
                MessageBox.Show("Download Canceled");
            }
        }
        #endregion

        #region Other functions

        public bool FtpDelete(string filename)
        {

            string URI = this.Hostname + GetFullPath(filename);

            System.Net.FtpWebRequest ftp = GetRequest(URI);

            ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
            try
            {

                string str = GetStringResponse(ftp);

                NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Delete File", "DELE");
                OnNewMessageReceived(this, e);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public bool FtpFileExists(string filename)
        {
            //Try to obtain filesize: if we get error msg containing "550"
            try
            {
                long size = GetFileSize(filename);
                return true;

            }
            catch (Exception ex)
            {

                if (ex is System.Net.WebException)
                {

                    if (ex.Message.Contains("550"))
                    {
                        return false;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        return false;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }


        public long GetFileSize(string filename)
        {
            string path;
            if (filename.Contains("/"))
            {
                path = AdjustDir(filename);
            }
            else
            {
                path = this.CurrentDirectory + filename;
            }
            string URI = this.Hostname + path;
            System.Net.FtpWebRequest ftp = GetRequest(URI);

            ftp.Method = System.Net.WebRequestMethods.Ftp.GetFileSize;
            string tmp = this.GetStringResponse(ftp);

            NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Get File Size", "SIZE");
            OnNewMessageReceived(this, e);
            return GetSize(ftp);
        }

        public bool FtpRename(string sourceFilename, string newName)
        {

            string source = GetFullPath(sourceFilename);
            if (!FtpFileExists(source))
            {
                throw (new FileNotFoundException("File " + source + " not found"));
            }


            string target = GetFullPath(newName);
            if (target == source)
            {
                throw (new ApplicationException("Source and target are the same"));
            }
            else if (FtpFileExists(target))
            {
                throw (new ApplicationException("Target file " + target + " already exists"));
            }


            string URI = this.Hostname + source;

            System.Net.FtpWebRequest ftp = GetRequest(URI);

            ftp.Method = System.Net.WebRequestMethods.Ftp.Rename;
            ftp.RenameTo = target;
            try
            {

                string str = GetStringResponse(ftp);
                NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "File Rename", "RENAME");
                OnNewMessageReceived(this, e);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpCreateDirectory(string dirpath)
        {

            string URI = this.Hostname + AdjustDir(dirpath);
            System.Net.FtpWebRequest ftp = GetRequest(URI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                string str = GetStringResponse(ftp);
                NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Make Directory", "MKD");
                OnNewMessageReceived(this, e);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpDeleteDirectory(string dirpath)
        {
            string URI = this.Hostname + AdjustDir(dirpath);
            System.Net.FtpWebRequest ftp = GetRequest(URI);

            ftp.Method = System.Net.WebRequestMethods.Ftp.RemoveDirectory;
            try
            {

                string str = GetStringResponse(ftp);
                NewMessageEventArgs e = new NewMessageEventArgs("COMMAND", "Remove Directory", "RMD");
                OnNewMessageReceived(this, e);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region info
        private FtpWebRequest GetRequest(string URI)
        {

            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);

            result.Credentials = GetCredentials();

            result.KeepAlive = false;
            return result;
        }

        private System.Net.ICredentials GetCredentials()
        {
            return new System.Net.NetworkCredential(Username, Password);
        }

        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return AdjustDir(file);
            }
            else
            {
                return this.CurrentDirectory + file;
            }
        }


        private string AdjustDir(string path)
        {
            return ((path.StartsWith("/")) ? "" : "/").ToString() + path;
        }

        private string GetDirectory(string directory)
        {
            string URI;
            if (directory == "")
            {
                URI = Hostname + this.CurrentDirectory;
                _lastDirectory = this.CurrentDirectory;
            }
            else
            {
                if (!directory.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                URI = this.Hostname + directory;
                _lastDirectory = directory;
            }
            return URI;
        }


        private string _lastDirectory = "";

        private string GetStringResponse(FtpWebRequest ftp)
        {
            string result = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        _WelcomeMessage = response.WelcomeMessage;
                        _ExitMessage = response.ExitMessage;
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    try
                    {
                        NewMessageEventArgs e = new NewMessageEventArgs("RESPONSE", response.StatusDescription, response.StatusCode.ToString());
                        OnNewMessageReceived(this, e);
                    }
                    catch
                    {
                        
                    }
                    
                    datastream.Close();
                }
                response.Close();
            }
            return result;
        }


        private long GetSize(FtpWebRequest ftp)
        {
            long size;
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                size = response.ContentLength;
                response.Close();
            }

            return size;
        }
        #endregion

        #region Properties
        private string _hostname;

        public string Hostname
        {
            get
            {
                if (_hostname.StartsWith("ftp://"))
                {
                    return _hostname;
                }
                else
                {
                    return "ftp://" + _hostname;
                }
            }
            set
            {
                _hostname = value;
            }
        }
        private string _username;
        public string Username
        {
            get
            {
                return (_username == "" ? "anonymous" : _username);
            }
            set
            {
                _username = value;
            }
        }


        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }


        private string _currentDirectory = "/";
        public string CurrentDirectory
        {
            get
            {

                return _currentDirectory + ((_currentDirectory.EndsWith("/")) ? "" : "/").ToString();
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                _currentDirectory = value;
            }
        }


        #endregion

        #region Server Messages
        string _WelcomeMessage, _ExitMessage;
        public string WelcomeMessage
        {
            get
            {
                return _WelcomeMessage;
            }
            set
            {
                _WelcomeMessage = value;
            }
        }
        public string ExitMessage
        {
            get
            {
                return _ExitMessage;
            }
            set
            {
                _ExitMessage = value;
            }
        }
        #endregion

    }
    #endregion

    #region "FTP file info class"

    public class FTPfileInfo
    {

        #region "Properties"
        public string FullName
        {
            get
            {
                return Path + Filename;
            }
        }
        public string Filename
        {
            get
            {
                return _filename;
            }
        }
        public string Path
        {
            get
            {
                return _path;
            }
        }
        public DirectoryEntryTypes FileType
        {
            get
            {
                return _fileType;
            }
        }
        public long Size
        {
            get
            {
                return _size;
            }
        }
        public DateTime FileDateTime
        {
            get
            {
                return _fileDateTime;
            }
        }
        public string Permission
        {
            get
            {
                return _permission;
            }
        }
        public string Extension
        {
            get
            {
                int i = this.Filename.LastIndexOf(".");
                if (i >= 0 && i < (this.Filename.Length - 1))
                {
                    return this.Filename.Substring(i + 1);
                }
                else
                {
                    return "";
                }
            }
        }
        public string NameOnly
        {
            get
            {
                int i = this.Filename.LastIndexOf(".");
                if (i > 0)
                {
                    return this.Filename.Substring(0, i);
                }
                else
                {
                    return this.Filename;
                }
            }
        }


        private string _filename;
        private string _path;
        private DirectoryEntryTypes _fileType;
        private long _size;
        private DateTime _fileDateTime;
        private string _permission;

        #endregion


        public enum DirectoryEntryTypes {File, Directory}


        public FTPfileInfo(string line, string path)
        {
            Match m = GetMatchingRegex(line);
            if (m == null)
            {
                throw (new ApplicationException("Unable to parse line: " + line));
            }
            else
            {
                _filename = m.Groups["name"].Value;
                _path = path;

                Int64.TryParse(m.Groups["size"].Value, out _size);

                _permission = m.Groups["permission"].Value;
                string _dir = m.Groups["dir"].Value;
                if (_dir != "" && _dir != "-")
                {
                    _fileType = DirectoryEntryTypes.Directory;
                }
                else
                {
                    _fileType = DirectoryEntryTypes.File;
                }

                try
                {
                    _fileDateTime = DateTime.Parse(m.Groups["timestamp"].Value);
                }
                catch (Exception)
                {
                    _fileDateTime = Convert.ToDateTime(null);
                }

            }
        }

        private Match GetMatchingRegex(string line)
        {
            Regex rx;
            Match m;
            for (int i = 0; i <= _ParseFormats.Length - 1; i++)
            {
                rx = new Regex(_ParseFormats[i]);
                m = rx.Match(line);
                if (m.Success)
                {
                    return m;
                }
            }
            return null;
        }

        #region "Regular expressions for parsing LIST results"

        private static string[] _ParseFormats = new string[] { 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)", 
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)", 
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" };
        #endregion
    }
    #endregion

    #region "FTP Directory class"

    public class FTPdirectory : List<FTPfileInfo>
    {


        public FTPdirectory()
        {
        }

        public FTPdirectory(string dir, string path)
        {
            foreach (string line in dir.Replace("\n", "").Split(System.Convert.ToChar('\r')))
            {
                if (line != "")
                {
                    this.Add(new FTPfileInfo(line, path));
                }
            }
        }


        public FTPdirectory GetFiles(string ext)
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.File, ext);
        }


        public FTPdirectory GetDirectories()
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.Directory, "");
        }


        private FTPdirectory GetFileOrDir(FTPfileInfo.DirectoryEntryTypes type, string ext)
        {
            FTPdirectory result = new FTPdirectory();
            foreach (FTPfileInfo fi in this)
            {
                if (fi.FileType == type)
                {
                    if (ext == "")
                    {
                        result.Add(fi);
                    }
                    else if (ext == fi.Extension)
                    {
                        result.Add(fi);
                    }
                }
            }
            return result;

        }

        public bool FileExists(string filename)
        {
            foreach (FTPfileInfo ftpfile in this)
            {
                if (ftpfile.Filename == filename)
                {
                    return true;
                }
            }
            return false;
        }

        private const char slash = '/';

        public static string GetParentDirectory(string dir)
        {
            string tmp = dir.TrimEnd(slash);
            int i = tmp.LastIndexOf(slash);
            if (i > 0)
            {
                return tmp.Substring(0, i - 1);
            }
            else
            {
                throw (new ApplicationException("No parent for root"));
            }
        }
    }
    #endregion

    #region Events
    public class DownloadProgressChangedArgs : EventArgs
    {

        private Int64 _BytesDownload;
        private Int64 _TotalBytes;

        public DownloadProgressChangedArgs(Int64 BytesDownload, Int64 TotleBytes)
        {
            this._BytesDownload = BytesDownload;
            this._TotalBytes = TotleBytes;
        }


        public Int64 BytesDownloaded { get { return _BytesDownload; } }
        public Int64 TotleBytes { get { return _TotalBytes;} }
    }

    public class DownloadCompletedArgs : EventArgs
    {

        private bool _DownloadedCompleted;
        private string _DownloadStatus;


        public DownloadCompletedArgs(string Status, bool Completed)
        {
            this._DownloadedCompleted = Completed;
            this._DownloadStatus = Status;
        }


        public String DownloadStatus { get { return _DownloadStatus; } }
        public bool DownloadCompleted { get { return _DownloadedCompleted; } }
    }

    public class NewMessageEventArgs : EventArgs
    {

        private string _Message;
        private string _StatusCode;
        private string _Type;


        public NewMessageEventArgs(string Type, string Status, string Code)
        {
            this._Message = Status;
            this._StatusCode = Code;
            this._Type = Type;
        }


        public string StatusMessage { get { return _Message; } }
        public string StatusCode { get { return _StatusCode ; } }
        public string StatusType { get { return _Type; } }
    }

    public class UploadProgressChangedArgs : EventArgs
    {

        private Int64 _BytesUpload;
        private Int64 _TotalBytes;


        public UploadProgressChangedArgs(Int64 BytesUpload, Int64 TotleBytes)
        {
            this._BytesUpload = BytesUpload;
            this._TotalBytes = TotleBytes;
        }


        public Int64 BytesUploaded { get { return _BytesUpload; } }
        public Int64 TotleBytes { get { return _TotalBytes; } }
    }

    public class UploadCompletedArgs : EventArgs
    {

        private bool _UploadCompleted;
        private string _UploadStatus;

        public UploadCompletedArgs(string Status, bool Completed)
        {
            this._UploadCompleted = Completed;
            this._UploadStatus = Status;
        }


        public String UploadStatus { get { return _UploadStatus; } }
        public bool UploadCompleted { get { return _UploadCompleted; } }
    }
    #endregion

}

