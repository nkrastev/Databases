namespace MongoDataAccess.DataAccess
{
    using MongoDataAccess.Models;
    using MongoDB.Driver;
    using MongoDB.Driver.GeoJsonObjectModel;

    public class PetOwnerDataAccess
    {
        private const string ConnectionString = "mongodb://127.0.0.1:27017";        
        private const string DbName = "PetsAndOwners";
        private const string PetCollection = "pets";
        private const string OwnerCollection = "owners";

        // Generic method for Db connection to collection
        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DbName);
            return db.GetCollection<T>(collection);
        }

        // Return all db records from collection

        public async Task<List<Pet>> GetAllPets()
        {
            var petsCollection = ConnectToMongo<Pet>(PetCollection); 
            var results = await petsCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<Owner>> GetAllOwners()
        {
            var ownersCollection = ConnectToMongo<Owner>(OwnerCollection);
            var results = await ownersCollection.FindAsync(_ => true);
            return results.ToList();
        }

        // Creating and inserting record in DB
        public Task CreateOwner(Owner owner)
        {
            var ownerCollection = ConnectToMongo<Owner>(OwnerCollection);
            return ownerCollection.InsertOneAsync(owner);
        }

        public Task CreatePet(Pet pet)
        {
            var petsCollection = ConnectToMongo<Pet>(PetCollection);
            return petsCollection.InsertOneAsync(pet);
        }

        // Filtering and updating record
        public Task UpdateOwner(Owner owner)
        {
            var ownerCollection = ConnectToMongo<Owner>(OwnerCollection);
            var filter = Builders<Owner>.Filter.Eq("Id", owner.Id);
            return ownerCollection.ReplaceOneAsync(filter, owner, new ReplaceOptions { IsUpsert = true });
        }

        public Task UpdatePet(Pet pet)
        {
            var petCollection = ConnectToMongo<Pet>(PetCollection);
            var filter = Builders<Pet>.Filter.Eq("Id", pet.Id);
            return petCollection.ReplaceOneAsync(filter, pet, new ReplaceOptions { IsUpsert = true });
        }

        // Delete record from Database
        public Task DeleteOwner(Owner owner)
        {
            var ownerCollection = ConnectToMongo<Owner>(OwnerCollection);
            return ownerCollection.DeleteOneAsync(c => c.Id == owner.Id);
        }

        public Task DeletePet(Pet pet)
        {
            var petCollection = ConnectToMongo<Pet>(PetCollection);
            return petCollection.DeleteOneAsync(c => c.Id == pet.Id);
        }
    }
}
