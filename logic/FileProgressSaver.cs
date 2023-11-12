using System.Text.Json;

namespace InnoGotchi.logic
{
    public class FileProgressSaver : IProgressSaver
    {
        public string PetFilePath = "pets.txt";

        private static FileProgressSaver? s_instance = null;

        private FileProgressSaver() { }
        public static FileProgressSaver GetInstance()
        {
            if (s_instance == null)
                s_instance = new FileProgressSaver();
            return s_instance;
        }
        public async Task Write(List<Pet> pets)
        {
            File.WriteAllText(PetFilePath, null);

            FileStream createStream = File.OpenWrite(PetFilePath);
            await JsonSerializer.SerializeAsync(createStream, pets);
            await createStream.DisposeAsync();
        }

        public List<Pet> Read()
        {
            List<Pet> pets;

            using (FileStream createStream = File.OpenRead(PetFilePath))
            {
                pets = JsonSerializer.Deserialize(createStream, typeof(List<Pet>)) as List<Pet> ?? new List<Pet>();
            }

            return pets;
        }
    }
}