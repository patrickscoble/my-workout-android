namespace MySports.Models.MiniGolf
{
    public class Hole
    {
        public int Id { get; set; }

        public int RoundId { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }
        
        public int Par { get; set; }

        public int PlayerOneScore { get; set; }

        public int PlayerTwoScore { get; set; }
    }
}
