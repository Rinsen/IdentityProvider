using Rinsen.DatabaseInstaller;
using Rinsen.IdentityProviderWeb.IdentityExtensions;
using System.Collections.Generic;

namespace Rinsen.IdentityProviderWeb.Installation
{
    public class IdentityProviderWebFirstVersion : DatabaseVersion
    {
        public IdentityProviderWebFirstVersion() : base(1)
        {
        }

        public override void AddDbChanges(List<IDbChange> dbChangeList)
        {
            var administratorsTable = dbChangeList.AddNewTable<Administrator>();

            administratorsTable.AddColumn(m => m.IdentityId).PrimaryKey().ForeignKey("Identities", "IdentityId");
        }
    }
}
