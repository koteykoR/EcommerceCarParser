using System;
using CarPrice.Models;
using CarPrice.Helpers;
using static CarPrice.Models.BLogic;

namespace CarPrice.ViewModels
{
    internal sealed class MainViewModel : ViewModel
    {
        private Car curCar = new();
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

        public Command CalcCommand => new (obj => { Test = CalcCostCar(CurCar) + '\n' + CurCar.ToString(); });
    }
}
