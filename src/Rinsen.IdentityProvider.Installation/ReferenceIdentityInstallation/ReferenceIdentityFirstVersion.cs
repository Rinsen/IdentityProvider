using Rinsen.DatabaseInstaller;
using Rinsen.DatabaseInstaller.Sql.Generic;
using Rinsen.IdentityProvider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Installation.ReferenceIdentityInstallation
{
    public class ReferenceIdentityFirstVersion : DatabaseVersion
    {
        public ReferenceIdentityFirstVersion()
            : base(1)
        {
            
        }

        public override void AddDbChanges(List<IDbChange> dbChangeList)
        {
            var referenceIdentityTable =  dbChangeList.AddNewTable<ReferenceIdentity>();

            referenceIdentityTable.AddColumn(m => m.IdentityId);
            referenceIdentityTable.AddColumn(m => m.Created);
        }
    }
}
