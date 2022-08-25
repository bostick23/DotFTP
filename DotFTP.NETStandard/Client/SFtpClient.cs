using Renci.SshNet.Sftp;
using Renci.SshNet;
using System.Collections.Generic;
using DotFTP.Helpers;
using System.Linq;
using System.Collections;
using System.IO;

namespace DotFTP.Client
{
    public class SFtpClient : BaseFtpClient, IFtpClient
    {
        public SFtpClient(string host, string username, string password, int? port = null)
        {
            Host = FtpHelper.CheckAndFixPath(host);
            Username = username;
            Password = password;
            Port = port;
        }
        private ConnectionInfo CreateConnectionInfo()
        {
            string ftpHost = Host;
            /* if (!Host.StartsWith("sftp://"))
                 ftpHost = "sftp://" + Host;*/
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
            ConnectionInfo connectionInfo = CreateConnectionInfo();
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                var list = client.ListDirectory(path);
                if (list != null && list.Count() > 0)
                {
                    foreach (SftpFile file in list)
                    {
                        if (file.IsDirectory && !string.IsNullOrEmpty(file.Name) && file.Name != "." && file.Name != "..")
                            fileNameList.Add(file.Name);
                    }
                }
            }
            return fileNameList;
        }
        public List<string> GetFileList(string path)
        {
            path = "/" + FtpHelper.CheckAndFixPath(path);
            List<string> fileNameList = new List<string>();
            ConnectionInfo connectionInfo = CreateConnectionInfo();
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                var list = client.ListDirectory(path);
                if (list != null && list.Count() > 0)
                {
                    foreach (SftpFile file in list)
                    {
                        if (file.IsRegularFile)
                            fileNameList.Add(file.Name);
                    }
                }
            }
            return fileNameList;
        }

        public void DownloadFile(string path, string filename, string savepath)
        {
            string serverPath = "/" + FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            ConnectionInfo connectionInfo = CreateConnectionInfo();
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                string filePathDest = Path.Combine(savepath, filename);
                if (File.Exists(filePathDest))
                    File.Delete(filePathDest);

                using (Stream fileStream = File.Create(filePathDest))
                    client.DownloadFile(serverPath, fileStream);
            }
        }
    }
}
