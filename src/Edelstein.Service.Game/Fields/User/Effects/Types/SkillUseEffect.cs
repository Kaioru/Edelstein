using System;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Effects.Types
{
    public class SkillUseEffect : AbstractEffect
    {
        public override EffectType Type => EffectType.SkillUse;

        private readonly int _skillTemplateID;
        private readonly byte _skillLevel;
        private readonly Action<IPacket> _additional;

        public SkillUseEffect(int skillTemplateID, byte skillLevel, Action<IPacket> additional = null)
        {
            _skillTemplateID = skillTemplateID;
            _skillLevel = skillLevel;
            _additional = additional;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<int>(_skillTemplateID);
            packet.Encode<byte>(0);
            packet.Encode<byte>(_skillLevel);

            _additional?.Invoke(packet);
        }
    }
}