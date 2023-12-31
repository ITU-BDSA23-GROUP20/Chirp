//Test of cheep repository methods using Test_Utilites in-memory database

using Chirp.Core.Entities;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Test_Utilities;

namespace Chirp.InfrastructureTest.RepositoryTests;
public class CheepRepositoryTest{

    private readonly CheepRepository CheepRepository;
    private readonly ChirpDbContext db;

    private Author _author;

    public CheepRepositoryTest()
    {
        db = SqliteInMemoryBuilder.GetContext();
        CheepRepository = new CheepRepository(db);

        _author = new Author()
        {
            UserName = "TestAuthor", 
            Email = "mock@email.com" 
        };
        
        for(int i = 0; i < 34; i++)
        {

            Author authorDto = new Author
            { 
                UserName = "TestAuthor" + i, 
                Email = "mock" + i + "@email.com" 
            };
            
            Cheep cheepDto = new Cheep
            {
                CheepId = Guid.NewGuid(),
                AuthorId = authorDto.Id,
                Text = "TestCheep" + i,
                Author = authorDto
            };
            
            db.Users.Add(authorDto);
            db.Cheeps.Add(cheepDto);
        }

        db.SaveChanges();
    }

    [Fact]
    public void GetCheepsByPage_ShouldSkipFirst32Cheeps_ReturnXAmountOfCheeps()
    {
        //Act
        ICollection<Cheep> cheeps = CheepRepository.GetCheepsByPage(2);

        //Assert
        Assert.Equal(2, cheeps.Count);
    }

    [Fact]
    public void DeleteCheepById_ShouldOnlyDeleteSpecifiedCheep()
    {
        ICollection<Cheep> initialCheeps = CheepRepository.GetCheepsByPage(1);
        Cheep cheep = initialCheeps.First();
        Guid cheepId = cheep.CheepId;
        
        CheepRepository.DeleteCheepById(cheepId);

        ICollection<Cheep> updatedCheeps = CheepRepository.GetCheepsByPage(1);
        
        //Assert
        Assert.True(initialCheeps.Contains(cheep));
        Assert.False(updatedCheeps.Contains(cheep));

    }

    [Fact]
    public void addCheep_ShouldAddACheep()
    {
        Cheep cheepDto = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = _author.Id,
            Text = "TestCheep",
            TimeStamp = DateTime.Now,
            Author = _author
        };

        CheepRepository.AddCheep(cheepDto).Wait();

        ICollection<Cheep> updatedCheeps = CheepRepository.GetCheepsByPage(0);
        
        //Assert
        Assert.True(updatedCheeps.Contains(cheepDto));
    }
    
    [Fact]
    public void CreateCheepCreatesCheep()
    {
        CreateCheep createCheep = new CreateCheep(_author, "TestCheep");

        Cheep cheep = CheepRepository.AddCreateCheep(createCheep).Result;
        
        Assert.True(CheepRepository.GetCheepsByPage(0).Contains(cheep));
    }
}