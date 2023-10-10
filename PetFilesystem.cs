namespace InnoGotchi
{
    internal static class PetFilesystem
    {
        static public void Write(Pet[] pets, string filePath)
        {
            File.Delete(filePath);
            Append(pets, filePath);
        }
        static public void Append(Pet[] pets, string filePath)
        {
            foreach (var pet in pets)
            {
                File.AppendAllText(filePath, pet.ToString());
            }
        }

        static public Pet[] Read(string filePath)
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