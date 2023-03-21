using MongoDataAccess.DataAccess;
using MongoDataAccess.Models;

PetOwnerDataAccess db = new PetOwnerDataAccess();

Pet petSharo = new Pet() { Name = "Sharo" };
Owner ownerOfSharo = new Owner() { Name = "Sharo's Owner", Pet = petSharo };

Pet petGosho = new Pet() { Name = "Gosho" };
Owner ownerOfGosho = new Owner() { Name = "Gosho's Owner", Pet = petGosho };

await db.CreatePet(petSharo);
await db.CreatePet(petGosho);

await db.CreateOwner(ownerOfGosho);
await db.CreateOwner(ownerOfSharo);

// Update
ownerOfSharo.Name = "Updated Name of Sharo's Owner";
await db.UpdateOwner(ownerOfSharo);

// Delete
await db.DeleteOwner(ownerOfGosho);