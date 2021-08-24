using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Services
{
    public class SessionRegistry : ISessionRegistry
    {
        private static readonly string SessionScope = "sessions";
        private static readonly string SessionAccountScope = $"{SessionScope}:account";
        private static readonly string SessionCharacterScope = $"{SessionScope}:character";
        private static readonly TimeSpan SessionTimeoutDuration = TimeSpan.FromMinutes(2);

        private readonly ICache _sessionAccountCache;
        private readonly ICache _sessionCharacterCache;

        public SessionRegistry(ICache cache)
        {
            _sessionAccountCache = new ScopedCache(SessionAccountScope, cache);
            _sessionCharacterCache = new ScopedCache(SessionCharacterScope, cache);
        }

        public async Task<UpdateSessionResponse> Update(UpdateSessionRequest request)
        {
            var session = request.Session;
            var timeout = DateTime.UtcNow.Add(SessionTimeoutDuration);

            await _sessionAccountCache.Set(session.Account.ToString(), session, timeout);
            if (session.Character.HasValue)
                await _sessionCharacterCache.Set(session.Character.Value.ToString(), session, timeout);

            return new UpdateSessionResponse { Result = SessionRegistryResult.Ok };
        }

        public async Task<DescribeSessionByAccountResponse> DescribeByAccount(DescribeSessionByAccountRequest request)
        {
            var account = request.Account;
            var session = await _sessionAccountCache.Get<SessionContract>(account.ToString());

            return new DescribeSessionByAccountResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session ?? new SessionContract
                {
                    Account = account,
                    State = SessionState.Offline
                }
            };
        }

        public async Task<DescribeSessionByCharacterResponse> DescribeByCharacter(DescribeSessionByCharacterRequest request) {

            var character = request.Character;
            var session = await _sessionCharacterCache.Get<SessionContract>(character.ToString());

            return new DescribeSessionByCharacterResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session ?? new SessionContract
                {
                    Character = character,
                    State = SessionState.Offline
                }
            };
        }
    }
}
