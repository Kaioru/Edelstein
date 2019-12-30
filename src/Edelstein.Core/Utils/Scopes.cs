namespace Edelstein.Core.Utils
{
    public static class Scopes
    {
        public const string NodeSet = "nodes";
        public const string NodeMigration = "nodes:migrations";
        public const string NodeMessaging = "nodes:messaging";
        public const string NodeSocketCount = "nodes:socketCount";
        public const string NodeMigrationLock = "lock:node:migration";

        public const string StateAccount = "state:account";
        public const string StateCharacter = "state:character";
    }
}