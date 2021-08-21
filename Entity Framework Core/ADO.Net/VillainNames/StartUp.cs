using Microsoft.Data.SqlClient;
using System;

namespace VillainNames
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                SqlCommand command = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                        FROM Villains AS v
                                                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                                        GROUP BY v.Id, v.Name
                                                        HAVING COUNT(mv.VillainId) > 3
                                                        ORDER BY COUNT(mv.VillainId)", dbCon);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                    }
                }

            }
        }
    }
}
