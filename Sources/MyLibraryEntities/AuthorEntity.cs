using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryEntities
{
	[Table("Authors")]
	public class AuthorEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string? Bio { get; set; }

		public ICollection<string>? AlternateNames { get; set; } = new List<string>();

		public string SmallImage { get; set; }
		public string MediumImage { get; set; }
		public string LargeImage { get; set; }

		public List<LinkEntity> Links { get; set; }

		[Column("BirthDate", TypeName = "date")]
		public DateTime? BirthDate { get; set; }

		[Column("DeathDate", TypeName = "date")]
		public DateTime? DeathDate { get; set; }

		public List<WorkEntity> Works { get; } = new ();
		public List<BookEntity> Books { get; } = new ();

	}
}