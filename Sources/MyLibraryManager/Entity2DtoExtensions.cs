using System;
using LibraryDTO;
using MyLibraryEntities;

namespace MyLibraryManager
{
	public static class Entity2DtoExtensions
	{
		public static LibraryDTO.Languages ToDto(this MyLibraryEntities.Languages lang)
		{
			return Enum.GetValues<LibraryDTO.Languages>().SingleOrDefault(v =>
					Enum.GetName<LibraryDTO.Languages>(v) == Enum.GetName<MyLibraryEntities.Languages>(lang));
		}

		public static AuthorDTO ToDto(this AuthorEntity entity)
		{
			return new AuthorDTO
			{
				Id = entity.Id,
				AlternateNames = entity.AlternateNames != null ? new List<string>(entity.AlternateNames.Where(an => an != null)) : new List<string>(),
				Bio = entity.Bio,
				BirthDate = entity.BirthDate,
				DeathDate = entity.DeathDate,
				Name = entity.Name,
				Links = entity.Links != null ? new List<LinkDTO>(entity.Links.Where(l => l != null).ToDtos()) : new List<LinkDTO>(),
			};
		}

		public static IEnumerable<AuthorDTO> ToDtos(this IEnumerable<AuthorEntity> entities)
			=> entities.Select(a => a.ToDto());

		public static LinkDTO ToDto(this LinkEntity entity)
			=> new LinkDTO { Title = entity.Title, Url = entity.Url };

		public static IEnumerable<LinkDTO> ToDtos(this IEnumerable<LinkEntity> entities)
			=> entities.Select(l => l.ToDto());

		public static WorkDTO ToDto(this WorkEntity entity)
		{
			return new WorkDTO
			{
				Id = entity.Id,
				Description = entity.Description,
				Ratings = entity.RatingsAverage != null ?
							new RatingsDTO { Average = entity.RatingsAverage.Value, Count = entity.RatingsCount.Value } : null,
				Subjects = new List<string>(entity.Subjects),
				Title = entity.Title,
				Authors = new List<AuthorDTO>(entity.Authors.ToDtos()),
			};
		}

		public static IEnumerable<WorkDTO> ToDtos(this IEnumerable<WorkEntity> entities)
			=> entities.Select(w => w.ToDto());

		public static ContributorDTO ToDto(this ContributorEntity entity)
			=> new ContributorDTO { Name = entity.Name, Role = entity.Role };

		public static IEnumerable<ContributorDTO> ToDtos(this IEnumerable<ContributorEntity> entities)
			=> entities.Select(l => l.ToDto());

		public static BookDTO ToDto(this BookEntity entity)
		{
			return new BookDTO
			{
				Id = entity.Id,
				Authors = entity.Authors != null ? new List<AuthorDTO>(entity.Authors.ToDtos()) : null,
				Contributors = entity.Contributors != null ? new List<ContributorDTO>(entity.Contributors.ToDtos()) : null,
				Format = entity.Format,
				ISBN13 = entity.ISBN13,
				Language = entity.Language.ToDto(),
				NbPages = entity.NbPages,
				PublishDate = entity.PublishDate,
				Publishers = new List<string>(entity.Publishers),
				Series = new List<string>(entity.Series),
				Title = entity.Title,
				Works = new List<WorkDTO>(entity.Works.ToDtos()),
			};
		}

		public static IEnumerable<BookDTO> ToDtos(this IEnumerable<BookEntity> entities)
			=> entities.Select(b => b.ToDto());
	}
}

