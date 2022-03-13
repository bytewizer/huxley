namespace Bytewizer.TinyCLR.Boards.Huxley.Models
{
#pragma warning disable IDE1006 // Naming Styles
    public class CardTimeModel
    {
        public long time { get; set; }
        public string area { get; set; }
        public string zone { get; set; }
        public int minutes { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
    }
}