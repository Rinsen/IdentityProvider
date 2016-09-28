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
            identitiesTable.AddColumn(m => m.Created);
            identitiesTable.AddColumn(m => m.Email, 256);
            identitiesTable.AddColumn(m => m.EmailConfirmed);
            identitiesTable.AddColumn(m => m.GivenName, 128);
            identitiesTable.AddColumn(m => m.Surname, 128);
            identitiesTable.AddColumn(m => m.PhoneNumber, 128);
            identitiesTable.AddColumn(m => m.PhoneNumberConfirmed);
            identitiesTable.AddColumn(m => m.Updated);

            var localAccountsTable = dbChangeList.AddNewTable<LocalAccount>();
            localAccountsTable.AddAutoIncrementColumn(m => m.LocalAccountId);
            localAccountsTable.AddColumn(m => m.IdentityId).ForeignKey("Identities", "IdentityId").Unique().NotNull();
            localAccountsTable.AddColumn(m => m.Created);
            localAccountsTable.AddColumn(m => m.FailedLoginCount);
            localAccountsTable.AddColumn(m => m.IsDisabled);
            localAccountsTable.AddColumn(m => m.IterationCount);
            localAccountsTable.AddColumn(m => m.LoginId, 256);
            localAccountsTable.AddColumn(m => m.PasswordHash, 16);
            localAccountsTable.AddColumn(m => m.PasswordSalt, 16);
            localAccountsTable.AddColumn(m => m.Updated);

            var sessionsTable = dbChangeList.AddNewTable<Session>("UserSessions");
            sessionsTable.AddAutoIncrementColumn(m => m.Id, primaryKey: false);
            sessionsTable.AddColumn(m => m.SessionId, 60).PrimaryKey();
            sessionsTable.AddColumn(m => m.IdentityId).ForeignKey("Identities", "IdentityId");
            sessionsTable.AddColumn(m => m.LastAccess);
            sessionsTable.AddColumn(m => m.SerializedTicket);

        }
    }
}
