using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider
{
    public class ExternalApplicationStorage : IExternalApplicationStorage
    {
        private readonly IdentityOptions _identityOptions;

        private const string _createSql = @"INSERT INTO ExternalApplications (
                                                Active,
                                                ExternalApplicationId,
                                                HostName, 
                                                Password) 
                                            VALUES (
                                                @Active,
                                                @ExternalApplicationId,
                                                @HostName,
                                                @Password); 
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string _getSql = @"SELECT 
                                            Active,
                                            ClusteredId,
                                            ExternalApplicationId,
                                            HostName, 
                                            Password
                                        FROM 
                                            ExternalApplications 
                                        WHERE 
                                            HostName=@HostName";

        private const string _getAllSql = @"SELECT 
                                                Active,
                                                ClusteredId,
                                                ExternalApplicationId,
                                                HostName, 
                                                Password
                                            FROM 
                                                ExternalApplications";

        public ExternalApplicationStorage(IdentityOptions identityOptions)
        {
            _identityOptions = identityOptions;
        }

        public async Task CreateAsync(ExternalApplication externalApplication)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            {
                try
                {
                    using (var command = new SqlCommand(_createSql, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Active", externalApplication.Active));
                        command.Parameters.Add(new SqlParameter("@ClusteredId", externalApplication.ClusteredId));
                        command.Parameters.Add(new SqlParameter("@ExternalApplicationId", externalApplication.ExternalApplicationId));
                        command.Parameters.Add(new SqlParameter("@HostName", externalApplication.HostName));
                        command.Parameters.Add(new SqlParameter("@Password", externalApplication.Password));
                        connection.Open();

                        externalApplication.ClusteredId = (int)await command.ExecuteScalarAsync();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        throw new ExternalApplicationAlreadyExistException($"External application {externalApplication.HostName} already exist", ex);
                    }
                    throw;
                }
            }
        }

        public async Task DeleteAsync(ExternalApplication externalApplication)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExternalApplication>> GetAllAsync()
        {
            var result = new List<ExternalApplication>();

            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            {
                using (var command = new SqlCommand(_getAllSql, connection))
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(MapExternalApplication(reader));
                        }
                    }
                }
            }

            return result;
        }
        

        public async Task<ExternalApplication> GetAsync(string host)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            {
                using (var command = new SqlCommand(_getSql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@HostName", host));
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return MapExternalApplication(reader);
                        }
                    }
                }
            }

            return default(ExternalApplication);
        }

        private static ExternalApplication MapExternalApplication(SqlDataReader reader)
        {
            return new ExternalApplication
            {
                Active = (bool)reader["Active"],
                ClusteredId = (int)reader["ClusteredId"],
                ExternalApplicationId = (Guid)reader["ExternalApplicationId"],
                HostName = (string)reader["HostName"],
                Password = (string)reader["Password"]
            };
        }

        public async Task UpdateAsync(ExternalApplication externalApplication)
        {
            throw new NotImplementedException();
        }
    }
}
