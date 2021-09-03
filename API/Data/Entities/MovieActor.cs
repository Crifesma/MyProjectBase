using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Data.Entities
{
    public class MovieActor : IEntity
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("PublicationDate")]
        public DateTime PublicationDate { get; set; }
        [Column("MovieId")]
        public int MovieId { get; set; }
        [Column("ActorId")]
        public int ActorId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Actor Actor { get; set; }
    }
}
