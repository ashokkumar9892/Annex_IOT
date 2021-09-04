using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public class DataModels
    {
    }

    public class WeightDataModel
    {
        public string imei { get; set; }
        public long ts { get; set; }
        public int baMeryVoltage { get; set; }
        public int signalStrength { get; set; }
        public Values values { get; set; }
        public int rssi { get; set; }
        public long deviceId { get; set; }
    }

    public class Values
    {
        public int unit { get; set; }
        public int tare { get; set; }
        public int weight { get; set; }
    }
    public class DyanmoTableItems
    {
        public IEnumerable<Item> Items { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        // public string ActiveStatus { get; set; }
    }

}
