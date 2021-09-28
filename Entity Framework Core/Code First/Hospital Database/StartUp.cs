using P01_HospitalDatabase.Data;
using System.Linq;

namespace P01_HospitalDatabase 
{
    class StartUp
    {
        static void Main()
        {
            var context = new HospitalContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var patientsCatalogue = context.Patients.ToList();
        }
    }
}
