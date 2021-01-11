using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarPrice.Models
{
    internal sealed record Car(string Company, string Model, int Mileage, int EnginePower, int EngineVolume, DateTime Date, bool Transmission)
    {
        public Car() : this(default, default, default, default, default, DateTime.Now, true) { }
    }
}
