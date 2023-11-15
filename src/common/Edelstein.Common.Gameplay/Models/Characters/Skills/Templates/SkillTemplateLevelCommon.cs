using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Spatial;
using org.mariuszgromada.math.mxparser;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplateLevelCommon : ISkillTemplateLevel
{
    public int ID => Level;
    
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

    public SkillTemplateLevelCommon(int level, IDataNode node)
    {
        Level = level;
        
        var x = new Argument("x", level);

        var u = new Function("u", "ceil(x)", "x");
        var d = new Function("d", "floor(x)", "x");

        HP = ResolveExpression(node.ResolveString("hp"), x, u, d);
        MP = ResolveExpression(node.ResolveString("mp"), x, u, d);

        PAD = ResolveExpression(node.ResolveString("pad"), x, u, d);
        PDD = ResolveExpression(node.ResolveString("pdd"), x, u, d);
        MAD = ResolveExpression(node.ResolveString("mad"), x, u, d);
        MDD = ResolveExpression(node.ResolveString("mdd"), x, u, d);
        ACC = ResolveExpression(node.ResolveString("acc"), x, u, d);
        EVA = ResolveExpression(node.ResolveString("eva"), x, u, d);
        Craft = ResolveExpression(node.ResolveString("craft"), x, u, d);

        Speed = ResolveExpression(node.ResolveString("speed"), x, u, d);
        Jump = ResolveExpression(node.ResolveString("jump"), x, u, d);

        Morph = ResolveExpression(node.ResolveString("morph"), x, u, d);

        HPCon = ResolveExpression(node.ResolveString("hpCon"), x, u, d);
        MPCon = ResolveExpression(node.ResolveString("mpCon"), x, u, d);
        MoneyCon = ResolveExpression(node.ResolveString("moneyCon"), x, u, d);
        ItemCon = ResolveExpression(node.ResolveString("itemCon"), x, u, d);
        ItemConNo = ResolveExpression(node.ResolveString("itemConNo"), x, u, d);

        Damage = ResolveExpression(node.ResolveString("damage"), x, u, d);
        FixDamage = ResolveExpression(node.ResolveString("fixdamage"), x, u, d);

        SelfDestruction = ResolveExpression(node.ResolveString("selfDestruction"), x, u, d);

        Time = ResolveExpression(node.ResolveString("time"), x, u, d);
        SubTime = ResolveExpression(node.ResolveString("subTime"), x, u, d);

        Prop = ResolveExpression(node.ResolveString("prop"), x, u, d);
        SubProp = ResolveExpression(node.ResolveString("subProp"), x, u, d);

        AttackCount = ResolveExpression(node.ResolveString("attackCount") ?? "1", x, u, d);
        BulletCount = ResolveExpression(node.ResolveString("bulletCount"), x, u, d);
        BulletConsume = ResolveExpression(node.ResolveString("bulletConsume"), x, u, d);

        Mastery = ResolveExpression(node.ResolveString("mastery"), x, u, d);

        MobCount = ResolveExpression(node.ResolveString("mobCount"), x, u, d);

        X = ResolveExpression(node.ResolveString("x"), x, u, d);
        Y = ResolveExpression(node.ResolveString("y"), x, u, d);
        Z = ResolveExpression(node.ResolveString("z"), x, u, d);

        Action = ResolveExpression(node.ResolveString("action"), x, u, d);

        EMHP = ResolveExpression(node.ResolveString("emhp"), x, u, d);
        EMMP = ResolveExpression(node.ResolveString("emmp"), x, u, d);
        EPAD = ResolveExpression(node.ResolveString("epad"), x, u, d);
        EPDD = ResolveExpression(node.ResolveString("epdd"), x, u, d);
        EMDD = ResolveExpression(node.ResolveString("emdd"), x, u, d);

        Range = ResolveExpression(node.ResolveString("range"), x, u, d);

        Cooltime = ResolveExpression(node.ResolveString("cooltime"), x, u, d);

        var (l, t) = node.ResolveVector("lt") ?? new ValueTuple<int, int>(0, 0);
        var (r, b) = node.ResolveVector("rb") ?? new ValueTuple<int, int>(0, 0);

        Bounds = new Rectangle2D(new Point2D(l, t), new Point2D(r, b));

        MHPr = ResolveExpression(node.ResolveString("mhpR"), x, u, d);
        MMPr = ResolveExpression(node.ResolveString("mmpR"), x, u, d);

        Cr = ResolveExpression(node.ResolveString("cr"), x, u, d);
        CDMin = ResolveExpression(node.ResolveString("criticaldamageMin"), x, u, d);
        CDMax = ResolveExpression(node.ResolveString("criticaldamageMax"), x, u, d);

        ACCr = ResolveExpression(node.ResolveString("accR"), x, u, d);
        EVAr = ResolveExpression(node.ResolveString("evaR"), x, u, d);
        Ar = ResolveExpression(node.ResolveString("ar"), x, u, d);
        Er = ResolveExpression(node.ResolveString("er"), x, u, d);

        PDDr = ResolveExpression(node.ResolveString("pddR"), x, u, d);
        MDDr = ResolveExpression(node.ResolveString("mddR"), x, u, d);
        PDr = ResolveExpression(node.ResolveString("pdr"), x, u, d);
        MDr = ResolveExpression(node.ResolveString("mdr"), x, u, d);

        DIPr = ResolveExpression(node.ResolveString("damR"), x, u, d);

        PDamr = ResolveExpression(node.ResolveString("pdR"), x, u, d);
        MDamr = ResolveExpression(node.ResolveString("mdR"), x, u, d);

        PADr = ResolveExpression(node.ResolveString("padR"), x, u, d);
        MADr = ResolveExpression(node.ResolveString("madR"), x, u, d);

        EXPr = ResolveExpression(node.ResolveString("expR"), x, u, d);

        Dot = ResolveExpression(node.ResolveString("dot"), x, u, d);
        DotInterval = ResolveExpression(node.ResolveString("dotInterval"), x, u, d);
        DotTime = ResolveExpression(node.ResolveString("dotTime"), x, u, d);

        //IMPr = ResolveExpression(node.ResolveString("ignoreMobpdpR"), x, u, d);
        ASRr = ResolveExpression(node.ResolveString("asrR"), x, u, d);
        TERr = ResolveExpression(node.ResolveString("terR"), x, u, d);

        MESOr = ResolveExpression(node.ResolveString("mesoR"), x, u, d);

        PADx = ResolveExpression(node.ResolveString("padX"), x, u, d);
        MADx = ResolveExpression(node.ResolveString("madX"), x, u, d);

        IMDr = ResolveExpression(node.ResolveString("ignoreMobpdpR"), x, u, d);

        PsdJump = ResolveExpression(node.ResolveString("psdJump"), x, u, d);
        PsdSpeed = ResolveExpression(node.ResolveString("psdSpeed"), x, u, d);

        OCr = ResolveExpression(node.ResolveString("overChargeR"), x, u, d);
        DCr = ResolveExpression(node.ResolveString("disCountR"), x, u, d);

        ReqGL = ResolveExpression(node.ResolveString("reqGuildLevel"), x, u, d);

        Price = ResolveExpression(node.ResolveString("price"), x, u, d);

        S = ResolveExpression(node.ResolveString("s"), x, u, d);
        U = ResolveExpression(node.ResolveString("u"), x, u, d);
        V = ResolveExpression(node.ResolveString("v"), x, u, d);
        W = ResolveExpression(node.ResolveString("w"), x, u, d);
        T = ResolveExpression(node.ResolveString("t"), x, u, d);
    }

    private static short ResolveExpression(string? expression, params PrimitiveElement[] elements)
    {
        if (expression == null) return 0;
        return (short)new Expression(expression, elements).calculate();
    }
}
