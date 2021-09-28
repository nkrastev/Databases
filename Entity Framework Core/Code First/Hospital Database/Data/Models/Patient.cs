using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
            this.Diagnoses = new HashSet<Diagnose>();
            this.Visitations = new HashSet<Visitation>();
        }
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [MaxLength(80)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        //collection diagnoses, medicaments, visitations
        public ICollection<PatientMedicament> Prescriptions { get; set; }
        public ICollection<Diagnose> Diagnoses { get; set; }
        public ICollection<Visitation> Visitations { get; set; }
    }
}
