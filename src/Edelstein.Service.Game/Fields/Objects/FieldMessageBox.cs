using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.Objects
{
    public class FieldMessageBox : AbstractFieldObj, IUpdateable
    {
        public override FieldObjType Type => FieldObjType.MessageBox;

        private readonly int _templateID;
        private readonly string _hope;
        private readonly string _name;
        private readonly DateTime? _dateExpire;

        public FieldMessageBox(int templateID, string hope, string name, DateTime? dateExpire = null)
        {
            _templateID = templateID;
            _hope = hope;
            _name = name;
            _dateExpire = dateExpire;
        }

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.MessageBoxEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(_templateID);
                p.Encode<string>(_hope);
                p.Encode<string>(_name);
                p.Encode<Point>(Position);
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
            => GetLeaveFieldPacket(true);

        public IPacket GetLeaveFieldPacket(bool fadeOut)
        {
            using (var p = new Packet(SendPacketOperations.MessageBoxLeaveField))
            {
                p.Encode<bool>(fadeOut);
                p.Encode<int>(ID);
                return p;
            }
        }

        public async Task OnUpdate(DateTime now)
        {
            if (!_dateExpire.HasValue) return;
            if ((now - _dateExpire.Value).Seconds < 0) return;

            await Field.Leave(this);
        }
    }
}