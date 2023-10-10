using System.Text;

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

        public readonly DateTime _created = DateTime.Now;
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
            string[] fields = line.Split(',');

            _body = (Body)Enum.Parse(typeof(Body), fields[0]);
            _eyes = (Eyes)Enum.Parse(typeof(Eyes), fields[1]);
            _nose = (Nose)Enum.Parse(typeof(Nose), fields[2]);
            _mouth = (Mouth)Enum.Parse(typeof(Mouth), fields[3]);

            _name = fields[4];
            _created = DateTime.Parse(fields[5]);
            _updated = DateTime.Parse(fields[6]);

            _hunger = (Hunger)Enum.Parse(typeof(Hunger), fields[7]);
            _lastEatTime = DateTime.Parse(fields[8]);
            _thirst = (Thirsty)Enum.Parse(typeof(Thirsty), fields[9]);
            _lastDrinkTime = DateTime.Parse(fields[10]);

            _happinessDaysCount = Int32.Parse(fields[11]);
            _isDead = Boolean.Parse(fields[12]);
        }

        public override string ToString()
        {
            StringBuilder pet = new StringBuilder(256);

            object[] fields = new object[]
            {
                _body, _eyes, _nose, _mouth,
                _name,
                _created, _updated,
                _hunger, _lastEatTime, _thirst, _lastDrinkTime,
                _happinessDaysCount,
                _isDead
            };

            foreach (var field in fields)
            {
                pet.Append(field);
                pet.Append(',');
            }

            pet[^1] = '\n';
            return pet.ToString();
        }
        public void UpdateState()
        {
            if (_isDead)
            {
                _updated = DateTime.Now;
                return;
            }

            // Protects against double decrementations
            if((DateTime.Now - _updated).TotalHours > drinkInterval)
            {
                var thirstDifference = (DateTime.Now - _lastDrinkTime).TotalHours / drinkInterval;
                _thirst -= (thirstDifference < (int)_thirst ? (Thirsty)thirstDifference : _thirst);
            }
            if ((DateTime.Now - _updated).TotalHours > eatInterval)
            {
                var hungerDifference = (DateTime.Now - _lastEatTime).TotalHours / eatInterval;
                _hunger -= (hungerDifference < (int)_hunger ? (Hunger)hungerDifference : _hunger);
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
        public void GiveDrink()
        {
            UpdateState();

            if (_isDead)
            {
                return;
            }

            if (_thirst < Thirsty.Full)
            {
                _thirst++;
                _lastDrinkTime = DateTime.Now;
            }
        }
        public void Feed()
        {
            UpdateState();

            if (_isDead)
            {
                return;
            }

            if (_hunger < Hunger.Full)
            {
                _hunger++;
                _lastEatTime = DateTime.Now;
            }
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