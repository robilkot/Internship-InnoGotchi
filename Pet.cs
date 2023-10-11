using System.Text;

namespace InnoGotchi
{
    internal class Pet : IComparable
    {
        public static readonly int eatInterval = 12;
        public static readonly int drinkInterval = 12;

        public Body Body { get; set; } = Body.Medium;
        public Eyes Eyes { get; set; } = Eyes.Brown;
        public Nose Nose { get; set; } = Nose.Medium;
        public Mouth Mouth { get; set; } = Mouth.Medium;
        public string Name { get; set; } = "Unnamed";

        public readonly DateTime Created = DateTime.Now;
        private DateTime _updated = DateTime.Now;

        public Hunger Hunger { get; private set; } = Hunger.Full;
        public DateTime LastEatTime { get; private set; } = DateTime.Now;
        public Thirst Thirst { get; private set; } = Thirst.Full;
        public DateTime LastDrinkTime { get; private set; } = DateTime.Now;

        public int HappinessDaysCount { get; private set; } = 0;
        public bool Dead { get; private set; } = false;

        public Pet(Body Body, Eyes Eyes, Nose Nose, Mouth Mouth, string name)
        {
            this.Body = Body;
            this.Eyes = Eyes;
            this.Nose = Nose;
            this.Mouth = Mouth;
            this.Name = name;
        }

        public Pet(string line)
        {
            string[] fields = line.Split(',');

            Body = (Body)Enum.Parse(typeof(Body), fields[0]);
            Eyes = (Eyes)Enum.Parse(typeof(Eyes), fields[1]);
            Nose = (Nose)Enum.Parse(typeof(Nose), fields[2]);
            Mouth = (Mouth)Enum.Parse(typeof(Mouth), fields[3]);

            Name = fields[4];
            Created = DateTime.Parse(fields[5]);
            _updated = DateTime.Parse(fields[6]);

            Hunger = (Hunger)Enum.Parse(typeof(Hunger), fields[7]);
            LastEatTime = DateTime.Parse(fields[8]);
            Thirst = (Thirst)Enum.Parse(typeof(Thirst), fields[9]);
            LastDrinkTime = DateTime.Parse(fields[10]);

            HappinessDaysCount = Int32.Parse(fields[11]);
            Dead = Boolean.Parse(fields[12]);
        }

        public override string ToString()
        {
            StringBuilder pet = new(256);

            object[] fields = new object[]
            {
                Body, Eyes, Nose, Mouth,
                Name,
                Created, _updated,
                Hunger, LastEatTime, Thirst, LastDrinkTime,
                HappinessDaysCount,
                Dead
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
            if (Dead)
            {
                _updated = DateTime.Now;
                return;
            }

            // Protects against double decrementations
            if ((DateTime.Now - _updated).TotalHours > drinkInterval)
            {
                var thirstDifference = (DateTime.Now - LastDrinkTime).TotalHours / drinkInterval;
                Thirst -= (thirstDifference < (int)Thirst ? (Thirst)thirstDifference : Thirst);
            }
            if ((DateTime.Now - _updated).TotalHours > eatInterval)
            {
                var hungerDifference = (DateTime.Now - LastEatTime).TotalHours / eatInterval;
                Hunger -= (hungerDifference < (int)Hunger ? (Hunger)hungerDifference : Hunger);
            }

            if (Thirst == Thirst.Dead || Hunger == Hunger.Dead)
            {
                Dead = true;
            }
            else if (Thirst >= Thirst.Normal && Hunger >= Hunger.Normal)
            {
                HappinessDaysCount += (DateTime.Now - _updated).Days;
            }

            _updated = DateTime.Now;
        }
        public void GiveDrink()
        {
            UpdateState();

            if (Dead)
            {
                return;
            }

            if (Thirst < Thirst.Full)
            {
                Thirst++;
            }

            LastDrinkTime = DateTime.Now;
        }
        public void Feed()
        {
            UpdateState();

            if (Dead)
            {
                return;
            }

            if (Hunger < Hunger.Full)
            {
                Hunger++;
            }

            LastEatTime = DateTime.Now;
        }

        // Returns age as 1 full real week = 1 in-game year
        public int Age()
        {
            return (DateTime.Now - Created).Days / 7;
        }
        public int CompareTo(object? obj)
        {
            if (obj is Pet pet)
            {
                if (this.HappinessDaysCount < pet.HappinessDaysCount)
                {
                    return -1;
                }
                else if (this.HappinessDaysCount > pet.HappinessDaysCount)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}