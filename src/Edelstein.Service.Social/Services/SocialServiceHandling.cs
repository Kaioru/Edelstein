using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Core.Types;
using Edelstein.Service.Social.Logging;

namespace Edelstein.Service.Social.Services
{
    public partial class SocialService
    {
        private void HandleSocialUpdateState(SocialUpdateStateMessage message)
        {
            Logger.Debug($"{message.CharacterID} has {message.State} in {message.Service}");
        }

        private void HandleSocialUpdateLevel(SocialUpdateLevelMessage message)
        {
            Logger.Debug($"{message.CharacterID} is now level {message.Level}");
        }

        private void HandleSocialUpdateJob(SocialUpdateJobMessage message)
        {
            Logger.Debug($"{message.CharacterID} is now {(Job) message.Job}");
        }
    }
}