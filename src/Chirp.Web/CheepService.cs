using System.Globalization;
using Chirp.Core.Entities;
using Chirp.Core.Repository;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Repository;
using Chirp.Razor;
using Microsoft.EntityFrameworkCore;

public record CheepViewModel(string Author, string Message, string Timestamp);


public interface ICheepService
{
    public ICollection<CheepViewModel> GetCheeps(int page);
    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    
    private readonly IAuthorRepository _authorRepository;
    private readonly ICheepRepository _cheepRepository;

    public CheepService(ChirpDbContext db, ICheepRepository cheepRepositoryRepository, IAuthorRepository authorRepositoryRepository)
    {
        _cheepRepository = cheepRepositoryRepository;
        _authorRepository = authorRepositoryRepository;
    }
    
    public ICollection<CheepViewModel> GetCheeps(int page)
    {
        ICollection<CheepDTO> cheepDtos = _cheepRepository.GetCheepsByPage(page);
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (CheepDTO cheepDto in cheepDtos)
        {
            cheeps.Add(new CheepViewModel(cheepDto.AuthorDto.Name, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
    

    public ICollection<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        ICollection<CheepDTO> cheepDtos = _authorRepository.GetCheepsByAuthor(author, page);
        ICollection<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (CheepDTO cheepDto in cheepDtos)
        {
            cheeps.Add(new CheepViewModel(cheepDto.AuthorDto.Name, cheepDto.Text, cheepDto.TimeStamp.ToString(CultureInfo.InvariantCulture)));
        }
        
        return cheeps;
    }
}