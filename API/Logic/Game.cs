using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mba.Monopoly {
    public partial class Game {

        internal bool boughtAllGroup( 
            List<PlayerInterchanges> interchanges,
            List<BoughtStreets> lBoughtStreetObj,
            List<StreetGroup> streetGroups) {   
            
            foreach (StreetGroup streetGroup in streetGroups) {
                if (!HasStreet(interchanges, lBoughtStreetObj,streetGroup.Name)) return false;
            }
                
            
            if (!HasStreet(interchanges, lBoughtStreetObj,streetGroups[0].NameR)) return false;
            else return true;
        }

        internal void Pay(decimal price) => this.Money -= price;
        
        internal bool HasStreet(List<PlayerInterchanges> interchanges, ICollection<BoughtStreets>? lBoughtStreetObj, string streetName)
        {
            Console.WriteLine($"********************************* Interchanges: {interchanges.Count}");
            interchanges=interchanges.FindAll(pi => pi.StreetName==streetName);
            if (interchanges.Count > 0) {
                PlayerInterchanges lastInterchange = interchanges.OrderByDescending(pi => pi.InterchangeDateTime).First();
                if (lastInterchange!=null) 
                {
                    Console.WriteLine($"lastInterchange.BuyerPlayerName: {lastInterchange.BuyerPlayerName} == {this.PlayerName}");
                    return lastInterchange.BuyerPlayerName == this.PlayerName;
                }
                    
                else return false;
            } 
            else {
                if (lBoughtStreetObj == null) return false;
                else return lBoughtStreetObj.ToList().Find(bs => bs.StreetName == streetName) != null;
            }     
        }
        internal bool EnoughMoney(Street street) => this.Money >= street.Price;
        
        internal bool EnoughMoney(BoughtStreets boughtStreet, List<EstatePrices> estatePrices) =>
            this.Money>= estatePrices.Find(ep => ep.numberOfHotels==boughtStreet.numHotels &&
                                            ep.numberOfHouses==boughtStreet.numHouses)!.Price;
            
        internal bool EnoughMoney(decimal money) {
            Console.WriteLine($"EnoughMoney: {this.Money} >= {money}");
            return this.Money < money;
        }
    }
}