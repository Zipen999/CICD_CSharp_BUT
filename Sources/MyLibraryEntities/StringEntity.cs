using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryEntities
{
	public class StringEntity
	{
		public string Id { get; set; }
		public string Value { get; set; }

		[ForeignKey("AuthorId")]
		public AuthorEntity Author { get; set; }

		public string AuthorId { get; set; }
	}
}

