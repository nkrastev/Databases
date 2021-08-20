using Microsoft.Data.SqlClient;
using System;

namespace ConnectionToSql
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Countries", dbCon);
                int countriesCount = (int)command.ExecuteScalar();
                Console.WriteLine("Countries count: {0} ", countriesCount);
            }

        }
    }
}
