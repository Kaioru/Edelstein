using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Social.Memo.Events;
using Edelstein.Database;
using Edelstein.Entities.Characters;
using Foundatio.Caching;
using Foundatio.Lock;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public class SocialMemoManager : ISocialMemoManager
    {
        private const string MemoLockKey = "lock:memo";

        private readonly INode _node;
        private readonly IDataStore _store;
        private readonly ILockProvider _lockProvider;
        private readonly ICacheClient _characterCache;

        public SocialMemoManager(
            INode node,
            IDataStore store,
            ILockProvider lockProvider,
            ICacheClient cache
        )
        {
            _node = node;
            _store = store;
            _lockProvider = lockProvider;
            _characterCache = new ScopedCacheClient(cache, MigrationScopes.StateCharacter);
        }

        private async Task Lock(Func<Task> func)
        {
            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var @lock =
                await _lockProvider.AcquireAsync(
                    MemoLockKey,
                    cancellationToken: token.Token
                );

            if (@lock == null)
                throw new MemoException("Request timed out");

            try
            {
                await func.Invoke();
            }
            catch (Exception e)
            {
                throw new MemoException(e.Message);
            }
            finally
            {
                await @lock.ReleaseAsync();
            }
        }

        private async Task<T> Lock<T>(Func<Task<T>> func)
        {
            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var @lock =
                await _lockProvider.AcquireAsync(
                    MemoLockKey,
                    cancellationToken: token.Token
                );

            if (@lock == null)
                throw new MemoException("Request timed out");

            try
            {
                return await func.Invoke();
            }
            catch (Exception e)
            {
                throw new MemoException(e.Message);
            }
            finally
            {
                await @lock.ReleaseAsync();
            }
        }

        private async Task SendMessage<T>(int characterID, T message) where T : class
        {
            var value = await _characterCache.GetAsync<INodeState>(characterID.ToString());

            if (value.HasValue)
                await Task.WhenAll((await _node.GetPeers())
                    .Where(p => value.Value.Name == p.State.Name)
                    .Select(p => p.SendMessage<T>(message)));
        }

        private async Task<ICollection<ISocialMemo>> LoadInner(Character character)
        {
            using var store = _store.StartSession();
            var records = store
                .Query<Entities.Social.Memo>()
                .Where(m => m.CharacterID == character.ID)
                .ToImmutableList()
                .Select<Entities.Social.Memo, ISocialMemo>(m => new SocialMemo(this, m))
                .ToImmutableList();
            return records;
        }

        private async Task SendInner(
            int characterID,
            string sender,
            string content,
            byte flag = 0,
            DateTime? dateSent = null
        )
        {
            using var store = _store.StartSession();
            var memo = new Entities.Social.Memo
            {
                CharacterID = characterID,
                Sender = sender,
                Content = content,
                DateSent = dateSent ?? DateTime.UtcNow,
                Flag = flag
            };

            await store.InsertAsync(memo);
            await SendMessage(characterID, new MemoReceiveEvent(characterID, memo));
        }

        private async Task DeleteInner(ISocialMemo memo)
        {
            using var store = _store.StartSession();
            await store.DeleteAsync<Entities.Social.Memo>(memo.ID);
        }


        public async Task BulkDeleteInner(ICollection<ISocialMemo> memos)
        {
            using var store = _store.StartBatch();

            await Task.WhenAll(memos.Select(m => store.DeleteAsync<Entities.Social.Memo>(m.ID)));
            await store.SaveChangesAsync();
        }

        public Task<ICollection<ISocialMemo>> Load(Character character)
            => Lock(() => LoadInner(character));

        public Task Send(int characterID, string sender, string content, byte flag = 0, DateTime? dateSent = null)
            => Lock(() => SendInner(characterID, sender, content, flag, dateSent));

        public Task Delete(ISocialMemo memo)
            => Lock(() => DeleteInner(memo));

        public Task BulkDelete(ICollection<ISocialMemo> memos)
            => Lock(() => BulkDeleteInner(memos));
    }
}