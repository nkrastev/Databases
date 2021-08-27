using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PrintAllMinionNames
{
    class StartUp
    {
        static void Main()
        {
            List<string> minions = GetDataFromDatabase();

            //Console.WriteLine(String.Join("\n", minions));
            string position = "first";
            while (minions.Count>0)
            {
                if (position=="first")
                {
                    Console.WriteLine(minions[0]);
                    minions.RemoveAt(0);
                    position = "last";
                }
                else
                {
                    Console.WriteLine(minions[minions.Count - 1]);
                    minions.RemoveAt(minions.Count - 1);
                    position = "first";
                }
            }            
        }

        private static List<string> GetDataFromDatabase()
        {
            List<string> list = new List<string>();
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                SqlCommand command = new SqlCommand(@"SELECT Name FROM Minions", dbCon);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        list.Add((string)reader["Name"]);
                    }
                }
            }
            return list;
        }
    }
}
