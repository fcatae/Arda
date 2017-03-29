using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arda.Kanban.Models;
using System.Data.SqlClient;

namespace IntegrationTests
{
    public class TransactionalKanbanContext : KanbanContext, IDisposable
    {
        private SqlConnection _connection;

        public static TransactionalKanbanContext Create(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand sqlcmd = new SqlCommand("SET IMPLICIT_TRANSACTIONS ON", connection);
            sqlcmd.ExecuteNonQuery();

            var opts = (new DbContextOptionsBuilder<KanbanContext>())
                            .UseSqlServer(connection);

            return new TransactionalKanbanContext(opts.Options, connection);
        }

        public TransactionalKanbanContext(DbContextOptions<KanbanContext> options, SqlConnection connection) : base(options)
        {
            this._connection = connection;
        }

        public new void Dispose()
        {
            if(_connection != null)
            {
                try
                {
                    SqlCommand rollbackCmd = new SqlCommand("ROLLBACK", _connection);
                    rollbackCmd.ExecuteNonQuery();
                }
                catch { }

                _connection.Dispose();
                _connection = null;
            }

            base.Dispose();
        }        
    }
}
