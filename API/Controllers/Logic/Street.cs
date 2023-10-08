using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mba.Monopoly {
    public partial class Street {
        internal bool boughtAllGroup(List<BoughtStreets> boughtStreets, List<StreetGroup> streetGroups)
        {
            foreach(StreetGroup streetGroup in streetGroups)
                if(boughtStreets.Find(s => s.StreetName == streetGroup.Name) == null) return false;
                
            return true;
        }

        internal bool Sold(List<BoughtStreets> boughtStreets)
        {
            return boughtStreets.Find(bs => bs.StreetName == this.Name) != null;
        }
    }
}
