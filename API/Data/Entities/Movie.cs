using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entities
{
    public class Movie : IEntity
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("Name")]
        [StringLength(250)]
        public string Name { get; set; }
        [Column("Genre")]
        [StringLength(100)]
        public string Genre { get; set; }
        [Column("UrlImg")]
        [StringLength(500)]
        public string UrlImg { get; set; }

        public ICollection<Actor> Actors { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
