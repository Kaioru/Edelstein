using System;
using System.Collections.Generic;

namespace Edelstein.Core.Services.Info
{
    [Serializable]
    public class ShopServiceInfo : ServerServiceInfo
    {
        public ICollection<byte> Worlds { get; set; }
    }
}