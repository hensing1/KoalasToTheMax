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
                Console.Write('>');
                args = Console.ReadLine().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (args[0] == "run")
                {
                    int startX = 673, startY = 318, width = 512, pauseAfterLine = 140, pauseAfterStep = 2;
                    if (args.Length > 1)
                    {
                        if (args.Contains("-d"))
                        {
                            if (args.Length > 2)
                            {
                                Console.WriteLine("Too many arguments.");
                                continue;
                            }
                        }
                        else if (args.Contains("-c"))
                        {
                            if (args.Contains("-t"))
                            {
                                Console.WriteLine("Too many arguments.");
                                continue;
                            }

                            try
                            {
                                int indexOfC = args.ToList().IndexOf("-c");
                                startX = Int32.Parse(args[indexOfC + 1]);
                                startY = Int32.Parse(args[indexOfC + 2]);
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
                            Console.WriteLine("Step 1: Position pointer at top left corner");
                            Console.WriteLine("Step 2: Switch to this console (with Alt+Tab)");
                            Console.WriteLine("Step 3: Hit Enter");

                            bool abort = false;

                            while (true)
                            {
                                if (Console.KeyAvailable)
                                {
                                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                    {
                                        startX = Cursor.Position.X;
                                        startY = Cursor.Position.Y;
                                        Console.WriteLine($"startX = {startX}, startY = {startY}");
                                        break;
                                    }
                                    else if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                                    {
                                        Console.WriteLine("Aborted by user");
                                        abort = true;
                                        break;
                                    }
                                }
                            }
                            if (abort)
                                continue;
                        }
                        if (args.Contains("-s"))
                        {
                            try
                            {
                                int indexOfS = args.ToList().IndexOf("-s");
                                pauseAfterStep = Int32.Parse(args[indexOfS + 1]);
                                pauseAfterLine = Int32.Parse(args[indexOfS + 2]);
                            }
                            catch (IndexOutOfRangeException ioore)
                            {
                                Console.WriteLine("Insufficient arguments");
                                Console.WriteLine(ioore.StackTrace);
                                continue;
                            }
                            catch (FormatException re)
                            {
                                Console.WriteLine("Invalid format");
                                Console.WriteLine(re.StackTrace);
                                continue;
                            }
                        }
                    }
                    new Program().Run(startX, startY, width, pauseAfterStep, pauseAfterLine);
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
                    Console.WriteLine(String.Format("{0, -20} {1}", "  -c <X> <Y>", "Run after manually specifying coordinates (space delimited, top left corner)"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "  -t", "Run after positioning pointer at top left corner"));
                    Console.WriteLine(String.Format("{0, -20} {1}", "  -s <step> <line>", "Specify the amount of milliseconds to wait after each step (default: 2) and line (default: 140)"));
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

        void Run(int startX = 673, int startY = 318, int width = 512, int pauseAfterStep = 2, int pauseAfterLine = 140)
        {
            Console.Write("Running... ");
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Math.Pow(2, i); j++)
                {
                    int y = startY + width / (int)Math.Pow(2, i + 1) + width * j / (int)Math.Pow(2, i);
                    Cursor.Position = new Point(startX, y);

                    Thread.Sleep(pauseAfterLine / 2);

                    for (int x = startX; x < startX + width; x += 4)
                    {
                        Cursor.Position = new Point(x, y);
                        Thread.Sleep(pauseAfterStep);
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
                            Console.Write("\nContinuing... ");
                        }
                    }
                    Thread.Sleep(pauseAfterLine / 2);
                }
            }
            Console.WriteLine("Done!");
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
