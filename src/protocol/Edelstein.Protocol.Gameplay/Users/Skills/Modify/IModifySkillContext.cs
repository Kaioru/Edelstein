using System;

namespace Edelstein.Protocol.Gameplay.Users.Skills.Modify
{
    public interface IModifySkillContext
    {
        public void Add(int templateID, int increment = 1);
        public void Set(int templateID, int level, int? masterLevel = null, DateTime? dateExpire = null);

        //public int ResetByTemplate(int templateID);
        //public int ResetByJobLevel(int jobLevel);
        //public int ResetAll();
    }
}
