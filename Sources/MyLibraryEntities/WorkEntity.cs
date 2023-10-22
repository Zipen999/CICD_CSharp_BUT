using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryEntities
{
	public class WorkEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }
		public string? Description { get; set; }
		[Required]
		public string Title { get; set; }
		public ICollection<string>? Subjects { get; set; } = new List<string>();
		public List<AuthorEntity> Authors { get; } = new ();
		public List<BookEntity> Books { get; } = new ();
		//public RatingsEntity Ratings { get; set; }
		public float? RatingsAverage { get; set; }
		public int? RatingsCount { get; set; }
	}
}

