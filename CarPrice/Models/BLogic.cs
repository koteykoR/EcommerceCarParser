using System;
using System.Collections.Generic;
using System.Text;
using CarPrice.Repository;
using EcommerceCarsParser.Parsers;

namespace CarPrice.Models
{
    static public class BLogic
    {
        private static readonly DBRepository<Car> db = new();

        public static string GetDataFromLibParser(string company, string model)
        {
            var parser = new AutoParser(company, model);

            var sb = new StringBuilder();

            foreach (var car in parser.Parse())
            {
                sb.Append(car);
                sb.Append('\n');

                db.Add(new(car.Company, car.Model, car.Mileage, 
                           car.EnginePower, car.EngineVolume, 
                           car.Year, car.Transmission, car.Price));
            }

            return sb.ToString();
        }
    }
}
