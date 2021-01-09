using System;

namespace CarPrice.Models
{
    static public class BLogic
    {
        public static string CalcCostCar(Car car) => car switch
        {
            Car when car.Transmission => "High price :)",
            _ => "Low price :("
        };
    }
}
