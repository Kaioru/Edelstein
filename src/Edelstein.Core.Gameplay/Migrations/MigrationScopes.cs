namespace Edelstein.Core.Gameplay.Migrations
{
    public static class MigrationScopes
    {
        public const string NodeMigration = "nodes:migrations";
        public const string NodeSocketCount = "nodes:socketCount";
        public const string NodeMigrationLock = "lock:node:migration";

        public const string StateAccount = "state:account";
        public const string StateCharacter = "state:character";
    }
}