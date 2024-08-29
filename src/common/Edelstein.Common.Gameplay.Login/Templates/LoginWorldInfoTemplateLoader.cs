using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Templates;

public class LoginWorldInfoTemplateLoader(
    ILogger<AbstractTemplateLoader<LoginWorldInfoTemplate>> logger, 
    ITemplateManagerContext<LoginWorldInfoTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<LoginWorldInfoTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<LoginWorldInfoTemplate> context)
    {
        var directory = data.ResolvePath("Server/World.img");
        
        if (directory == null) return;
        await Task.WhenAll(directory.Select(n =>
        {
            var id = Convert.ToInt32(n.Name.Split(".")[0]);
            return context.Insert(new TemplateProviderLazy<LoginWorldInfoTemplate>(
                id, 
                () => new LoginWorldInfoTemplate(id, n.Cache())
            ));
        }));
    }
}
