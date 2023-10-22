using System;
using StubbedDTO;
using System.Linq;
using LibraryDTO;
using MyLibraryEntities;

namespace StubbedDB
{
	public static class Json2Data
	{
		public static MyLibraryEntities.Languages ToEntity(this LibraryDTO.Languages lang)
		{
			return Enum.GetValues<MyLibraryEntities.Languages>().SingleOrDefault(v =>
					Enum.GetName<MyLibraryEntities.Languages>(v) == Enum.GetName<LibraryDTO.Languages>(lang));
		}

		public static IEnumerable<AuthorEntity> ToAuthorsData()
		{
			var temp = Stub.Authors.Select(a =>
			new AuthorEntity
			{
				Id = a.Id,
				Name = a.Name,
				Bio = a.Bio,
				BirthDate = a.BirthDate,
				DeathDate = a.DeathDate,
				SmallImage = $"https://covers.openlibrary.org/a/olid/{a.Id.Substring(a.Id.LastIndexOf("/"))}-S.jpg",
				MediumImage = $"https://covers.openlibrary.org/a/olid/{a.Id.Substring(a.Id.LastIndexOf("/"))}-M.jpg", 
				LargeImage = $"https://covers.openlibrary.org/a/olid/{a.Id.Substring(a.Id.LastIndexOf("/"))}-L.jpg",
				AlternateNames = a.AlternateNames
			});
			return temp;
		}

		public static IEnumerable<object> ToLinksData()
		{
			return Stub.Authors.Where(a => a.Links != null && a.Links.Count > 0)
				.SelectMany(a => a.Links, (author,link) => 
			new
			{
				Id = $"{author.Id.Substring(author.Id.LastIndexOf("/"))}-{author.Links.IndexOf(link)}",
				Title = link.Title,
				Url = link.Url,
				AuthorId = author.Id
			});
		}

		public static IEnumerable<object> ToWorksData()
		{
			return Stub.Works.Select(w =>
			new
			{
				Id = w.Id,
				Description = w.Description,
				RatingsAverage = w.Ratings?.Average,
				RatingsCount = w.Ratings?.Count,
				Title = w.Title,
				Subjects = w.Subjects
			});
		}

		public static IEnumerable<object> ToAuthorsWorksData()
		{
			return Stub.Works.SelectMany(w => w.Authors,
				(w, a) => new { AuthorsId = a.Id, WorksId = w.Id });
		}

		public static IEnumerable<object> ToBooksData()
		{
			return Stub.Books.Select(b =>
			new 
			{
				Publishers = b.Publishers,
				Title = b.Title,
				NbPages = b.NbPages,
				ISBN13 = b.ISBN13,
				Language = b.Language.ToEntity(),
				PublishDate = b.PublishDate,
				Id = b.Id,
				Series = b.Series,
				Format = b.Format,
				SmallImage = $"https://covers.openlibrary.org/b/isbn/{b.ISBN13}-S.jpg",
				MediumImage = $"https://covers.openlibrary.org/b/isbn/{b.ISBN13}-M.jpg",
				LargeImage = $"https://covers.openlibrary.org/b/isbn/{b.ISBN13}-l.jpg"
			});
		}

		public static IEnumerable<object> ToBooksWorksData()
		{
			return Stub.Books.SelectMany(b => b.Works,
				(b, w) => new {BooksId = b.Id, WorksId = w.Id});
		}

		public static IEnumerable<object> ToContributorsData()
		{
			return Stub.Books.SelectMany( b => b.Contributors,
				(b, c) => new
				{
					Id = $"{b.Id}-c{b.Contributors.IndexOf(c)}",
					BookId = b.Id,
					Name = c.Name,
					Role = c.Role
				});
		}

		public static IEnumerable<object> ToAuthorsBooksData()
		{
			var proj = (BookDTO b, AuthorDTO a) => new { AuthorsId = a.Id, BooksId = b.Id};
			var collec1 = Stub.Books.SelectMany(b => b.Authors, proj);
			var collec2 = Stub.Books.SelectMany(b => b.Works.SelectMany(w => w.Authors), proj);
			return collec1.Union(collec2);
		}
	}
}

