using System;

namespace CarPrice.Models
{
    static public class BLogic
    {
        internal static string CalcCostCar(Car car) => car switch
        {
            Car when car.Transmission => "High price :)",
            _ => "Low price :("
        };
    }
}
