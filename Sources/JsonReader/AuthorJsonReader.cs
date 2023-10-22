using System;
using LibraryDTO;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace JsonReader
{
	public static class AuthorJsonReader
	{
        public static AuthorDTO ReadAuthor(string json)
        {
            JObject o = JObject.Parse(json);
            string bioTokenAsString = null;
            if(o.TryGetValue("bio", out JToken? bioToken))
            {
                if(bioToken.Type == JTokenType.String)
                {
                    bioTokenAsString = (string)bioToken;
                }
                else
                {
                    var bioTokenValue = o["bio"]?["value"];
                    bioTokenAsString = (string)bioTokenValue;
                }
            }

            AuthorDTO author = new AuthorDTO
            {
                Id = (string)o["key"],
                Name = (string)o["name"],
                Bio = bioTokenAsString,
                BirthDate = o.TryGetValue("birth_date", out JToken? bd) ? ReadDate((string)o["birth_date"]) : null,
                DeathDate = o.TryGetValue("death_date", out JToken? dd) ? ReadDate((string)o["death_date"]) : null,
                Links = o.TryGetValue("links", out JToken? links) ? links.Select(l => new LinkDTO { Title = (string)l["title"], Url = (string)l["url"] }).ToList() : new List<LinkDTO>(),
                AlternateNames = o.TryGetValue("alternate_names", out JToken? altNames) ? altNames.Select(alt => (string)alt).ToList() : new List<string?>()
            };
            return author;
        }

        public static DateTime? ReadDate(string dateInJson)
        {
            if(dateInJson == null) return null;

            List<Tuple<string, CultureInfo>> pubDateFormat =new List<Tuple<string, CultureInfo>>()
            {
                Tuple.Create("d MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR")),
                Tuple.Create("d MMMM yyyy", CultureInfo.InvariantCulture),
                Tuple.Create("MMM dd, yyyy", CultureInfo.InvariantCulture)
            };

            DateTime? publishDate = null;
            foreach(var format in pubDateFormat)
            {
                if(DateTime.TryParseExact(dateInJson, format.Item1, format.Item2, DateTimeStyles.None, out DateTime readDate))
                {
                    publishDate = readDate;
                    break;
                }
            }
            if(!publishDate.HasValue && int.TryParse(dateInJson, out int year))
            {
                publishDate = new DateTime(year, 12, 31);
            }
            return publishDate;
        }

        public static Tuple<long, IEnumerable<AuthorDTO>> GetAuthorsByName(string json)
        {
            JObject o = JObject.Parse(json);
            long numFound = (long)o["numFound"];
            var authors = o["docs"].Select(doc => new AuthorDTO
            {
                Id = $"/authors/{(string)doc["key"]}",
                Name = (string)doc["name"],
            });
            return Tuple.Create(numFound, authors);
        }
	}
}

