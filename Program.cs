namespace InnoGotchi
{
    internal class InnoGotchi
    {
        async static Task Main(string[] args)
        {
            Pet[] pets = new Pet[100];

            for(var i = 0; i < pets.Length; i++ )
            {
                pets[i] = new Pet();
            }

            Console.WriteLine("begin main");

            Task writeToFile = PetFilesystem.Write(pets, "test.txt");

            Console.WriteLine("end main");
            await writeToFile;
        }
    }

    //class Farm
    //{
    //    private readonly int _deadPets = 0;
    //    private readonly int _alivePets = 0;
    //    private readonly int _avgFeedingPeriod = 0;
    //    private readonly int _avgThirstQuenchingPeriod = 0;
    //    private readonly int _avgHappinessDaysCount = 0;
    //    private readonly int _avgAge = 0;

    //    private List<Pet>? _pets;
    //}
}