using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace DotFTP.Client
{
    public interface IFtpClient: IDisposable
    {
        void Connect();
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
        /// <summary>
        /// Carica un file sul server FTP
        /// </summary>
        /// <param name="path">Path relativo dove salvare il file sull'host FTP</param>
        /// <param name="localpath">Path locale di dove si trova il file da uploadare</param>
        /// <param name="filename">Base-name del file da uploadare</param>
        /// <param name="overwriteIfExists">Sovrascrive il file se esistente, altrimenti lancia un'eccezione</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>    
        void UploadFile(string path, string localpath, string filename, bool overwriteIfExists = true);
        /// <summary>
        /// Elimina un file dall'FTP
        /// </summary>
        /// <param name="path">Path relativo da dove eliminare il file sull'host FTP</param>
        /// <param name="filename">Base-name del file da eliminare</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        void DeleteFile(string path, string filename);
        /// <summary>
        /// Verifica se un file esiste sull'FTP
        /// </summary>
        /// <param name="path">il path relativo dove salvare il file sull'host FTP</param>
        /// <param name="fileToFind">il base-name del file da cercare</param>
        /// <returns>true se il file è stato trovato, false in caso contrario o di errore</returns>
        bool ExistFile(string path, string fileToFind);
        /// <summary>
        /// Rinomina il file su FTP
        /// </summary>
        /// <param name="path">Percorso locale all'interno dell'FTP dove è situato il file da rinominare</param>
        /// <param name="oldFilename">Nome file attuale</param>
        /// <param name="newFilename">Nome file nuovo</param>
        /// <returns></returns>
        bool RenameFile(string path, string oldFilename, string newFilename);
        /// <summary>
        /// Crea una directory sull'FTP
        /// </summary>
        /// <param name="path">il path relativo dove creare la directory sull'host FTP</param>
        /// <param name="directory">il nome della directory</param>
        /// <returns>true se la funzione ha avuto successo, false in caso di errore</returns>
        bool MakeDirectory(string path, string directory);
        /// <summary>
        /// Restituisce l'esistenza di una cartella
        /// </summary>
        /// <param name="path">il path relativo da cercare sull'host FTP</param>
        /// <param name="directory">il nome della cartella da cercare</param>
        /// <returns>true se la cartella è stato trovata, false in caso contrario o di errore</returns>
        bool DirectoryExists(string path, string directory);
        /// <summary>
        /// GetDateTimeFileModification: restituisce la data di ultima modifica di un file
        /// </summary>
        /// <param name="path">Path relativo da cercare sull'host FTP</param>
        /// <param name="filename">Base-name del file da verificare</param>
        /// <returns>Data dell'ultima modifica</returns>
        DateTime GetDateTimeFileModification(string path, string filename);
    }
}
