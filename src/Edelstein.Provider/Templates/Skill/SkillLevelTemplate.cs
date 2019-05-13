using org.mariuszgromada.math.mxparser;

namespace Edelstein.Provider.Templates.Skill
{
    public class SkillLevelTemplate : ITemplate
    {
        public int ID { get; }

        public short HP { get; set; }
        public short MP { get; set; }
        public short PAD { get; set; }
        public short PDD { get; set; }
        public short MAD { get; set; }
        public short MDD { get; set; }
        public short ACC { get; set; }
        public short EVA { get; set; }
        public short Craft { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public short Morph { get; set; }
        public short HPCon { get; set; }
        public short MPCon { get; set; }
        public short MoneyCon { get; set; }
        public short ItemCon { get; set; }
        public short ItemConNo { get; set; }
        public short Damage { get; set; }
        public short FixDamage { get; set; }
        public short SelfDesctruction { get; set; }

        public short Time { get; set; }
        public short SubTime { get; set; }

        public short Prop { get; set; }
        public short SubProp { get; set; }

        public short AttackCount { get; set; }
        public short BulletCount { get; set; }
        public short BulletConsume { get; set; }
        public short Mastery { get; set; }
        public short MobCount { get; set; }

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        public short EMHP { get; set; }
        public short EMMP { get; set; }
        public short EPAD { get; set; }
        public short EMAD { get; set; }
        public short EPDD { get; set; }
        public short EMDD { get; set; }

        public SkillLevelTemplate(int id, IDataProperty property)
        {
            ID = id;

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
        }

        private static short ResolveExpression(string expression, params PrimitiveElement[] elements)
        {
            if (expression == null) return 0;
            return (short) new Expression(expression, elements).calculate();
        }
    }
}