using Microsoft.EntityFrameworkCore;

namespace Edelstein.Data.Context
{
    public class MySQLDataContextFactory : IDataContextFactory
    {
        private readonly string _connection;

        public MySQLDataContextFactory(string connection)
            => _connection = connection;

        public DataContext Create()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();

            builder.UseMySql(_connection);
            return new DataContext(builder.Options);
        }
    }
}