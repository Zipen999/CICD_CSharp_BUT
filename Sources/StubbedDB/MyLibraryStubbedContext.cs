using Microsoft.EntityFrameworkCore;
using MyLibraryDB;
using MyLibraryEntities;
using static System.Reflection.Metadata.BlobBuilder;

namespace StubbedDB;

public class MyLibraryStubbedContext : MyLibraryContext
{
    public MyLibraryStubbedContext() : base() { }

    public MyLibraryStubbedContext(string dbPlatformPath)
        :base(dbPlatformPath)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AuthorEntity>().HasData(
            Json2Data.ToAuthorsData());
        modelBuilder.Entity<LinkEntity>().HasData(
            Json2Data.ToLinksData());
        modelBuilder.Entity<WorkEntity>().HasData(
            Json2Data.ToWorksData());
        modelBuilder.Entity("AuthorEntityWorkEntity").HasData(
            Json2Data.ToAuthorsWorksData());
        modelBuilder.Entity<BookEntity>().HasData(
            Json2Data.ToBooksData());
        modelBuilder.Entity("BookEntityWorkEntity").HasData(
            Json2Data.ToBooksWorksData());
        modelBuilder.Entity<ContributorEntity>().HasData(
            Json2Data.ToContributorsData());
        modelBuilder.Entity("AuthorEntityBookEntity").HasData(
            Json2Data.ToAuthorsBooksData());
    }
}

