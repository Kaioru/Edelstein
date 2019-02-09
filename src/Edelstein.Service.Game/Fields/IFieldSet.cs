using System.Collections.Generic;
using Edelstein.Provider.Templates.Server.FieldSet;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldSet : IField
    {
        FieldSetTemplate SetTemplate { get; }
        ICollection<IField> Fields { get; }
    }
}