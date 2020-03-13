using System;

namespace Edelstein.Core.Gameplay.Social.Party
{
    public class PartyException : Exception
    {
        public PartyException(string message) : base(message)
        {
        }
    }
}