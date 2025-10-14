using System;
using System.Collections.Generic;

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
        private FishFactory _fishFactory = new FishFactory();

        private const int MaxFishes = 10;
        private List<Fish> _aquarium = new List<Fish>();

        public Aquarium()
        {
            Create();
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
                Console.WriteLine($"В аквариуме: {_aquarium.Count} рыбок");

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
                Close(isOpen);

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void Create()
        {
            for (int i = 0; i < _fishFactory.GetCount(); i++)
            {
                _aquarium.Add(_fishFactory.GetList()[i].Clone());
            }
        }

        private bool IsOpen()
        {
            return _aquarium.Count > 1;
        }

        private void Close(bool isOpen)
        {
            if (isOpen == false)
            {
                Console.WriteLine($"В Аквариуме закончились рыбки");
               
            }
        }

        private void ShowFishes()
        {
            for (int i = 0; i < _aquarium.Count; i++)
            {
                Console.WriteLine($"{i + 1}) Рыба {_aquarium[i].Name} - возраст: {_aquarium[i].Age}");
            }
        }

        private void GrowOldFishes()
        {
            foreach (Fish fish in _aquarium)
            {
                fish.GrowOld();
            }
        }

        private void AddFish()
        {
            if (_aquarium.Count < MaxFishes)
            {
                _aquarium.Add((_fishFactory.GetList()[Utilite.GenerateRandomNumber(0, _fishFactory.GetList().Count)].Clone()));
                Console.WriteLine($"Была добавлена {_aquarium[_aquarium.Count - 1].Name} рыба с возрастом {_aquarium[_aquarium.Count - 1].Age}");
            }

            else
                Console.WriteLine("В аквариуме нет места");
        }

        private void RemoveFish()
        {
            Console.WriteLine("Введите номер рыбы которую хотите удалить:");

            int number = Utilite.GetNumberInRange(1, _aquarium.Count);

            Console.WriteLine($"Удалена рыба {_aquarium[number-1].Name}.");

            _aquarium.RemoveAt(number-1);
        }

        private void RemovedDeadFishes()
        {
            for (int i = 0; i < _aquarium.Count; i++)
            {
                if (_aquarium[i].IsDead())
                {
                    Console.WriteLine($"Удалена рыба {_aquarium[i].Name}. Срок жизни был {_aquarium[i].MaxLife}");

                    _aquarium.RemoveAt(i);

                    i--;
                }
            }
        }
    }

    public class FishFactory
    {
        private List<Fish> _fish = new List<Fish>();

        public FishFactory()
        {
            Create();
        }

        private void Create()
        {
            _fish.Add(new Fish("Скалярия", 6));
            _fish.Add(new Fish("Золотая рыбка", 5));
            _fish.Add(new Fish("Гурами", 8));
            _fish.Add(new Fish("Макропод", 7));
            _fish.Add(new Fish("Попугай", 10));
        }

        public List<Fish> GetList()
        {
            return new List<Fish>(_fish);
        }

        public int GetCount()
        {
            return _fish.Count;
        }
    }

    public class Fish
    {
        private const int MinAge = 1;

        public Fish(string name, int maxLife)
        {
            Name = name;
            MaxLife = maxLife;
            Age = Utilite.GenerateRandomNumber(MinAge, maxLife);
        }

        public string Name { get; private set; }
        public int Age { get; private set; }
        public int MaxLife { get; private set; }

        public void GrowOld()
        {
            Age++;
        }

        public Fish Clone()
        {
            return new Fish(Name, MaxLife);
        }

        public bool IsDead()
        {
            return Age >= MaxLife;
        }
    }

    class Utilite
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int lowerLimitRangeRandom, int upperLimitRangeRandom)
        {
            return s_random.Next(lowerLimitRangeRandom, upperLimitRangeRandom);
        }

        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (VerifyForAcceptableNumber(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool VerifyForAcceptableNumber(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}