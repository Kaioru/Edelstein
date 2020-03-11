using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Edelstein.Core.Gameplay.Extensions.Packets;
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

        public IConversationContext ConversationContext { get; set; }

        public FieldUser(GameServiceAdapter socketAdapter)
        {
            Adapter = socketAdapter;
            Watching = new IFieldSplit[9];
            Controlling = new List<IFieldControlled>();

            BasicStat = new BasicStat(this);
            ForcedStat = new ForcedStat(this);
        }

        public Task SendPacket(IPacket packet)
            => Adapter.SendPacket(packet);

        public IFieldObj GetWatchedObject(int id)
            => Watching
                .Select(w => w.GetObject(id))
                .FirstOrDefault(o => o != null);

        public T GetWatchedObject<T>(int id) where T : IFieldObj
            => Watching
                .Select(w => w.GetObject<T>(id))
                .FirstOrDefault(o => o != null);

        public IEnumerable<IFieldObj> GetWatchedObjects()
            => Watching.SelectMany(w => w.GetObjects());

        public IEnumerable<T> GetWatchedObjects<T>() where T : IFieldObj
            => Watching.SelectMany(w => w.GetObjects<T>());

        public override IPacket GetEnterFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.UserEnterField);

            p.Encode<int>(ID);

            p.Encode<byte>(Character.Level);
            p.Encode<string>(Character.Name);

            // Guild
            p.Encode<string>("");
            p.Encode<short>(0);
            p.Encode<byte>(0);
            p.Encode<short>(0);
            p.Encode<byte>(0);

            p.Encode<long>(0);
            p.Encode<long>(0);
            p.Encode<byte>(0); // nDefenseAtt
            p.Encode<byte>(0); // nDefenseState

            p.Encode<short>(Character.Job);
            Character.EncodeLook(p);

            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(BasicStat.CompletedSetItemID);
            p.Encode<int>(0);

            p.Encode<Point>(Position);
            p.Encode<byte>(MoveAction);
            p.Encode<short>(Foothold);
            p.Encode<byte>(0);

            p.Encode<bool>(false);
            p.Encode<bool>(false);
            p.Encode<bool>(false);

            p.Encode<bool>(false);

            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(0);

            p.Encode<byte>(0);

            p.Encode<bool>(false);

            p.Encode<bool>(false);
            p.Encode<bool>(false);
            p.Encode<bool>(false);

            p.Encode<byte>(0);

            p.Encode<byte>(0);
            p.Encode<int>(0);
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.UserLeaveField);

            p.Encode<int>(ID);

            return p;
        }

        public IPacket GetSetFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.SetField);

            p.Encode<short>(0); // ClientOpt

            p.Encode<int>(Service.State.ChannelID);
            p.Encode<int>(AccountWorld.WorldID);

            p.Encode<bool>(true); // sNotifierMessage._m_pStr
            p.Encode<bool>(!IsInstantiated);
            p.Encode<short>(0); // nNotifierCheck, loops

            if (!IsInstantiated)
            {
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                Character.EncodeData(p);

                p.Encode<int>(0);
                for (var i = 0; i < 3; i++) p.Encode<int>(0);
            }
            else
            {
                p.Encode<byte>(0);
                p.Encode<int>(Character.FieldID);
                p.Encode<byte>(Character.FieldPortal);
                p.Encode<int>(Character.HP);
                p.Encode<bool>(false);
            }

            p.Encode<DateTime>(DateTime.Now);

            return p;
        }

        public void Dispose()
        {
            ConversationContext?.SafeDispose();
        }
    }
}