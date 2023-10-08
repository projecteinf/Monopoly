using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mba.Monopoly {
    public partial class StreetGroup {
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string NameR { get; set; }
        public Street? StreetObj { get; set; }
        public Street? StreetObjR { get; set; }
        
    }
}
