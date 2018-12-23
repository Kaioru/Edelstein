using System.Threading.Tasks;

namespace Edelstein.Core.Commands
{
    public interface ICommandSender
    {
        Task Message(string text);
    }
}