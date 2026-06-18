using System.Data;

namespace MoviePlatform.Application.Common.Data;

public interface ISqlConnectionFactory
{
	IDbConnection CreateConnection();
}
