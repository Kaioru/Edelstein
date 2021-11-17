using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Common.Gameplay.Stages.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.AffectedArea;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Common
{
    public class DebugCommandArgs : CommandArgs
    {
    }

    public class DebugCommand : AbstractCommand<DebugCommandArgs>
    {
        public override string Name => "Debug";
        public override string Description => "A testing debug command";

        public override async Task Execute(IFieldObjUser user, DebugCommandArgs args)
        {
            var affectedArea = new FieldObjAffectedArea
            {
                AffectedAreaType = AffectedAreaType.Buff,
                OwnerID = user.ID,
                Area = new Rect2D(user.Position, new Size2D(150, 100)),
                SkillID = 5281000,
                DateExpire = DateTime.UtcNow.AddSeconds(3)
            };

            await user.Field.Enter(affectedArea);
        }
    }
}
