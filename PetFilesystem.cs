namespace InnoGotchi
{
    internal static class PetFilesystem
    {
        async static public Task Write(Pet[] pets, string filePath)
        {
            File.Delete(filePath);
            await Task.Run(() => Append(pets, filePath));
        }
        async static public Task Append(Pet[] pets, string filePath)
        {
            foreach (var pet in pets)
            {
                await File.AppendAllTextAsync(filePath, pet.ToString());
            }
        }

        static public Pet[] Load(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            Pet[] pets = new Pet[lines.Length];

            for(var i = 0; i < lines.Length; i++)
            {
                pets[i] = new Pet(lines[i]);
            }

            return pets;
        }
    }
}