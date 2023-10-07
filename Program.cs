namespace InnoGotchi
{
    internal class InnoGotchi
    {
        async static Task Main(string[] args)
        {
            ConsoleUserInterface.AddPet();
            await Task.Run(() => ConsoleUserInterface.SaveToFile("test.txt"));

            ConsoleUserInterface.ReadFromFile("test.txt");
            ConsoleUserInterface.ShowFarm();
        }
    }
}