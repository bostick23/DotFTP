using DotFTP.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using static DotFTP.FTPAgent;

namespace DotFTP.Client
{
    public class FtpClient : BaseFtpClient, IFtpClient
    {
        public FtpClient(string host, string username, string password, int? port = null)
        {
            Host = host;
            Username = username;
            Password = password;
            Port = port;
        }
        private FtpWebRequest CreateRequest(string path, string filename = null)
        {
            if (!Host.StartsWith("ftp://"))
                Host = "ftp://" + Host;
            string tmpUri = FtpHelper.CheckAndFixPath(Host);
            if (Port != null)
                tmpUri += ":" + Port.Value;
            tmpUri += "/" + FtpHelper.CheckAndFixPath(path) + "/";
            if (!string.IsNullOrEmpty(filename))
                tmpUri += "/" + FtpHelper.CheckAndFixPath(filename);
            Uri uri = new Uri(tmpUri);
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
            request.Credentials = new NetworkCredential(Username, Password);
            return request;
        }
        public List<string> GetDirectoryList(string path)
        {
            FtpWebRequest request = CreateRequest(path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            string ftpData = "";
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        ftpData = reader.ReadToEnd();
                    }
                }
            }
            string[] fileList = ftpData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> fileListResult = new List<string>();
            foreach (string nomeFile in fileList)
            {
                FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    fileListResult.Add(ftpFinfo.FileName);
            }
            return fileListResult;
        }
        public List<string> GetFileList(string path)
        {
            FtpWebRequest request = CreateRequest(path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            string ftpData = "";
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        ftpData = reader.ReadToEnd();
                    }
                }
            }
            string[] fileList = ftpData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> fileListResult = new List<string>();
            foreach (string nomeFile in fileList)
            {
                FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                if (!ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    fileListResult.Add(ftpFinfo.FileName);
            }
            return fileListResult;
        }

        public void DownloadFile(string path, string filename, string savepath)
        {
            if (!Directory.Exists(savepath))
                throw new ArgumentException("Percorso savepath inesistente");
            FtpWebRequest request = CreateRequest(path, filename);
            string ftpData = "";
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        ftpData = reader.ReadToEnd();
                    }
                }
            }
            string completeFileName = Path.Combine(savepath, filename);
            if (File.Exists(completeFileName))
                File.Delete(completeFileName);
            File.WriteAllText(completeFileName, ftpData);
        }
    }
}
