using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryEntities;

public class ContributorEntity
{
	public string Id { get; set; }
    public string Name { get; set; }
	public string Role { get; set; }

    [ForeignKey("BookId")]
	public BookEntity Book { get; set; }

	public string BookId { get; set; }
}

