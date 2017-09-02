using System.Data;

namespace Refactor_me.Data.Interfaces
{
    public interface IConnectionCreator
    {
        IDbConnection GetOpenConnection();
    }
}
