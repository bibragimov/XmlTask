namespace XmlTask.Utils
{
    public class StringExtension
    {
        public static readonly string ConnectionString =
            @"Data Source=DESKTOP-QQGSJNI;Initial Catalog=XmlTaskBd;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static readonly string InsertFileQuery =
            "INSERT INTO Files (Name,FileVersion,DateTime) VALUES (@val1, @val2, @val3)";

        public static readonly string GetAllQuery = "SELECT * FROM Files";

        public static readonly string UpdateFileQuery =
            "UPDATE Files SET Name = @val1, FileVersion = @val2, DateTime = @val3 Where Id = @val4";
    }
}