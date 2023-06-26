using System;
using System.Collections.Generic;

namespace Warrior
{
    class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield();
            battlefield.Play();
        }
    }

    class Battlefield
    {
        private Platoon _platoonRed = new Platoon();
        private Platoon _platoonBlue = new Platoon();
        private Soldier _firstSolider;
        private Soldier _secondSolider;

        public void Play()
        {
            Console.WriteLine("Война между взводами двух стран, краной и синей");
            Console.ReadKey();

            Battle();
            ShowBattleResult();
        }

        public void Battle()
        {
            while (_platoonRed.GetCountSolders() > 0 && _platoonBlue.GetCountSolders() > 0)
            {
                _firstSolider = _platoonRed.GetSoldier();
                _secondSolider = _platoonBlue.GetSoldier();

                Console.WriteLine("Красный взвод:");
                _platoonRed.ShowSoldiers();

                Console.WriteLine("Синий взвод:");
                _platoonBlue.ShowSoldiers();

                _firstSolider.TakeDamage(_secondSolider.Damage);
                _secondSolider.TakeDamage(_firstSolider.Damage);

                _firstSolider.SuperAttack();
                _secondSolider.SuperAttack();

                RemoveSoldierFromPlatoon();

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowBattleResult()
        {
            if (_platoonRed.GetCountSolders() < 0 && _platoonBlue.GetCountSolders() < 0)
            {
                Console.WriteLine("Ничья, оба взвода погибли");
            }
            else if (_platoonRed.GetCountSolders() <= 0)
            {
                Console.WriteLine("Победила синяя страна");
            }
            else if (_platoonBlue.GetCountSolders() <= 0)
            {
                Console.WriteLine("Победила красная страна");
            }
        }

        private void RemoveSoldierFromPlatoon()
        {
            if (_firstSolider.Health <= 0)
            {
                _platoonRed.RemoveSoldier(_firstSolider);
            }

            if (_secondSolider.Health <= 0)
            {
                _platoonBlue.RemoveSoldier(_secondSolider);
            }
        }
    }

    class Platoon
    {
        private List<Soldier> _soldiers = new List<Soldier>();
        private Random _random = new Random();

        public Platoon()
        {
            Create(5, _soldiers);
        }

        public void ShowSoldiers()
        {
            foreach (Soldier soldier in _soldiers)
            {
                soldier.ShowInfo();
            }
        }

        public void RemoveSoldier(Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }

        public int GetCountSolders()
        {
            return _soldiers.Count;
        }

        public Soldier GetSoldier()
        {
            return _soldiers[_random.Next(0, _soldiers.Count)];
        }

        private void Create(int numberOfPlatoon, List<Soldier> soldier)
        {
            for (int i = 0; i < numberOfPlatoon; i++)
            {
                soldier.Add(ChooseSoldier());
            }
        }

        private Soldier ChooseSoldier()
        {
            Random random = new Random();

            int min = 0;
            int max = 3;
            int soldier = random.Next(min, max);

            if (soldier == 0)
            {
                return new Infantry(100, 30, 5);
            }
            else if (soldier == 2)
            {
                return new Artillery(100, 40, 10);
            }
            else
            {
                return new AirForce(100, 50, 15);
            }
        }
    }

    class Soldier
    {
        public Soldier(int health, int damage, int armor)
        {
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public int Armor { get; protected set; }

        public void TakeDamage(int damage)
        {
            Health -= damage - Armor;
            Console.WriteLine($"\n{GetType().Name} нанёс {damage} урона.");
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя: {GetType().Name}, Здоровье: {Health}, Урон: {Damage}, Броня: {Armor}.");
        }

        public void SuperAttack()
        {
            Random random = new Random();

            int randomNumber;
            int number = 1;
            int maximumNumber = 3;

            randomNumber = random.Next(0, maximumNumber);

            if (number == randomNumber)
            {
                Console.WriteLine();
                IncreaseStrength();
            }
        }

        protected virtual void IncreaseStrength() { }
    }

    class Infantry : Soldier
    {
        public Infantry(int health, int damage, int armor) : base(health, damage, armor) { }

        protected override void IncreaseStrength()
        {
            int increaseDamage = 5;

            Console.WriteLine($"{GetType().Name} начала использовать гранаты и увеличил урон на {increaseDamage}");
            Damage += increaseDamage;
        }
    }

    class Artillery : Soldier
    {
        public Artillery(int health, int damage, int armor) : base(health, damage, armor) { }

        protected override void IncreaseStrength()
        {
            int increaseDamage = 5;

            Console.WriteLine($"{GetType().Name} начала попадать точно в цель и увеличил урон на {increaseDamage}");
            Damage += increaseDamage;
        }
    }

    class AirForce : Soldier
    {
        public AirForce(int health, int damage, int armor) : base(health, damage, armor) { }

        protected override void IncreaseStrength()
        {
            int increaseDamage = 5;

            Console.WriteLine($"{GetType().Name} начал бомбить ракетами и увеличил урон на {increaseDamage}");
            Damage += increaseDamage;
        }
    }
}
