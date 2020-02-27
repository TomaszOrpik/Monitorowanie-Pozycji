using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Monitorowanie_Pozycji;

namespace Monitorowanie_Pozycji_Console_App
{
    class Program
    {
        public static bool _loop = true;

        static void Main(string[] args)
        {

            Field field = new Field();
            int playersCounter;
            List<Player> playersList = new List<Player>();
            List<Player> startPlayerPos = new List<Player>();
            Dictionary<Player, double> FinalPlayerDistance = new Dictionary<Player, double>();
            List<Thread> threads = new List<Thread>();

            Console.WriteLine("Type field width:");
            try { field.width = Convert.ToInt32(Console.ReadLine()); }
            catch (Exception) { Console.WriteLine("Invalid input"); }
            Console.WriteLine("Type field height:");
            field.height = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type number of players:");
            playersCounter = Convert.ToInt32(Console.ReadLine());
            for(int i = 0; i<playersCounter; i++)
            {

                Console.WriteLine($"Type x position of player {i + 1}");
                int x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine($"Type y position of player {i + 1}");
                int y = Convert.ToInt32(Console.ReadLine());
                DateTime time = DateTime.Now;
                playersList.Add(new Player(i + 1, new Location(x,y,time)));
                startPlayerPos.Add(new Player(i + 1, new Location(x,y,time)));
            }

            for(int i=0; i<field.height; i++)
            {
                for (int j = 0; j < field.width; j++)
                {
                    
                    //if sprawdzający czy lokacja istnieje w liście graczy
                    foreach(Player player in playersList)
                    {
                        if (i == player._location.x && j == player._location.y)
                            Console.Write("*");
                        else
                            Console.Write(".");
                    }
                }
                Console.Write("\n");
            }
            Console.WriteLine("Press enter to start moving players!");
            Console.WriteLine("Press enter to stop");
            Console.ReadKey();
            foreach(Player player in playersList)
            {
                Thread t = new Thread(() => PlayerMove(player));
                threads.Add(t);
                t.Start();
            }

            if(Console.ReadKey(true).Key == ConsoleKey.Enter) //while not press enter
            {
                _loop = false;
            }

            Console.WriteLine("New positions:");
            for (int i = 0; i < field.height; i++)
            {
                for (int j = 0; j < field.width; j++)
                {

                    //if sprawdzający czy lokacja istnieje w liście graczy
                    foreach (Player player in playersList)
                    {
                        if (i == player._location.x && j == player._location.y)
                            Console.Write("*");
                        else
                            Console.Write(".");
                    }
                }
                Console.Write("\n");
            }

        for(int i =0; i<playersList.Count - 1; i++)
            {
                
                double distance = Math.Sqrt(Math.Pow(startPlayerPos[i]._location.x - playersList[i]._location.x, 2.0) + Math.Pow(startPlayerPos[i]._location.y - playersList[i]._location.y, 2.0));
                FinalPlayerDistance.Add(playersList[i], distance);
            }

            double maxDistance = FinalPlayerDistance.Max(x => x.Value);
            foreach(var item in FinalPlayerDistance) 
                if(item.Value == maxDistance) 
                    Console.WriteLine($"Longest distance was runned by player with id {item.Key._id} and distance {item.Value}");

            Console.ReadKey();
        }

        static void trkr_LocationChanged(object seder, LocationChangeEventArgs args)
        {
            //location list add current location from args
            args.player._locationList.Add(args.location);
        }

        public static void PlayerMove(Player player)
        {
            
            while (_loop)
            {
                Tracker trkr = new Tracker(ref player);
                trkr.LocationChange += trkr_LocationChanged; //nadpisuje player locationList
                trkr.newLocation();
                Console.WriteLine($"Player {player._id} has moved to {player._location.x} horizontally and to {player._location.y} vertically at {player._location.time.ToString("hh:mm")}");
                Thread.Sleep(1000);
            }
        }

    }

    //process isn't looped
    //dla każdego gracza porównać init values z final values i który pokonał najwięcej

    //excetions for lower than zero
}
