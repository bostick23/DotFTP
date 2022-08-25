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
    public class ScpClient : BaseFtpClient, IFtpClient
    {
        private Renci.SshNet.ScpClient client;
        public ScpClient(string host, string username, string password, int? port = null)
        {
            Host = FtpHelper.CheckAndFixPath(host);
            Username = username;
            Password = password;
            Port = port;
            client = CreateClient();
        }
        private Renci.SshNet.ScpClient CreateClient() => new Renci.SshNet.ScpClient(CreateConnectionInfo());
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
            throw new NotImplementedException();
        }
        public List<string> GetFileList(string path)
        {
            throw new NotImplementedException();
        }

        public void DownloadFile(string path, string filename, string savepath)
        {
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            string filePathDest = Path.Combine(savepath, filename);
            if (File.Exists(filePathDest))
                File.Delete(filePathDest);

            using (Stream fileStream = File.Create(filePathDest))
                client.Download(serverPath, fileStream);
        }
        public void UploadFile(string path, string localpath, string filename, bool overwriteIfExists = true)
        {
            string localFile = Path.Combine(localpath, filename);
            if (!File.Exists(localFile))
                throw new ArgumentException($"File {localFile} inesistente");
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            using (FileStream fs = new FileStream(localFile, FileMode.Open, FileAccess.Read))
            {
                client.BufferSize = 4 * 1024;
                client.Upload(fs, serverPath);
            }
        }
        public void DeleteFile(string path, string filename)
        {
            throw new NotImplementedException();
        }

        public bool ExistFile(string path, string filename)
        {
            throw new NotImplementedException();
        }

        public bool RenameFile(string path, string oldFilename, string newFilename)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public bool MakeDirectory(string path, string directory)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path, string directory)
        {
            throw new NotImplementedException();
        }
        public DateTime GetDateTimeFileModification(string path, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
