using System.Text.Json;

namespace InnoGotchi.logic
{
    public interface IProgressSaver
    {
        public Task Write(List<Pet> pets);
        public List<Pet> Read();
    }
}
