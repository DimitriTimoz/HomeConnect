using System;
using System.Collections.Generic;
using System.Text;

namespace HomeConnect.Models
{
    public class RoomModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } 
        public RoomType.Type type { get; set; }

     
    }

    public class RoomType
    {
        public enum Type
        {
            ROOM,
            LIVINGROOM,
            BATHROOM,
            TOILET,
            KITCHEN,
            BASEMENT,
            ENTRANCE,
            DINNINGROOM,
            GARAGE
        }
    }

}
