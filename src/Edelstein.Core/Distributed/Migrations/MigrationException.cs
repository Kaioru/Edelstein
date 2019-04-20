using System;

namespace Edelstein.Core.Distributed.Migrations
{
    public class MigrationException : Exception
    {
        public MigrationException(string message) : base(message)
        {
        }
    }
}