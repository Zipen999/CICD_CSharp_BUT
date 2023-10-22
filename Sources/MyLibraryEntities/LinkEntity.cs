using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryEntities
{
	public class LinkEntity
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }

		[ForeignKey("AuthorId")]
		public AuthorEntity Author { get; set; }

		public string AuthorId { get; set; }
	}
}

