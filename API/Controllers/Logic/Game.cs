using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Game {
        internal bool HasStreet(List<PlayerInterchanges> interchanges, ICollection<BoughtStreets>? lBoughtStreetObj, string streetName)
        {
            if (interchanges.Count > 0) {
                PlayerInterchanges lastInterchange = interchanges.OrderByDescending(pi => pi.InterchangeDateTime).First();
                if (lastInterchange!=null) return lastInterchange.BuyerPlayerName == this.PlayerName;
                else return false;
            } 
            else {
                if (LBoughtStreetObj == null) return false;
                else return LBoughtStreetObj.ToList().Find(bs => bs.StreetName == streetName) != null;
            }     
        }
        internal bool HasAvailable(Street street) {
            return this.Money >= street.Price;
        }
    }
}