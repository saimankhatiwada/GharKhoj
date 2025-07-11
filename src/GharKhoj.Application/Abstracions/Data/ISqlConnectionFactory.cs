using System.Data;

namespace GharKhoj.Application.Abstracions.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
