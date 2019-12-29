using System.Collections.Generic;
using Edelstein.Core.Services.Distributed.States;

namespace Edelstein.Service.All.Service
{
    public class ContainerServiceState
    {
        public ICollection<LoginServiceState> LoginServices { get; set; }
        public ICollection<GameServiceState> GameServices { get; set; }
    }
}