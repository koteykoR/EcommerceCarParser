using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarPrice.Models
{
    public sealed class Car : INotifyPropertyChanged
    {
        private string company;
        public string Company
        {
            get => company;
            set { company = value; OnPropertyChanged("Company"); }
        }

        private string model;
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged("Model"); }
        }

        private int mileage;
        public int Mileage
        {
            get => mileage;
            set { mileage = value; OnPropertyChanged("Mileage"); }
        }

        private int enginePower;
        public int EnginePower
        {
            get => enginePower;
            set { enginePower = value; OnPropertyChanged("EnginePower"); }
        }

        private int engineVolume;
        public int EngineVolume
        {
            get => engineVolume;
            set { engineVolume = value; OnPropertyChanged("EngineVolume"); }
        }

        private DateTime date;
        public DateTime Date
        {
            get => date == default ? DateTime.Now : date;
            set { date = value; OnPropertyChanged("Date"); }
        }

        private bool transmission;
        public bool Transmission
        {
            get => transmission;
            set { transmission = value; OnPropertyChanged("Transmission"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new(prop));

        public override string ToString() => $"Company: {Company} Model: {Model} Mileage: {Mileage}\n" +
            $"EnginePower: {EnginePower} EngineVolume: {EngineVolume} Date: {Date}\n" + $"Trans: {Transmission}";
    }
}
