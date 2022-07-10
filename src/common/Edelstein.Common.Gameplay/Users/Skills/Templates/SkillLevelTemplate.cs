using System.Drawing;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Users.Skills.Templates
{
    public record SkillLevelTemplate : ISkillLevelInfo, ITemplate
    {
        public int ID { get; }

        public short HP { get; }
        public short MP { get; }

        public short PAD { get; }
        public short PDD { get; }
        public short MAD { get; }
        public short MDD { get; }
        public short ACC { get; }
        public short EVA { get; }
        public short Craft { get; }

        public short Speed { get; }
        public short Jump { get; }

        public short Morph { get; }

        public short HPCon { get; }
        public short MPCon { get; }
        public short MoneyCon { get; }
        public short ItemCon { get; }
        public short ItemConNo { get; }

        public short Damage { get; }
        public short FixDamage { get; }

        public short SelfDestruction { get; }

        public short Time { get; }
        public short SubTime { get; }

        public short Prop { get; }
        public short SubProp { get; }

        public short AttackCount { get; }
        public short BulletCount { get; }
        public short BulletConsume { get; }

        public short Mastery { get; }

        public short MobCount { get; }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }

        public short Action { get; }

        public short EMHP { get; }
        public short EMMP { get; }
        public short EPAD { get; }
        public short EPDD { get; }
        public short EMDD { get; }

        public short Range { get; }

        public short Cooltime { get; }

        public Rect2D AffectedArea { get; }

        public short MHPr { get; }
        public short MMPr { get; }

        public short Cr { get; }
        public short CDMin { get; }
        public short CDMax { get; }

        public short ACCr { get; }
        public short EVAr { get; }
        public short Ar { get; }
        public short Er { get; }

        public short PDDr { get; }
        public short MDDr { get; }
        public short PDr { get; }
        public short MDr { get; }

        public short DIPr { get; }

        public short PDamr { get; }
        public short MDamr { get; }

        public short PADr { get; }
        public short MADr { get; }

        public short EXPr { get; }

        public short Dot { get; }
        public short DotInterval { get; }
        public short DotTime { get; }

        public short IMPr { get; }
        public short ASRr { get; }
        public short TERr { get; }

        public short MESOr { get; }

        public short PADx { get; }
        public short MADx { get; }

        public short IMDr { get; }

        public short PsdJump { get; }
        public short PsdSpeed { get; }

        public short OCr { get; }
        public short DCr { get; }

        public short ReqGL { get; }

        public short Price { get; }

        public short S { get; }
        public short U { get; }
        public short V { get; }
        public short W { get; }
        public short T { get; }

        public SkillLevelTemplate(int id, IDataProperty property)
        {
            HP = property.Resolve<short>("hp") ?? 0;
            MP = property.Resolve<short>("mp") ?? 0;

            PAD = property.Resolve<short>("pad") ?? 0;
            PDD = property.Resolve<short>("pdd") ?? 0;
            MAD = property.Resolve<short>("mad") ?? 0;
            MDD = property.Resolve<short>("mdd") ?? 0;
            ACC = property.Resolve<short>("acc") ?? 0;
            EVA = property.Resolve<short>("eva") ?? 0;
            Craft = property.Resolve<short>("craft") ?? 0;

            Speed = property.Resolve<short>("speed") ?? 0;
            Jump = property.Resolve<short>("jump") ?? 0;

            Morph = property.Resolve<short>("morph") ?? 0;

            HPCon = property.Resolve<short>("hpCon") ?? 0;
            MPCon = property.Resolve<short>("mpCon") ?? 0;
            MoneyCon = property.Resolve<short>("moneyCon") ?? 0;
            ItemCon = property.Resolve<short>("itemCon") ?? 0;
            ItemConNo = property.Resolve<short>("itemConNo") ?? 0;

            Damage = property.Resolve<short>("damage") ?? 0;
            FixDamage = property.Resolve<short>("fixdamage") ?? 0;

            SelfDestruction = property.Resolve<short>("selfDestruction") ?? 0;

            Time = property.Resolve<short>("time") ?? 0;
            SubTime = property.Resolve<short>("subTime") ?? 0;

            Prop = property.Resolve<short>("prop") ?? 0;
            SubProp = property.Resolve<short>("subProp") ?? 0;

            AttackCount = property.Resolve<short>("attackCount") ?? 1;
            BulletCount = property.Resolve<short>("bulletCount") ?? 0;
            BulletConsume = property.Resolve<short>("bulletConsume") ?? 0;

            Mastery = property.Resolve<short>("mastery") ?? 0;

            MobCount = property.Resolve<short>("mobCount") ?? 0;

            X = property.Resolve<short>("x") ?? 0;
            Y = property.Resolve<short>("y") ?? 0;
            Z = property.Resolve<short>("z") ?? 0;

            Action = property.Resolve<short>("action") ?? 0;

            EMHP = property.Resolve<short>("emhp") ?? 0;
            EMMP = property.Resolve<short>("emmp") ?? 0;
            EPAD = property.Resolve<short>("epad") ?? 0;
            EPDD = property.Resolve<short>("epdd") ?? 0;
            EMDD = property.Resolve<short>("emdd") ?? 0;

            Range = property.Resolve<short>("range") ?? 0;

            Cooltime = property.Resolve<short>("cooltime") ?? 0;

            var lt = property.Resolve<Point>("lt") ?? new Point(0, 0);
            var rb = property.Resolve<Point>("rb") ?? new Point(0, 0);

            AffectedArea = new Rect2D(new Point2D(lt.X, lt.Y), new Point2D(rb.X, rb.Y));

            MHPr = property.Resolve<short>("mhpR") ?? 0;
            MMPr = property.Resolve<short>("mmpR") ?? 0;

            Cr = property.Resolve<short>("cr") ?? 0;
            CDMin = property.Resolve<short>("criticaldamageMin") ?? 0;
            CDMax = property.Resolve<short>("criticaldamageMax") ?? 0;

            ACCr = property.Resolve<short>("accR") ?? 0;
            EVAr = property.Resolve<short>("evaR") ?? 0;
            Ar = property.Resolve<short>("ar") ?? 0;
            Er = property.Resolve<short>("er") ?? 0;

            PDDr = property.Resolve<short>("pddR") ?? 0;
            MDDr = property.Resolve<short>("mddR") ?? 0;
            PDr = property.Resolve<short>("pdr") ?? 0;
            MDr = property.Resolve<short>("mdr") ?? 0;

            DIPr = property.Resolve<short>("damR") ?? 0;

            PDamr = property.Resolve<short>("pdR") ?? 0;
            MDamr = property.Resolve<short>("mdR") ?? 0;

            PADr = property.Resolve<short>("padR") ?? 0;
            MADr = property.Resolve<short>("madR") ?? 0;

            EXPr = property.Resolve<short>("expR") ?? 0;

            Dot = property.Resolve<short>("dot") ?? 0;
            DotInterval = property.Resolve<short>("dotInterval") ?? 0;
            DotTime = property.Resolve<short>("dotTime") ?? 0;

            IMPr = property.Resolve<short>("ignoreMobpdpR") ?? 0;
            ASRr = property.Resolve<short>("asrR") ?? 0;
            TERr = property.Resolve<short>("terR") ?? 0;

            MESOr = property.Resolve<short>("mesoR") ?? 0;

            PADx = property.Resolve<short>("padX") ?? 0;
            MADx = property.Resolve<short>("madX") ?? 0;

            IMDr = property.Resolve<short>("ignoreMobDamR") ?? 0;

            PsdJump = property.Resolve<short>("psdJump") ?? 0;
            PsdSpeed = property.Resolve<short>("psdSpeed") ?? 0;

            OCr = property.Resolve<short>("overChargeR") ?? 0;
            DCr = property.Resolve<short>("disCountR") ?? 0;

            ReqGL = property.Resolve<short>("reqGuildLevel") ?? 0;

            Price = property.Resolve<short>("price") ?? 0;

            S = property.Resolve<short>("s") ?? 0;
            U = property.Resolve<short>("u") ?? 0;
            V = property.Resolve<short>("v") ?? 0;
            W = property.Resolve<short>("w") ?? 0;
            T = property.Resolve<short>("t") ?? 0;
        }
    }
}
