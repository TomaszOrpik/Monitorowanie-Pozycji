using System;
using System.Collections.Generic;

namespace Monitorowanie_Pozycji
{
    public class Player
    {
        public int _id;
        public Location _location;
        public List<Location> _locationList = new List<Location>();

        public Player(int id, Location location)
        {
            _id = id;
            _location = location;
            _locationList.Add(location);
        }

    }

    public class Tracker
    {
        private Player _player;

        public Tracker(ref Player player)
        {
            _player = player;
        }

        public void newLocation()
        {
            Location location = _player._location;
            Random rnd = new Random();
            int x = location.x;
            int y = location.y;

            location.x = location.x + rnd.Next(-1, 3);
            location.y = location.y + rnd.Next(-1, 3);
            location.time = location.time.AddMinutes(1);
            _player._location = location;

            if (location.x == 0 || location.y == 0)
            {
                Console.WriteLine($"Player {_player._id} Run out!");
                return;
            }

            if (x != location.x || y != location.y)
            {
                ///event handler
                LocationChangeEventArgs args = new LocationChangeEventArgs();
                args.location = location;
                args.player = _player;
                OnLocationChange(args);
            }
        }

        protected virtual void OnLocationChange(LocationChangeEventArgs args)
        {
            LocationChange?.Invoke(this, args);
        }

        public event EventHandler<LocationChangeEventArgs> LocationChange;

        public Player GetPlayer() => _player;
    }

    public class LocationChangeEventArgs : EventArgs
    {
        public Location location { get; set; }
        public Player player { get; set; }
    }

    public class Location
    {
        public int x { get; set; }
        public int y { get; set; }
        public DateTime time { get; set; }

        public Location(int x, int y, DateTime time)
        {
            this.time = time;
            this.x = x;
            this.y = y;
        }
    }

    public class Field
    {
        public int width { get; set; }
        public int height { get; set; }
    }
}
