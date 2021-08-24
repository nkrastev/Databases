using Microsoft.Data.SqlClient;
using System;

namespace AddMinion
{
    class StartUp
    {
        static void Main()
        {
            //input
            string connectionString = @"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true";
            bool isTownExisting = false;
            bool isVillainExisting = false;

            //Minion: <Name> <Age> <TownName>
            string[] minionInfo = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string townName = minionInfo[3];
            
            //Villain: <Name>
            string[] villainInfo = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainInfo[1];

            
            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();
            using (dbCon)
            {
                //check if town exists
                SqlCommand queryTownsExist = new SqlCommand(@"SELECT * FROM Towns WHERE Name=@townName", dbCon);
                queryTownsExist.Parameters.AddWithValue("@townName", townName);
                SqlDataReader reader = queryTownsExist.ExecuteReader();
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        isTownExisting = true;
                    }                    
                }
                if (!isTownExisting)
                {
                    //insert new town
                    SqlCommand queryInsertTown = new SqlCommand(@"INSERT INTO Towns (Name)VALUES (@newTownName)", dbCon);
                    queryInsertTown.Parameters.AddWithValue("@newTownName", townName);
                    queryInsertTown.ExecuteNonQuery();
                    Console.WriteLine($"Town {townName} was added to the database.");
                }
                //get the ID of the town
                SqlCommand queryGetIdOfTown = new SqlCommand(@"SELECT Id FROM Towns WHERE Name=@newTownName", dbCon);
                queryGetIdOfTown.Parameters.AddWithValue("@newTownName", townName);
                int townId = (int)queryGetIdOfTown.ExecuteScalar();

                Console.WriteLine($"Town Id {townId}");

                //check if villain exists  
                SqlCommand queryVillainExist = new SqlCommand(@"SELECT * FROM Villains WHERE Name=@villainName", dbCon);
                queryVillainExist.Parameters.AddWithValue("@villainName", villainName);
                reader = queryVillainExist.ExecuteReader();
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        isVillainExisting = true;
                    }                    
                }
                if (!isVillainExisting)
                {
                    //insert new villain with evilness factor 4 (save 1 query for the evilness factor :)
                    SqlCommand queryInsertVillain = new SqlCommand(@"INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, 4)", dbCon);
                    queryInsertVillain.Parameters.AddWithValue("@villainName", villainName);
                    queryInsertVillain.ExecuteNonQuery();
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                //get the ID of the villain
                SqlCommand queryGetIdOfVillain = new SqlCommand(@"SELECT Id FROM Villains WHERE Name=@villainName", dbCon);
                queryGetIdOfVillain.Parameters.AddWithValue("@villainName", villainName);
                int villainId = (int)queryGetIdOfVillain.ExecuteScalar();
                Console.WriteLine($"Villain Id {villainId}");

                //insert Minion, get Minion Id
                SqlCommand queryInsertMinion = new SqlCommand(@"INSERT INTO Minions (Name, Age, TownId) VALUES (@minionName, @minionAge, @townId)", dbCon);
                queryInsertMinion.Parameters.AddWithValue("@minionName", minionInfo[1]);
                queryInsertMinion.Parameters.AddWithValue("@minionAge", int.Parse(minionInfo[2]));
                queryInsertMinion.Parameters.AddWithValue("@townId", townId);
                queryInsertMinion.ExecuteNonQuery();

                SqlCommand queryGetMinionId = new SqlCommand(@"SELECT Id FROM Minions WHERE Name=@minionName AND Age=@minionAge AND TownId=@townId", dbCon);
                queryGetMinionId.Parameters.AddWithValue("@minionName", minionInfo[1]);
                queryGetMinionId.Parameters.AddWithValue("@minionAge", int.Parse(minionInfo[2]));
                queryGetMinionId.Parameters.AddWithValue("@townId", townId);
                int minionId = (int)queryGetMinionId.ExecuteScalar();

                //insert connection Minion Villain
                SqlCommand queryInsertConnection = new SqlCommand(@"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)", dbCon);
                queryInsertConnection.Parameters.AddWithValue("@minionId", minionId);
                queryInsertConnection.Parameters.AddWithValue("@villainId", villainId);
                queryInsertConnection.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionInfo[1]} to be minion of {villainName}.");
            }
            
            

        }

    }
}
