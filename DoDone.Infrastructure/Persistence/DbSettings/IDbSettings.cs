using System.Data;

namespace DoDone.Infrastructure.Persistence.DbSettings
{
    public interface IDbSettings
    {
        public string ConnectionString { get; }

        IDbConnection CreateConnection();
    }
}
