using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Service.Social.Logging;
using Edelstein.Service.Social.Services;
using MoreLinq;

namespace Edelstein.Service.Social.Managers
{
    public class RankingManager : ITickable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly SocialService _service;
        private DateTime NextUpdate { get; set; }

        public RankingManager(SocialService service)
        {
            _service = service;
            Reset();
            Rank();
        }

        public void Reset()
        {
            NextUpdate = DateTime.Now.Date.AddDays(1);
            Logger.Info($"Ranking is scheduled at {NextUpdate}");
        }

        public void Rank()
        {
            _service.Info.Worlds.ForEach(w =>
            {
                var watch = Stopwatch.StartNew();

                using (var store = _service.DataStore.OpenSession())
                using (var batch = store.Batch())
                {
                    var characters = store
                        .Query<AccountData>()
                        .Where(d => d.WorldID == w)
                        .ToList()
                        .SelectMany(d => store
                            .Query<Character>()
                            .Where(c => c.AccountDataID == d.ID)
                            .ToList())
                        .ToList();

                    var worldRanking = characters
                        .OrderBy(c => c.Level, OrderByDirection.Descending)
                        .Select((c, index) => new {CharacterID = c.ID, Rank = index + 1})
                        .ToDictionary(r => r.CharacterID, r => r.Rank);
                    var jobRanking = new Dictionary<int, int>();

                    characters
                        .GroupBy(c => c.Job % 1000 / 100)
                        .ForEach(job => job
                            .OrderBy(c => c.Level, OrderByDirection.Descending)
                            .Select((c, index) => new {CharacterID = c.ID, Rank = index + 1})
                            .ToDictionary(r => r.CharacterID, r => r.Rank)
                            .ForEach(kv => jobRanking.Add(kv.Key, kv.Value)));

                    characters.ForEach(c =>
                    {
                        var ranking = store
                            .Query<RankRecord>()
                            .FirstOrDefault(r => r.CharacterID == c.ID);

                        if (ranking == null)
                        {
                            ranking = new RankRecord
                            {
                                CharacterID = c.ID
                            };
                            batch.Insert(ranking);
                        }
                        else
                        {
                            ranking.WorldRankGap = ranking.WorldRank - worldRanking[c.ID];
                            ranking.JobRankGap = ranking.JobRank - jobRanking[c.ID];
                        }

                        ranking.WorldRank = worldRanking[c.ID];
                        ranking.JobRank = jobRanking[c.ID];

                        batch.Update(ranking);
                    });

                    batch.SaveChanges();
                    Logger.Info($"Ranked {characters.Count} characters (world {w}) in {watch.ElapsedMilliseconds}ms");
                }
            });
        }

        public async Task OnTick(DateTime now)
        {
            if (now > NextUpdate)
            {
                Reset();
                Rank();
            }
        }
    }
}