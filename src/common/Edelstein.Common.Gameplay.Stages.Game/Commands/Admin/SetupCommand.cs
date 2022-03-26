using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Admin
{
    public class SetupCommand : AbstractCommand
    {
        public override string Name => "setup";
        public override string Description => "A debug character setup command to test items and skills";

        public override async Task Execute(IFieldObjUser user, string[] args)
        {
            await user.ModifyStats(s =>
            {
                s.Level = 200;
                s.STR = 10_000;
                s.DEX = 10_000;
                s.INT = 10_000;
                s.LUK = 10_000;

                s.SP = 999;
                s.SetExtendSP(0, 100);
                s.SetExtendSP(1, 100);
                s.SetExtendSP(2, 100);
                s.SetExtendSP(3, 100);
                s.SetExtendSP(4, 100);
                s.SetExtendSP(5, 100);
                s.SetExtendSP(6, 100);
                s.SetExtendSP(7, 100);
                s.SetExtendSP(8, 100);
            });

            await user.ModifyInventory(i =>
            {
                i.Add(01302059);
                i.Add(01402037);
                i.Add(01312030);
                i.Add(01412026);
                i.Add(01322062);
                i.Add(01422027);
                i.Add(01452060);
                i.Add(01462039);
                i.Add(01472052);
                i.Add(01332050);
                i.Add(01332049);
                i.Add(01432038);
                i.Add(01442045);
                i.Add(01372032);
                i.Add(01382036);
                i.Add(01482013);
                i.Add(01492012);

                i.Add(02070016, 5);
                i.Add(02060000, 5);
                i.Add(02061000, 5);
                i.Add(02330005, 5);
            });
        }
    }
}
