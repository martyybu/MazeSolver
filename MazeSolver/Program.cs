using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Please specify the path of the file: ");
            // Reads the file
            var path = Console.ReadLine();
            var txtFile = System.IO.File.ReadAllText(path);

            // Displays the input
            //Console.WriteLine("Your input: {0}", txtFile);

            // Split the txt file into lines
            var lines = txtFile.Split(new[] { '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Creating variables, a list for the Map
            var Map = new List<String>();
            int intNr = 0, Height = 0, Width = 0, StartX = 0, StartY = 0, EndX = 0, EndY = 0;
            var mapDrawn = 0;

            // Loop all lines
            foreach (var line in lines)
            {
                // Split line by space and loop through all the numeric values
                foreach (var s in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (intNr == 0)
                    {
                        Width = Convert.ToInt16(s);
                        intNr++;
                    }
                    else if (intNr == 1)
                    {
                        Height = Convert.ToInt16(s);
                        intNr++;
                    }
                    else if (intNr == 2)
                    {
                        StartX = Convert.ToInt16(s);
                        intNr++;
                    }
                    else if (intNr == 3)
                    {
                        StartY = Convert.ToInt16(s);
                        intNr++;
                    }
                    else if (intNr == 4)
                    {
                        EndX = Convert.ToInt16(s);
                        intNr++;
                    }
                    else if (intNr == 5)
                    {
                        EndY = Convert.ToInt16(s);
                        intNr++;
                    }
                    else
                    {
                        //Adding integers from the text file to the map 
                        Map.Add(s);
                    }
                }
            }

            // Creating a 2D Array
            string[,] Array2D = new string[Width, Height];

            //Adding the map to 2D array
            for (var Col = 0; Col < Height; Col++)
            {
                for (var Row = 0; Row < Width; Row++)
                {
                    //Creating a 2D map
                    Array2D[Row, Col] = Map[mapDrawn];
                    if (Array2D[Row, Col] == "1")
                    {
                        Array2D[Row, Col] = "#";
                    }
                    else if (Array2D[Row, Col] == "0")
                    {
                        Array2D[Row, Col] = " ";
                    }
                    Array2D[StartX, StartY] = "X";
                    //Console.Write(Array2D[Row, Col]);
                    mapDrawn++;
                }
                //Console.Write("\n");
            }

            // Creating a walker and assigning starting coordinates to it
            int WalkerX = StartX;
            int WalkerY = StartY;

            //Creating checkpoint and final path coordinate lists
            var CheckPoint = new List<Coordinates>
            {
            };
            var FinalPath = new List<Coordinates>
            {
                new Coordinates(StartX, StartY)
            };

            // Check if there is more than one path to walk
            CheckForPath(WalkerX, WalkerY);

            // Letting the walker find a route to the destination
            while (WalkerX != EndX || WalkerY != EndY)
            {

                // Check if there are more than one path to walk
                CheckForPath(WalkerX, WalkerY);
                // Check for walls or walked paths, if they are empty, take a step and continue to checking the while statement
                // STEP RIGHT
                if (Array2D[WalkerX + 1, WalkerY] != "#" && Array2D[WalkerX + 1, WalkerY] != "X")
                {
                    // If bounds were reached (if the maze is wrapped)
                    if (WalkerX + 1 == Width - 1)
                    {
                        // Checks if there is a valid path through to the other side
                        if (Array2D[0, WalkerY] != "#" && Array2D[0, WalkerY] != "X" && Array2D[1, WalkerY] != "#" && Array2D[1, WalkerY] != "X")
                        {
                            Array2D[WalkerX + 1, WalkerY] = "X";
                            Array2D[0, WalkerY] = "X";
                            Array2D[1, WalkerY] = "X";
                            FinalPath.Add(new Coordinates(WalkerX + 1, WalkerY));
                            FinalPath.Add(new Coordinates(0, WalkerY));
                            FinalPath.Add(new Coordinates(1, WalkerY));
                            WalkerX = 1;
                            continue;
                        }
                        // If there is no valid path, closes the path by marking it as X (already checked path)
                        else
                        {
                            Array2D[WalkerX + 1, WalkerY] = "X";
                            continue;
                        }
                    }
                    // Takes a step
                    else
                    {
                        WalkerX += 1;
                        Array2D[WalkerX, WalkerY] = "X";
                        FinalPath.Add(new Coordinates(WalkerX, WalkerY));
                        continue;
                    }
                }
                // STEP DOWN
                if (Array2D[WalkerX, WalkerY + 1] != "#" && Array2D[WalkerX, WalkerY + 1] != "X")
                {
                    if (WalkerY + 1 == Height - 1)
                    {
                        if (Array2D[WalkerX, 0] != "#" && Array2D[WalkerX, 0] != "X" && Array2D[WalkerX, 1] != "#" && Array2D[WalkerX, 1] != "X")
                        {
                            Array2D[WalkerX, WalkerY + 1] = "X";
                            Array2D[WalkerX, 0] = "X";
                            Array2D[WalkerX, 1] = "X";
                            FinalPath.Add(new Coordinates(WalkerX, WalkerY + 1));
                            FinalPath.Add(new Coordinates(WalkerX, 0));
                            FinalPath.Add(new Coordinates(WalkerX, 1));
                            WalkerY = 1;
                            continue;
                        }
                        else
                        {
                            Array2D[WalkerX, WalkerY + 1] = "X";
                            continue;
                        }
                    }
                    else
                    {
                        WalkerY += 1;
                        Array2D[WalkerX, WalkerY] = "X";
                        FinalPath.Add(new Coordinates(WalkerX, WalkerY));
                        continue;
                    }
                }
                // STEP LEFT
                if (Array2D[WalkerX - 1, WalkerY] != "#" && Array2D[WalkerX - 1, WalkerY] != "X")
                {
                    if (WalkerX - 1 == 0)
                    {
                        if (Array2D[Width - 1, WalkerY] != "#" && Array2D[Width - 1, WalkerY] != "X" && Array2D[Width - 2, WalkerY] != "#" && Array2D[Width - 2, WalkerY] != "X")
                        {
                            Array2D[WalkerX - 1, WalkerY] = "X";
                            Array2D[Width - 1, WalkerY] = "X";
                            Array2D[Width - 2, WalkerY] = "X";
                            FinalPath.Add(new Coordinates(WalkerX - 1, WalkerY));
                            FinalPath.Add(new Coordinates(Width - 1, WalkerY));
                            FinalPath.Add(new Coordinates(Width - 2, WalkerY));
                            WalkerX = Width - 2;
                            continue;
                        }
                        else
                        {
                            Array2D[WalkerX - 1, WalkerY] = "X";
                            continue;
                        }
                    }
                    else
                    {
                        WalkerX += -1;
                        Array2D[WalkerX, WalkerY] = "X";
                        FinalPath.Add(new Coordinates(WalkerX, WalkerY));
                        continue;
                    }
                }
                // STEP UP
                if (Array2D[WalkerX, WalkerY - 1] != "#" && Array2D[WalkerX, WalkerY - 1] != "X")
                {
                    if (WalkerY - 1 == 0)
                    {
                        if (Array2D[WalkerX, Height - 1] != "#" && Array2D[WalkerX, Height - 1] != "X" && Array2D[WalkerX, Height - 2] != "#" && Array2D[WalkerX, Height - 2] != "X")
                        {
                            Array2D[WalkerX, WalkerY - 1] = "X";
                            Array2D[WalkerX, Height - 1] = "X";
                            Array2D[WalkerX, Height - 2] = "X";
                            FinalPath.Add(new Coordinates(WalkerX, WalkerY - 1));
                            FinalPath.Add(new Coordinates(WalkerX, Height - 1));
                            FinalPath.Add(new Coordinates(WalkerX, Height - 2));
                            WalkerX = Height - 2;
                            continue;
                        }
                        else
                        {
                            Array2D[WalkerX, WalkerY - 1] = "X";
                            continue;
                        }
                    }
                    else
                    {
                        WalkerY += -1;
                        Array2D[WalkerX, WalkerY] = "X";
                        FinalPath.Add(new Coordinates(WalkerX, WalkerY));
                        continue;
                    }
                }

                //If checkpoints run out and no possible steps left, leaves a message.
                if (CheckPoint.Count == 0)
                {
                    Console.WriteLine("No solveable path found.");
                    break;
                }

                // If there are no more steps to take, return walker to a checkpoint
                WalkerX = CheckPoint[CheckPoint.Count - 1].X;
                WalkerY = CheckPoint[CheckPoint.Count - 1].Y;

                //Remove all the previous steps taken in the Final Path back to the checkpoint
                for (var i = 0; i < FinalPath.Count; i++)
                {
                    if (FinalPath[i].X == WalkerX && FinalPath[i].Y == WalkerY)
                    {
                        var index = FinalPath.IndexOf(FinalPath[i]);
                        FinalPath.RemoveRange(index, FinalPath.Count - index);
                    }
                }

                FinalPath.Add(new Coordinates(WalkerX, WalkerY));
                //Remove the checkpoint
                CheckPoint.RemoveAt(CheckPoint.Count - 1);
            }

            // Replace all X with A's to draw a real final path
            for (var i = 0; i < FinalPath.Count; i++)
            {
                Array2D[FinalPath[i].X, FinalPath[i].Y] = "A";
            }

            // Drawing the map
            for (var Col = 0; Col < Height; Col++)
            {
                for (var Row = 0; Row < Width; Row++)
                {
                    // Replacing X's with spaces
                    if (Array2D[Row, Col] == "X")
                    {
                        Array2D[Row, Col] = " ";
                    }
                    // Replacing A's with X ( The true path)
                    if (Array2D[Row, Col] == "A")
                    {
                        Array2D[Row, Col] = "X";
                    }
                    Console.Write(Array2D[Row, Col]);
                }
                Console.Write("\n");
                Array2D[StartX, StartY] = "S";
                Array2D[EndX, EndY] = "E";
            }

            //Checks if there's a path. If there is more than one path, add a checkpoint.
            void CheckForPath(int X, int Y)
            {
                //Breaks if the bounds are broken
                if (WalkerX == 0 || WalkerY == 0)
                {
                    return;
                }
                int PathsFound = 0;
                if (Array2D[WalkerX + 1, WalkerY] != "#" && Array2D[WalkerX + 1, WalkerY] != "X")
                {
                    PathsFound += 1;
                }
                if (Array2D[WalkerX - 1, WalkerY] != "#" && Array2D[WalkerX - 1, WalkerY] != "X")
                {
                    PathsFound += 1;
                }
                if (Array2D[WalkerX, WalkerY + 1] != "#" && Array2D[WalkerX, WalkerY + 1] != "X")
                {
                    PathsFound += 1;
                }
                if (Array2D[WalkerX, WalkerY - 1] != "#" && Array2D[WalkerX, WalkerY - 1] != "X")
                {
                    PathsFound += 1;
                }
                if (PathsFound > 1)
                {
                    CheckPoint.Add(new Coordinates(WalkerX, WalkerY));
                }
            }
            Console.ReadLine();
        }
    }

    // Coordinates class for the lists
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
