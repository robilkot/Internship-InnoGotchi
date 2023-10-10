using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InnoGotchi
{
    internal static class ConsoleUserInterface
    {
        private static Farm _farm = new Farm();
        private static string filePath = "pets.txt";

        static ConsoleUserInterface()
        {
            ReadFromFile();
            UpdateState();
        }

        private static T ReadProperty<T>() where T : Enum
        {
            Console.WriteLine($"Choose {typeof(T).Name}:");

            foreach (var i in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{(int)i} - {(T)i}");
            }

            int inputNumber = 0;
            Int32.TryParse(Console.ReadLine(), out inputNumber);
            return (T)Enum.ToObject(typeof(T), inputNumber);
        }
        private static string ReadName()
        {
            Console.WriteLine("Input name:");
            return Console.ReadLine() ?? "Unnamed";
        }

        public static void UpdateState()
        {
            foreach (Pet pet in _farm._pets) {
                pet.UpdateState();
            }
        }
        public static void Start()
        {
            while(true)
            {
                Console.Clear();

                ShowFarm();

                Console.WriteLine("Choose the action:");
                Console.WriteLine("1. Add new pet");
                Console.WriteLine("2. Remove pet");
                Console.WriteLine("3. Edit pet");
                Console.WriteLine("4. Change path to file with pets");
                Console.WriteLine("5. Exit\n");

                int action = 0;
                Int32.TryParse(Console.ReadLine() ?? "2", out action);

                switch(action)
                {
                    case 1: AddPet(); break;
                    case 2: RemovePet(); break;
                    case 3: EditPet(); break;
                    case 4: ChangePath(); break;
                    case 5: SaveToFile(); return;
                }
            }
        }
        public static void AddPet()
        {
            Body body = ReadProperty<Body>();
            Nose nose = ReadProperty<Nose>();
            Mouth mouth = ReadProperty<Mouth>();
            Eyes eyes = ReadProperty<Eyes>();

            string name = ReadName();

            Pet pet = new(body, eyes, nose, mouth, name);
            _farm.AddPet(pet);
        }
        public static void RemovePet()
        {
            Console.WriteLine("Input name of pet to remove");
            string name = Console.ReadLine() ?? "Unnamed";

            var pet = _farm._pets.Find(p => p._name == name);
            if(pet != null) _farm._pets.Remove(pet);
        }

        public static void EditPet()
        {
            Pet toEdit = SelectPet();

            while (true)
            {
                Console.Clear();
                ShowPet(toEdit);

                Console.WriteLine("1. Give food");
                Console.WriteLine("2. Give drink");
                Console.WriteLine("3. Rename");
                Console.WriteLine("4. Edit body");
                Console.WriteLine("5. Edit nose");
                Console.WriteLine("6. Edit mouth");
                Console.WriteLine("7. Edit eyes");
                Console.WriteLine("8. Return to menu");

                int action = 0;
                int.TryParse(Console.ReadLine() ?? "0", out action);

                switch (action)
                {
                    case 1: toEdit.Feed(); break;
                    case 2: toEdit.GiveDrink(); break;
                    case 3: toEdit._name = ReadName(); break;
                    case 4: toEdit._body = ReadProperty<Body>(); break;
                    case 5: toEdit._nose = ReadProperty<Nose>(); break;
                    case 6: toEdit._mouth = ReadProperty<Mouth>(); break;
                    case 7: toEdit._eyes = ReadProperty<Eyes>(); break;
                    case 8: return;
                    default: Console.WriteLine("Invalid number. Re-input."); break;
                }
            }
        }
        public static Pet SelectPet()
        {
            Pet pet;

            Console.WriteLine("Select pet number:");

            int petNumber = 0;
            while (true)
            {
                Int32.TryParse(Console.ReadLine() ?? "0", out petNumber);
                if (petNumber < _farm._pets.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Number out of range, re-input.");
                }
            }

            pet = _farm._pets[petNumber];
            return pet;
        }
        public static void ChangePath()
        {
            Console.Clear();

            Console.WriteLine("Enter new path:");
            filePath = Console.ReadLine() ?? filePath;

            if(File.Exists(filePath))
            {
                ReadFromFile();
            }
            else
            {
                SaveToFile();
            }
        }

        public static void ReadFromFile()
        {
            var pets = PetFilesystem.Read(filePath);
            _farm.Clear();
            
            foreach (var pet in pets)
            {
                _farm.AddPet(pet);
            }
        }
        public static void SaveToFile()
        {
            UpdateState();
            PetFilesystem.Write(_farm._pets.ToArray(), filePath);
        }

        public static void ShowPet(in Pet pet)
        {
            pet.UpdateState();

            if (pet._isDead) Console.Write("(Dead) ");
            Console.WriteLine($"{pet._name}, {pet.Age()} y.o.\n" +
                $"Body: {pet._body}, Eyes: {pet._eyes}, Mouth: {pet._mouth}, Nose: {pet._nose}\n" +
                $"Last eat time: {pet._lastEatTime}, Last drink time: {pet._lastDrinkTime}\n" +
                $"Hunger: {pet._hunger}, Thirst: {pet._thirst}, Happiness Days: {pet._happinessDaysCount}\n");
        }
        public static void ShowFarm()
        {
            Console.WriteLine("Farm:");
            foreach(var pet in _farm._pets)
            {
                Console.Write(_farm._pets.IndexOf(pet) + ". ");
                ShowPet(pet);
            }
        }
    }
}