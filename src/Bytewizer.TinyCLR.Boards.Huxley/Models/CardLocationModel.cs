namespace Bytewizer.TinyCLR.Boards.Huxley.Models
{
#pragma warning disable IDE1006 // Naming Styles
    public class CardLocationModel
    {
        public string status { get; set; }
        public string mode { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int time { get; set; }
        public int max { get; set; }
    }
}