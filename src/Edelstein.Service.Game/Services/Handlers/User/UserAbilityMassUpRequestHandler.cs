using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Stats.Modify;
using MoreLinq;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserAbilityMassUpRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var count = packet.Decode<int>();
            var inc = new Dictionary<int, int>();

            for (var i = 0; i < count; i++)
                inc.Add(packet.Decode<int>(), packet.Decode<int>());

            var total = inc.Values.Sum();

            if (user.Character.AP < total) return;

            await user.ModifyStats(s =>
            {
                inc.ForEach(p =>
                {
                    var type = (ModifyStatType) p.Key;
                    var value = (short) p.Value;

                    switch (type)
                    {
                        case ModifyStatType.STR:
                            s.STR += value;
                            break;
                        case ModifyStatType.DEX:
                            s.DEX += value;
                            break;
                        case ModifyStatType.INT:
                            s.INT += value;
                            break;
                        case ModifyStatType.LUK:
                            s.LUK += value;
                            break;
                    }
                });

                s.AP -= (short) total;
            }, true);
        }
    }
}