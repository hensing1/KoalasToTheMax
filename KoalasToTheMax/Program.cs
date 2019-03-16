using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace KoalasToTheMax
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nWelcome to the koalastothemax.com auto-solver. \n\"help\" for commands");
            while (true)
            {
                args = Console.ReadLine().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (args[0] == "run")
                {
                    if (args.Length > 1)
                    {
                        if (args.Contains("-d"))
                        {
                            if (args.Contains("-t") || args.Contains("-c"))
                                Console.WriteLine("Too many arguments.");
                            else
                                new Program().Run();
                        }
                        else if (args.Contains("-c"))
                        {
                            try
                            {
                                new Program().Run(int.Parse(args[2]), int.Parse(args[3]));
                            }
                            catch (IndexOutOfRangeException ioore)
                            {
                                Console.WriteLine("Insufficient arguments");
                                Console.WriteLine(ioore.StackTrace);
                            }
                            catch (FormatException re)
                            {
                                Console.WriteLine("Invalid format");
                                Console.WriteLine(re.StackTrace);
                            }
                        }
                        else if (args.Contains("-t"))
                        {
                            //Console.WriteLine("Please click the top left corner as precisely as possible.");

                            Console.WriteLine("Uh, no. That's way more complicated than I anticipated it was going to be. Try sth else.");

                            //UserActivityHook actHook = new UserActivityHook();
                        }
                        if (args.Contains("-s"))
                        {

                        }
                    }
                    else
                    {
                        new Program().Run();
                    }
                }
                else if (args[0] == "track")
                {
                    new Program().Track();
                }
                else if (args[0] == "beep")
                {
                    Console.Beep();
                }
                else if (args[0] == "help")
                {
                    Console.WriteLine(String.Format("\n{0, -20} {1}\n", "Command", "Description"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "run", "Runs application with specified settings (esc to cancel, p to pause)"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "  -d", "Run with default settings (FullHD screen)"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "  -c coordinates", "Run with custom settings (space delimited coordinates of top left corner)"));
                    Console.WriteLine(String.Format("{0, -10} {1}", "  -t", "Run after clicking the corners of the image"));
                    //Console.WriteLine(String.Format("{0, -10} {1}", "  -s step line", "Specify the amount of milliseconds to wait after each step (std: 4) and line (std: 50)"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "track", "Output pointer position (any key to cancel)"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "beep", "beep"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "help", "This."));
                    Console.WriteLine(String.Format("{0, -20} {1}\n", "exit", "Kills the application (you monster)"));
                }
                else if (args[0] == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("unknown command");
                }
            }
        }

        void Run(int startX = 673, int startY = 318, int width = 512)
        {
            Console.WriteLine("Running...");
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Math.Pow(2, i); j++)
                {
                    int y = startY + width / (int)Math.Pow(2, i + 1) + width * j / (int)Math.Pow(2, i);
                    Cursor.Position = new Point(startX, y);

                    Thread.Sleep(50);

                    for (int x = startX; x < startX + width; x += 4)
                    {
                        Cursor.Position = new Point(x, y);
                        Thread.Sleep(2);
                        if (Console.KeyAvailable)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.P)
                            {
                                Console.WriteLine("Paused. p to continue");
                                while (Console.KeyAvailable) { }
                                while (true)
                                {
                                    if (Console.KeyAvailable && (Console.ReadKey(true).Key == ConsoleKey.P || Console.ReadKey(true).Key == ConsoleKey.Escape))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Console.WriteLine("Program stopped (terminated by user)");
                                return;
                            }
                            Console.WriteLine("Continuing...");
                        }
                    }
                    Thread.Sleep(70);
                }
            }
            Console.WriteLine("Finished!");
        }

        void Track()
        {
            int x = 0;
            int y = 0;

            while (!Console.KeyAvailable)
            {
                if (Cursor.Position.X != x || Cursor.Position.Y != y)
                {
                    x = Cursor.Position.X;
                    y = Cursor.Position.Y;

                    Console.WriteLine($"x: {x}, y: {y}");
                }
            }
        }
    }
}
