using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace DotFTP.Client
{
    public interface IFtpClient
    {
        /// <summary>
        /// Ottiene l'elenco delle directory contenute all'interno di un path locale dell'FTP 
        /// </summary>
        /// <param name="path">Path locale dell'FTP</param>
        /// <returns>Elenco delle directory contenute nel path</returns>
        List<string> GetDirectoryList(string path);
        /// <summary>
        /// Ottiene l'elenco dei file contenuti all'interno di un path locale dell'FTP 
        /// </summary>
        /// <param name="path">Path locale dell'FTP</param>
        /// <returns>Elenco dei file contenuti nel path</returns>
        List<string> GetFileList(string path);
        /// <summary>
        /// Scarica un file dal server FTP
        /// </summary>
        /// <param name="path">il path relativo dove cui scaricare il file sull'host FTP</param>
        /// <param name="filename">il base-name del file da scaricare</param>
        /// <param name="savepath">la directory locale dove salvare il file scaricato</param>
        void DownloadFile(string path, string filename, string savepath);
        /*
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
    */
    }
}
