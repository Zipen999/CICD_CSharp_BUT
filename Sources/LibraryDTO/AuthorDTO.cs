using System;
namespace LibraryDTO
{
	public class AuthorDTO
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string ImageSmall => $"https://covers.openlibrary.org/a/olid/{Id.Substring(Id.LastIndexOf("/"))}-S.jpg";
		public string ImageMedium => $"https://covers.openlibrary.org/a/olid/{Id.Substring(Id.LastIndexOf("/"))}-M.jpg";
		public string ImageLarge => $"https://covers.openlibrary.org/a/olid/{Id.Substring(Id.LastIndexOf("/"))}-L.jpg";
		public string Bio { get; set; }
		public List<string> AlternateNames { get; set; } = new List<string>();
		public List<LinkDTO> Links { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? DeathDate { get; set; }
	}
}

