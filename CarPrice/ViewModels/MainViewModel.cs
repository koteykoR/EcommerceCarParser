using System;
using CarPrice.Models;
using CarPrice.Helpers;
using static CarPrice.Models.BLogic;

namespace CarPrice.ViewModels
{
    internal sealed class MainViewModel : ViewModel
    {
        private Car curCar;
        public Car CurCar
        {
            get => curCar;
            set => Set(ref curCar, value);
        }

        private string test = "Тест";
        public string Test
        {
            get => test;
            set => Set(ref test, value);
        }

        public MainViewModel()
        {
            CurCar = new();
        }

        public Command CalcCommand => new (obj => { Test = $"Price: {CalcCostCar(obj as Car)}\n{obj}"; });
    }
}
