using Chirp.Core.Entities;

namespace Chirp.Core.Repository;

public interface IAuthorRepository
{
    public void AddAuthor(Author authorDto);

    
    public Author GetAuthorById(Guid authorId);
    public Task<Author?> GetAuthorByIdAsync(Guid authorId);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    


    public ICollection<Cheep> GetCheepsByAuthor(Guid authorId, int page);
    public ICollection<Cheep> GetCheepsByAuthorAndFollowed(Guid authorId, int page);
    
    
    public int GetCheepCountByAuthor(Guid authorId);
    public int GetCheepCountByAuthorAndFollowed(Guid authorId);
    
    
    public int GetPageCountByAuthor(Guid authorId);
    public int GetPageCountByAuthorAndFollowed(Guid authorId);


    public ICollection<Author?> GetFollowersById(Guid authorId);
    public ICollection<Author?> GetFollowingById(Guid authorId);
    

    public Task AddFollow(Author? followingAuthor, Author? followedAuthor);
    public Task RemoveFollow(Author? followingAuthor, Author? followedAuthor);
    
    
    public Task DeleteUserById(Guid authorId);
    
    public Task DeleteCheepsByAuthorId(Guid authorId);
    
    public Task RemoveAllFollowersByAuthorId(Guid id);
        
    public Task RemoveUserById(Guid id);

    public Task RemoveReactionsByAuthorId(Guid id);
    public Task SaveContextAsync();
}