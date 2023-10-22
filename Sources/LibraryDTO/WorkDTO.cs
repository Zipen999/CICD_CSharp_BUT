using System;
namespace LibraryDTO
{
	public class WorkDTO
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public List<string> Subjects { get; set; } = new List<string>();
		public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
		public RatingsDTO Ratings { get; set; }
	}
}

