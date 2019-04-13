using Microsoft.EntityFrameworkCore;

namespace Edelstein.Database.Factory
{
    public class MySQLDataContextFactory : IDataContextFactory
    {
        private readonly string _connection;

        public MySQLDataContextFactory(string connection)
            => _connection = connection;

        public DataContext Build()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();

            builder.UseMySql(_connection);
            return new DataContext(builder.Options);
        }
    }
}