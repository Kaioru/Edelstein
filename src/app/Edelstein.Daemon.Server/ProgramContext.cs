using Edelstein.Common.Util.Templates;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

namespace Edelstein.Daemon.Server;

public record ProgramContext(
    IEnumerable<ITemplateLoader> TemplateLoaders,
    ILoginContextTemplates LoginTemplates
);
