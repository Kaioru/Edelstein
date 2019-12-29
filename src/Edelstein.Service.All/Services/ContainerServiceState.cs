using System.Collections.Generic;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Service.All.Services
{
    public class ContainerServiceState
    {
        public ICollection<LoginServiceState> LoginServices { get; set; }
        public ICollection<GameServiceState> GameServices { get; set; }
    }
}