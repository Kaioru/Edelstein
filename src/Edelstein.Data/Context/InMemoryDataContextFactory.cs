using Microsoft.EntityFrameworkCore;

namespace Edelstein.Data.Context
{
    public class InMemoryDataContextFactory : IDataContextFactory
    {
        private readonly string _connection;

        public InMemoryDataContextFactory(string connection)
            => _connection = connection;

        public DataContext Create()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            
            builder.UseInMemoryDatabase(_connection);
            return new DataContext(builder.Options);
        }
    }
}