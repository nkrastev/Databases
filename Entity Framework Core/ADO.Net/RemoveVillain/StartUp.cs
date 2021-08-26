using Microsoft.Data.SqlClient;
using System;

namespace RemoveVillain
{
    class StartUp
    {
        static void Main()
        {
            string connectionString = @"Server=DATA2\MSSQLSERVER01; Database=MinionsDB; Integrated Security=true";            
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                connection.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    int villainId = int.Parse(Console.ReadLine());

                    //Get Villian Name
                    command.CommandText = "SELECT Name FROM Villains WHERE Id=@villainId";
                    command.Parameters.AddWithValue("@villainId", villainId);
                    string villainName = (string)command.ExecuteScalar();

                    //Delete connections
                    command.CommandText = "DELETE FROM MinionsVillains WHERE VillainId=@villainId";
                    int deletedConnections=(int)command.ExecuteNonQuery();

                    //Delete Villain item
                    command.CommandText = "DELETE FROM Villains WHERE Id=@villainId";                    
                    int deletedItem=(int)command.ExecuteNonQuery();

                    if (deletedItem==0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        // Commit the transaction.
                        sqlTran.Commit();
                        Console.WriteLine($"{villainName} was deleted.");
                        Console.WriteLine($"{deletedConnections} minions were released.");
                    }
                    
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine("No such villain was found.");
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex);

                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine("No such villain was found.");
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }
    }
}
