using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public interface ISupportSnapshot<R>
    {
        IEnumerable<R> GetSnapshot(TransactionalKanbanContext context);
    }

    public class TableSnapshot
    {
    }
}
