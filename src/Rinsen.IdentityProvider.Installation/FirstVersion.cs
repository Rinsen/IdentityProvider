using Rinsen.DatabaseInstaller;
using Rinsen.DatabaseInstaller.Sql.Generic;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.LocalAccounts;
using Rinsen.IdentityProvider.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Installation
{
    public class FirstVersion : DatabaseVersion
    {
        public FirstVersion()
            :base(1)
        {
        }

        public override void AddDbChanges(List<IDbChange> dbChangeList)
        {
            var identitiesTable = dbChangeList.AddNewTable<Identity>("Identities");
            identitiesTable.AddAutoIncrementColumn(m => m.Id, primaryKey: false);
            identitiesTable.AddColumn(m => m.IdentityId).PrimaryKey();
            identitiesTable.AddColumn(m => m.Created).NotNull();
            identitiesTable.AddColumn(m => m.Email).NotNull();
            identitiesTable.AddColumn(m => m.EmailConfirmed).NotNull();
            identitiesTable.AddColumn(m => m.GivenName).NotNull();
            identitiesTable.AddColumn(m => m.Surname).NotNull();
            identitiesTable.AddColumn(m => m.PhoneNumber).NotNull();
            identitiesTable.AddColumn(m => m.PhoneNumberConfirmed).NotNull();
            identitiesTable.AddColumn(m => m.Updated).NotNull();

            var localAccountsTable = dbChangeList.AddNewTable<LocalAccount>();
            localAccountsTable.AddAutoIncrementColumn(m => m.LocalAccountId);
            localAccountsTable.AddColumn(m => m.IdentityId).ForeignKey("Identities", "IdentityId").Unique().NotNull();
            localAccountsTable.AddColumn(m => m.Created).NotNull();
            localAccountsTable.AddColumn(m => m.FailedLoginCount).NotNull();
            localAccountsTable.AddColumn(m => m.IsDisabled).NotNull();
            localAccountsTable.AddColumn(m => m.IterationCount).NotNull();
            localAccountsTable.AddColumn(m => m.LoginId).NotNull();
            localAccountsTable.AddColumn(m => m.PasswordHash, 16).NotNull();
            localAccountsTable.AddColumn(m => m.PasswordSalt, 16).NotNull();
            localAccountsTable.AddColumn(m => m.Updated).NotNull();

            var sessionsTable = dbChangeList.AddNewTable<Session>("UserSessions");
            sessionsTable.AddColumn(m => m.Id).Unique();
            sessionsTable.AddColumn(m => m.IdentityId).ForeignKey("Identities", "IdentityId").NotNull();
            sessionsTable.AddColumn(m => m.LastAccess).NotNull();
            sessionsTable.AddColumn(m => m.SerializedTicket, length: 8000).NotNull();

        }
    }
}
