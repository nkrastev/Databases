using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main()
        {
            int targetId = int.Parse(Console.ReadLine());

            SqlConnection dbCon = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                //call stored procedure
                using (SqlCommand cmd = new SqlCommand("usp_GetOlder", dbCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = targetId;
                    SqlDataReader reader = cmd.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                        }
                    }
                }
            }
        }
    }
}
