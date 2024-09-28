using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSet : IRepositoryEntry<string>, IDisposable, IFieldObjectPool
{
    Task Initialize(IFieldManager manager);
    
    Task OnUserEnter(IFieldUser user);
    Task OnUserLeave(IFieldUser user);
    
    Task OnUserMigrate(IFieldUser user, IField from, IField to);
    
    Task Reset();
}
