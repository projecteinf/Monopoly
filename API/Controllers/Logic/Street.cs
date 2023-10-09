using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mba.Monopoly {
    public partial class Street {
        internal bool Sold(List<BoughtStreets> boughtStreets)
        {
            return boughtStreets.Find(bs => bs.StreetName == this.Name) != null;
        }
    }
}
