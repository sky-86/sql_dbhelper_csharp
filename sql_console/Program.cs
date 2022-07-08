namespace Program
{
    class Program
    {
        static private string sqlConnectionString = @"Data Source=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True";

        static void Main(string[] args)
        {
            DatabaseHelper dbUsers = new DatabaseHelper(sqlConnectionString);

            //dbUsers.ExecuteReader("DROP TABLE tblUsers");
            //dbUsers.DeleteTable("tblUsers");

            //dbUsers.ExecuteReader("CREATE TABLE tblUsers(id INT PRIMARY KEY IDENTITY (1,1), first VARCHAR(255), last VARCHAR(255))");
            //dbUsers.AddTable("tblUsers");

            //dbUsers.FlushTable("tblUsers");

            //dbUsers.ExecuteReader("INSERT INTO tblUsers (first, last) VALUES ('skyler', 'favors')");

            //object[][] results = dbUsers.ExecuteReader("SELECT * FROM tblUsers");

            int length = dbUsers.GetTableRecordCount("tblUsers");
            Console.WriteLine(length);
        }
    }
}

