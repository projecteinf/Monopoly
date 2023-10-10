namespace mba.Monopoly {
    public partial class BoughtStreets {
        public void Build(int numHouses,int numHotels) {
            this.numHouses += numHouses;
            this.numHotels += numHotels;
        }
    }
}