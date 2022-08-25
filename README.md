# DotFTP.NETStandard

DotFTP è una libreria per gestire i seguenti protocolli:
- FTP
- FTPS
- SFTP
- SCP

consentendo di eseguire le operazioni più comuni quali Upload, Download, Delete e consultazione dei file su host remoto.

## Piattaforme supportate

DotFTP ha come target .NET Standard 2.0 ed è quindi compatibile con:

Implementazione .NET|Versioni
---|---
.NET e .NET Core|1.0, 1.1, 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0
.NET Framework|4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8
Mono|4.6, 5.4, 6.4
Xamarin.iOS|10.0, 10.14, 12.16
Xamarin.Mac|3.0, 3.8, 5.16
Xamarin.Android|7.0, 8.0, 10.0
Piattaforma UWP (Universal Windows Platform)|8.0, 8.1, 10.0, 10.0.16299, TBD
Unity|2018.1

## Librerie utilizzate
DotFTP utilizza le librerie open source [FluentFTP](https://github.com/robinrodricks/FluentFTP) per FTP, FTPS e [SSH.NET](https://github.com/sshnet/SSH.NET) per FTPS e SCP

## Esempi di utilizzo
DotFTP è molto semplice da utilizzare. Gli esempi riportati si riferiscono all'implementazione dell'interfaccia per FTP/FTPS ma sono sostanzialmente identici per SFTP e SCP. Sarà sufficiente inizializzare l'interfaccia utilizzando SFtpClient e ScpClient rispettivamente

### Lettura file nella directory root
```c#
using (IFtpClient client = new FtpClient("localhost", "username", "Password"))
{
    try
    {
        client.Connect();
        List<string> files = client.GetFileList("/");
        Console.WriteLine("Elenco file: ");
        foreach (string file in files)
            Console.WriteLine(file);
    }
    catch (Exception ex)
    { Console.WriteLine(ex.Message); }
}
```
### Upload di un file nella cartella /folder
```c#
using (IFtpClient client = new FtpClient("localhost", "username", "Password"))
{
    try
    {
        client.Connect();
        client.UploadFile("/folder", @"C:\tmp\DotFTP", "upload.txt");
    }
    catch (Exception ex)
    { Console.WriteLine(ex.Message); }
}
```
### Download di un file da /folder
```c#
using (IFtpClient client = new FtpClient("localhost", "username", "Password"))
{
    try
    {
        client.Connect();
        client.DownloadFile("/folder", "upload.txt", @"C:\tmp\DotFTP");
    }
    catch (Exception ex)
    { Console.WriteLine(ex.Message); }
}
```

## Migrazione da IFTPAgent

La migrazione da IFTPAgent a IFtpClient è molto semplice. I metodi hanno mantenuto lo stesso nome e sono semplicemente stati rimossi i parametri host, port, user e pass e quelli rimasti hanno lo stesso ordine e modalità di funzionamento.

Questo è un esempio di come convertire il download di un file:
```c#
//Implementazione con IFTPAgent: 
IFTPAgent ftpAgent = new FTPAgent();
    ftpAgent.DownloadFile("localhost", 21, "/", "upload.txt", @"C:\tmp\DotFTP", "username", "Password");

//Implementazione con IFtpClient:
using (IFtpClient client = new FtpClient("localhost", "username", "Password", 21))
{
    try
    {
        client.Connect();
        client.DownloadFile("/folder", "upload.txt", @"C:\tmp\DotFTP");
    }
    catch (Exception ex)
    { Console.WriteLine(ex.Message); }
}
```
