using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Collections;
using Renci.SshNet;
using System.Linq;
using Renci.SshNet.Sftp;

namespace DotFTP
{
    [Obsolete("Usare SFtpClient")]
    public class SFTPAgent : BaseFTPAgent, IFTPAgent
    {
        public SFTPAgent()
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
        /// <param name="port">port</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
        public string[] GetDirectoryContentEx(string host, string path, string user, string pass, out int fileCount)
        {
            try
            {
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                List<string> fileNameList = new List<string>();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    var list = client.ListDirectory(path);
                    fileCount = list != null ? list.Count() : 0;
                    if (fileCount > 0)
                    {

                        foreach (SftpFile file in list)
                        {
                            if (!file.IsDirectory)
                                fileNameList.Add(file.Name);
                        }
                    }
                }
                return fileNameList.ToArray();
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

                        ArrayList fileListResult = new ArrayList
                        {
                            "?"
                        };
                        return fileListResult;
                    }
                }
            }
            fileCount = 0;

            ArrayList fileListResultd = new ArrayList
            {
                "?"
            };
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
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    var list = client.ListDirectory(path);
                    fileCount = list != null ? list.Count() : 0;
                    if (fileCount > 0)
                    {

                        foreach (SftpFile file in list)
                        {
                            if (file.IsDirectory)
                                fileNameList.Add(file.Name);
                        }
                    }
                }
                return fileNameList;
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
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                ArrayList fileNameList = new ArrayList();
                ConnectionInfo connectionInfo = null;

                if (proxy != null)
                {
                    ProxyTypes proxyType = ProxyTypes.Http;
                    string proxyHost = proxy.Address.Host;
                    int proxyPort = proxy.Address.Port;
                    string proxyUsername = null;
                    string proxyPassword = null;
                    NetworkCredential nc = proxy.Credentials.GetCredential(proxy.Address, "");
                    if (nc != null)
                    {
                        proxyUsername = nc.UserName;
                        proxyPassword = nc.Password;
                    }
                    connectionInfo = new ConnectionInfo(host, 21, user,
                        proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword,
                        new PasswordAuthenticationMethod(user, pass), new PrivateKeyAuthenticationMethod("rsa.key"));
                }
                else
                {
                    connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                }
                using (var client = new SftpClient(connectionInfo))
                {

                    client.Connect();
                    var list = client.ListDirectory(path);
                    fileCount = list != null ? list.Count() : 0;
                    if (fileCount > 0)
                    {

                        foreach (SftpFile file in list)
                        {
                            if (file.IsDirectory)
                                fileNameList.Add(file.Name);
                        }
                    }
                }
                return fileNameList;
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
                host = CheckAndFixPath(host);
                string serverPath = CheckAndFixPath(path) + "/" + CheckAndFixPath(filename);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
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
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    string filePathSource = Path.Combine(localpath, filename);
                    using (FileStream fs = new FileStream(filePathSource, FileMode.Open, FileAccess.Read))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, filename);
                    }
                }
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
            return UploadFile(host, path, localpath, filename, user, pass);
        }
        [Obsolete]
        public int UploadFileTurbo(string host, string path, string localpath, string filename, string user, string pass, int port)
        {
            return UploadFile(host, path, localpath, filename, user, pass);
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
        public int UploadFileNoStream(string host, string path, string filename, string user, string pass)
        {
            int result = 0;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(ms, filename);
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
        [Obsolete]
        public bool ExistFile(string host, string path, string fileToFind, string user, string pass)
        {
            return ExistsFileOrDirectory(host, path, fileToFind, user, pass);
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
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        client.BufferSize = 4 * 1024;
                        client.Delete(filename);
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
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    client.CreateDirectory(directory);
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = 1;//error
            }
            return result;
        }

        private bool ExistsFileOrDirectory(string host, string path, string fileOrDir, string user, string pass)
        {
            bool IsExists = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    IsExists = client.Exists(fileOrDir);
                }
            }
            catch
            {
                IsExists = false;
            }
            return IsExists;
        }

        public bool DirectoryExists(string host, string path, string directory, string user, string pass)
        {
            return ExistsFileOrDirectory(host, path, directory, user, pass);
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
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    SftpFileAttributes attributes = client.GetAttributes(filename);
                    DateValue = attributes.LastWriteTime;
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
        /// Non funziona il mask
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [Obsolete]
        public ArrayList GetDirectories(string host, string path, string mask, string user, string pass)
        {
            return GetDirectoryListInternal(host, path, user, pass, out int fileCount);
        }

        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// Non funziona il mask
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [Obsolete]
        public ArrayList GetFiles(string host, string path, string mask, string user, string pass)
        {
            var stringArray = GetDirectoryContentEx(host, path, user, pass, out int fileCount);
            ArrayList retVal = new ArrayList();
            retVal.AddRange(stringArray);
            return retVal;
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
        /// <returns>Un array di nomi di file</returns>
        public string[] GetDirectoryContent(string host, int port, string path, string user, string pass)
        {
            int fileCount = 0;
            return GetDirectoryContentEx(host, port, path, user, pass, out fileCount);
        }

        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">port</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
        public string[] GetDirectoryContentEx(string host, int port, string path, string user, string pass, out int fileCount)
        {
            try
            {
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                List<string> fileNameList = new List<string>();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                fileCount = 0;
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    var list = client.ListDirectory(path);
                    if (list != null && list.Count() > 0)
                    {
                        foreach (SftpFile file in list)
                        {
                            if (!file.IsDirectory)
                            {
                                fileCount++;
                                fileNameList.Add(file.Name);
                            }
                        }
                    }
                }
                return fileNameList.ToArray();
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
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
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
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
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
        /*
        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <returns>Un array di nomi di file</returns>
        public List<string> GetDGetDirectoryListirectoryList(string host, string path, string user, string pass, WebProxy proxy, out int fileCount, int port = 22)
        {
            bool errore = true;
            int nTry = 5;
            int i = 0;
            while (errore && i < nTry)
            {
                try
                {
                    return GetDirectoryListInternal(host, path, user, pass, proxy, out fileCount, port);
                    errore = false;
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
        */
        protected List<string> GetDirectoryListInternal(string host, int port, string path, string user, string pass, out int fileCount)
        {
            try
            {
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                List<string> fileNameList = new List<string>();
                var connectionInfo = new ConnectionInfo(host, port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    var list = client.ListDirectory(path);
                    fileCount = list != null ? list.Count() : 0;
                    if (fileCount > 0)
                    {

                        foreach (SftpFile file in list)
                        {
                            if (file.IsDirectory)
                                fileNameList.Add(file.Name);
                        }
                    }
                }
                return fileNameList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected List<string> GetDirectoryListInternal(string host, int port, string path, string user, string pass, WebProxy proxy, out int fileCount)
        {
            try
            {
                host = CheckAndFixPath(host);
                path = CheckAndFixPath(path);
                List<string> fileNameList = new List<string>();
                ConnectionInfo connectionInfo = null;

                if (proxy != null)
                {
                    ProxyTypes proxyType = ProxyTypes.Http;
                    string proxyHost = proxy.Address.Host;
                    int proxyPort = proxy.Address.Port;
                    string proxyUsername = null;
                    string proxyPassword = null;
                    NetworkCredential nc = proxy.Credentials.GetCredential(proxy.Address, "");
                    if (nc != null)
                    {
                        proxyUsername = nc.UserName;
                        proxyPassword = nc.Password;
                    }
                    connectionInfo = new ConnectionInfo(host, port, user,
                        proxyType, proxyHost, proxyPort, proxyUsername, proxyPassword,
                        new PasswordAuthenticationMethod(user, pass), new PrivateKeyAuthenticationMethod("rsa.key"));
                }
                else
                {
                    connectionInfo = new ConnectionInfo(host,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                }
                using (var client = new SftpClient(connectionInfo))
                {

                    client.Connect();
                    var list = client.ListDirectory(path);
                    fileCount = list != null ? list.Count() : 0;
                    if (fileCount > 0)
                    {

                        foreach (SftpFile file in list)
                        {
                            if (file.IsDirectory)
                                fileNameList.Add(file.Name);
                        }
                    }
                }
                return fileNameList;
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
        public bool DownloadFile(string host, int port, string path, string filename, string savepath, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                string serverPath = CheckAndFixPath(path) + "/" + CheckAndFixPath(filename);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
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
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>
        public bool UploadFile(string host, int port, string path, string localpath, string filename, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    string filePathSource = Path.Combine(localpath, filename);
                    using (FileStream fs = new FileStream(filePathSource, FileMode.Open, FileAccess.Read))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, filename);
                    }
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
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>0 se la funzione ha avuto successo, 1 in caso di errore</returns>
        public bool UploadFileNoStream(string host, int port, string path, string filename, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(ms, filename);
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

        public bool ExistFile(string host, int port, string path, string fileToFind, string user, string pass)
        {
            return ExistsFileOrDirectory(host, port, path, fileToFind, user, pass);
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
        public bool DeleteFile(string host, int port, string path, string filename, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                List<string> fileNameList = new List<string>();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        client.BufferSize = 4 * 1024;
                        client.Delete(filename);
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

        public bool RenameFile(string host, int port, string path, string oldFilename, string newFilename, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    string filePathSource = Path.Combine(oldFilename);
                    string newFilePathDest = Path.Combine(newFilename);
                    client.RenameFile(filePathSource, newFilePathDest);
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
                //chech and delete for partially uploaded file
                DeleteFile(host, port, path, newFilename, user, pass);
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
        public bool MakeDirectory(string host, int port, string path, string directory, string user, string pass)
        {
            bool result = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    client.CreateDirectory(directory);
                }
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;
                result = false;//error
            }
            return result;
        }


        private bool ExistsFileOrDirectory(string host, int port, string path, string fileOrDir, string user, string pass)
        {
            bool IsExists = true;
            try
            {
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host,
                                        port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    IsExists = client.Exists(fileOrDir);
                }
            }
            catch
            {
                IsExists = false;
            }
            return IsExists;
        }


        public bool DirectoryExists(string host, int port, string path, string directory, string user, string pass)
        {
            return ExistsFileOrDirectory(host, port, path, directory, user, pass);
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
                host = CheckAndFixPath(host);
                ArrayList fileNameList = new ArrayList();
                var connectionInfo = new ConnectionInfo(host, port,
                                        user,
                                        new PasswordAuthenticationMethod(user, pass),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    client.ChangeDirectory(path);
                    SftpFileAttributes attributes = client.GetAttributes(filename);
                    DateValue = attributes.LastWriteTime;
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
        /// Non funziona il mask
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<string> GetDirectories(string host, int port, string path, string mask, string user, string pass)
        {
            return GetDirectoryListInternal(host, port, path, user, pass, out int fileCount);
        }


        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// Non funziona il mask
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<string> GetFiles(string host, int port, string path, string mask, string user, string pass)
        {
            string[] stringArray = GetDirectoryContentEx(host, port, path, user, pass, out int fileCount);
            if (stringArray == null || stringArray.Length == 0)
                return new List<string>();
            List<string> retVal = new List<string>();
            retVal.AddRange(stringArray);
            return retVal;
        }
        #endregion
    }
}

