using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects.User.Stats;

namespace Edelstein.Service.Game.Fields.Objects.User
{
    public class FieldUser : AbstractFieldLife, IFieldUser, IDisposable
    {
        public override FieldObjType Type => FieldObjType.User;

        public GameService Service => Adapter.Service;
        public GameServiceAdapter Adapter { get; }

        public Account Account => Adapter.Account;
        public AccountWorld AccountWorld => Adapter.AccountWorld;
        public Character Character => Adapter.Character;

        public IFieldSplit[] Watching { get; }
        public ICollection<IFieldControlled> Controlling { get; }

        public bool IsInstantiated { get; set; }

        public BasicStat BasicStat { get; }
        public ForcedStat ForcedStat { get; }

        public IConversationContext? ConversationContext { get; set; }

        public IDictionary<int, ISocialMemo> Memos { get; }
        public ISocialParty? Party { get; set; }
        public ISocialGuild? Guild { get; set; }

        public FieldUser(GameServiceAdapter socketAdapter)
        {
            Adapter = socketAdapter;
            Watching = new IFieldSplit[9];
            Controlling = new List<IFieldControlled>();

            BasicStat = new BasicStat(this);
            ForcedStat = new ForcedStat(this);

            Memos = new ConcurrentDictionary<int, ISocialMemo>();
        }

        public Task SendPacket(IPacket packet)
            => Adapter.SendPacket(packet);

        public IFieldObj GetWatchedObject(int id)
            => GetWatchedObjects()
                .FirstOrDefault(o => o.ID == id);

        public T GetWatchedObject<T>(int id) where T : IFieldObj
            => GetWatchedObjects<T>()
                .FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetWatchedObjects()
            => Watching
                .Where(w => w != null)
                .SelectMany(w => w.GetObjects());

        public IEnumerable<T> GetWatchedObjects<T>() where T : IFieldObj
            => Watching
                .Where(w => w != null)
                .SelectMany(w => w.GetObjects<T>());

        public override IPacket GetEnterFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.UserEnterField);

            p.EncodeInt(ID);

            p.EncodeByte(Character.Level);
            p.EncodeString(Character.Name);

            // Guild
            p.EncodeString(Guild?.Name ?? "");
            p.EncodeShort(Guild?.MarkBg ?? 0);
            p.EncodeByte(Guild?.MarkBgColor ?? 0);
            p.EncodeShort(Guild?.Mark ?? 0);
            p.EncodeByte(Guild?.MarkColor ?? 0);

            p.EncodeLong(0);
            p.EncodeLong(0);
            p.EncodeByte(0); // nDefenseAtt
            p.EncodeByte(0); // nDefenseState

            p.EncodeShort(Character.Job);
            Character.EncodeLook(p);

            p.EncodeInt(0);
            p.EncodeInt(0);
            p.EncodeInt(0);
            p.EncodeInt(0);
            p.EncodeInt(BasicStat.CompletedSetItemID);
            p.EncodeInt(0);

            p.EncodePoint(Position);
            p.EncodeByte(MoveAction);
            p.EncodeShort(Foothold);
            p.EncodeByte(0);

            p.EncodeBool(false);
            p.EncodeBool(false);
            p.EncodeBool(false);

            p.EncodeBool(false);

            p.EncodeInt(0);
            p.EncodeInt(0);
            p.EncodeInt(0);

            p.EncodeByte(0);

            p.EncodeBool(false);

            p.EncodeBool(false);
            p.EncodeBool(false);
            p.EncodeBool(false);

            p.EncodeByte(0);

            p.EncodeByte(0);
            p.EncodeInt(0);
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.UserLeaveField);

            p.EncodeInt(ID);

            return p;
        }

        public IPacket GetSetFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.SetField);

            p.EncodeShort(0); // ClientOpt

            p.EncodeInt(Service.State.ChannelID);
            p.EncodeInt(AccountWorld.WorldID);

            p.EncodeBool(true); // sNotifierMessage._m_pStr
            p.EncodeBool(!IsInstantiated);
            p.EncodeShort(0); // nNotifierCheck, loops

            if (!IsInstantiated)
            {
                p.EncodeInt(0);
                p.EncodeInt(0);
                p.EncodeInt(0);

                Character.EncodeData(p);

                p.EncodeInt(0);
                for (var i = 0; i < 3; i++) p.EncodeInt(0);
            }
            else
            {
                p.EncodeByte(0);
                p.EncodeInt(Character.FieldID);
                p.EncodeByte(Character.FieldPortal);
                p.EncodeInt(Character.HP);
                p.EncodeBool(false);
            }

            p.EncodeDateTime(DateTime.Now);

            return p;
        }

        public void Dispose()
        {
            ConversationContext?.SafeDispose();
        }
    }
}