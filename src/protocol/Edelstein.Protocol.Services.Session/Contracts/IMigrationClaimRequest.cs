﻿using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IMigrationClaimRequest : IIdentifiable<int>
{
    string Key { get; }
}
