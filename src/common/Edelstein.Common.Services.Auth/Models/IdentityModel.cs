using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Services.Auth.Models;

public record IdentityModel : IIdentifiable<int>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public int ID { get; set; }
}
