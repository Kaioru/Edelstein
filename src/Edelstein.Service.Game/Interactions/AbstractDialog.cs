using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions
{
    public abstract class AbstractDialog : IDialog
    {
        protected FieldUser User { get; }

        protected AbstractDialog(FieldUser user)
        {
            User = user;
        }

        public abstract Task Enter();
        public abstract Task Leave();
    }
}