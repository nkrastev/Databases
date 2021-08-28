using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncreaseMinionAge
{
    class Program
    {
        static void Main()
        {
            SqlConnection dbCon = new SqlConnection(@"Server=.; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {
                List<string> input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();                
                
                try
                {
                    //update age and name
                    foreach (var item in input)
                    {
                        SqlCommand queryUpdateAge = new SqlCommand(@"
                                        UPDATE Minions 
                                        SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 
                                        WHERE Id = @Id", dbCon);
                        queryUpdateAge.Parameters.AddWithValue("@Id", item);
                        queryUpdateAge.ExecuteNonQuery();
                    }

                    //print result
                    SqlCommand queryAllMinions = new SqlCommand(@"SELECT Name, Age FROM Minions", dbCon);
                    SqlDataReader reader = queryAllMinions.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["Age"]}");
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
