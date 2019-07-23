using System.Drawing;
using Edelstein.Core.Extensions;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Inventories;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Quests;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Effects;
using Edelstein.Service.Game.Fields.Objects.User.Effects.Field;
using Edelstein.Service.Game.Fields.Objects.User.Effects.User;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types;
using MoonSharp.Interpreter.Interop;

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

        public short Str
        {
            get => Obj.Character.STR;
            set => Obj.ModifyStats(s => s.STR = value).Wait();
        }

        public short Dex
        {
            get => Obj.Character.DEX;
            set => Obj.ModifyStats(s => s.DEX = value).Wait();
        }

        public short Int
        {
            get => Obj.Character.INT;
            set => Obj.ModifyStats(s => s.INT = value).Wait();
        }

        public short Luk
        {
            get => Obj.Character.LUK;
            set => Obj.ModifyStats(s => s.LUK = value).Wait();
        }

        public int Hp
        {
            get => Obj.Character.HP;
            set => Obj.ModifyStats(s => s.HP = value).Wait();
        }

        public int MaxHp
        {
            get => Obj.Character.MaxHP;
            set => Obj.ModifyStats(s => s.MaxHP = value).Wait();
        }

        public int Mp
        {
            get => Obj.Character.MP;
            set => Obj.ModifyStats(s => s.MP = value).Wait();
        }

        public int MaxMp
        {
            get => Obj.Character.MaxMP;
            set => Obj.ModifyStats(s => s.MaxMP = value).Wait();
        }

        public short Ap
        {
            get => Obj.Character.AP;
            set => Obj.ModifyStats(s => s.AP = value).Wait();
        }

        public short Sp
        {
            get => Obj.Character.SP;
            set
            {
                Obj.Message(new IncSPMessage(Job, (byte) (value - Money))).Wait();
                Obj.ModifyStats(s => s.SP = value).Wait();
            }
        }

        public byte GetExtendSp(byte jobLevel)
            => Obj.Character.GetExtendSP(jobLevel);

        public void SetExtendSp(byte jobLevel, byte sp)
        {
            Obj.Message(new IncSPMessage(Job, (byte) (sp - GetExtendSp(jobLevel)))).Wait();
            Obj.ModifyStats(s => s.SetExtendSP(jobLevel, sp)).Wait();
        }

        public int Exp
        {
            get => Obj.Character.EXP;
            set
            {
                Obj.Message(new IncEXPMessage
                {
                    EXP = value - Exp,
                    OnQuest = true
                }).Wait();
                Obj.ModifyStats(s => s.EXP = value).Wait();
            }
        }

        public short Pop
        {
            get => Obj.Character.POP;
            set
            {
                Obj.Message(new IncPOPMessage(value - Pop)).Wait();
                Obj.ModifyStats(s => s.POP = value).Wait();
            }
        }

        public int Money
        {
            get => Obj.Character.Money;
            set
            {
                Obj.Message(new IncMoneyMessage(value - Money)).Wait();
                Obj.ModifyStats(s => s.Money = value).Wait();
            }
        }

        public int TempExp
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


        public bool DirectionMode
        {
            get => Obj.DirectionMode;
            set => Obj.DirectionMode = value;
        }

        public bool StandAloneMode
        {
            get => Obj.StandAloneMode;
            set => Obj.StandAloneMode = value;
        }

        public void Message(string text)
            => Obj.Message(text).Wait();

        public void Converse(string script, ISpeaker self, ISpeaker target)
            => Obj.Service.ConversationManager.Build(
                script,
                Context,
                self,
                target
            ).Result.Start().Wait();

        public void BalloonMessage(string text, int width, int height)
            => Obj.BalloonMessage(text, new Size(width, height)).Wait();

        public void HireTutor(bool spawn)
            => Obj.HireTutor(spawn).Wait();

        public void TutorMessage(int idx, int duration)
            => Obj.TutorMessage(idx, duration).Wait();

        public void TutorMessage(string text, int width, int duration)
            => Obj.TutorMessage(text, width, duration).Wait();

        public void Effect(EffectType type, bool remote = true)
            => Obj.Effect(new Effect(type), true, remote).Wait();

        public void PlayPortalSoundEffect()
            => Obj.Effect(EffectType.PlayPortalSE);

        public void SquibEffect(string path)
            => Obj.Effect(new SquibEffect(path)).Wait();

        public void ReservedEffect(string path)
            => Obj.Effect(new ReservedEffect(path)).Wait();

        public void AvatarOrientedEffect(string path)
            => Obj.Effect(new AvatarOrientedEffect(path)).Wait();

        public void ScreenFieldEffect(string path)
            => Obj.Effect(new ScreenFieldEffect(path)).Wait();
    }
}