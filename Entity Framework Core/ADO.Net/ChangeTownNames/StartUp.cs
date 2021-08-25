using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ChangeTownNamesCasing
{
    class StartUp
    {
        static void Main()
        {
            string countryName = Console.ReadLine();
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");

            dbCon.Open();
            using (dbCon)
            {
                SqlCommand queryGetCountryId = new SqlCommand($@"SELECT Id FROM Countries WHERE Name=@countryName", dbCon);
                queryGetCountryId.Parameters.AddWithValue("@countryName", countryName);                
                try
                {
                    int? countryId = (int)queryGetCountryId.ExecuteScalar();
                    
                    //update query
                    SqlCommand updateCitiesById = new SqlCommand($@"
                        UPDATE Towns
                        SET Name = UPPER(Name)
                        WHERE CountryCode=@countryId", dbCon);
                    updateCitiesById.Parameters.AddWithValue("@countryId", countryId);
                    int affectedRows=updateCitiesById.ExecuteNonQuery();

                    if (affectedRows==0)
                    {
                        Console.WriteLine($"No town names were affected. (* Valid Query. 0 Towns)");
                    }
                    else
                    {
                        Console.WriteLine($"{affectedRows} town names were affected.");
                        PrintTownsById(countryId);
                    }
                    
                    //get results :)

                }
                catch (Exception ex)
                {
                    Console.WriteLine("No town names were affected. (* Invalid Country Query)");
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

        private static void PrintTownsById(int? countryId)
        {
            SqlConnection dbCon = new SqlConnection(@"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true");
            dbCon.Open();
            using (dbCon)
            {

                SqlCommand queryGetTownsById = new SqlCommand($@"SELECT Name FROM Towns WHERE CountryCode=@countryId", dbCon);
                queryGetTownsById.Parameters.AddWithValue("@countryId", countryId);
                SqlDataReader townsReader = queryGetTownsById.ExecuteReader();

                List<String> townsList = new List<string>();
                using (townsReader)
                {
                    using (townsReader)
                    {
                        while (townsReader.Read())
                        {
                            townsList.Add((string)townsReader["Name"]);
                        }
                    }
                }
                Console.WriteLine(String.Join(", ", townsList));
            }
        }
    }
}
