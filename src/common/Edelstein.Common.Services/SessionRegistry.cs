using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Foundatio.Caching;

namespace Edelstein.Common.Services
{
    public class SessionRegistry : ISessionRegistry
    {
        private static readonly string SessionScope = "sessions";
        private static readonly string SessionAccountScope = $"{SessionScope}:account";
        private static readonly string SessionCharacterScope = $"{SessionScope}:character";
        private static readonly TimeSpan SessionTimeoutDuration = TimeSpan.FromMinutes(2);

        private readonly ICacheClient _sessionAccountCache;
        private readonly ICacheClient _sessionCharacterCache;

        public SessionRegistry(ICacheClient cache)
        {
            _sessionAccountCache = new ScopedCacheClient(cache, SessionAccountScope);
            _sessionCharacterCache = new ScopedCacheClient(cache, SessionCharacterScope);
        }

        public async Task<UpdateSessionResponse> Update(UpdateSessionRequest request)
        {
            var session = request.Session;
            var timeout = DateTime.UtcNow.Add(SessionTimeoutDuration);

            await _sessionAccountCache.SetAsync(session.Account.ToString(), session, timeout);
            if (session.Character.HasValue)
                await _sessionCharacterCache.SetAsync(session.Character.Value.ToString(), session, timeout);

            return new UpdateSessionResponse { Result = SessionRegistryResult.Ok };
        }

        public async Task<DescribeSessionByAccountResponse> DescribeByAccount(DescribeSessionByAccountRequest request)
        {
            var account = request.Account;
            var session = await _sessionAccountCache.GetAsync<SessionContract>(account.ToString());

            return new DescribeSessionByAccountResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session.HasValue ? session.Value : new SessionContract
                {
                    Account = account,
                    State = SessionState.Offline
                }
            };
        }

        public async Task<DescribeSessionByCharacterResponse> DescribeByCharacter(DescribeSessionByCharacterRequest request)
        {

            var character = request.Character;
            var session = await _sessionCharacterCache.GetAsync<SessionContract>(character.ToString());

            return new DescribeSessionByCharacterResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session.HasValue ? session.Value : new SessionContract
                {
                    Character = character,
                    State = SessionState.Offline
                }
            };
        }
    }
}
