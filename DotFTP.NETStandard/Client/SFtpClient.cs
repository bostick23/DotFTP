using Renci.SshNet.Sftp;
using Renci.SshNet;
using System.Collections.Generic;
using DotFTP.Helpers;
using System.Linq;
using System.IO;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace DotFTP.Client
{
    public class SFtpClient : BaseFtpClient, IFtpClient
    {
        private SftpClient client;
        public SFtpClient(string host, string username, string password, int? port = null)
        {
            Host = FtpHelper.CheckAndFixPath(host);
            Username = username;
            Password = password;
            Port = port;
            client = CreateClient();
        }
        private SftpClient CreateClient() => new SftpClient(CreateConnectionInfo());
        public void Connect()
        {
            if (client == null)
            {
                client = CreateClient();
                client.Connect();
            }
            else if (!client.IsConnected)
                client.Connect();
        }
        private ConnectionInfo CreateConnectionInfo()
        {
            if (Host.StartsWith("sftp://"))
                Host = Host.Substring(7);
            string ftpHost = Host;
            ConnectionInfo connectionInfo;
            if (Port != null)
            {
                connectionInfo = new ConnectionInfo(ftpHost, Port.Value,
                                        Username,
                                        new PasswordAuthenticationMethod(Username, Password),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
            }
            else
            {
                connectionInfo = new ConnectionInfo(ftpHost,
                                        Username,
                                        new PasswordAuthenticationMethod(Username, Password),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
            }
            return connectionInfo;
        }
        public List<string> GetDirectoryList(string path)
        {
            path = "/" + FtpHelper.CheckAndFixPath(path);
            List<string> fileNameList = new List<string>();
            var list = client.ListDirectory(path);
            if (list != null && list.Count() > 0)
            {
                foreach (SftpFile file in list)
                {
                    if (file.IsDirectory && !string.IsNullOrEmpty(file.Name) && file.Name != "." && file.Name != "..")
                        fileNameList.Add(file.Name);
                }
            }
            return fileNameList;
        }
        public List<string> GetFileList(string path)
        {
            path = "/" + FtpHelper.CheckAndFixPath(path);
            List<string> fileNameList = new List<string>();
            var list = client.ListDirectory(path);
            if (list != null && list.Count() > 0)
            {
                foreach (SftpFile file in list)
                {
                    if (file.IsRegularFile)
                        fileNameList.Add(file.Name);
                }
            }
            return fileNameList;
        }

        public void DownloadFile(string path, string filename, string savepath)
        {
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            string filePathDest = Path.Combine(savepath, filename);
            if (File.Exists(filePathDest))
                File.Delete(filePathDest);

            using (Stream fileStream = File.Create(filePathDest))
                client.DownloadFile(serverPath, fileStream);
        }
        public void UploadFile(string path, string localpath, string filename, bool overwriteIfExists = true)
        {
            string localFile = Path.Combine(localpath, filename);
            if (!File.Exists(localFile))
                throw new ArgumentException($"File {localFile} inesistente");
            path = "/" + FtpHelper.CheckAndFixPath(path);
            client.ChangeDirectory(path);
            using (FileStream fs = new FileStream(localFile, FileMode.Open, FileAccess.Read))
            {
                client.BufferSize = 4 * 1024;
                client.UploadFile(fs, filename);
            }
        }
        public void DeleteFile(string path, string filename)
        {
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            client.DeleteFile(serverPath);
        }

        public bool ExistFile(string path, string filename)
        {
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            return client.Exists(serverPath);
        }

        public bool RenameFile(string path, string oldFilename, string newFilename)
        {
            path = "/" + FtpHelper.CheckAndFixPath(path);
            client.ChangeDirectory(path);
            string oldPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(oldFilename);
            string newPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(newFilename);
            client.RenameFile(oldPath, newPath);
            return true;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public bool MakeDirectory(string path, string directory)
        {
            string ftpPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(directory);
            client.CreateDirectory(ftpPath);
            return true;
        }

        public bool DirectoryExists(string path, string directory)
        {
            string ftpPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(directory);
            client.Exists(ftpPath);
            return true;
        }
        public DateTime GetDateTimeFileModification(string path, string filename)
        {
            string ftpPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            SftpFileAttributes attributes = client.GetAttributes(ftpPath);
            return attributes.LastWriteTime;
        }
    }
}
