using System;
namespace MyLibraryEntities
{
	public class BookEntity
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public ICollection<string> Publishers { get; set; } = new List<string>();
		public DateTime PublishDate { get; set; }
		public string ISBN13 { get; set; }
		public ICollection<string> Series { get; set; } = new List<string>();
		public int NbPages { get; set; }
		public string? Format { get; set; }
		public Languages Language { get; set; }
		public List<ContributorEntity> Contributors { get; set; }
		public List<WorkEntity> Works  { get; } = new ();
		public List<AuthorEntity> Authors  { get; } = new ();
		public string SmallImage { get; set; }
		public string MediumImage { get; set; }
		public string LargeImage { get; set; }
	}
}

