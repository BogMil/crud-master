using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace CrudMaster.UnitTests
{
    public class InMemoryDbTester<TContext> where TContext : DbContext, new()
    {
        public void UsingContext(Action<TContext> func)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                using (var context = CreateContext(connection))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = CreateContext(connection))
                {
                    func(context);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public TContext CreateContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<TContext>()
                    .UseSqlite(connection)
                    .Options;

            var context = Activator.CreateInstance(typeof(TContext),
                 new object[] { options }) as TContext;

            return context;
        }
    }
}
