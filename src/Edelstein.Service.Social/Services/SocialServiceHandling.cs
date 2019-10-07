using System.Linq;
using Edelstein.Core.Gameplay.Social;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Core.Types;
using Edelstein.Service.Social.Logging;

namespace Edelstein.Service.Social.Services
{
    public partial class SocialService
    {
        private void HandleSocialUpdateState(SocialStateMessage message)
        {
            Logger.Debug($"{message.CharacterID} has {message.State} in {message.Service}");

            // TODO: cleanup
            
            var partyMember = PartyManager.GetPartyMember(message.CharacterID);
            var party = partyMember?.Party;
            
            if (party == null) return;
            Logger.Debug($"{message.CharacterID} is in a party of {party.Members.Count}");
            
            var service = Peers.FirstOrDefault(p => p.Name == message.Service);
            if (service == null) return;

            partyMember.Record.ChannelID = service.ID;
            partyMember.Record.FieldID = 310020100;
            
            SendMessage(service, new SocialInitPartyMessage
            {
                CharacterID = message.CharacterID,
                Data = new PartyData
                {
                    ID = party.Record.ID,
                    BossCharacterID = party.Record.BossCharacterID,
                    Members = party.Members
                        .Select(m => new PartyMemberData
                        {
                            CharacterID = m.Record.CharacterID,
                            Name = m.Record.Name,
                            Job = m.Record.Job,
                            Level = m.Record.Level,
                            ChannelID = m.Record.ChannelID,
                            FieldID = m.Record.FieldID
                        })
                        .ToList()
                }
            }).Wait();
        }

        private void HandleSocialUpdateLevel(SocialLevelMessage message)
        {
            Logger.Debug($"{message.CharacterID} is now level {message.Level}");
        }

        private void HandleSocialUpdateJob(SocialJobMessage message)
        {
            Logger.Debug($"{message.CharacterID} is now {(Job) message.Job}");
        }
    }
}