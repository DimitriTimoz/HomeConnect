using System;
using System.Collections.Generic;
using System.Text;

namespace HomeConnect.Models
{
    public class DeviceModel
    {

        public int id { get; set; }
        public int room { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
        public DeviceType.Type Type { get; set; }
    }

    public class DeviceType
    {
        public enum Type{
            THERMOMETER,
            RELAY
        }
    }
}
