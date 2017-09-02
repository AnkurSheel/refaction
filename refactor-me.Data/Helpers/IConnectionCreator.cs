using System.Data;

namespace Refactor_me.Data.Helpers
{
    public interface IConnectionCreator
    {
        IDbConnection GetOpenConnection();
    }
}