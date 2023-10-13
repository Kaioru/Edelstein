using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplateLevel : ISkillTemplateLevel
{
    public int Level { get; }
    
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

    public IRectangle2D Bounds { get; }

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

    public SkillTemplateLevel(int level, IDataNode node)
    {
        Level = level;
        
        HP = node.ResolveShort("hp") ?? 0;
        MP = node.ResolveShort("mp") ?? 0;

        PAD = node.ResolveShort("pad") ?? 0;
        PDD = node.ResolveShort("pdd") ?? 0;
        MAD = node.ResolveShort("mad") ?? 0;
        MDD = node.ResolveShort("mdd") ?? 0;
        ACC = node.ResolveShort("acc") ?? 0;
        EVA = node.ResolveShort("eva") ?? 0;
        Craft = node.ResolveShort("craft") ?? 0;

        Speed = node.ResolveShort("speed") ?? 0;
        Jump = node.ResolveShort("jump") ?? 0;

        Morph = node.ResolveShort("morph") ?? 0;

        HPCon = node.ResolveShort("hpCon") ?? 0;
        MPCon = node.ResolveShort("mpCon") ?? 0;
        MoneyCon = node.ResolveShort("moneyCon") ?? 0;
        ItemCon = node.ResolveShort("itemCon") ?? 0;
        ItemConNo = node.ResolveShort("itemConNo") ?? 0;

        Damage = node.ResolveShort("damage") ?? 0;
        FixDamage = node.ResolveShort("fixdamage") ?? 0;

        SelfDestruction = node.ResolveShort("selfDestruction") ?? 0;

        Time = node.ResolveShort("time") ?? 0;
        SubTime = node.ResolveShort("subTime") ?? 0;

        Prop = node.ResolveShort("prop") ?? 0;
        SubProp = node.ResolveShort("subProp") ?? 0;

        AttackCount = node.ResolveShort("attackCount") ?? 1;
        BulletCount = node.ResolveShort("bulletCount") ?? 0;
        BulletConsume = node.ResolveShort("bulletConsume") ?? 0;

        Mastery = node.ResolveShort("mastery") ?? 0;

        MobCount = node.ResolveShort("mobCount") ?? 0;

        X = node.ResolveShort("x") ?? 0;
        Y = node.ResolveShort("y") ?? 0;
        Z = node.ResolveShort("z") ?? 0;

        Action = node.ResolveShort("action") ?? 0;

        EMHP = node.ResolveShort("emhp") ?? 0;
        EMMP = node.ResolveShort("emmp") ?? 0;
        EPAD = node.ResolveShort("epad") ?? 0;
        EPDD = node.ResolveShort("epdd") ?? 0;
        EMDD = node.ResolveShort("emdd") ?? 0;

        Range = node.ResolveShort("range") ?? 0;

        Cooltime = node.ResolveShort("cooltime") ?? 0;

        var (l, t) = node.ResolveVector("lt") ?? new ValueTuple<int, int>(0, 0);
        var (r, b) = node.ResolveVector("rb") ?? new ValueTuple<int, int>(0, 0);

        Bounds = new Rectangle2D(new Point2D(l, t), new Point2D(r, b));

        MHPr = node.ResolveShort("mhpR") ?? 0;
        MMPr = node.ResolveShort("mmpR") ?? 0;

        Cr = node.ResolveShort("cr") ?? 0;
        CDMin = node.ResolveShort("criticaldamageMin") ?? 0;
        CDMax = node.ResolveShort("criticaldamageMax") ?? 0;

        ACCr = node.ResolveShort("accR") ?? 0;
        EVAr = node.ResolveShort("evaR") ?? 0;
        Ar = node.ResolveShort("ar") ?? 0;
        Er = node.ResolveShort("er") ?? 0;

        PDDr = node.ResolveShort("pddR") ?? 0;
        MDDr = node.ResolveShort("mddR") ?? 0;
        PDr = node.ResolveShort("pdr") ?? 0;
        MDr = node.ResolveShort("mdr") ?? 0;

        DIPr = node.ResolveShort("damR") ?? 0;

        PDamr = node.ResolveShort("pdR") ?? 0;
        MDamr = node.ResolveShort("mdR") ?? 0;

        PADr = node.ResolveShort("padR") ?? 0;
        MADr = node.ResolveShort("madR") ?? 0;

        EXPr = node.ResolveShort("expR") ?? 0;

        Dot = node.ResolveShort("dot") ?? 0;
        DotInterval = node.ResolveShort("dotInterval") ?? 0;
        DotTime = node.ResolveShort("dotTime") ?? 0;

        //IMPr = node.ResolveShort("ignoreMobpdpR") ?? 0;
        ASRr = node.ResolveShort("asrR") ?? 0;
        TERr = node.ResolveShort("terR") ?? 0;

        MESOr = node.ResolveShort("mesoR") ?? 0;

        PADx = node.ResolveShort("padX") ?? 0;
        MADx = node.ResolveShort("madX") ?? 0;

        IMDr = node.ResolveShort("ignoreMobpdpR") ?? 0;

        PsdJump = node.ResolveShort("psdJump") ?? 0;
        PsdSpeed = node.ResolveShort("psdSpeed") ?? 0;

        OCr = node.ResolveShort("overChargeR") ?? 0;
        DCr = node.ResolveShort("disCountR") ?? 0;

        ReqGL = node.ResolveShort("reqGuildLevel") ?? 0;

        Price = node.ResolveShort("price") ?? 0;

        S = node.ResolveShort("s") ?? 0;
        U = node.ResolveShort("u") ?? 0;
        V = node.ResolveShort("v") ?? 0;
        W = node.ResolveShort("w") ?? 0;
        T = node.ResolveShort("t") ?? 0;
    }
}
