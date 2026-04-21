using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connStr = "Server=db43195.public.databaseasp.net; Database=db43195; User Id=db43195; Password=Mo@123456; Encrypt=False; MultipleActiveResultSets=True;";
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
        
            string sql = "SELECT Email, FullName, UserTypes FROM [auth].[ApplicationUsers] WHERE (UserTypes & 1) = 1 OR (UserTypes & 2) = 2";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Email: {reader["Email"]}, Name: {reader["FullName"]}, UserTypes: {reader["UserTypes"]}");
                    }
                }
            }
        }
    }
}
