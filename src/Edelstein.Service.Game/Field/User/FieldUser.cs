using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Data.Entities;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Field.User.Stats;
using Edelstein.Service.Game.Logging;
using Edelstein.Service.Game.Sockets;

namespace Edelstein.Service.Game.Field.User
{
    public partial class FieldUser : AbstractFieldLife
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public override FieldObjType Type => FieldObjType.User;

        public WvsGameSocket Socket { get; }
        public Character Character { get; }

        public BasicStat BasicStat { get; }
        public ForcedStat ForcedStat { get; }

        public FieldUser(WvsGameSocket socket, Character character)
        {
            Socket = socket;
            Character = character;

            BasicStat = new BasicStat(this);
            ForcedStat = new ForcedStat();
            ValidateStat();
        }

        public void ValidateStat()
        {
            BasicStat.Calculate();

            if (Character.HP > BasicStat.MaxHP) ModifyStats(s => s.HP = BasicStat.MaxHP);
            if (Character.MP > BasicStat.MaxMP) ModifyStats(s => s.MP = BasicStat.MaxMP);
        }

        public void AvatarModified()
        {
            using (var p = new Packet(SendPacketOperations.UserAvatarModified))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(0x1); // Flag
                Character.EncodeLook(p);
                p.Encode<bool>(false); // bCouple
                p.Encode<bool>(false); // bFriendship
                p.Encode<bool>(false); // Marriage
                p.Encode<int>(BasicStat.CompletedSetItemID);

                Field.BroadcastPacket(this, p);
            }
        }

        public IPacket GetSetFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.SetField))
            {
                p.Encode<short>(0); // ClientOpt

                p.Encode<int>(Socket.WvsGame.Info.ID);
                p.Encode<int>(Socket.WvsGame.Info.WorldID);

                p.Encode<bool>(true); // sNotifierMessage._m_pStr
                p.Encode<bool>(!Socket.IsInstantiated);
                p.Encode<short>(0); // nNotifierCheck, loops

                if (!Socket.IsInstantiated)
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

                p.Encode<long>(0);
                return p;
            }
        }

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.UserEnterField))
            {
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
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                p.Encode<short>(Character.Job);
                Character.EncodeLook(p);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<byte>(0);

                p.Encode<bool>(false);

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<byte>(0);
                p.Encode<int>(0);
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.UserLeaveField))
            {
                p.Encode<int>(ID);
                return p;
            }
        }

        public Task SendPacket(IPacket packet) => Socket.SendPacket(packet);
    }
}