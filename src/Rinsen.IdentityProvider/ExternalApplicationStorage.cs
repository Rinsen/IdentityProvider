using Rinsen.IdentityProvider;
using Rinsen.IdentityProvider.ExternalApplications;
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
                                                ActiveUntil,
                                                ExternalApplicationId,
                                                HostName, 
                                                ApplicationKey) 
                                            VALUES (
                                                @Active,
                                                @ActiveUntil,
                                                @ExternalApplicationId,
                                                @HostName,
                                                @ApplicationKey); 
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string _getFromHostNameSql = @"SELECT 
                                            Active,
                                            ActiveUntil,
                                            ClusteredId,
                                            ExternalApplicationId,
                                            HostName, 
                                            ApplicationKey
                                        FROM 
                                            ExternalApplications 
                                        WHERE 
                                            HostName=@HostName";

        private const string _getFromApplicationKeySql = @"SELECT 
                                            Active,
                                            ActiveUntil,
                                            ClusteredId,
                                            ExternalApplicationId,
                                            HostName, 
                                            ApplicationKey
                                        FROM 
                                            ExternalApplications 
                                        WHERE 
                                            ApplicationKey=@ApplicationKey";

        private const string _getFromExternalApplicationIdSql = @"SELECT 
                                            Active,
                                            ActiveUntil,
                                            ClusteredId,
                                            ExternalApplicationId,
                                            HostName, 
                                            ApplicationKey
                                        FROM 
                                            ExternalApplications 
                                        WHERE 
                                            ExternalApplicationId=@ExternalApplicationId";

        private const string _getAllSql = @"SELECT 
                                                Active,
                                                ActiveUntil,
                                                ClusteredId,
                                                ExternalApplicationId,
                                                HostName, 
                                                ApplicationKey
                                            FROM 
                                                ExternalApplications";

        private const string _updateSql = @"UPDATE 
                                                ExternalApplications
                                            SET
                                                Active = @Active,
                                                ActiveUntil = @ActiveUntil,
                                                ApplicationKey=@ApplicationKey,
                                                HostName = @HostName
                                            WHERE 
                                                ExternalApplicationId = @ExternalApplicationId";

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
                        command.Parameters.Add(new SqlParameter("@ActiveUntil", externalApplication.ActiveUntil));
                        command.Parameters.Add(new SqlParameter("@ExternalApplicationId", externalApplication.ExternalApplicationId));
                        command.Parameters.Add(new SqlParameter("@HostName", externalApplication.Hostname));
                        command.Parameters.Add(new SqlParameter("@ApplicationKey", externalApplication.ApplicationKey));
                        connection.Open();

                        externalApplication.ClusteredId = (int)await command.ExecuteScalarAsync();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        throw new ExternalApplicationAlreadyExistException($"External application {externalApplication.Hostname} already exist", ex);
                    }
                    throw;
                }
            }
        }

        public Task DeleteAsync(ExternalApplication externalApplication)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExternalApplication>> GetAllAsync()
        {
            var result = new List<ExternalApplication>();

            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            using (var command = new SqlCommand(_getAllSql, connection))
            {
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(MapExternalApplication(reader));
                    }
                }
            }

            return result;
        }
        

        public async Task<ExternalApplication> GetFromHostAsync(string host)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            using (var command = new SqlCommand(_getFromHostNameSql, connection))
            {
                command.Parameters.Add(new SqlParameter("@HostName", host));
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        return MapExternalApplication(reader);
                    }
                }
            }

            return default(ExternalApplication);
        }

        public async Task<ExternalApplication> GetFromApplicationKeyAsync(string applicationKey)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            using (var command = new SqlCommand(_getFromApplicationKeySql, connection))
            {
                command.Parameters.Add(new SqlParameter("@ApplicationKey", applicationKey));
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        return MapExternalApplication(reader);
                    }
                }
            }

            return default(ExternalApplication);
        }

        public async Task<ExternalApplication> GetFromExternalApplicationIdAsync(Guid externalApplicationId)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            using (var command = new SqlCommand(_getFromExternalApplicationIdSql, connection))
            {
                command.Parameters.Add(new SqlParameter("@ExternalApplicationId", externalApplicationId));
                connection.Open();

                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        return MapExternalApplication(reader);
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
                ActiveUntil = (DateTimeOffset)reader["ActiveUntil"],
                ClusteredId = (int)reader["ClusteredId"],
                ExternalApplicationId = (Guid)reader["ExternalApplicationId"],
                Hostname = (string)reader["HostName"],
                ApplicationKey = (string)reader["ApplicationKey"]
            };
        }

        public async Task UpdateAsync(ExternalApplication externalApplication)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            using (var command = new SqlCommand(_updateSql, connection))
            {
                command.Parameters.Add(new SqlParameter("@Active", externalApplication.Active));
                command.Parameters.Add(new SqlParameter("@ActiveUntil", externalApplication.ActiveUntil));
                command.Parameters.Add(new SqlParameter("@ExternalApplicationId", externalApplication.ExternalApplicationId));
                command.Parameters.Add(new SqlParameter("@HostName", externalApplication.Hostname));
                command.Parameters.Add(new SqlParameter("@ApplicationKey", externalApplication.ApplicationKey));
                connection.Open();

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
