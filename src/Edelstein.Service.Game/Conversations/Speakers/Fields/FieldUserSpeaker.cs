using Edelstein.Service.Game.Conversations.Speakers.Fields.Inventories;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Quests;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public class FieldUserSpeaker : FieldObjSpeaker<FieldUser>
    {
        public override SpeakerParamType ParamType => SpeakerParamType.NPCReplacedByUser;

        public FieldUserSpeaker(IConversationContext context, FieldUser obj) : base(context, obj)
        {
        }

        public FieldUserInventorySpeaker GetInventory()
            => new FieldUserInventorySpeaker(Context, Obj);

        public QuestSpeaker GetQuest(short questTemplateID, int npcTemplateID = 9010000)
            => new QuestSpeaker(Context, Obj, questTemplateID, npcTemplateID);

        public byte Gender => Obj.Character.Gender;

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

        public byte GetExtendSP(byte jobLevel)
            => Obj.Character.GetExtendSP(jobLevel);

        public void SetExtendSP(byte jobLevel, byte sp)
            => Obj.ModifyStats(s => s.SetExtendSP(jobLevel, sp)).Wait();

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

        public int Field
        {
            get => Obj.Field.ID;
            set => TransferField(value);
        }

        public bool TransferField(int id, string portal = "")
        {
            var fieldManager = Obj.Service.FieldManager;
            var field = fieldManager.Get(id);

            if (field == null) return false;

            field.Enter(Obj, portal);
            return true;
        }

        public void Message(string text)
            => Obj.Message(text);

        public void Converse(string script, ISpeaker self, ISpeaker target)
            => Obj.Service.ConversationManager.Build(
                script,
                Context,
                self,
                target
            ).Result.Start().Wait();
    }
}