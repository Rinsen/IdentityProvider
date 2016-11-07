using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class LocalIdentityForReferenceHandler
    {
        private readonly string _connectionString;

        private const string _getSql = @"";

        private const string _insertSql = @"";

        public LocalIdentityForReferenceHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateReferenceIdentityIfNotExists(ClaimsPrincipal claimsPrincipal)
        {
            var identityId = claimsPrincipal.GetClaimGuidValue(ClaimTypes.NameIdentifier);

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(_getSql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdentityId", identityId));
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        return;
                    }
                }

                using (var command = new SqlCommand(_insertSql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdentityId", identityId));
                    command.Parameters.Add(new SqlParameter("@Created", DateTimeOffset.Now));
                    connection.Open();

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
