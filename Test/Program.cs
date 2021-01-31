using System;
using EcommerceCarsParser.Parsers;
using static System.Console;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var time = System.Diagnostics.Stopwatch.StartNew();

            var parser = new AutoParser();

            var cars = parser.Parse();

            var countCar = 0;

            foreach (var car in cars)
            {
                countCar++;
                WriteLine(car);
            }

            WriteLine($"Count car: {countCar}");

            time.Stop();

            WriteLine($"Time: {time.ElapsedMilliseconds}");
        }
    }
}
