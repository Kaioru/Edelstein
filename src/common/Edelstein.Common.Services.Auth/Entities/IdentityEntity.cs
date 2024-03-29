﻿using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Auth.Entities;

public record IdentityEntity : IIdentifiable<int>
{
    public string Username { get; set; }
    public string Password { get; set; }
    
    public int ID { get; set; }
}
