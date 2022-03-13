namespace Bytewizer.TinyCLR.Boards.Huxley.Models
{
#pragma warning disable IDE1006 // Naming Styles
    public class CardWirelessModel
    {
        public string status { get; set; }
        public int count { get; set; }
        public Net net { get; set; }
    }

    public class Net
    {
        public string iccid { get; set; }
        public string imsi { get; set; }
        public string imei { get; set; }
        public string modem { get; set; }
        public string band { get; set; }
        public string rat { get; set; }
        public int rssir { get; set; }
        public int rssi { get; set; }
        public int rsrp { get; set; }
        public int sinr { get; set; }
        public int rsrq { get; set; }
        public int bars { get; set; }
        public int mcc { get; set; }
        public int mnc { get; set; }
        public int lac { get; set; }
        public int cid { get; set; }
        public int updated { get; set; }
    }
}