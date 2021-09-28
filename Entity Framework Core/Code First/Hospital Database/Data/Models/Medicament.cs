using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Medicament
    {
        public Medicament()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
        }
        [Key]
        public int MedicamentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //JUDGE test 2
        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
