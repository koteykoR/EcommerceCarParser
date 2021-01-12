using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPrice.Models
{
    internal sealed record Car(string Company, string Model, int Mileage, 
                               int EnginePower, int EngineVolume, string Year, 
                               bool Transmission, int Price = 0)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Car() : this(default, default, default, default, default, "2020", true) { }
    }
}
