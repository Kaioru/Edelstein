using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using org.mariuszgromada.math.mxparser;

namespace Edelstein.Common.Gameplay.Users.Skills.Templates
{
    public record CharacterSkillLevelTemplate : ITemplate
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
        public short SelfDesctruction { get; }

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

        public short EMHP { get; }
        public short EMMP { get; }
        public short EPAD { get; }
        public short EMAD { get; }
        public short EPDD { get; }
        public short EMDD { get; }

        public CharacterSkillLevelTemplate(int id, IDataProperty property, bool resolveExpressions = true)
        {
            ID = id;

            if (resolveExpressions)
            {
                var x = new Argument("x", id);

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
                Time = ResolveExpression(property.ResolveOrDefault<string>("time"), x, u, d);
                X = ResolveExpression(property.ResolveOrDefault<string>("x"), x, u, d);
                Y = ResolveExpression(property.ResolveOrDefault<string>("y"), x, u, d);
                Z = ResolveExpression(property.ResolveOrDefault<string>("z"), x, u, d);
                EMHP = ResolveExpression(property.ResolveOrDefault<string>("emhp"), x, u, d);
                EMMP = ResolveExpression(property.ResolveOrDefault<string>("emmp"), x, u, d);
                EPAD = ResolveExpression(property.ResolveOrDefault<string>("epad"), x, u, d);
                EMAD = ResolveExpression(property.ResolveOrDefault<string>("emad"), x, u, d);
                EPDD = ResolveExpression(property.ResolveOrDefault<string>("epdd"), x, u, d);
                EMDD = ResolveExpression(property.ResolveOrDefault<string>("emdd"), x, u, d);
                return;
            }

            HP = (short)(property.Resolve<int>("hp") ?? 0);
            MP = (short)(property.Resolve<int>("mp") ?? 0);
            PAD = (short)(property.Resolve<int>("pad") ?? 0);
            PDD = (short)(property.Resolve<int>("pdd") ?? 0);
            MAD = (short)(property.Resolve<int>("mad") ?? 0);
            MDD = (short)(property.Resolve<int>("mdd") ?? 0);
            ACC = (short)(property.Resolve<int>("acc") ?? 0);
            EVA = (short)(property.Resolve<int>("eva") ?? 0);
            Craft = (short)(property.Resolve<int>("craft") ?? 0);
            Speed = (short)(property.Resolve<int>("speed") ?? 0);
            Jump = (short)(property.Resolve<int>("jump") ?? 0);
            Morph = (short)(property.Resolve<int>("morph") ?? 0);
            Time = (short)(property.Resolve<int>("time") ?? 0);
            X = (short)(property.Resolve<int>("x") ?? 0);
            Y = (short)(property.Resolve<int>("y") ?? 0);
            Z = (short)(property.Resolve<int>("z") ?? 0);
            EMHP = (short)(property.Resolve<int>("emhp") ?? 0);
            EMMP = (short)(property.Resolve<int>("emmp") ?? 0);
            EPAD = (short)(property.Resolve<int>("epad") ?? 0);
            EMAD = (short)(property.Resolve<int>("emad") ?? 0);
            EPDD = (short)(property.Resolve<int>("epdd") ?? 0);
            EMDD = (short)(property.Resolve<int>("emdd") ?? 0);
        }

        private static short ResolveExpression(string expression, params PrimitiveElement[] elements)
        {
            if (expression == null) return 0;
            return (short)new Expression(expression, elements).calculate();
        }
    }
}