using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InnoGotchi
{
    internal static class ConsoleUserInterface
    {
        private static Farm _farm = new Farm();

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
            string name = Console.ReadLine() ?? "Unnamed";

            var pet = _farm._pets.Find(p => p._name == name);
            if(pet != null) _farm._pets.Remove(pet);
        }

        public static void ReadFromFile(string filePath)
        {
            var pets = PetFilesystem.Read(filePath);
            _farm.Clear();
            
            foreach (var pet in pets)
            {
                _farm.AddPet(pet);
            }
        }
        public async static Task SaveToFile(string filePath)
        {
            await PetFilesystem.Write(_farm._pets.ToArray(), filePath);
        }

        public static void ShowPet(in Pet pet)
        {
            Console.WriteLine($"{pet._name}, {pet.Age()} y.o.\n" +
                $"Body: {pet._body}, Eyes: {pet._eyes}, Mouth: {pet._mouth}, Nose: {pet._nose}\n" +
                $"Last eat time: {pet._lastEatTime}, Last drink time: {pet._lastDrinkTime}\n" +
                $"Hunger: {pet._hunger}, Thirst: {pet._thirst}, Happiness Days: {pet._happinessDaysCount}, Dead: {pet._isDead}\n");
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