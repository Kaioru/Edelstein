using System.Linq;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Conversations
{
    public class FieldUserSpeaker : AbstractFieldObjSpeaker<FieldUser>
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => ScriptMessageParam.NPCReplacedByUser;

        public FieldUserSpeaker(IConversationContext context, FieldUser obj) : base(context, obj)
        {
        }


        public byte Skin
        {
            get => Obj.Character.Skin;
            set => Obj.ModifyStats(s => s.Skin = value).Wait();
        }

        public int Face
        {
            get => Obj.Character.Face;
            set => Obj.ModifyStats(s => s.Face = value).Wait();
        }

        public int Hair
        {
            get => Obj.Character.Hair;
            set => Obj.ModifyStats(s => s.Hair = value).Wait();
        }

        public byte Level
        {
            get => Obj.Character.Level;
            set => Obj.ModifyStats(s => s.Level = value).Wait();
        }

        public short Job
        {
            get => Obj.Character.Job;
            set => Obj.ModifyStats(s => s.Job = value).Wait();
        }

        public short STR
        {
            get => Obj.Character.STR;
            set => Obj.ModifyStats(s => s.STR = value).Wait();
        }

        public short DEX
        {
            get => Obj.Character.DEX;
            set => Obj.ModifyStats(s => s.DEX = value).Wait();
        }

        public short INT
        {
            get => Obj.Character.INT;
            set => Obj.ModifyStats(s => s.INT = value).Wait();
        }

        public short LUK
        {
            get => Obj.Character.LUK;
            set => Obj.ModifyStats(s => s.LUK = value).Wait();
        }

        public int HP
        {
            get => Obj.Character.HP;
            set => Obj.ModifyStats(s => s.HP = value).Wait();
        }

        public int MaxHP
        {
            get => Obj.Character.MaxHP;
            set => Obj.ModifyStats(s => s.MaxHP = value).Wait();
        }

        public int MP
        {
            get => Obj.Character.MP;
            set => Obj.ModifyStats(s => s.MP = value).Wait();
        }

        public int MaxMP
        {
            get => Obj.Character.MaxMP;
            set => Obj.ModifyStats(s => s.MaxMP = value).Wait();
        }

        public short AP
        {
            get => Obj.Character.AP;
            set => Obj.ModifyStats(s => s.AP = value).Wait();
        }

        public short SP
        {
            get => Obj.Character.SP;
            set => Obj.ModifyStats(s => s.SP = value).Wait();
        }

        public int EXP
        {
            get => Obj.Character.EXP;
            set => Obj.ModifyStats(s => s.EXP = value).Wait();
        }

        public short POP
        {
            get => Obj.Character.POP;
            set => Obj.ModifyStats(s => s.POP = value).Wait();
        }

        public int Money
        {
            get => Obj.Character.Money;
            set => Obj.ModifyStats(s => s.Money = value).Wait();
        }

        public int TempEXP
        {
            get => Obj.Character.TempEXP;
            set => Obj.ModifyStats(s => s.TempEXP = value).Wait();
        }

        public bool TransferField(int id, string portal = "")
        {
            var fieldManager = Obj.Socket.WvsGame.FieldManager;
            var field = fieldManager.Get(id);

            if (field == null) return false;

            if (!string.IsNullOrEmpty(portal))
                Obj.Character.FieldPortal = (byte) (
                    field.Template.Portals.Values
                        .FirstOrDefault(p => p.Name.Equals(portal))?.ID ?? 0
                );
            else Obj.Character.FieldPortal = 0;

            field.Enter(Obj);
            return true;
        }
    }
}