using InnoGotchi.interfaces;
using InnoGotchi.logic;
using InnoGotchi.sqlserver;

namespace InnoGotchi.progresssaver
{
    public class DBProgressSaver : IProgressSaver
    {
        private readonly InnoGotchiContext db = new();
        private static DBProgressSaver? s_instance = null;
        private DBProgressSaver()
        { }

        public static DBProgressSaver GetInstance()
        {
            if (s_instance == null)
                s_instance = new DBProgressSaver();
            return s_instance;
        }

        public async Task Write(List<Pet> pets)
        {
            await db.SaveChangesAsync();
        }

        public List<Pet> Read()
        {
            return db.Pets.ToList();
        }

        public void Delete(Pet pet)
        {
            db.Pets.Remove(pet);
            db.SaveChanges();
        }
    }
}