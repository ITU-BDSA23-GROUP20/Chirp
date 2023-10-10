using Chirp.Razor.Models;

namespace Chirp.Razor.Repository;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    
    public IEnumerable<CheepViewModel> GetCheepsByAuthor(Author author)
    {
        //Check that author has cheeps
        if (!db.Cheeps.Any(c => c.AuthorId == author.AuthorId))
        {
            throw new Exception("Author " + author.Name + " has no cheeps");
        }
        
        //Get cheeps by Author object
        return db.Cheeps
            .Where(c => c.AuthorId == author.AuthorId)
            .Select(c => new CheepViewModel(author.Name, c.Message, c.Timestamp))
            .ToList();
    }
    
    public String GetAuthorById(int authorId)
    {
        return db.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => a.Name)
            .FirstOrDefault()!;
    }
}