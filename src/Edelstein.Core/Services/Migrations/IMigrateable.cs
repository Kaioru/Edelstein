using System;
using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrateable
    {
        Task<bool> TryMigrateTo(
            ISocket socket, Character character, ServerServiceInfo to,
            Func<ServerServiceInfo, IPacket> getMigrationCommand
        );

        Task<bool> TryMigrateFrom(Character character, ServerServiceInfo current);
    }
}