using InnoGotchi.sqlserver;
using InnoGotchi.logic;
using InnoGotchi.view;

namespace InnoGotchi
{
    internal class InnoGotchi
    {
        static async Task Main()
        {
            await ConsoleUserInterface.Start();
        }
    }
}