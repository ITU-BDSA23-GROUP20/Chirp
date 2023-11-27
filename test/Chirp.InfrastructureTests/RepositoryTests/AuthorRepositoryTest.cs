using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Test_Utilities;

namespace Chirp.InfrastructureTest.RepositoryTests;

public class AuthorRepositoryTest
{

    private readonly ChirpDbContext context;

    public AuthorRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();
    }

    [Fact]
    public void GetAuthorByName_ShouldReturnCorrectAuthorDTO()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }
      
            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        string theAuthorName = theAuthor.UserName;
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorByName(theAuthorName);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);

    }

    [Fact]
    public void GetCheepsByAuthor_ShouldReturnCorrectCheeps()
    {

        //Arrange
        var authorRepository = new AuthorRepository(context);

        Author author1 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor",
            Email = "mock@email.com",
            Cheeps = new List<Cheep>()
        };
        Cheep cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author1.Id,
            Text = "TestCheep",
            Author = author1
        };

        context.Users.Add(author1);
        context.Cheeps.Add(cheep1);

        context.SaveChanges();

        //Act
        ICollection<Cheep> expectedCheep = new List<Cheep>();
        expectedCheep.Add(cheep1);

        ICollection<Cheep> returnedCheep = authorRepository.GetCheepsByAuthor(author1.UserName, 0);

        //Assert
        Assert.Equal(expectedCheep, returnedCheep);
    }

    [Fact]
    public void addAuthor_ShouldAddAuthorToDatabase()
    {
        //Arrange
        var authorRepository = new AuthorRepository(context);
        
        Author authorDto1 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor",
            Email = "mock@email.com"
        };
        
        context.Users.Add(authorDto1);
        context.SaveChanges();
        
        // Note: authorDto2 is not yet added to context
        Author authorDto2 = new Author
        {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor2",
            Email = "TestEmail2@test.com"
        };

        // Act
        int initialAuthorCount = context.Users.Count();
        
        authorRepository.AddAuthor(authorDto2);

        context.SaveChanges();

        int updatedAuthorCount = context.Users.Count();

        // Assert
        Assert.Equal(initialAuthorCount + 1, updatedAuthorCount);
    }

    [Fact]
    public void GetAuthorById_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorById(theAuthor.Id);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }

    [Fact]
    public void GetAuthorByEmail_ShouldReturnCorrectAuthor()
    {
        // Arrange
        var authorRepository = new AuthorRepository(context);
        
        // Created to test for the correct author
        Author theAuthor = null;
        
        // Create 3 authors
        for (int i = 0; i < 3; i++)
        {
            Author authorDto = new Author
            {
                Id = Guid.NewGuid(),
                UserName = "TestAuthor" + i,
                Email = "mock" + i + "@email.com"
            };
            
            // Set theAuthor to the middle author
            if (i == 1)
            {
                theAuthor = authorDto;
            }

            context.Users.Add(authorDto);
        }

        context.SaveChanges();

        //Act
        Author expectedAuthor = theAuthor;
        Author returnedAuthor = authorRepository.GetAuthorByEmail(theAuthor.Email);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
}