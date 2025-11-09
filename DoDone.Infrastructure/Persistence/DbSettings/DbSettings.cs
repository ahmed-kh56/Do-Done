using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Asn1.Ess;
using System.Data;

namespace DoDone.Infrastructure.Persistence.DbSettings
{
    public class DbSettings :IDbSettings
    {
        public string ConnectionString {  get; set; }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}