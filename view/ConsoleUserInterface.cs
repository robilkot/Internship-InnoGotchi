using InnoGotchi.logic;

namespace InnoGotchi.view
{
    internal static class ConsoleUserInterface
    {
        private static readonly Farm s_farm = new();
        private static IProgressSaver? s_progressSaver = null;

        static ConsoleUserInterface()
        {
            s_progressSaver = DBProgressSaver.GetInstance();
            ReadProgress();
        }

        private static void DisplayError(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        private static T ReadProperty<T>() where T : Enum
        {
            Console.WriteLine($"Choose {typeof(T).Name}:");

            foreach (var i in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{(int)i} - {(T)i}");
            }

            _ = int.TryParse(Console.ReadLine(), out int inputNumber);
            return (T)Enum.ToObject(typeof(T), inputNumber);
        }
        private static string ReadName()
        {
            Console.WriteLine("Input name:");
            return Console.ReadLine() ?? Pet.DefaultName;
        }

        public static void UpdateState()
        {
            s_farm.UpdateState();
        }
        private static void ShowActions()
        {
            Console.WriteLine("Choose the action:");
            Console.WriteLine("1. Add new pet");
            Console.WriteLine("2. Remove pet");
            Console.WriteLine("3. Edit pet");
            Console.WriteLine("4. Save and exit\n");
        }
        public static async Task Start()
        {
            while (true)
            {
                Console.Clear();

                UpdateState();
                ShowFarm();
                ShowActions();

                _ = int.TryParse(Console.ReadLine(), out int action);
                switch (action)
                {
                    case 1: AddPet(); break;
                    case 2: RemovePet(); break;
                    case 3: EditPet(); break;
                    case 4: await SaveProgress(); return;
                    default: Console.WriteLine("Invalid number. Re-input"); break;
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
            s_farm.Pets.Add(pet);
        }
        public static void RemovePet()
        {
            try
            {
                Pet toRemove = SelectPet();
                s_farm.Pets.Remove(toRemove);
                s_progressSaver?.Delete(toRemove);
            }
            catch (KeyNotFoundException)
            {
                return;
            }
        }

        private static void ShowEditOptions()
        {
            Console.WriteLine("1. Give food");
            Console.WriteLine("2. Give drink");
            Console.WriteLine("3. Rename");
            Console.WriteLine("4. Edit Body");
            Console.WriteLine("5. Edit Nose");
            Console.WriteLine("6. Edit Mouth");
            Console.WriteLine("7. Edit Eyes");
            Console.WriteLine("8. Return to menu");
        }
        public static void EditPet()
        {
            Pet toEdit;

            try
            {
                toEdit = SelectPet();
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            while (true)
            {
                Console.Clear();
                ShowPet(toEdit);
                ShowEditOptions();

                _ = int.TryParse(Console.ReadLine(), out int action);
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
            int petNumber;

            if (s_farm.Pets.Count > 1)
            {
                Console.WriteLine("Select pet number:");

                while (true)
                {
                    petNumber = int.Parse(Console.ReadLine() ?? "0");
                    if (petNumber < s_farm.Pets.Count)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Number out of range, re-input.");
                    }
                }
            }
            else if (s_farm.Pets.Count == 1)
            {
                petNumber = 0;
            }
            else
            {
                Console.WriteLine("No pets found in farm. Do you want to create one? (y/n)");
                switch (Console.ReadKey().KeyChar)
                {
                    case 'y':
                    case 'Y': AddPet(); petNumber = 0; break;
                    default: throw new KeyNotFoundException();
                }
            }

            return s_farm.Pets[petNumber];
        }

        public static void ReadProgress()
        {
            try
            {
                s_farm.Pets = s_progressSaver?.Read() ?? new();
            }
            catch (Exception e)
            {
                DisplayError(e.Message);
            }
        }
        public static async Task SaveProgress()
        {
            UpdateState();
            try
            {
                await s_progressSaver?.Write(s_farm.Pets);
            }
            catch (Exception e)
            {
                DisplayError(e.Message);
            }
        }

        public static void ShowPet(Pet pet)
        {
            Console.WriteLine(pet.ToString());
            Console.WriteLine();
        }
        public static void ShowFarm()
        {
            Console.WriteLine("Farm:");
            for (var i = 0; i < s_farm.Pets.Count; i++)
            {
                Console.Write(i + ". ");
                ShowPet(s_farm.Pets[i]);
            }
        }
    }
}