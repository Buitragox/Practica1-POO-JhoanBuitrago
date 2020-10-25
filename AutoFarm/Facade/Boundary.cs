using System.Collections.Generic;
using AutoFarm.Lands;
using AutoFarm.Crops;
using System;
using AutoFarm.Robots;
using AutoFarm.Events;
using AutoFarm.Controller;
using AutoFarm.Validators;
using System.Linq;
using AutoFarm.Rules;

namespace AutoFarm.Facade
{
    public class Boundary
    {
        public List<Robot> RobotList { get; set; }
        public List<Event> EventList { get; set; }
        public List<Land> LandList { get; set; }
        public Interactor Handler;
        public Validator StringValidate;

        public Validator PercentajeValidate;

        public Boundary()
        {
            RobotList = new List<Robot>();
            RobotList.Add(new Robot("Ro1"));
            RobotList.Add(new Robot("Ro2"));
            RobotList.Add(new Robot("Ro3"));

            EventList = new List<Event>();
            EventList.Add(new Event("Plague", 10));
            EventList.Add(new Event("Drought", 5));

            LandList = new List<Land>();
            LandList.Add(new Land("La1"));
            LandList.Add(new Land("La2"));
            LandList.Add(new Land("La3"));

            Handler = new Interactor();

            StringValidate = new Validator();
            StringValidate.RuleList.Add(new StrRule());
            StringValidate.RuleList.Add(new LengthRule());

            PercentajeValidate = new Validator();
            PercentajeValidate.RuleList.Add(new PercentajeRule());
            
        }

        public void Menu()
        {
            string option;
            Console.WriteLine("Welcome to the AutoFarm system");
            do
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1.Plant new crop\n2.Show lands\n3.Upgrade a robot");
                Console.WriteLine("4.Charge a robot\n5.Turn on/off a robot");
                Console.WriteLine("6.Turn on/off a land irrigators\n7.Modify a land irrigator");
                Console.WriteLine("8.Asign robot to event\n9.Fertilize land\n10.Harvest crop");
                Console.WriteLine("0.Exit");
                Console.Write("> ");
                option = Console.ReadLine();
                switch (option)
                {
                    case "0":
                        break;

                    case "1":
                        CropData();
                        break;

                    case "2":
                        Handler.ShowLands(LandList);
                        break;

                    case "3":
                        RobotData(option);
                        break;

                    case "4":
                        RobotData(option);
                        break;

                    case "5":
                        RobotData(option);
                        break;

                    case "6":
                        IrrigatorData(option);
                        break;

                    case "7":
                        IrrigatorData(option);
                        break;

                    case "8":
                        AsignData();
                        break;

                    case "9":
                        FertilizeData();
                        break;
                    
                    case "10":
                        HaverstData();
                        break;
                        
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
                Handler.PassTime(LandList, EventList, RobotList);
            } while(option != "0");
        }

        public void CropData()
        {
            string id;
            bool validate;
            string name;
            int count = 0;
            Console.WriteLine("Available lands:");
            LandList.ForEach(l => 
            {
                if(!l.Used)
                {
                    Console.WriteLine(l.Id);
                    count++;
                }
            });
            if(count > 0)
            {
                Console.Write("Enter the land id: ");
                id = Console.ReadLine();
                StringValidate.Value = id;
                validate = StringValidate.ValidateField();
                Console.Write("Enter the name of the new crop: ");
                name = Console.ReadLine();
                StringValidate.Value = name;
                validate = validate && StringValidate.ValidateField();
                if(validate)
                {
                    Land l = LandList.FirstOrDefault(p => p.Id == id && !p.Used);
                    if(l != null)
                    {
                        Handler.NewCrop(l, name);
                    }
                    else
                    {
                        Console.WriteLine("No available lands match the entered id");
                    }
                }
                else
                {
                    Console.WriteLine("The data input was invalid");
                }
            }
            else
            {
                Console.WriteLine("No available lands");
            }
            
            
        }

        public void RobotData(string option)
        {
            string id;
            bool validate;
            string state;
            Console.WriteLine("List of robots:");
            RobotList.ForEach(r =>
            {
                state = (r.Activated) ? "on" : "off";
                Console.Write($"ID:{r.Id} | Battery: {r.Battery}/{r.MaxBattery} | ");
                Console.WriteLine($"State: {state}");
            });
            Console.Write("Enter the robot id: ");
            id = Console.ReadLine();
            StringValidate.Value = id;
            validate = StringValidate.ValidateField();
            if(validate)
            {
                Robot r = RobotList.FirstOrDefault(c => c.Id == id);
                if(r != null)
                {
                    if(option == "3")
                        Handler.Upgrade(r);
                    else if(option == "4")
                        Handler.ChargeRobot(r);
                    else if(option == "5")
                        Handler.OnOffRobot(r);
                }
                else
                {
                    Console.WriteLine("No robots match the entered id");
                }
            }
            else
            {
                Console.WriteLine("The data input was invalid");
            }
        }

        public void IrrigatorData(string option)
        {   
            string id;
            string state;
            string perString;
            double per = 0;
            bool validate;
            Console.WriteLine("List of lands:");
            LandList.ForEach(l => 
            {
                state = l.IrrigatorBool ? "on" : "off";
                Console.Write($"ID: {l.Id} | Irrigators: {state} | ");
                Console.WriteLine($"Irrigation percentaje: {l.IrrPercentaje}%");
            });
            Console.Write("Enter the land id: ");
            id = Console.ReadLine();
            StringValidate.Value = id;
            validate = StringValidate.ValidateField();
            if(option == "7")
            {
                Console.Write("Enter the new irrigation percentaje: ");
                perString = Console.ReadLine();
                PercentajeValidate.Value = perString;
                validate = validate && PercentajeValidate.ValidateField();
                if(validate)
                    per = Convert.ToDouble(perString);
            }
            if(validate)
            {
                Land l = LandList.FirstOrDefault(c => c.Id == id);
                if(l != null)
                {
                    if(option == "6")
                        Handler.OnOffIrrigator(l);
                    else if(option == "7")
                        Handler.ModifyIrrigator(l, per);
                }
                else
                {
                    Console.WriteLine("No robots match the entered id");
                }
            }
            else
            {
                Console.WriteLine("The data input was invalid");
            }
        }

        public void AsignData()
        {
            string landId;
            string robotId;
            bool validate;
            int countRobots = 0;
            int countEvents = 0;
            Console.WriteLine("Lands with active events:");
            LandList.ForEach(l => 
            {
                if(l.Used && l.LandCrop.EventBool)
                {
                    Console.WriteLine($"ID: {l.Id} | Event: {l.LandCrop.CurrentEvent.Name}");
                    countEvents++;
                }
            });
            if(countEvents > 0)
            {
                Console.WriteLine("Available robots:");
                RobotList.ForEach(r =>
                {
                    if(r.Activated)
                        Console.WriteLine($"ID:{r.Id} | Battery: {r.Battery}/{r.MaxBattery}");
                        countRobots++;
                });
                if(countRobots > 0)
                {
                    Console.Write("Enter the land id: ");
                    landId = Console.ReadLine();
                    StringValidate.Value = landId;
                    validate = StringValidate.ValidateField();

                    Console.Write("Enter the robot id: ");
                    robotId = Console.ReadLine();
                    StringValidate.Value = robotId;
                    validate = validate && StringValidate.ValidateField();
                    if(validate)
                    {
                        Land l = LandList.FirstOrDefault(c => 
                                                c.Id == landId && c.Used && c.LandCrop.EventBool);
                        Robot r = RobotList.FirstOrDefault(c => c.Id == robotId && c.Activated);
                        if(l != null && r != null)
                        {
                            Handler.ClearEvent(l);
                        }
                        else
                        {
                            Console.WriteLine("No robots or lands match the entered id");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The data input was invalid");
                    }
                }
                else
                {
                    Console.WriteLine("No available robots");
                }
            }
            else
            {
                Console.WriteLine("No current events");
            }
            
        }

        public void FertilizeData()
        {
            string robotId;
            string landId;
            bool validate;
            int countRobots = 0;
            Console.WriteLine("List of lands:");
            LandList.ForEach(l => 
            {
                Console.WriteLine($"ID: {l.Id} | Fertility: {l.Fertility}%");
            });
            Console.WriteLine("Available robots:");
            RobotList.ForEach(r =>
            {
                if(r.Activated)
                    Console.WriteLine($"ID:{r.Id} | Battery: {r.Battery}/{r.MaxBattery}");
                    countRobots++;
            });
            if(countRobots > 0)
            {
                Console.Write("Enter the land id: ");
                landId = Console.ReadLine();
                StringValidate.Value = landId;
                validate = StringValidate.ValidateField();

                Console.Write("Enter the robot id: ");
                robotId = Console.ReadLine();
                StringValidate.Value = robotId;
                validate = validate && StringValidate.ValidateField();
                if(validate)
                {
                    Land l = LandList.FirstOrDefault(p => p.Id == landId);
                    Robot r = RobotList.FirstOrDefault(c => c.Id == robotId && c.Activated);
                    if(l != null)
                    {
                        Handler.Fertilize(l);
                    }
                    else
                    {
                        Console.WriteLine("No available lands match the entered id");
                    }
                }   
                else
                {
                    Console.WriteLine("The data input was invalid");
                }
            }
            else
            {
                Console.WriteLine("No available robots");
            }
        }
    
        public void HaverstData()
        {
            string landId;
            string robotId;
            bool validate;
            int countRobots = 0;
            int countCrops = 0;
            Console.WriteLine("Lands with crops ready to harvest:");
            LandList.ForEach(l => 
            {
                if(l.Used && l.LandCrop.Growth == 100)
                {
                    Console.Write($"ID: {l.Id} | Crop: {l.LandCrop.Type} | ");
                    Console.WriteLine($"Edibility {l.LandCrop.Edibility}%");
                    countCrops++;
                }
            });
            if(countCrops > 0)
            {
                Console.WriteLine("Available robots:");
                RobotList.ForEach(r =>
                {
                    if(r.Activated)
                        Console.WriteLine($"ID:{r.Id} | Battery: {r.Battery}/{r.MaxBattery}");
                        countRobots++;
                });
                if(countRobots > 0)
                {
                    Console.Write("Enter the land id: ");
                    landId = Console.ReadLine();
                    StringValidate.Value = landId;
                    validate = StringValidate.ValidateField();

                    Console.Write("Enter the robot id: ");
                    robotId = Console.ReadLine();
                    StringValidate.Value = robotId;
                    validate = validate && StringValidate.ValidateField();
                    if(validate)
                    {
                        Land l = LandList.FirstOrDefault(c => 
                                            c.Id == landId && c.Used && c.LandCrop.Growth == 100);
                        Robot r = RobotList.FirstOrDefault(c => c.Id == robotId && c.Activated);
                        if(l != null && r != null)
                        {
                            Handler.Harvest(l);
                        }
                        else
                        {
                            Console.WriteLine("No robots or lands match the entered id");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The data input was invalid");
                    }
                }
                else
                {
                    Console.WriteLine("No available robots");
                }
            }
            else
            {
                Console.WriteLine("No current crops");
            }
            
        }
    }
}