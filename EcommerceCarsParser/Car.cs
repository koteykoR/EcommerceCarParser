using System;

namespace EcommerceCarsParser
{
    public sealed record ParseCar(string Company, string Model, 
                                  string Year, int Price, int Mileage, 
                                  bool Transmission, int EnginePower, double EngineVolume);
}
