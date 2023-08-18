namespace MySports.Models.Tennis
{
    public class Match
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public string Date { get; set; }

        public string PlayerOne { get; set; }

        public string PlayerTwo { get; set; }
    }
}
