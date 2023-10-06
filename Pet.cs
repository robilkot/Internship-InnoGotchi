using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi
{
    internal class Pet : IComparable
    {
        public static readonly int eatInterval = 24;
        public static readonly int drinkInterval = 24;

        public Body _body { get; set; } = Body.Medium;
        public Eyes _eyes { get; set; } = Eyes.Brown;
        public Nose _nose { get; set; } = Nose.Medium;
        public Mouth _mouth { get; set; } = Mouth.Medium;
        public string _name { get; set; } = "Unnamed";

        private readonly DateTime _created = DateTime.Now;
        private DateTime _updated = DateTime.Now;

        public Hunger _hunger { get; private set; } = Hunger.Full;
        public DateTime _lastEatTime { get; private set; } = DateTime.Now;
        public Thirsty _thirst { get; private set; } = Thirsty.Full;
        public DateTime _lastDrinkTime { get; private set; } = DateTime.Now;

        public int _happinessDaysCount { get; private set; } = 0;
        public bool _isDead { get; private set; } = false;

        public Pet() { }
        public Pet(Body body, Eyes eyes, Nose nose, Mouth mouth, string name)
        {
            _body = body;
            _eyes = eyes;
            _nose = nose;
            _mouth = mouth;
            _name = name;
        }

        public Pet(string line)
        {

        }
        //public override string ToString()
        //{
        
        //}
        public void UpdateState()
        {
            if (_isDead) return;

            if ((DateTime.Now - _lastDrinkTime).Hours > drinkInterval)
            {
                _thirst--;
            }
            if ((DateTime.Now - _lastEatTime).Hours > eatInterval)
            {
                _hunger--;
            }

            if (_thirst == Thirsty.Dead || _hunger == Hunger.Dead)
            {
                _isDead = true;
            }
            else if (_thirst >= Thirsty.Normal && _hunger >= Hunger.Normal)
            {
                _happinessDaysCount += (DateTime.Now - _updated).Days;
            }

            _updated = DateTime.Now;
        }
        // Returns age as 1 full real week = 1 in-game year
        public int Age()
        {
            return (DateTime.Now - _created).Days / 7;
        }
        public int CompareTo(object? obj)
        {
            if (obj is Pet pet)
            {
                if (this._happinessDaysCount < pet._happinessDaysCount)
                {
                    return -1;
                }
                else if (this._happinessDaysCount > pet._happinessDaysCount)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}