namespace ReservationP.Models
{
    public class ReservationM : Base
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

    }
}
