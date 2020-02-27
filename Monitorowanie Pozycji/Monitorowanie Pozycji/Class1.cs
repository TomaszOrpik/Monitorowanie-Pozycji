using System;
using System.Collections.Generic;

namespace Monitorowanie_Pozycji
{
    public class Player
    {
        private int _id;
        private List<Location> _locationList;

        public Player(int id, Location location)
        {
            _id = id;
            _locationList.Add(location);
        }

        public void NewLocationList(List<Location> locationList)
        {
            _locationList = locationList;
        }

        public List<Location> GetPlayerLocationList => _locationList;
    }

    public class Tracker
    {
        private List<Location> _locationList;
        
        public void OnLocationChange(Location location)
        {
            _locationList.Add(location);
        }

        public List<Location> GetLocations() => _locationList; 
    }

    public class Location
    {
        int _x { get; set; }
        int _y { get; set; }
        DateTime _time { get; set; }
    }
}
