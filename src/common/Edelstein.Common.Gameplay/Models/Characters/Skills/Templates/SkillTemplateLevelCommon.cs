using System.Drawing;
using Duey.Types;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using org.mariuszgromada.math.mxparser;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplateLevelCommon : ISkillTemplateLevel
{
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

    public Rectangle2D Bounds { get; }

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

    public SkillTemplateLevelCommon(int level, IDataProperty property)
    {
        var x = new Argument("x", level);

        var u = new Function("u", "ceil(x)", "x");
        var d = new Function("d", "floor(x)", "x");

        HP = ResolveExpression(property.ResolveOrDefault<string>("hp"), x, u, d);
        MP = ResolveExpression(property.ResolveOrDefault<string>("mp"), x, u, d);

        PAD = ResolveExpression(property.ResolveOrDefault<string>("pad"), x, u, d);
        PDD = ResolveExpression(property.ResolveOrDefault<string>("pdd"), x, u, d);
        MAD = ResolveExpression(property.ResolveOrDefault<string>("mad"), x, u, d);
        MDD = ResolveExpression(property.ResolveOrDefault<string>("mdd"), x, u, d);
        ACC = ResolveExpression(property.ResolveOrDefault<string>("acc"), x, u, d);
        EVA = ResolveExpression(property.ResolveOrDefault<string>("eva"), x, u, d);
        Craft = ResolveExpression(property.ResolveOrDefault<string>("craft"), x, u, d);

        Speed = ResolveExpression(property.ResolveOrDefault<string>("speed"), x, u, d);
        Jump = ResolveExpression(property.ResolveOrDefault<string>("jump"), x, u, d);

        Morph = ResolveExpression(property.ResolveOrDefault<string>("morph"), x, u, d);

        HPCon = ResolveExpression(property.ResolveOrDefault<string>("hpCon"), x, u, d);
        MPCon = ResolveExpression(property.ResolveOrDefault<string>("mpCon"), x, u, d);
        MoneyCon = ResolveExpression(property.ResolveOrDefault<string>("moneyCon"), x, u, d);
        ItemCon = ResolveExpression(property.ResolveOrDefault<string>("itemCon"), x, u, d);
        ItemConNo = ResolveExpression(property.ResolveOrDefault<string>("itemConNo"), x, u, d);

        Damage = ResolveExpression(property.ResolveOrDefault<string>("damage"), x, u, d);
        FixDamage = ResolveExpression(property.ResolveOrDefault<string>("fixdamage"), x, u, d);

        SelfDestruction = ResolveExpression(property.ResolveOrDefault<string>("selfDestruction"), x, u, d);

        Time = ResolveExpression(property.ResolveOrDefault<string>("time"), x, u, d);
        SubTime = ResolveExpression(property.ResolveOrDefault<string>("subTime"), x, u, d);

        Prop = ResolveExpression(property.ResolveOrDefault<string>("prop"), x, u, d);
        SubProp = ResolveExpression(property.ResolveOrDefault<string>("subProp"), x, u, d);

        AttackCount = ResolveExpression(property.ResolveOrDefault<string>("attackCount") ?? "1", x, u, d);
        BulletCount = ResolveExpression(property.ResolveOrDefault<string>("bulletCount"), x, u, d);
        BulletConsume = ResolveExpression(property.ResolveOrDefault<string>("bulletConsume"), x, u, d);

        Mastery = ResolveExpression(property.ResolveOrDefault<string>("mastery"), x, u, d);

        MobCount = ResolveExpression(property.ResolveOrDefault<string>("mobCount"), x, u, d);

        X = ResolveExpression(property.ResolveOrDefault<string>("x"), x, u, d);
        Y = ResolveExpression(property.ResolveOrDefault<string>("y"), x, u, d);
        Z = ResolveExpression(property.ResolveOrDefault<string>("z"), x, u, d);

        Action = ResolveExpression(property.ResolveOrDefault<string>("action"), x, u, d);

        EMHP = ResolveExpression(property.ResolveOrDefault<string>("emhp"), x, u, d);
        EMMP = ResolveExpression(property.ResolveOrDefault<string>("emmp"), x, u, d);
        EPAD = ResolveExpression(property.ResolveOrDefault<string>("epad"), x, u, d);
        EPDD = ResolveExpression(property.ResolveOrDefault<string>("epdd"), x, u, d);
        EMDD = ResolveExpression(property.ResolveOrDefault<string>("emdd"), x, u, d);

        Range = ResolveExpression(property.ResolveOrDefault<string>("range"), x, u, d);

        Cooltime = ResolveExpression(property.ResolveOrDefault<string>("cooltime"), x, u, d);

        var lt = property.ResolveOrDefault<NXVector>("lt") ?? new NXVector(0, 0);
        var rb = property.ResolveOrDefault<NXVector>("rb") ?? new NXVector(0, 0);
        
        Bounds = new Rectangle2D(new Point2D(lt.X, lt.Y), new Point2D(rb.X, rb.Y));

        MHPr = ResolveExpression(property.ResolveOrDefault<string>("mhpR"), x, u, d);
        MMPr = ResolveExpression(property.ResolveOrDefault<string>("mmpR"), x, u, d);

        Cr = ResolveExpression(property.ResolveOrDefault<string>("cr"), x, u, d);
        CDMin = ResolveExpression(property.ResolveOrDefault<string>("criticaldamageMin"), x, u, d);
        CDMax = ResolveExpression(property.ResolveOrDefault<string>("criticaldamageMax"), x, u, d);

        ACCr = ResolveExpression(property.ResolveOrDefault<string>("accR"), x, u, d);
        EVAr = ResolveExpression(property.ResolveOrDefault<string>("evaR"), x, u, d);
        Ar = ResolveExpression(property.ResolveOrDefault<string>("ar"), x, u, d);
        Er = ResolveExpression(property.ResolveOrDefault<string>("er"), x, u, d);

        PDDr = ResolveExpression(property.ResolveOrDefault<string>("pddR"), x, u, d);
        MDDr = ResolveExpression(property.ResolveOrDefault<string>("mddR"), x, u, d);
        PDr = ResolveExpression(property.ResolveOrDefault<string>("pdr"), x, u, d);
        MDr = ResolveExpression(property.ResolveOrDefault<string>("mdr"), x, u, d);

        DIPr = ResolveExpression(property.ResolveOrDefault<string>("damR"), x, u, d);

        PDamr = ResolveExpression(property.ResolveOrDefault<string>("pdR"), x, u, d);
        MDamr = ResolveExpression(property.ResolveOrDefault<string>("mdR"), x, u, d);

        PADr = ResolveExpression(property.ResolveOrDefault<string>("padR"), x, u, d);
        MADr = ResolveExpression(property.ResolveOrDefault<string>("madR"), x, u, d);

        EXPr = ResolveExpression(property.ResolveOrDefault<string>("expR"), x, u, d);

        Dot = ResolveExpression(property.ResolveOrDefault<string>("dot"), x, u, d);
        DotInterval = ResolveExpression(property.ResolveOrDefault<string>("dotInterval"), x, u, d);
        DotTime = ResolveExpression(property.ResolveOrDefault<string>("dotTime"), x, u, d);

        //IMPr = ResolveExpression(property.ResolveOrDefault<string>("ignoreMobpdpR"), x, u, d);
        ASRr = ResolveExpression(property.ResolveOrDefault<string>("asrR"), x, u, d);
        TERr = ResolveExpression(property.ResolveOrDefault<string>("terR"), x, u, d);

        MESOr = ResolveExpression(property.ResolveOrDefault<string>("mesoR"), x, u, d);

        PADx = ResolveExpression(property.ResolveOrDefault<string>("padX"), x, u, d);
        MADx = ResolveExpression(property.ResolveOrDefault<string>("madX"), x, u, d);

        IMDr = ResolveExpression(property.ResolveOrDefault<string>("ignoreMobpdpR"), x, u, d);

        PsdJump = ResolveExpression(property.ResolveOrDefault<string>("psdJump"), x, u, d);
        PsdSpeed = ResolveExpression(property.ResolveOrDefault<string>("psdSpeed"), x, u, d);

        OCr = ResolveExpression(property.ResolveOrDefault<string>("overChargeR"), x, u, d);
        DCr = ResolveExpression(property.ResolveOrDefault<string>("disCountR"), x, u, d);

        ReqGL = ResolveExpression(property.ResolveOrDefault<string>("reqGuildLevel"), x, u, d);

        Price = ResolveExpression(property.ResolveOrDefault<string>("price"), x, u, d);

        S = ResolveExpression(property.ResolveOrDefault<string>("s"), x, u, d);
        U = ResolveExpression(property.ResolveOrDefault<string>("u"), x, u, d);
        V = ResolveExpression(property.ResolveOrDefault<string>("v"), x, u, d);
        W = ResolveExpression(property.ResolveOrDefault<string>("w"), x, u, d);
        T = ResolveExpression(property.ResolveOrDefault<string>("t"), x, u, d);
    }

    private static short ResolveExpression(string? expression, params PrimitiveElement[] elements)
    {
        if (expression == null) return 0;
        return (short)new Expression(expression, elements).calculate();
    }
}
