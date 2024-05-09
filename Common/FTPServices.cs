namespace Common
{
    public class FTPServices
    {
        public static void ConnectToFtpAndCreateDirectory(string baseDir)
        {
            using (FluentFTP.FtpClient client = new FluentFTP.FtpClient("155.254.244.39"))
            {
                client.Credentials = new System.Net.NetworkCredential("mrkatsu2212", "Thangkatsu@123");
                client.Connect();

                if (!client.DirectoryExists(baseDir))
                {
                    client.CreateDirectory(baseDir);
                }
            }
        }
    }
}
