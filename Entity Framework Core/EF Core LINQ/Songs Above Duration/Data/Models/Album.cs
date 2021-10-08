using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate  { get; set; }

        //TODO calculated property (the sum of all song prices in the album) in the model builder!!!
        public decimal Price { get; set; }

        //къде пише че може да е нулабъл?
        public int? ProducerId { get; set; }
        public Producer? Producer { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}