using DotFTP;
using DotFTP.Client;

IFtpClient client = new FtpClient("localhost", "sistemi", "Sistemi123");
IFtpClient sftpClient = new SFtpClient("10.254.254.12", "sistnet-admin", "pd7Norm3");

#region GetDirectoryList
List<string> files = client.GetDirectoryList("/");
Console.WriteLine("Elenco directory: ");
foreach (string file in files)
{
    Console.WriteLine(file);
}
files = sftpClient.GetDirectoryList("/var/opt/docker/bosca");
Console.WriteLine("Elenco directory: ");
foreach (string file in files)
{
    Console.WriteLine(file);
}
#endregion
#region GetFileList
files = client.GetFileList("/");
Console.WriteLine("Elenco file: ");
foreach (string file in files)
{
    Console.WriteLine(file);
}
files = sftpClient.GetFileList("/var/opt/docker/bosca");
Console.WriteLine("Elenco file: ");
foreach (string file in files)
{
    Console.WriteLine(file);
}
#endregion
#region DownloadFile
client.DownloadFile("/", "test1.txt", @"C:\tmp\DotFTP");
sftpClient.DownloadFile("/var/opt/docker/bosca/data", "mastlog.ldf", @"C:\tmp\DotFTP");
#endregion
Console.ReadLine();