﻿using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class ItemCommand : AbstractTemplateCommand<IItemTemplate>
{
    public override string Name => "Item";
    public override string Description => "Creates a specified item";

    private readonly ITemplateManager<IItemStringTemplate> _strings;

    public ItemCommand(
        ITemplateManager<IItemTemplate> templates,
        ITemplateManager<IItemStringTemplate> strings
    ) : base(templates)
    {
        _strings = strings;

        Aliases.Add("Create");
    }

    protected override async Task<IEnumerable<TemplateCommandIndex>> Indices()
    {
        var result = new List<TemplateCommandIndex>();
        var strings = await _strings.RetrieveAll();

        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.ID.ToString(), s.Name)));
        result.AddRange(strings.Select(s => new TemplateCommandIndex(s.ID, s.Name, s.Name)));

        return result;
    }

    protected override async Task Execute(IFieldUser user, IItemTemplate template, TemplateCommandArgs args)
    {
        var quantity = 1;
        
        if (template is IItemBundleTemplate)
            quantity = await user.Prompt(s => s.AskNumber($"How many would you like?", 1), -1);
        if (quantity == -1) 
            return;
        
        await user.ModifyInventory(i => i.Add(template.ID, (short)quantity));
    }
}
