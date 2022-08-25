using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;

namespace DotFTP
{
    [Obsolete ("Usare FtpClient")]
    public class FTPAgent : BaseFTPAgent, IFTPAgent
    {
        public FTPAgent()
        {
        }

        #region Obsolete
        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>Un array di nomi di file</returns>
        [Obsolete]
        public string[] GetDirectoryContent(string host, string path, string user, string pass)
        {
            int fileCount = 0;
            return GetDirectoryContentEx(host, path, user, pass, out fileCount);
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
        [Obsolete]
        public string[] GetDirectoryContentEx(string host, string path, string user, string pass, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }

                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                fileCount = fileList.Length;
                return fileList;
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                fileCount = 0;
                return new string[] { "?" };
            }
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>

        [Obsolete]
        public ArrayList GetDirectoryList(string host, string path, string user, string pass, out int fileCount)
        {
            bool errore = true;
            int nTry = 5;
            int i = 0;
            while (errore && i < nTry)
            {
                try
                {
                    return GetDirectoryListInternal(host, path, user, pass, out fileCount);
                }
                catch (Exception e)
                {
                    i++;
                    if (i >= nTry)
                    {
                        lastErrorMessage = e.Message;
                        fileCount = 0;

                        ArrayList fileListResult = new ArrayList();
                        fileListResult.Add("?");
                        return fileListResult;
                    }
                }
            }
            fileCount = 0;

            ArrayList fileListResultd = new ArrayList();
            fileListResultd.Add("?");
            return fileListResultd;
        }
        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>

        [Obsolete]
        public ArrayList GetDirectoryList(string host, string path, string user, string pass, WebProxy proxy, out int fileCount)
        {
            bool errore = true;
            int nTry = 5;
            int i = 0;
            while (errore && i < nTry)
            {
                try
                {
                    return GetDirectoryListInternal(host, path, user, pass, proxy, out fileCount);
                }
                catch (Exception e)
                {
                    i++;
                    if (i >= nTry)
                    {
                        lastErrorMessage = e.Message;
                        fileCount = 0;

                        ArrayList fileListResult = new ArrayList();
                        fileListResult.Add("?");
                        return fileListResult;
                    }
                }
            }
            fileCount = 0;

            ArrayList fileListResultd = new ArrayList();
            fileListResultd.Add("?");
            return fileListResultd;
        }

        [Obsolete]
        public ArrayList GetDirectoryListInternal(string host, string path, string user, string pass, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }
                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                ArrayList fileListResult = new ArrayList();
                foreach (string nomeFile in fileList)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        fileListResult.Add(ftpFinfo.FileName);
                    }
                }
                fileCount = fileList.Length;
                return fileListResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Obsolete]
        public ArrayList GetDirectoryListInternal(string host, string path, string user, string pass, WebProxy proxy, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                if (proxy != null)
                    request.Proxy = proxy;
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }

                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                ArrayList fileListResult = new ArrayList();
                foreach (string nomeFile in fileList)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        fileListResult.Add(ftpFinfo.FileName);
                    }
                }

                fileCount = fileList.Length;
                return fileListResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Scarica un file dal server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove cui scaricare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da scaricare</param>
        /// <param name="localpath">path locale di dove si trova il file da uploadare</param>
        /// <param name="savepath">la directory locale dove salvare il file scaricato</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>


        [Obsolete]
        public int DownloadFile(string host, string path, string filename, string savepath, string user, string pass)
        {
            int result = 0;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));

                //FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                //request.Method = WebRequestMethods.Ftp.ListDirectory;
                ////Notice that the Create function needs to have the FTP address and the file name, so for example, it would be something like: ftp://myserver.com/myfile.txt. Once you have that, you can ask the FTP server for the filesize like so:
                //WebResponse responseLength = request.GetResponse();

                //long dataLength = responseLength.ContentLength;

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.Default, true);
                if (File.Exists(savepath + "\\" + filename))
                {
                    File.Delete(savepath + "\\" + filename);
                }

                using (FileStream fs = new FileStream(savepath + "\\" + filename, FileMode.CreateNew))
                {
                    using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                    {
                        writer.Write(stream.ReadToEnd());
                    }
                }


            }

            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error
            }

            return result;
        }


        /// <summary>
        /// Manda un file al server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="localpath">path locale di dove si trova il file da uploadare</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">porta</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>

        [Obsolete]
        public int UploadFile(string host, string path, string localpath, string filename, string user, string pass)
        {
            int result = 0;

            try
            {
                FileInfo finfoUp = new FileInfo(localpath + @"\" + filename);

                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                //Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);

                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.KeepAlive = false;
                request.UseBinary = true;
                request.ContentLength = finfoUp.Length;
                request.UsePassive = true;
                FileStream fin = new FileStream(localpath + "\\" + filename, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fin.Length];
                fin.Read(buffer, 0, buffer.Length);
                fin.Close();

                Stream stream = request.GetRequestStream();
                for (int i = 0; i < buffer.Length; i++)
                {
                    stream.WriteByte(buffer[i]);
                }

                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error

                //chech and delete for partially uploaded file
                DeleteFile(host, path, filename, user, pass);
            }

            return result;
        }


        [Obsolete]
        public int UploadFileTurbo(string host, string path, string localpath, string filename, string user, string pass)
        {
            return UploadFileTurbo(host, path, localpath, filename, user, pass, 21);
        }

        [Obsolete]
        public int UploadFileTurbo(string host, string path, string localpath, string filename, string user, string pass, int port)
        {
            int result = 0;

            try
            {
                FileInfo finfoUp = new FileInfo(localpath + @"\" + filename);
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                //Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Timeout = -1;
                request.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.KeepAlive = false;
                request.UseBinary = true;
                request.ContentLength = finfoUp.Length;
                request.UsePassive = true;
                FileStream fin = File.OpenRead(localpath + "\\" + filename);

                byte[] buffer = new byte[fin.Length];
                fin.Read(buffer, 0, buffer.Length);
                fin.Close();

                Stream stream = request.GetRequestStream();
                int buffUnit = 512;
                int b = 0;
                while (b < buffer.Length)
                {
                    if ((buffer.Length - (b + buffUnit)) < 0)
                    {
                        buffUnit = buffer.Length - b;
                    }
                    stream.Write(buffer, b, buffUnit);
                    b += buffUnit;

                }
                //stream.Write(buffer, 0, buffer.Length);

                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error

                //chech and delete for partially uploaded file
                DeleteFile(host, path, filename, user, pass);
            }

            return result;
        }
        /// <summary>
        /// Crea un file (0 byte) al server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>

        [Obsolete]
        public int UploadFileNoStream(string host,
                                              string path,
                                              string filename,
                                              string user,
                                              string pass)
        {
            int result = 0;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                Stream stream = request.GetRequestStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error
            }

            return result;
        }
        [Obsolete("Utilizzare ExistFile(string host, int port, string path, string fileToFind, string user, string pass)")]
        public bool ExistFile(string host, string path, string fileToFind, string user, string pass)
        {
            string[] listaFilesClient = this.GetDirectoryContent(host, path, user, pass);
            bool trovatoF = false;
            foreach (string fileFTP in listaFilesClient)
            {
                if (fileFTP.CompareTo(fileToFind) == 0)
                {
                    trovatoF = true;
                    break;
                }
            }
            return trovatoF;

        }

        /// <summary>
        /// Elimina un file dall'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da dove eliminare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da eliminare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>

        [Obsolete]
        public int DeleteFile(string host, string path, string filename, string user, string pass)
        {
            int result = 0;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error
            }

            return result;
        }

        /// <summary>
        /// Crea una directory sull'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove creare la directory sull'host FTP</param>
        /// <param name="directory">il nome della directory</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>

        [Obsolete]
        public int MakeDirectory(string host, string path, string directory, string user, string pass)
        {
            int result = 0;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(directory));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error
            }

            return result;
        }


        public bool DirectoryExists(string host, string path, string directory, string user, string pass)
        {
            bool IsExists = true;
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(directory));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                //                request = (FtpWebRequest)WebRequest.Create(directory);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch
            {
                IsExists = false;
            }
            return IsExists;
        }


        /// <summary>
        /// GetDateTimeFileModification: restituisce la data di modifica di un file
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public DateTime GetDateTimeFileModification(string host, string path, string filename, string user, string pass)
        {
            DateTime DateValue = DateTime.MinValue;
            try
            {

                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);

                Request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                Request.UseBinary = false;

                using (FtpWebResponse Response = (FtpWebResponse)Request.GetResponse())
                using (StreamReader sr = new StreamReader(Response.GetResponseStream()))
                {
                    DateValue = Response.LastModified;
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }

            return DateValue;
        }

        /// <summary>
        /// GetDirectories: restituisce una lista di directoryes
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>

        [Obsolete]
        public ArrayList GetDirectories(string host, string path, string mask, string user, string pass)
        {
            ArrayList directoryList = new ArrayList();
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + mask);
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);
                Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();

                StreamReader streamReader = new StreamReader(Response.GetResponseStream());

                string line = streamReader.ReadLine();
                while (line != null)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(line);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        directoryList.Add(ftpFinfo.FileName);
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
                Response.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }
            return directoryList;
        }

        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>

        [Obsolete]
        public ArrayList GetFiles(string host, string path, string mask, string user, string pass)
        {
            ArrayList fileList = new ArrayList();

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(path) + "/" + mask);
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);
                Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();

                StreamReader streamReader = new StreamReader(Response.GetResponseStream());

                string line = streamReader.ReadLine();
                while (line != null)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(line);
                    if (!ftpFinfo.IsDirectory)
                    {
                        fileList.Add(ftpFinfo.FileName);
                    }

                    line = streamReader.ReadLine();
                }
                streamReader.Close();
                Response.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }
            return fileList;
        }
        #endregion

        #region new

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <param name="port">Porta dell'host FTP</param>
        /// <returns>Un array di nomi di file</returns>
        public string[] GetDirectoryContentEx(string host, int port, string path, string user, string pass, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }

                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                fileCount = fileList.Length;
                return fileList;
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                fileCount = 0;
                return null;
            }
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Una lista di stringhe di nomi di file</returns>
        public List<string> GetDirectoryList(string host, int port, string path, string user, string pass, out int fileCount)
        {
            bool errore = true;
            int nTry = 5;
            int i = 0;
            while (errore && i < nTry)
            {
                try
                {
                    return GetDirectoryListInternal(host, port, path, user, pass, out fileCount);
                }
                catch (Exception e)
                {
                    i++;
                    if (i >= nTry)
                    {
                        lastErrorMessage = e.Message;
                        fileCount = 0;
                        return null;
                    }
                }
            }
            fileCount = 0;

            return null;
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>>Una lista di stringhe di nomi di file</returns>
        public List<string> GetDirectoryList(string host, int port, string path, string user, string pass, WebProxy proxy, out int fileCount)
        {
            bool errore = true;
            int nTry = 5;
            int i = 0;
            while (errore && i < nTry)
            {
                try
                {
                    return GetDirectoryListInternal(host, port, path, user, pass, proxy, out fileCount);
                }
                catch (Exception e)
                {
                    i++;
                    if (i >= nTry)
                    {
                        lastErrorMessage = e.Message;
                        fileCount = 0;
                        return null;
                    }
                }
            }
            fileCount = 0;
            return null;
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="fileCount"></param>
        /// <returns></returns>
        protected List<string> GetDirectoryListInternal(string host, int port, string path, string user, string pass, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }
                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                List<string> fileListResult = new List<string>();
                foreach (string nomeFile in fileList)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        fileListResult.Add(ftpFinfo.FileName);
                    }
                }

                fileCount = fileList.Length;
                return fileListResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="proxy"></param>
        /// <param name="fileCount"></param>
        /// <returns></returns>
        protected List<string> GetDirectoryListInternal(string host, int port, string path, string user, string pass, WebProxy proxy, out int fileCount)
        {
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                if (proxy != null)
                    request.Proxy = proxy;
                request.Credentials = new NetworkCredential(user, pass);
                string s = "";
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                while (stream.CanRead == true)
                {
                    s += (char)stream.ReadByte();
                }

                string[] fileList = s.Split(new string[] { "\r\n" }, 4096, StringSplitOptions.RemoveEmptyEntries);
                List<string> fileListResult = new List<string>();
                foreach (string nomeFile in fileList)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(nomeFile);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        fileListResult.Add(ftpFinfo.FileName);
                    }
                }
                fileCount = fileList.Length;
                return fileListResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Scarica un file dal server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <param name="path">il path relativo dove cui scaricare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da scaricare</param>
        /// <param name="savepath">la directory locale dove salvare il file scaricato</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        public bool DownloadFile(string host, int port, string path, string filename, string savepath, string user, string pass)
        {
            bool result = true;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));

                //FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                //request.Method = WebRequestMethods.Ftp.ListDirectory;
                ////Notice that the Create function needs to have the FTP address and the file name, so for example, it would be something like: ftp://myserver.com/myfile.txt. Once you have that, you can ask the FTP server for the filesize like so:
                //WebResponse responseLength = request.GetResponse();

                //long dataLength = responseLength.ContentLength;

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.Default, true);
                if (File.Exists(savepath + "\\" + filename))
                {
                    File.Delete(savepath + "\\" + filename);
                }

                using (FileStream fs = new FileStream(savepath + "\\" + filename, FileMode.CreateNew))
                {
                    using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                    {
                        writer.Write(stream.ReadToEnd());
                    }
                }


            }

            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }

            return result;
        }


        /// <summary>
        /// Manda un file al server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="localpath">path locale di dove si trova il file da uploadare</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">porta</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>        
        public bool UploadFile(string host, int port, string path, string localpath, string filename, string user, string pass)
        {
            bool result = true;
            try
            {
                FileInfo finfoUp = new FileInfo(localpath + @"\" + filename);
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                //Uri serverUri = new Uri(CheckAndFixPath(host) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.KeepAlive = false;
                request.UseBinary = true;
                request.ContentLength = finfoUp.Length;
                request.UsePassive = true;
                using (FileStream fin = new FileStream(localpath + "\\" + filename, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fin.Length];
                    fin.Read(buffer, 0, buffer.Length);
                    fin.Close();
                    Stream stream = request.GetRequestStream();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stream.WriteByte(buffer[i]);
                    }
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
                //chech and delete for partially uploaded file
                DeleteFile(host, port, path, filename, user, pass);
            }

            return result;
        }

        /// <summary>
        /// Crea un file (0 byte) al server FTP
        /// </summary>
        /// <param name="port">la porta dell'host FTP</param>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        public bool UploadFileNoStream(string host, int port, string path, string filename, string user, string pass)
        {
            bool result = true;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                Stream stream = request.GetRequestStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }

            return result;
        }

        /// <summary>
        /// Verifica se un file esiste su FTP
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="fileToFind"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool ExistFile(string host, int port, string path, string fileToFind, string user, string pass)
        {
            string[] listaFilesClient = this.GetDirectoryContentEx(host, port, path, user, pass, out _);
            bool trovatoF = false;
            foreach (string fileFTP in listaFilesClient)
            {
                if (fileFTP.CompareTo(fileToFind) == 0)
                {
                    trovatoF = true;
                    break;
                }
            }
            return trovatoF;

        }

        /// <summary>
        /// Elimina un file dall'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da dove eliminare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da eliminare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        public bool DeleteFile(string host, int port, string path, string filename, string user, string pass)
        {
            bool result = true;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }

            return result;
        }

        /// <summary>
        /// Rinomina i file su FTP, attenzione se utilizzata dopo un'upload lanciare prima una getListDirectory della cartella altrimenti va in errore di sistema la prima volta.
        /// </summary>
        /// <param name="host">Il nome dell'host FTP</param>
        /// <param name="port">Porta</param>
        /// <param name="path">il path relativo di dove si trova il file.</param>
        /// <param name="oldFilename">Nome del file da rinominare</param>
        /// <param name="newFilename">Nuovo nome del file da fornire</param>
        /// <param name="user">utente ftp</param>
        /// <param name="pass">password ftp</param>
        /// <returns></returns>
        public bool RenameFile(string host, int port, string path, string oldFilename, string newFilename, string user, string pass)
        {
            bool result = true;
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(oldFilename));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = CheckAndFixPath(newFilename);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.Close();
                stream.Dispose();
                response.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }
            return result;
        }

        /// <summary>
        /// Crea una directory sull'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove creare la directory sull'host FTP</param>
        /// <param name="directory">il nome della directory</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        public bool MakeDirectory(string host, int port, string path, string directory, string user, string pass)
        {
            bool result = true;

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(directory));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                stream.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }

            return result;
        }

        /// <summary>
        /// Verifica se una cartella esiste su FTP
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="directory"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool DirectoryExists(string host, int port, string path, string directory, string user, string pass)
        {
            bool IsExists = true;
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(directory));
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                request.Credentials = new NetworkCredential(user, pass);
                //                request = (FtpWebRequest)WebRequest.Create(directory);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch
            {
                IsExists = false;
            }
            return IsExists;
        }

        /// <summary>
        /// GetDateTimeFileModification: restituisce la data di modifica di un file
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public DateTime GetDateTimeFileModification(string host, int port, string path, string filename, string user, string pass)
        {
            DateTime DateValue = DateTime.MinValue;
            try
            {

                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + CheckAndFixPath(filename));
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);

                Request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                Request.UseBinary = false;

                using (FtpWebResponse Response = (FtpWebResponse)Request.GetResponse())
                using (StreamReader sr = new StreamReader(Response.GetResponseStream()))
                {
                    DateValue = Response.LastModified;
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }

            return DateValue;
        }

        /// <summary>
        /// GetDirectories: restituisce una lista di directoryes
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<string> GetDirectories(string host, int port, string path, string mask, string user, string pass)
        {
            List<string> directoryList = new List<string>();
            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + mask);
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);
                Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();

                StreamReader streamReader = new StreamReader(Response.GetResponseStream());


                string line = streamReader.ReadLine();
                while (line != null)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(line);
                    if (ftpFinfo.IsDirectory && !string.IsNullOrEmpty(ftpFinfo.FileName) && ftpFinfo.FileName != "." && ftpFinfo.FileName != "..")
                    {
                        directoryList.Add(ftpFinfo.FileName);
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
                Response.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }
            return directoryList;
        }

        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<string> GetFiles(string host, int port, string path, string mask, string user, string pass)
        {
            List<string> fileList = new List<string>();

            try
            {
                Uri serverUri = new Uri(CheckAndFixPath(host) + ":" + port + "/" + CheckAndFixPath(path) + "/" + mask);
                FtpWebRequest Request = (FtpWebRequest)FtpWebRequest.Create(serverUri);
                Request.Credentials = new NetworkCredential(user, pass);
                Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse Response = (FtpWebResponse)Request.GetResponse();

                StreamReader streamReader = new StreamReader(Response.GetResponseStream());

                string line = streamReader.ReadLine();
                while (line != null)
                {
                    FtpFileInfo ftpFinfo = new FtpFileInfo(line);
                    if (!ftpFinfo.IsDirectory)
                    {
                        fileList.Add(ftpFinfo.FileName);
                    }

                    line = streamReader.ReadLine();
                }
                streamReader.Close();
                Response.Close();
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
            }
            return fileList;
        }
        #endregion


        /// <summary>
        /// Classe che rappresenta le informazioni di un file su FTP
        /// </summary>
        public class FtpFileInfo
        {
            public Boolean IsDirectory { get; set; }
            public String PermissionsList { get; set; }
            public Char[] Permissions { get; set; }
            public Int32 NrOfInodes { get; set; }
            public String Type { get; set; }
            public String User { get; set; }
            public String Group { get; set; }
            public Int64 FileSize { get; set; }
            public DateTime LastModifiedDate { get; set; }
            public String FileName { get; set; }
            public FtpFileInfo()
            { }
            public FtpFileInfo(string line)
            {
                Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                IFormatProvider culture = System.Globalization.CultureInfo.GetCultureInfo("it-it");
                Match match = regex.Match(line);
                Type = match.Groups[1].Value;
                if (Type == "d")
                {
                    IsDirectory = true;
                }
                PermissionsList = match.Groups[2].Value;
                //long size = long.Parse(match.Groups[3].Value, culture);
                //DateTime lastModifiedDate = Convert.ToDateTime(match.Groups[4].Value);
                FileName = match.Groups[6].Value;
            }
        }
    }
}

