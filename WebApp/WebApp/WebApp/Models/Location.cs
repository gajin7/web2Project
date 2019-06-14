using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}