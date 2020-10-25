using System.Collections.Generic;
using AutoFarm.Lands;
using System.Linq;
using AutoFarm.Crops;
using System;
using AutoFarm.Robots;
using AutoFarm.Events;

namespace AutoFarm.Controller
{
    public class Interactor
    {
        public void NewCrop(Land l, string type)
        {
            l.Used = true;
            l.LandCrop.Type = type;
            l.LandCrop.Growth = 0;
            l.LandCrop.Edibility = 100;
            l.LandCrop.EventBool = false;
        }
        public void ShowLands(List<Land> lands)
        {
            Crop c;
            lands.ForEach(l =>
            {
                Console.WriteLine($"Land {l.Id}");
                Console.WriteLine($"Irrigation level = {l.Irrigation}");
                if(l.IrrigatorBool)
                {
                    Console.WriteLine($"Irrigators are on");
                    Console.WriteLine($"Land gets automatically irrigated at {l.IrrPercentaje}%");
                }
                else
                {
                    Console.WriteLine($"Irrigators are off");
                }
                Console.WriteLine($"Fertility level = {l.Fertility}%");
                if(l.Used)
                {
                    Console.WriteLine();
                    c = l.LandCrop;
                    Console.WriteLine($"Crop type: {c.Type}");
                    Console.WriteLine($"Growth = {c.Growth}%");
                    Console.WriteLine($"Edibility = {c.Edibility}%");
                    if(c.EventBool)
                    {
                        Console.WriteLine($"Current event = {c.CurrentEvent.Name}");
                    }
                    else
                    {
                        Console.WriteLine("No current events");
                    }
                }
                Console.WriteLine();
            });
        }
        public void ModifyIrrigator(Land l, double per)  
        {
            l.IrrPercentaje = per;
        }

        public void OnOffIrrigator(Land l)
        {
            l.IrrigatorBool = !l.IrrigatorBool;
        }

        public void Upgrade(Robot r)
        {
            r.MaxBattery += 100;
        }
        
        public void OnOffRobot(Robot r)
        {
            r.Activated = !r.Activated;
        }

        public void ChargeRobot(Robot r)
        {
            r.Battery = r.MaxBattery;
        }

        private void UpdateCrops(List<Land> lands, List<Event> eList)
        {
            Crop c;
            lands.ForEach(l =>
            {
                if(l.Fertility >= 5)
                    l.Fertility -= 5;
                else
                    l.Fertility = 0;

                if(l.Irrigation >= 20)
                    l.Irrigation -= 20;
                else
                    l.Irrigation = 0;

                if(l.IrrigatorBool && l.Irrigation <= l.IrrPercentaje)
                    l.Irrigation = 100;
                
                if(l.Used)
                {
                    c = l.LandCrop;
                    if(c.Growth == 100)
                    {
                        Console.WriteLine($"Crop of {c.Type} in {l.Id} is ready for harvest");
                        c.Edibility -= 5;
                    }
                    else if(c.Growth <= 90)
                    {
                        c.Growth += 10;
                        if(c.Growth == 100)
                        {
                            Console.WriteLine($"Crop of {c.Type} in {l.Id} is ready for harvest");
                        }
                    }

                    if(l.Irrigation <= 10)
                    {
                        Console.WriteLine($"Warning: please activate irrigators at {l.Id}");
                        c.Edibility -= 5;
                    }
                    if(l.Fertility <= 10)
                    {
                        Console.WriteLine($"Warning: please fertilize the land at {l.Id}");
                        c.Edibility -= 5;
                    }
                    
                    if(c.EventBool)
                    {
                        Console.WriteLine($"Warning: {c.CurrentEvent.Name} at {l.Id}");
                        Console.WriteLine($"Please asign a robot to solve the problem");
                        c.Edibility -= c.CurrentEvent.Damage;
                    }
                    else
                    {
                        Random rnd = new Random();
                        if(rnd.Next(11) == 0)
                        {
                            Event e = eList[rnd.Next(eList.Count)];
                            c.CurrentEvent = e;
                            Console.WriteLine($"Warning: New event {e.Name} at {l.Id}");
                            Console.WriteLine($"Please asign a robot to solve the problem");
                            c.EventBool = true;
                        }
                    }
                    if(c.Edibility <= 10)
                    {
                        Console.WriteLine($"Crop at {l.Id} became unfit for consumption");
                        Console.WriteLine($"The crop will be removed");
                        l.Used = !l.Used;
                    }
                }
            });
        }
        private void UpdateRobots(List<Robot> rList)
        {
            rList.ForEach(r => 
            {
                if(r.Activated)
                {
                    r.Battery -= 50;
                    if(r.Battery == 0)
                    {
                        r.Activated = false;
                        Console.WriteLine($"Warning: robot {r.Id} went off due to low battery");
                    }
                }
            });
        }
        
        public void PassTime(List<Land> lands, List<Event> eList, List<Robot> rList)
        {
            UpdateCrops(lands, eList);
            UpdateRobots(rList);
        }

        public void Harvest(Land l)
        {
            l.Used = false;
        }

        public void ClearEvent(Land l)
        {
            l.LandCrop.EventBool = false;
        }

        public void Fertilize(Land l)
        {
            l.Fertility = 100;
        }
    }
}