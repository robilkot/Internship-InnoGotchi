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
        private static readonly Farm s_farm = new();
        private static string s_filePath = "pets.txt";

        static ConsoleUserInterface()
        {
            ReadFromFile();
        }

        private static T ReadProperty<T>() where T : Enum
        {
            Console.WriteLine($"Choose {typeof(T).Name}:");

            foreach (var i in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{(int)i} - {(T)i}");
            }

            var inputNumber = Int32.Parse(Console.ReadLine() ?? "0");
            return (T)Enum.ToObject(typeof(T), inputNumber);
        }
        private static string ReadName()
        {
            Console.WriteLine("Input name:");
            return Console.ReadLine() ?? "Unnamed";
        }

        public static void UpdateState()
        {
            s_farm.UpdateState();
        }
        public static void Start()
        {
            while(true)
            {
                Console.Clear();

                UpdateState();
                ShowFarm();

                Console.WriteLine("Choose the action:");
                Console.WriteLine("1. Add new pet");
                Console.WriteLine("2. Remove pet");
                Console.WriteLine("3. Edit pet");
                Console.WriteLine("4. Change path to file with pets");
                Console.WriteLine("5. Save and exit\n");

                var action = Int32.Parse(Console.ReadLine() ?? "2");

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
            s_farm.AddPet(pet);
        }
        public static void RemovePet()
        {
            Console.WriteLine("Input name of pet to remove");
            string name = Console.ReadLine() ?? "Unnamed";

            var pet = s_farm.Pets.Find(p => p.Name == name);
            if(pet != null) s_farm.Pets.Remove(pet);
        }

        public static void EditPet()
        {
            Pet toEdit;
            
            try
            {
                toEdit = SelectPet();
            }
            catch (KeyNotFoundException) {
                return;
            }

            while (true)
            {
                Console.Clear();
                ShowPet(toEdit);

                Console.WriteLine("1. Give food");
                Console.WriteLine("2. Give drink");
                Console.WriteLine("3. Rename");
                Console.WriteLine("4. Edit Body");
                Console.WriteLine("5. Edit Nose");
                Console.WriteLine("6. Edit Mouth");
                Console.WriteLine("7. Edit Eyes");
                Console.WriteLine("8. Return to menu");

                var action = Int32.Parse(Console.ReadLine() ?? "0");

                switch (action)
                {
                    case 1: toEdit.Feed(); break;
                    case 2: toEdit.GiveDrink(); break;
                    case 3: toEdit.Name = ReadName(); break;
                    case 4: toEdit.Body = ReadProperty<Body>(); break;
                    case 5: toEdit.Nose = ReadProperty<Nose>(); break;
                    case 6: toEdit.Mouth = ReadProperty<Mouth>(); break;
                    case 7: toEdit.Eyes = ReadProperty<Eyes>(); break;
                    case 8: return;
                    default: Console.WriteLine("Invalid number. Re-input."); break;
                }
            }
        }
        public static Pet SelectPet()
        {
            Pet pet;
            int petNumber;
            
            if(s_farm.Pets.Count > 1)
            {
                Console.WriteLine("Select pet number:");

                while (true)
                {
                    petNumber = Int32.Parse(Console.ReadLine() ?? "0");
                    if (petNumber < s_farm.Pets.Count)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Number out of range, re-input.");
                    }
                }
            } else if (s_farm.Pets.Count == 1)
            {
                petNumber = 0;
            } else
            {
                Console.WriteLine("No pets found in farm. Do you want to create one? (y/n)");
                switch(Console.ReadKey().KeyChar)
                {
                    case 'y':
                    case 'Y': AddPet(); petNumber = 0; break;
                    default: throw new KeyNotFoundException();
                }
            }

            pet = s_farm.Pets[petNumber];
            return pet;
        }
        public static void ChangePath()
        {
            Console.Clear();

            Console.WriteLine("Enter new path:");
            s_filePath = Console.ReadLine() ?? s_filePath;

            if(File.Exists(s_filePath))
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
            try
            {
                var pets = PetFilesystem.Read(s_filePath);
                s_farm.Clear();
            
                foreach (var pet in pets)
                {
                    s_farm.AddPet(pet);
                }
            }
            catch
            {
                Console.WriteLine("Error reading file!");
                Thread.Sleep(1500);
            }
        }
        public static void SaveToFile()
        {
            UpdateState();
            PetFilesystem.Write(s_farm.Pets.ToArray(), s_filePath);
        }

        public static void ShowPet(in Pet pet)
        {
            if (pet.Dead) Console.Write("(Dead) ");
            Console.WriteLine($"{pet.Name}, {pet.Age()} y.o.\n" +
                $"Body: {pet.Body}, Eyes: {pet.Eyes}, Mouth: {pet.Mouth}, Nose: {pet.Nose}\n" +
                $"Last eat time: {pet.LastEatTime}, Last drink time: {pet.LastDrinkTime}\n" +
                $"Hunger: {pet.Hunger}, Thirst: {pet.Thirst}, Happiness Days: {pet.HappinessDaysCount}\n");
        }
        public static void ShowFarm()
        {
            Console.WriteLine("Farm:");
            foreach(var pet in s_farm.Pets)
            {
                Console.Write(s_farm.Pets.IndexOf(pet) + ". ");
                ShowPet(pet);
            }
        }
    }
}