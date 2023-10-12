using System.Text.Json;

namespace InnoGotchi
{
    internal static class PetFilesystem
    {
        static public async Task Write(Pet[] pets, string fileName)
        {
            // Зачем using FileStream ... ?
            File.WriteAllText(fileName, string.Empty);

            FileStream createStream = File.OpenWrite(fileName);
            await JsonSerializer.SerializeAsync(createStream, pets);
            await createStream.DisposeAsync();
        }

        static public Pet[] Read(string fileName)
        {
            FileStream createStream = File.OpenRead(fileName);
            Pet[] pets = JsonSerializer.Deserialize(createStream, typeof(Pet[])) as Pet[] ?? Array.Empty<Pet>();
            createStream.Dispose();

            return pets;
        }
    }
}