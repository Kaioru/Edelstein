using Microsoft.EntityFrameworkCore;

namespace Edelstein.Database.Factory
{
    public class InMemoryDataContextFactory : IDataContextFactory
    {
        private readonly string _connection;

        public InMemoryDataContextFactory(string connection)
            => _connection = connection;

        public DataContext Build()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();

            builder.UseInMemoryDatabase(_connection);
            return new DataContext(builder.Options);
        }
    }
}