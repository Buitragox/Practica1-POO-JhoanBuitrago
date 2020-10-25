using System;
using AutoFarm.Lands;
using AutoFarm.Facade;
namespace AutoFarm
{
    class Program
    {
        static void Main(string[] args)
        {
            Boundary facade = new Boundary();
            facade.Menu();
        }
    }
}
