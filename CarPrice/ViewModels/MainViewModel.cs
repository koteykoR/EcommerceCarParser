using System;
using CarPrice.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static CarPrice.Models.BLogic;

namespace CarPrice.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private Car curCar;
        public Car CurCar
        {
            get => curCar;
            set { curCar = value; OnPropertyChanged("Car"); }
        }

        private string test;
        public string Test
        {
            get => test;
            set { test = value; OnPropertyChanged("Test"); }
        }

        public MainViewModel()
        {
            CurCar = new();
        }

        private Command calcCommand;
        public Command CalcCommand
        {
            get => calcCommand ??= new Command(obj => { Test = CalcCostCar(obj as Car).ToString(); });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new(prop));
    }
}
