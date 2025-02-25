using DotFTP.Helpers;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace DotFTP.Client
{
    public class FtpClient : BaseFtpClient, IFtpClient
    {
        private FluentFTP.FtpClient client;
        public FtpClient(string host, string username, string password, int? port = null, FtpEncryptionMode encryptionMode = FtpEncryptionMode.Auto)
        {
            Host = host;
            Username = username;
            Password = password;
            Port = port;
            client = CreateClient(encryptionMode);
        }
        private FluentFTP.FtpClient CreateClient(FtpEncryptionMode encryptionMode = FtpEncryptionMode.Auto)
        {
            if (Port != null)
                client = new FluentFTP.FtpClient(Host, Username, Password, Port.Value);
            else
                client = new FluentFTP.FtpClient(Host, Username, Password);
            client.Config.EncryptionMode = encryptionMode;
            client.Config.ValidateAnyCertificate = true;
            return client;
        }
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
        public List<string> GetDirectoryList(string path)
        {
            List<string> result = new List<string>();
            FtpListItem[] list = client.GetListing(FtpHelper.CheckAndFixPath(path));
            if (list != null && list.Length > 0)
            {
                foreach (FtpListItem item in list)
                {
                    if (item.Type == FtpObjectType.Directory)
                        result.Add(item.Name);
                }
            }
            return result;
        }
        public List<string> GetFileList(string path)
        {
            List<string> result = new List<string>();
            FtpListItem[] list = client.GetListing(FtpHelper.CheckAndFixPath(path));
            if (list != null && list.Length > 0)
            {
                foreach (FtpListItem item in list)
                {
                    if (item.Type == FtpObjectType.File)
                        result.Add(item.Name);
                }
            }
            return result;
        }
        public void DownloadFile(string path, string filename, string savepath)
        {
            string completeFileName = Path.Combine(savepath, filename);
            if (File.Exists(completeFileName))
                File.Delete(completeFileName);
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            FtpStatus ftpStatus = client.DownloadFile(completeFileName, fileFtp);
            if (ftpStatus != FtpStatus.Success)
                throw new Exception("Errore in download file: " + ftpStatus.ToString());
        }

        public void UploadFile(string path, string localpath, string filename, bool overwriteIfExists = true)
        {
            string localFile = Path.Combine(localpath, filename);
            if (!File.Exists(localFile))
                throw new ArgumentException($"File {localFile} inesistente");
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            if (overwriteIfExists)
            {
                if (ExistFile(path, filename))
                    DeleteFile(path, filename);
            }
            FtpStatus ftpStatus = client.UploadFile(localFile, fileFtp);
            if (ftpStatus != FtpStatus.Success)
                throw new Exception("Errore in upload file: " + ftpStatus.ToString());
        }

        public void DeleteFile(string path, string filename)
        {
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            client.DeleteFile(fileFtp);
        }

        public bool ExistFile(string path, string filename)
        {
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            return client.FileExists(fileFtp);
        }

        public bool RenameFile(string path, string oldFilename, string newFilename)
        {
            string fileFtpOld = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(oldFilename);
            string fileFtpNew = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(newFilename);
            return client.MoveFile(fileFtpOld, fileFtpNew);
        }

        public void Dispose()
        {
            if (client != null && !client.IsDisposed)
                client.Dispose();
        }

        public bool MakeDirectory(string path, string directory)
        {
            string ftpPath = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(directory);
            return client.CreateDirectory(ftpPath);
        }

        public bool DirectoryExists(string path, string directory)
        {
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(directory);
            return client.FileExists(fileFtp);

        }

        public DateTime GetDateTimeFileModification(string path, string filename)
        {
            string fileFtp = FtpHelper.CheckAndFixPath(path) + "/" + FtpHelper.CheckAndFixPath(filename);
            return client.GetModifiedTime(fileFtp);
        }
    }
}
