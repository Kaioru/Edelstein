using System;

namespace Edelstein.Core.Services.Info
{
    [Serializable]
    public class GameServiceInfo : ServerServiceInfo
    {
        public byte WorldID { get; set; }
        public bool AdultChannel { get; set; }
    }
}