using Rinsen.IdentityProvider.Core;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider
{
    public class IdentityStorage : IIdentityStorage
    {
        private readonly IdentityOptions _identityOptions;

        public IdentityStorage(IdentityOptions identityOptions)
        {
            _identityOptions = identityOptions;
        }

        public async Task CreateAsync(Identity identity)
        {
            string sql = @"INSERT INTO Identities (Created, Email, EmailConfirmed, FirstName, IdentityId, LastName, PhoneNumber, PhoneNumberConfirmed, Updated) VALUES (@Created, @Email, @EmailConfirmed, @FirstName, @IdentityId, @LastName, @PhoneNumber, @PhoneNumberConfirmed, @Updated); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            {
                try
                {
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Created", identity.Created));
                        command.Parameters.Add(new SqlParameter("@Email", identity.Email));
                        command.Parameters.Add(new SqlParameter("@EmailConfirmed", identity.EmailConfirmed));
                        command.Parameters.Add(new SqlParameter("@FirstName", identity.FirstName));
                        command.Parameters.Add(new SqlParameter("@IdentityId", identity.IdentityId));
                        command.Parameters.Add(new SqlParameter("@LastName", identity.LastName));
                        command.Parameters.Add(new SqlParameter("@PhoneNumber", identity.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@PhoneNumberConfirmed", identity.PhoneNumberConfirmed));
                        command.Parameters.Add(new SqlParameter("@Updated", identity.Updated));
                        
                        connection.Open();

                        identity.Id = (int)await command.ExecuteScalarAsync();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        throw new IdentityAlreadyExistException(string.Format("Identity {0} already exist", identity.IdentityId), ex);
                    }
                    throw;
                }
            }
        }
        
        public Identity Get(Guid identityId)
        {
            using (var connection = new SqlConnection(_identityOptions.ConnectionString))
            {
                using (var command = new SqlCommand("SELECT Created, Email, EmailConfirmed, FirstName, Id, IdentityId, LastName, PhoneNumber, PhoneNumberConfirmed, Updated FROM Users WHERE IdentityId=@IdentityId", connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdentityId", identityId));
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return new Identity
                            {
                                Created = (DateTimeOffset)reader["Created"],
                                Email = (string)reader["Email"],
                                EmailConfirmed = (bool)reader["EmailConfirmed"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Id = (int)reader["Id"],
                                IdentityId = (Guid)reader["IdentityId"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"],
                                Updated = (DateTimeOffset)reader["Updated"]
                            };
                        }
                    }
                }
            }

            return default(Identity);
        }

        private Identity MapIdentityFromReader(SqlDataReader reader)
        {
            return new Identity
            {
                Created = (DateTimeOffset)reader["Created"],
                Email = (string)reader["Email"],
                EmailConfirmed = (bool)reader["EmailConfirmed"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Id = (int)reader["Id"],
                IdentityId = (Guid)reader["IdentityId"],
                PhoneNumber = (string)reader["PhoneNumber"],
                PhoneNumberConfirmed = (bool)reader["PhoneNumberConfirmed"],
                Updated = (DateTimeOffset)reader["Updated"]
            };
        }
    }
}
