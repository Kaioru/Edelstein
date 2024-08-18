using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Auth;

public class Identity : IRepositoryEntry<int>
{
    public int ID { get; set; }
    
    public required string Username { get; set; }
    public required string Password { get; set; }
}
