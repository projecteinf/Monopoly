using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Licenses;

namespace mba.Monopoly
{
    public class StreetGroupConfiguration : IEntityTypeConfiguration<StreetGroup> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StreetGroup> builder)
        {
            builder.HasKey(sg => new { sg.Name, sg.NameR });
            builder.HasOne(sg=>sg.StreetObj)
                .WithMany(s=>s.LStreetGroupObj)
                .HasForeignKey(s=>s.Name).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sg=>sg.StreetObjR)
                .WithMany(s=>s.LStreetGroupObjR)
                .HasForeignKey(s=>s.NameR).OnDelete(DeleteBehavior.Restrict);


            List<StreetGroup> streetGroups = new List<StreetGroup>();
            string[] lines = File.ReadAllLines("Data/Street.csv");
            foreach (string line in lines.Skip(1))
            {
                string streetName = line.Split(",")[0];
                string relatedStreets = line.Split(",")[2];
                foreach (string relatedStreet in relatedStreets.Split(":"))
                {
                    streetGroups.Add(CrearStreetGroup(streetName, relatedStreet));
                }
            }
            builder.HasData(streetGroups);
        }
        private StreetGroup CrearStreetGroup(string streetName, string relatedStreet)
        {
            StreetGroup streetGroup = new StreetGroup();
            streetGroup.Name = streetName;
            streetGroup.NameR = relatedStreet;
            return streetGroup;
        }


    }
}