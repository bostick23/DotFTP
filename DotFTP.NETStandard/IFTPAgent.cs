using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace DotFTP
{
    public interface IFTPAgent
    {

        /// <summary>
        /// Ultimo messaggio di errore
        /// </summary>
        string LastErrorMessage { get; }
        /// <summary>
        /// Verifica connessione
        /// </summary>
        /// <returns></returns>
        bool IsNetworkConnectionAvailable();

        /// <summary>
        /// Verifica funzionamento connessione
        /// </summary>
        /// <returns></returns>
        bool IsNetworkConnectionWorking();

        /// <summary>
        /// Test connesione con host
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <returns></returns>
        bool CanReachRemoteHost(string host);

        /// <summary>
        /// Test connesione con host
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <param name="waitFor">tempo di attesa</param>
        /// <returns></returns>
        bool CanReachRemoteHost(string host, int port, int waitFor);

        /// <summary>
        /// Test connesione con host
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="secondi">secondi di attesa</param>
        /// <returns></returns>
        bool CanReachRemoteHost(string host, int secondi);

        #region Obsolete
        string[] GetDirectoryContent(string host, string path, string user, string pass);
        string[] GetDirectoryContentEx(string host, string path, string user, string pass, out int fileCount);
        [Obsolete]
        ArrayList GetDirectoryList(string host, string path, string user, string pass, out int fileCount);
        [Obsolete]
        ArrayList GetDirectoryList(string host, string path, string user, string pass, WebProxy proxy, out int fileCount);
        [Obsolete]
        ArrayList GetDirectoryListInternal(string host, string path, string user, string pass, out int fileCount);
        [Obsolete]
        ArrayList GetDirectoryListInternal(string host, string path, string user, string pass, WebProxy proxy, out int fileCount);
        [Obsolete]
        int DownloadFile(string host, string path, string filename, string savepath, string user, string pass);
        [Obsolete]
        int UploadFile(string host, string path, string localpath, string filename, string user, string pass);
        [Obsolete]
        int UploadFileTurbo(string host, string path, string localpath, string filename, string user, string pass);
        [Obsolete]
        int UploadFileTurbo(string host, string path, string localpath, string filename, string user, string pass, int port);
        [Obsolete]
        int UploadFileNoStream(string host, string path, string filename, string user, string pass);
        bool ExistFile(string host, string path, string fileToFind, string user, string pass);
        [Obsolete]
        int DeleteFile(string host, string path, string filename, string user, string pass);
        [Obsolete]
        int MakeDirectory(string host, string path, string directory, string user, string pass);
        bool DirectoryExists(string host, string path, string directory, string user, string pass);
        DateTime GetDateTimeFileModification(string host, string path, string filename, string user, string pass);
        [Obsolete]
        ArrayList GetDirectories(string host, string path, string mask, string user, string pass);
        [Obsolete]
        ArrayList GetFiles(string host, string path, string mask, string user, string pass);
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
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>Un array di nomi di file</returns>
        string[] GetDirectoryContentEx(string host, int port, string path, string user, string pass, out int fileCount);


        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>Una lista di stringhe di nomi di file</returns>
        List<string> GetDirectoryList(string host, int port, string path, string user, string pass, out int fileCount);


        /// <summary>
        /// Ottiene la lista di file dentro una cartella FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo della directory da cui ottenere il listing dei file sull'host FTP</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="proxy">proxy server</param>
        /// <param name="fileCount">Puntatore a intero che viene valorizzato con il numero di file</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>Una lista di stringhe di nomi di file</returns>
        List<string> GetDirectoryList(string host, int port, string path, string user, string pass, WebProxy proxy, out int fileCount);


        /// <summary>
        /// Scarica un file dal server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove cui scaricare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da scaricare</param>
        /// <param name="savepath">la directory locale dove salvare il file scaricato</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        bool DownloadFile(string host, int port, string path, string filename, string savepath, string user, string pass);


        /// <summary>
        /// Manda un file al server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="localpath">path locale di dove si trova il file da uploadare</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>        
        bool UploadFile(string host, int port, string path, string localpath, string filename, string user, string pass);


        /// <summary>
        /// Crea un file (0 byte) al server FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da uploadarte</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        bool UploadFileNoStream(string host, int port, string path, string filename, string user, string pass);

        /// <summary>
        /// Restituisce l'esistenza di un file
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="fileToFind">il base-name del file da cercare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se il file è stato trovato, false in caso contrario o di errore</returns>
        bool ExistFile(string host, int port, string path, string fileToFind, string user, string pass);


        /// <summary>
        /// Elimina un file dall'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da dove eliminare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da eliminare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        bool DeleteFile(string host, int port, string path, string filename, string user, string pass);

        /// <summary>
        /// Rinomina il file da FTP
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="path"></param>
        /// <param name="oldFilename"></param>
        /// <param name="newFilename"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        bool RenameFile(string host, int port, string path, string oldFilename, string newFilename, string user, string pass);

        /// <summary>
        /// Crea una directory sull'FTP
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo dove creare la directory sull'host FTP</param>
        /// <param name="directory">il nome della directory</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        bool MakeDirectory(string host, int port, string path, string directory, string user, string pass);

        /// <summary>
        /// Restituisce l'esistenza di una cartella
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da cercare sull'host FTP</param>
        /// <param name="directory">il nome della cartella da cercare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>true se il file è stato trovato, false in caso contrario o di errore</returns>
        bool DirectoryExists(string host, int port, string path, string directory, string user, string pass);


        /// <summary>
        /// GetDateTimeFileModification: restituisce la data di modifica di un file
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da cercare sull'host FTP</param>
        /// <param name="filename">il base-name del file da verificare</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>Data dell'ultima modifica</returns>
        DateTime GetDateTimeFileModification(string host, int port, string path, string filename, string user, string pass);


        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da cercare sull'host FTP</param>
        /// <param name="mask">Maschera sulle directories</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <returns>Lista dei nomi dei files</returns>
        List<string> GetDirectories(string host, int port, string path, string mask, string user, string pass);

        /// <summary>
        /// GetFiles: restituisce una lista di files presenti una caratella
        /// </summary>
        /// <param name="host">il nome dell'host FTP</param>
        /// <param name="path">il path relativo da cercare sull'host FTP</param>
        /// <param name="mask">Maschera sulle directories</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="port">la porta dell'host FTP</param>
        /// <returns>Lista dei nomi dei files</returns>
        List<string> GetFiles(string host, int port, string path, string mask, string user, string pass);
        #endregion


    }
}
