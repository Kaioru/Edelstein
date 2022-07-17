using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextTemplates
{
    ITemplateManager<IWorldTemplate> World { get; }
}
