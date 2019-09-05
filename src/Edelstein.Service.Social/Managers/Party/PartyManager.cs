using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Database.Store;
using Edelstein.Service.Social.Entities;

namespace Edelstein.Service.Social.Managers.Party
{
    public class PartyManager
    {
        private readonly IDataStore _dataStore;

        public IDictionary<int, Party> Parties { get; }
        public IDictionary<int, PartyMember> PartyMembers { get; }

        public PartyManager(IDataStore dataStore)
        {
            _dataStore = dataStore;

            Parties = new Dictionary<int, Party>();
            PartyMembers = new Dictionary<int, PartyMember>();
        }

        public Party GetParty(int id)
        {
            if (Parties.ContainsKey(id))
                return Parties[id];

            using (var store = _dataStore.OpenSession())
            {
                var partyRecord = store
                    .Query<PartyRecord>()
                    .FirstOrDefault(r => r.ID == id);

                if (partyRecord == null)
                    return null;

                var memberRecords = store
                    .Query<PartyMemberRecord>()
                    .Where(r => r.PartyRecordID == partyRecord.ID)
                    .ToList();

                var party = new Party
                {
                    Record = partyRecord
                };

                party.Members = memberRecords
                    .Select(r => new PartyMember
                    {
                        Record = r,
                        Party = party
                    })
                    .ToList();
                party.Boss = party.Members
                    .FirstOrDefault(m => m.Record.CharacterID == party.Record.BossCharacterID);

                Parties[id] = party;
                party.Members.ForEach(m => PartyMembers[m.Record.CharacterID] = m);
                return party;
            }
        }

        public PartyMember GetPartyMember(int id)
        {
            if (PartyMembers.ContainsKey(id))
                return PartyMembers[id];

            using (var store = _dataStore.OpenSession())
            {
                var memberRecord = store
                    .Query<PartyMemberRecord>()
                    .FirstOrDefault(r => r.CharacterID == id);

                if (memberRecord == null)
                    return null;

                GetParty(memberRecord.PartyRecordID);
                return PartyMembers[id];
            }
        }
    }
}