using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class SessionRegistryService : ISessionRegistryService
    {
        public Task<UpdateSessionResult> UpdateSession(UpdateSessionRequest request) { throw new NotImplementedException(); }
        public Task<DescribeSessionResult> DescribeSessionByAccount(DescribeSessionByAccountRequest request) { throw new NotImplementedException(); }
        public Task<DescribeSessionResult> DescribeSessionByCharacter(DescribeSessionByCharacterRequest request) { throw new NotImplementedException(); }
    }
}
