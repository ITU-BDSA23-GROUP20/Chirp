using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test_Utilities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Timer = System.Timers.Timer;

namespace Chirp.InfrastructureTest.RepositoryTests;

public class AuthorRepositoryTest
{

    private readonly ChirpDbContext context;
    private readonly IAuthorRepository _authorRepository;

    private Author _author1;
    private Author _author2;
    private Author _author3;
    private Cheep _cheep1;
    private Cheep _cheep2;
    private Cheep _cheep3;

    public AuthorRepositoryTest()
    {
        context = SqliteInMemoryBuilder.GetContext();

        _authorRepository = new AuthorRepository(context);
        
        _author1 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor1",
            Email = "mock1@email.com"
        };
        _author2 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor2",
            Email = "mock2@email.com"
        };
        _author3 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor3",
            Email = "mock3@email.com"
        };
        
        _cheep1 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author1.Id,
            Text = "TestCheep by author 1",
            Author = _author1
        };
        
        _cheep2 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author2.Id,
            Text = "TestCheep by author 2",
            Author = _author2
        };
        
        _cheep3 = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author3.Id,
            Text = "TestCheep by author 3",
            Author = _author3
        };

        context.Add(_author1);
        context.Add(_author2);
        context.Add(_author3);

        context.Add(_cheep1);
        context.Add(_cheep2);
        context.Add(_cheep3);

        context.SaveChanges();
    }
    
    
    // ----- Add Author Methods ----- //
    [Fact]
    public void addAuthor_ShouldAddAuthorToDatabase()
    {
        // Act
        int initialAuthorCount = context.Users.Count();
        
        Author author4 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor4",
            Email = "mock4@email.com"
        };
        
        _authorRepository.AddAuthor(author4);

        int updatedAuthorCount = context.Users.Count();

        // Assert
        Assert.Equal(initialAuthorCount + 1, updatedAuthorCount);
    }
    
    
    // ----- Get Author Methods ----- //
    [Fact]
    public void GetAuthorById_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author returnedAuthor = _authorRepository.GetAuthorById(_author2.Id);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public void GetAuthorByIdAsync_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author? returnedAuthor = _authorRepository.GetAuthorByIdAsync(_author2.Id).Result;

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }
    
    [Fact]
    public void GetAuthorByName_ShouldReturnCorrectAuthorDTO()
    {
        //Arange
        Author expectedAuthor = _author2;
        
        //Act
        Author returnedAuthor = _authorRepository.GetAuthorByName(_author2.UserName);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);

    }
    
    [Fact]
    public void GetAuthorByEmail_ShouldReturnCorrectAuthor()
    {
        //Act
        Author expectedAuthor = _author2;
        Author returnedAuthor = _authorRepository.GetAuthorByEmail(_author2.Email);

        //Assert
        Assert.Equal(expectedAuthor, returnedAuthor);
    }

    
    // ----- Get Cheeps By Author and Page Methods ----- //
    [Fact]
    public void GetCheepsByAuthor_ShouldReturnCorrectCheeps()
    {
        //Act
        ICollection<Cheep> expectedCheep = new List<Cheep>();
        expectedCheep.Add(_cheep1);

        ICollection<Cheep> returnedCheep = _authorRepository.GetCheepsByAuthor(_author1.Id, 0);

        //Assert
        Assert.Equal(expectedCheep, returnedCheep);
    }
    
    // ----- Get Page and Cheep Count Methods ----- //
    [Fact]
    public void GetPageCountByAuthor_ShouldReturn1PageCountWhenCheepCountUnder32()
    {
        
        //Assert
        Assert.Equal(1, _authorRepository.GetPageCountByAuthor(_author1.Id));
        
    }

    [Fact]
    public void GetPageCountByAuthor_ShouldReturn2PageCountWhenCheepCountOver32()
    {
        //Arrange
        for (int i = 0; i < 33; i++)
        {
            Cheep cheep = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = _author1.Id,
                Text = "TestCheep by author 1",
                Author = _author1
            };
            context.Add(cheep);
        }
        context.SaveChanges();
        
        //Assert
        Assert.Equal(2, _authorRepository.GetPageCountByAuthor(_author1.Id));
        
    }
    // ----- Get Followers and Following Methods ----- //
    [Fact]
    public async void GetFollowersByAuthorId_ShouldReturnCorrectFollowers()
    {
        await _authorRepository.AddFollow(_author2, _author1);
        await _authorRepository.AddFollow(_author3, _author1);

        ICollection<Author?> expectedFollowers = new List<Author?>();
        expectedFollowers.Add(_author2);
        expectedFollowers.Add(_author3);

        ICollection<Author?> returnedFollowers = _authorRepository.GetFollowersById(_author1.Id);

        //Assert
        Assert.Equal(expectedFollowers, returnedFollowers);
    }
    
    [Fact]
    public async void GetFollowingByAuthorId_ShouldReturnCorrectFollowing()
    {
        await _authorRepository.AddFollow(_author1, _author2);
        await _authorRepository.AddFollow(_author1, _author3);

        ICollection<Author?> expectedFollowing = new List<Author?>();
        expectedFollowing.Add(_author2);
        expectedFollowing.Add(_author3);

        ICollection<Author?> returnedFollowing = _authorRepository.GetFollowingById(_author1.Id);

        //Assert
        Assert.Equal(expectedFollowing, returnedFollowing);
    }
    
    
    // ----- Add/Remove Follow Methods ----- //
    //TODO move addFollow functionality and tests to FollowRepository and FollowRepository Tests
    [Fact]
    public async void AddFollow_ShouldAddFollowingToAuthor()
    {
        //Act
        await _authorRepository.AddFollow(_author1, _author2);
        
        //Assert
        Assert.True(_author1.Following.FirstOrDefault()!.FollowedAuthorId == _author2.Id);
        Assert.True(_author2.Followers.FirstOrDefault()!.FollowingAuthorId == _author1.Id);
    }

    [Fact]
    public async void RemoveFollow_ShouldRemoveFollowingFromAuthor()
    {
        await _authorRepository.AddFollow(_author1, _author2);
        
        Assert.Equal(_author2.Id, _author1.Following.FirstOrDefault().FollowedAuthor.Id);
        Assert.Equal(_author1.Id, _author2.Followers.FirstOrDefault().FollowingAuthor.Id);

        await _authorRepository.RemoveFollow(_author1, _author2);

        //Assert
        Assert.True(_author1.Followers.IsNullOrEmpty());
        Assert.True(_author2.Followers.IsNullOrEmpty());
    }

    
    // ----- Delete (All) Author Data Methods ----- //
   [Fact]
    public async void DeleteCheepsByAuthorId_ShouldRemoveAllCheepsByAuthor()
    {
        Assert.Equal(3, context.Cheeps.Count());
        Assert.Equal(1, _author1.Cheeps.Count);

        // Act
        await _authorRepository.DeleteCheepsByAuthorId(_author1.Id);

        context.SaveChanges();
        
        // Assert
        Assert.Empty(_author1.Cheeps);
        Assert.Equal(2, context.Cheeps.Count());
    }
    
    [Fact]
    public async void RemoveAllFollowersByAuthorId_ShouldRemoveAllFollowersByAuthor()
    {
        await _authorRepository.AddFollow(_author1, _author2);
        await _authorRepository.AddFollow(_author1, _author3);
        
        Assert.Equal(2, _author1.Following.Count);
        Assert.Equal(1, _author2.Followers.Count);
        Assert.Equal(1, _author3.Followers.Count);

        // Act
        await _authorRepository.RemoveAllFollowersByAuthorId(_author1.Id);

        await context.SaveChangesAsync();
        
        // Assert
        Assert.Empty(_author1.Following);
        Assert.Empty(_author2.Followers);
        Assert.Empty(_author3.Followers);
    }
    
    [Fact]
    public async void RemoveUserById_ShouldRemoveUserById()
    {
        // Act
        await _authorRepository.RemoveUserById(_author1.Id);

        await context.SaveChangesAsync();
        
        // Assert
        Assert.Equal(2, context.Users.Count());
    }
    
    [Fact]
    public async void RemoveReactionsByAuthorId_ShouldRemoveReactionsByAuthor()
    {
        // Arrange
        Reaction reaction1 = new Reaction
        {
            CheepId = _cheep1.CheepId,
            AuthorId = _author2.Id,
            ReactionType = ReactionType.Like
        };
        
        context.Add(reaction1);
        await context.SaveChangesAsync();
        // Act
        await _authorRepository.RemoveReactionsByAuthorId(_author2.Id);
        await context.SaveChangesAsync();
        // Assert
        Assert.Equal(0, context.Reactions.Count());
        Assert.Null(_author1.Reactions);
        Assert.Empty(_cheep1.Reactions);
        
    }
    
    
    // ----- Save Context Method ----- //
    [Fact]
    public async void SaveContextAsync_ShouldSaveChanges()
    {
        //Act
        Author _author4 = new Author {
            Id = Guid.NewGuid(),
            UserName = "TestAuthor4",
            Email = "mock1@email.com"
        };
        context.Add(_author4);
        
        //Arrange
        _authorRepository.SaveContextAsync();
        //Assert
        Assert.Equal(4, context.Users.Count());
    }
}