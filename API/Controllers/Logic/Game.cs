using System.ComponentModel.DataAnnotations;

namespace mba.Monopoly {
    public partial class Game {

        internal bool boughtAllGroup(
            List<PlayerInterchanges> interchanges, 
            List<BoughtStreets> lBoughtStreetObj,
            List<StreetGroup> streetGroups) {   
            
            foreach (StreetGroup streetGroup in streetGroups) 
                if (!HasStreet(interchanges, lBoughtStreetObj,streetGroup.Name)) return false;
            
            if (!HasStreet(interchanges, lBoughtStreetObj,streetGroups[0].NameR)) return false;
            else return true;
        }


        internal bool HasStreet(List<PlayerInterchanges> interchanges, ICollection<BoughtStreets>? lBoughtStreetObj, string streetName)
        {
            if (interchanges.Count > 0) {
                PlayerInterchanges lastInterchange = interchanges.OrderByDescending(pi => pi.InterchangeDateTime).First();
                if (lastInterchange!=null) return lastInterchange.BuyerPlayerName == this.PlayerName;
                else return false;
            } 
            else {
                if (lBoughtStreetObj == null) return false;
                else return lBoughtStreetObj.ToList().Find(bs => bs.StreetName == streetName) != null;
            }     
        }
        internal bool HasAvailable(Street street) {
            return this.Money >= street.Price;
        }

        internal bool HasAvailableHouse(BoughtStreets boughtStreet, List<EstatePrices> estatePrices) {
            decimal price=estatePrices
                            .Find(ep => ep.numberOfHotels==boughtStreet.numHotels &&
                                        ep.numberOfHouses==boughtStreet.numHouses)
                            .Price;
            return this.Money >= price;
            
        }
    }
}