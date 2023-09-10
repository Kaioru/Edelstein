using Edelstein.Common.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

public interface ISkillTemplateLevel
{
    short HP { get; }
    short MP { get; }

    short PAD { get; }
    short PDD { get; }
    short MAD { get; }
    short MDD { get; }
    short ACC { get; }
    short EVA { get; }
    short Craft { get; }

    short Speed { get; }
    short Jump { get; }

    short Morph { get; }

    short HPCon { get; }
    short MPCon { get; }
    short MoneyCon { get; }
    short ItemCon { get; }
    short ItemConNo { get; }

    short Damage { get; }
    short FixDamage { get; }

    short SelfDestruction { get; }

    short Time { get; }
    short SubTime { get; }

    short Prop { get; }
    short SubProp { get; }

    short AttackCount { get; }
    short BulletCount { get; }
    short BulletConsume { get; }

    short Mastery { get; }

    short MobCount { get; }

    short X { get; }
    short Y { get; }
    short Z { get; }

    short Action { get; }

    short EMHP { get; }
    short EMMP { get; }
    short EPAD { get; }
    short EPDD { get; }
    short EMDD { get; }

    short Range { get; }

    short Cooltime { get; }

    Rectangle2D Bounds { get; }

    short MHPr { get; }
    short MMPr { get; }

    short Cr { get; }
    short CDMin { get; }
    short CDMax { get; }

    short ACCr { get; }
    short EVAr { get; }
    short Ar { get; }
    short Er { get; }

    short PDDr { get; }
    short MDDr { get; }
    short PDr { get; }
    short MDr { get; }

    short DIPr { get; }

    short PDamr { get; }
    short MDamr { get; }

    short PADr { get; }
    short MADr { get; }

    short EXPr { get; }

    short Dot { get; }
    short DotInterval { get; }
    short DotTime { get; }

    short IMPr { get; }
    short ASRr { get; }
    short TERr { get; }

    short MESOr { get; }

    short PADx { get; }
    short MADx { get; }

    short IMDr { get; }

    short PsdJump { get; }
    short PsdSpeed { get; }

    short OCr { get; }
    short DCr { get; }

    short ReqGL { get; }

    short Price { get; }

    short S { get; }
    short U { get; }
    short V { get; }
    short W { get; }
    short T { get; }
}
