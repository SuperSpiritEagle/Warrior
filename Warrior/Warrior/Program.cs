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
        private Platoon _redPlatoon = new Platoon();
        private Platoon _bluePlatoon = new Platoon();
        private Soldier _redSolider;
        private Soldier _blueSolider;

        public void Play()
        {
            Console.WriteLine("Война между взводами двух стран, краной и синей");
            Console.ReadKey();

            Battle();
            ShowBattleResult();
        }

        public void Battle()
        {
            while (_redPlatoon.HasSoldiers && _bluePlatoon.HasSoldiers)
            {
                _redSolider = _redPlatoon.GetSoldier();
                _blueSolider = _bluePlatoon.GetSoldier();

                Console.WriteLine("Красный взвод:");
                _redPlatoon.ShowSoldiers();

                Console.WriteLine("Синий взвод:");
                _bluePlatoon.ShowSoldiers();

                _redSolider.Attack(_blueSolider);
                _blueSolider.Attack(_redSolider);
              
                _redSolider.SuperAttack();
                _blueSolider.SuperAttack();

                RemoveSoldierFromPlatoon(_redSolider, _redPlatoon);
                RemoveSoldierFromPlatoon(_blueSolider, _bluePlatoon);

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowBattleResult()
        {
            if (_redPlatoon.HasSoldiers && _bluePlatoon.HasSoldiers)
            {
                Console.WriteLine("Ничья, оба взвода погибли");
            }
            else if (_redPlatoon.HasSoldiers)
            {
                Console.WriteLine("Победила синяя страна");
            }
            else if (_bluePlatoon.HasSoldiers)
            {
                Console.WriteLine("Победила красная страна");
            }
        }

        private void RemoveSoldierFromPlatoon(Soldier soldier, Platoon platoon)
        {
            if (soldier.IsDead)
            {
                platoon.RemoveSoldier(soldier);
            }
        }
    }

    class Platoon
    {
        private List<Soldier> _soldiers = new List<Soldier>();
        private static Random _random = new Random();

        public Platoon()
        {
            Create(5, _soldiers);
        }

        public bool HasSoldiers => _soldiers.Count > 0;

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

        public Soldier GetSoldier()
        {
            return _soldiers[_random.Next(_soldiers.Count)];
        }

        private void Create(int numberOfPlatoon, List<Soldier> soldier)
        {
            Soldier[] soldiers = { new Infantry(0, 0, 0), new Artillery(0, 0, 0), new AirForce(0, 0, 0) };

            for (int i = 0; i < numberOfPlatoon; i++)
            {
                soldier.Add(soldiers[_random.Next(soldiers.Length)].Clone());
            }
        }
    }

    abstract class Soldier
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

        public bool IsDead => Health <= 0;

        public void Attack(Soldier target)
        {
            Console.Write($"Атакую цель: {target.GetType().Name}");

            TakeDamage(target.Damage);
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

        public abstract Soldier Clone();

        protected virtual void IncreaseStrength() { }

        private void TakeDamage(int damage)
        {
            Health -= (Math.Abs(damage - Armor));
            Console.WriteLine($"\n{GetType().Name} нанёс {damage} урона.\n");
        }
    }

    class Infantry : Soldier
    {
        public Infantry(int health, int damage, int armor) : base(health, damage, armor) { }

        public override Soldier Clone()
        {
            return new Infantry(100, 30, 5);
        }

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

        public override Soldier Clone()
        {
            return new Artillery(100, 40, 10);
        }

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

        public override Soldier Clone()
        {
            return new AirForce(100, 50, 15);
        }

        protected override void IncreaseStrength()
        {
            int increaseDamage = 5;

            Console.WriteLine($"{GetType().Name} начал бомбить ракетами и увеличил урон на {increaseDamage}");
            Damage += increaseDamage;
        }
    }
}
