using System;
using LibraryDTO;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace JsonReader
{
	public static class WorkJsonReader
	{
		public static WorkDTO ReadWork(string json, string ratingsJson)
        {
            JObject o = JObject.Parse(json);
            JObject r = JObject.Parse(ratingsJson);
            var ratingsDto = new RatingsDTO();
            if(r["summary"]["average"].Type != JTokenType.Float)
            {
                ratingsDto.Average = -1;
                ratingsDto.Count = 0;
            }
            else
            {
                ratingsDto.Average = (float)r["summary"]["average"];
                ratingsDto.Count = (int)r["summary"]["count"];
            }

            string description = null;
            if(o.TryGetValue("description", out JToken? descr))
            {
                if(descr.Type == JTokenType.String)
                {
                    description = (string)descr;
                }
                else
                {
                    if (descr["value"].Type == JTokenType.String)
                    {
                        description = (string)descr["value"];
                    }
                }
            }

            WorkDTO work = new WorkDTO
            {
                Id = (string)o["key"],
                Title = (string)o["title"],
                Authors = o.TryGetValue("authors", out JToken? authors) ? authors.Select(a => new AuthorDTO { Id = (string)a["author"]["key"] }).ToList() : new List<AuthorDTO>(),
                Description = description,
                Subjects = o.TryGetValue("subjects", out JToken? subjects) ? subjects.Select(s => (string)s).ToList() : new List<string>(),
                Ratings = ratingsDto
            };
            return work;
        }
	}
}

