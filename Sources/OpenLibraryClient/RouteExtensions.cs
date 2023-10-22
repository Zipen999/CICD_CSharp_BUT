using System;
namespace OpenLibraryClient
{
	public static class RouteExtensions
	{
		public static string AddPagination(this string route, int index, int count)
        {
            if(index <= -1 || count<0)
            {
                return route;
            }
            string delimiter = route.Contains("?") ? "&" : "?";
            return $"{route}{delimiter}limit={count}&page={index+1}";
        }

        public static string AddSort(this string route, string sort)
        {
            string sortCriterium = sort switch
            {
                "new" => "new",
                "old" => "old",
                "random" => "random",
                "key" => "key",
                _ => null
            };
            if(sortCriterium == null) return route;

            string delimiter = route.Contains("?") ? "&" : "?";
            return $"{route}{delimiter}sort={sortCriterium}";
        }
	}
}

