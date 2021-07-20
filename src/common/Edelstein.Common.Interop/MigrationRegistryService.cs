using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class MigrationRegistryService : IMigrationRegistryService
    {
        public Task<MigrationRegistryResponse> Register(RegisterMigrationRequest request) { throw new NotImplementedException(); }
        public Task<MigrationRegistryResponse> Deregister(DeregisterMigrationRequest request) { throw new NotImplementedException(); }
        public Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request) { throw new NotImplementedException(); }
    }
}
