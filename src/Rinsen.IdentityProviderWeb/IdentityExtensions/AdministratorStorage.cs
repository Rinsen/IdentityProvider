using Rinsen.IdentityProvider.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.IdentityExtensions
{
    public class AdministratorStorage
    {
        private readonly IdentityOptions _options;

        private const string _getSql = @"SELECT 
                                            IdentityId
                                        FROM 
                                            Administrators 
                                        WHERE 
                                            IdentityId=@IdentityId";

        public AdministratorStorage(IdentityOptions options)
        {
            _options = options;
        }

        internal async Task<Administrator> GetAsync(Guid identityId)
        {
            using (var connection = new SqlConnection(_options.ConnectionString))
            {
                using (var command = new SqlCommand(_getSql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdentityId", identityId));
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return new Administrator
                            {
                                IdentityId = (Guid)reader["IdentityId"]
                            };
                        }
                    }
                }
            }

            return default(Administrator);
        }
    }
}
