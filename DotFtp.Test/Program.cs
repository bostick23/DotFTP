using DotFTP.Client;

using (IFtpClient client = new FtpClient("localhost", "sistemi", "Sistemi123"))
{
    client.Connect();
    //List<string> files = client.GetDirectoryList("/");
    //Console.WriteLine("Elenco directory: ");
    //foreach (string file in files)
    //{
    //    Console.WriteLine(file);
    //}
    //files = client.GetFileList("/");
    //Console.WriteLine("Elenco file: ");
    //foreach (string file in files)
    //{
    //    Console.WriteLine(file);
    //}
    //Console.WriteLine("Download File :");
    //client.DownloadFile("/", "upload.txt", @"C:\tmp\DotFTP");

    //Console.WriteLine("Esistenza file FTP");
    //Console.WriteLine("Test41.txt: " + client.ExistFile("/", "test41.txt"));
    //Console.WriteLine("Test1.txt: " + client.ExistFile("/", "test1.txt"));

    //Console.WriteLine("Upload File :");
    //client.UploadFile("/", @"C:\tmp\DotFTP", "upload.txt");

    //Console.WriteLine("Rename File :");
    //client.RenameFile("/", "upload.txt", "upload2.txt");
    //client.MakeDirectory("/", "NewDirectory");
    //Console.WriteLine("Directory esiste: " + client.DirectoryExists("/", "NewDirectory"));
    Console.WriteLine("Test1.txt: " + client.GetDateTimeFileModification("/", "test1.txt"));
}

using (IFtpClient sftpClient = new SFtpClient("10.254.254.12", "sistnet-admin", "pd7Norm3"))
{
    sftpClient.Connect();
    //List<string> files = sftpClient.GetDirectoryList("/var/opt/docker/bosca");
    //Console.WriteLine("Elenco directory: ");
    //foreach (string file in files)
    //{
    //    Console.WriteLine(file);
    //}

    //files = sftpClient.GetFileList("/var/opt/docker/bosca");
    //Console.WriteLine("Elenco file: ");
    //foreach (string file in files)
    //{
    //    Console.WriteLine(file);
    //}

    //sftpClient.DownloadFile("/var/opt/docker/bosca/data", "mastlog.ldf", @"C:\tmp\DotFTP");
    //Console.WriteLine("Esistenza file SFTP");
    //Console.WriteLine("Test41.txt: " + sftpClient.ExistFile("/var/opt/docker/bosca/data", "test41.txt"));
    //Console.WriteLine("master.mdf: " + sftpClient.ExistFile("/var/opt/docker/bosca/data", "master.mdf"));
    //sftpClient.UploadFile("/var/opt/docker/bosca/data", @"C:\tmp\DotFTP", "upload.txt");
    //Console.WriteLine("Rename File :");
    //sftpClient.RenameFile("/var/opt/docker/bosca/data", "upload.txt", "upload2.txt");
    //sftpClient.MakeDirectory("/var/opt/", "NewDirectory");
    //Console.WriteLine("Directory esiste: " + sftpClient.DirectoryExists("/var/opt/", "NewDirectory"));
    Console.WriteLine("Directory esiste: " + sftpClient.GetDateTimeFileModification("/var/opt/docker/bosca/data", "master.mdf"));
}
Console.ReadLine();