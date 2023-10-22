using System;
namespace LibraryDTO
{
	public class BookDTO
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public List<string> Publishers { get; set; } = new List<string>();
		public DateTime PublishDate { get; set; }
		public string ISBN13 { get; set; }
		public List<string> Series { get; set; } = new List<string>();
		public int NbPages { get; set; }
		public string Format { get; set; }
		public Languages Language { get; set; }
		public List<ContributorDTO> Contributors { get; set; }
		public string ImageSmall => $"https://covers.openlibrary.org/b/isbn/{ISBN13}-S.jpg";
		public string ImageMedium => $"https://covers.openlibrary.org/b/isbn/{ISBN13}-M.jpg";
		public string ImageLarge  => $"https://covers.openlibrary.org/b/isbn/{ISBN13}-L.jpg";
		public List<WorkDTO> Works { get; set; } = new List<WorkDTO>();
		public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
	}
}

