using Chirp.Razor.Models;

namespace Chirp.Razor;

public class CheepRepository : ICheepRepository
{
    // use chirpDbContext
    private ChirpDbContext db;
    private const int PageSize = 32;

    public IEnumerable<CheepViewModel> GetCheepsByPage(int page)
    {
        //Use EF to get the specified page of cheeps from the database
        List<CheepViewModel> cheeps = db.Cheeps
            .OrderByDescending(c => c.CheepId)
            .Skip(PageSize * page)
            .Take(PageSize)
            .Select(c => new CheepViewModel(GetAuthorById(c.AuthorId), c.Message, c.Timestamp))
            .ToList();
        return cheeps;
    }

    public void DeleteCheepById(int cheepId)
    {
        throw new NotImplementedException();
    }

    public void AddCheep(Cheep cheep)
    {
        throw new NotImplementedException();
    }
    
    private String GetAuthorById(int authorId)
    {
        return db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
    }
}