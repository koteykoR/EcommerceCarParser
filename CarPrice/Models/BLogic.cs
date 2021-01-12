using System;
using System.Collections.Generic;
using CarPrice.Repository;

namespace CarPrice.Models
{
    static public class BLogic
    {
        private static readonly DBRepository<Car> db = new();

        internal static string CalcCostCar(Car car) => car switch
        {
            Car when car.Transmission => "High price :)",
            _ => "Low price :("
        };

        internal static void TestAddDb()
        {
            var testCars = new Car[]
            {
                new("company1", "model1", 1, 1, 1, "1", true, 1),
                new("company2", "model2", 2, 2, 2, "2", true, 2),
                new("company3", "model3", 3, 3, 3, "3", false, 3),
            };

            foreach (var testCar in testCars)
            {
                db.Add(testCar);
            }
        }

        internal static string TestGetAllDb()
        {
            var carsText = "";

            var cars = db.GetAll();

            foreach (var car in cars)
            {
                carsText += car.ToString() + "\n";
            }

            return carsText;
        }
    }
}
