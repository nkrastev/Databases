using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Diagnose
    {

        public int DiagnoseId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
              
        [MaxLength(250)]
        public string Comments { get; set; }

        public Patient Patient { get; set; }
        public int PatientId { get; set; }

    }
}
