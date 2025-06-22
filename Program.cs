using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Aquarium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();

            aquarium.Work();
        }
    }

    public class Aquarium
    {
        private AquariumFactory _factory = new AquariumFactory();

        private const int MaxFishes = 10;
        private List<Fish> _fishes;

        public Aquarium()
        {
            _fishes = _factory.GetList();
        }

        public void Work()
        {
            const string SkipLifeCycleCommand = "1";
            const string AddFishCommand = "2";
            const string RemoveFishCommand = "3";

            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"В аквариуме: {_fishes.Count} рыбок");

                ShowFishes();

                Console.WriteLine($"{SkipLifeCycleCommand} - Промотать 1 цикл\n" +
                                  $"{AddFishCommand} - Добавить рыбку в аквариум\n" +
                                  $"{RemoveFishCommand} - Убрать рыбку из аквариума\n");
                Console.Write("Введите команду: ");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case SkipLifeCycleCommand:
                        break;

                    case AddFishCommand:
                        AddFish();
                        break;

                    case RemoveFishCommand:
                        RemoveFish();
                        break;

                    default:
                        Console.WriteLine("такой команды нет, но рыбки постарели :(");
                        break;
                }

                isOpen = IsOpen();

                GrowOldFishes();
                RemovedDeadFishes();


                Console.ReadKey();
                Console.Clear();
            }
        }

        private bool IsOpen()
        {
            return _fishes.Count > 0;
        }

        private void ShowFishes()
        {
            foreach (Fish fish in _fishes)
            {
                Console.WriteLine($"Рыба #{fish.Name} - возраст: {fish.Age}");
            }
        }

        private void GrowOldFishes()
        {
            foreach (Fish fish in _fishes)
            {
                fish.GrowOld();
            }
        }

        private void AddFish()
        {
            if (_fishes.Count < MaxFishes)
                _fishes.Add(new Fish());
            else
                Console.WriteLine("В аквариуме нет места");
        }

        private void RemoveFish()
        {
            if (_fishes.Count > 0)
            {
                ShowFishes();

                Console.WriteLine("Введите название рыбы которую хотите удалить:");

                if (int.TryParse(Console.ReadLine(), out int index))
                {
                    foreach (Fish fish in _fishes)
                    {
                        if (fish.Name == index)
                        {
                            _fishes.Remove(fish);
                            return;
                        }
                    }
                }
            }

            if (_fishes.Count == 0)
            {
                Console.WriteLine("В аквариуме нет рыбок");
                return;
            }
        }

        private void RemovedDeadFishes()
        {
            for (int i = _fishes.Count - 1; i > -1; i--)
            {
                if (_fishes[i].IsDead)
                {
                    Console.WriteLine($"Удалена рыба с ID: {_fishes[i].Name}. Срок жизни был {_fishes[i].MaxLife}");
                    _fishes.RemoveAt(i);
                }
            }
        }
    }

    public class AquariumFactory
    {
        private List<Fish> _aquarium =new List<Fish>();

        public AquariumFactory()
        {
            Create();
        }

        private void Create()
        {
            _aquarium.Add(new Fish("Скалярия",6));
            _aquarium.Add(new Fish("Золотая рыбка",5));
            _aquarium.Add(new Fish("Гурами", 8));
            _aquarium.Add(new Fish("Макропод", 7));
            _aquarium.Add(new Fish("Попугай", 10));

        }

        public List<Fish> GetList()
        {
            return _aquarium;
        }
    }

    public class Fish
    {
        private const int MinAge = 1;
        private static int s_id = 1;

        public Fish(string name,int maxLife)
        {
            Name = name; 
            MaxLife = maxLife;
            Age = UserUtils.GenerateRandomNumber(MinAge, maxLife);
        }

        public string Name { get; private set; }
        public int Age { get; private set; }
        public int MaxLife { get; private set; }
        public bool IsDead => Age >= MaxLife;

        public void GrowOld()
        {
            Age++;
        }

        private int NewId()
        {
            return s_id++;
        }

        public Fish Clone()
        {
            return new Fish(Name,MaxLife);
        }
    }

    public class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }
    }
}