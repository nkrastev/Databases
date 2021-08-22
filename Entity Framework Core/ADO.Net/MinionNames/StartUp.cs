using Microsoft.Data.SqlClient;
using System;

namespace MinionNames
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");
            int villaidsId = int.Parse(Console.ReadLine());
            bool toCheckForMinions = true;
            dbCon.Open();
            using (dbCon)
            {

                SqlCommand queryVillian = new SqlCommand($@"SELECT Name FROM Villains WHERE Id=@villaidsId", dbCon);
                queryVillian.Parameters.AddWithValue("@villaidsId", villaidsId);                

                SqlDataReader readerVillian = queryVillian.ExecuteReader();
                
                using (readerVillian)
                {                    
                    if (readerVillian.HasRows)
                    {
                        while (readerVillian.Read())
                        {
                            Console.WriteLine($"Villain: {readerVillian["Name"]}");
                        }                        
                    }
                    else
                    {
                        Console.WriteLine($"No villain with ID {villaidsId} exists in the database.");
                        toCheckForMinions = false;
                    }

                }

                if (toCheckForMinions)
                {
                    SqlCommand queryMinions = new SqlCommand($@"SELECT m.Id, m.Name, m.Age, mv.MinionId, mv.VillainId
                                            FROM Minions AS m
                                            INNER JOIN MinionsVillains AS mv ON m.Id = mv.MinionId
                                            WHERE mv.VillainId={villaidsId}
                                            ORDER BY (m.Name)", dbCon);
                    queryMinions.Parameters.AddWithValue("@villaidsId", villaidsId);

                    SqlDataReader reader = queryMinions.ExecuteReader();

                    using (reader)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["Name"]} - {reader["Age"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"(no minions)");
                        }
                    }
                }
                
                
                
            }
        }
    }
}
