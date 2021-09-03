using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Data.Entities
{
    public class Actor : IEntity
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("Name")]
        [StringLength(250)]
        public string Name { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        public ICollection<Movie> Movies { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
