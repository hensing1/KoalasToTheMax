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
            Console.WriteLine("type \"go\" to start");
            while (true)
            {
                if (Console.ReadLine() == "go")
                {
                    new Program().Default();
                    break;
                }
                else
                {
                    Console.WriteLine("unknown command");
                }
            }

            int x = 0;
            int y = 0;

            while(true)
            {
                if (Cursor.Position.X != x || Cursor.Position.Y != y)
                {
                    x = Cursor.Position.X;
                    y = Cursor.Position.Y;

                    Console.WriteLine($"x: {x}, y: {y}");
                }
            }
        }

        void Default()
        {
            int startX = 673;
            int startY = 318;
            int endX = 1184;
            int endY = 829;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Math.Pow(2, i); j++)
                {
                    int y = startY + (endY - startY) * 1 / (int)Math.Pow(2, i + 1) + (endY - startY) * j / (int)Math.Pow(2, i);
                    Cursor.Position = new Point(startX, y);

                    Thread.Sleep(50);

                    for (int x = startX; x < endX; x += 4)
                    {
                        Cursor.Position = new Point(x, y);
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(50);
                }
            }
        }
    }
}
