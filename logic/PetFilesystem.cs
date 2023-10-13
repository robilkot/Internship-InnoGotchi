using System.Text.Json;

namespace InnoGotchi.logic
{
    internal static class PetFilesystem
    {
        static public async Task Write(Pet[] pets, string fileName)
        {
            File.WriteAllText(fileName, null);

            FileStream createStream = File.OpenWrite(fileName);
            await JsonSerializer.SerializeAsync(createStream, pets);
            await createStream.DisposeAsync();
        }

        static public Pet[] Read(string fileName)
        {
            Pet[] pets;
            
            using (FileStream createStream = File.OpenRead(fileName))
            {
                pets = JsonSerializer.Deserialize(createStream, typeof(Pet[])) as Pet[] ?? Array.Empty<Pet>();
            }

            return pets;
        }
    }
}