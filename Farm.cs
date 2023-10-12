namespace InnoGotchi
{
    [Serializable]
    internal class Farm
    {
        public int DeadPets { private set; get; } = 0;
        public int AlivePets { private set; get; } = 0;
        public float AvgHappinessDaysCount { private set; get; } = 0;
        public float AvgAge { private set; get; } = 0;

        public List<Pet> Pets { get; private set; } = new List<Pet>();

        public void AddPet(Pet pet)
        {
            Pets.Add(pet);
        }
        public void UpdateState()
        {
            Parallel.ForEach(Pets, pet => pet.UpdateState());

            DeadPets = Pets.Count(p => p.Dead == true);
            AlivePets = Pets.Count(p => p.Dead == false);
            AvgHappinessDaysCount = (Pets.Count != 0 ? Pets.Sum(pet => pet.HappinessDaysCount) / Pets.Count : 0);
            AvgAge = (Pets.Count != 0 ? Pets.Sum(pet => pet.Age()) / Pets.Count : 0);
        }
        public void Clear()
        {
            Pets.Clear();
            UpdateState();
        }
    }
}
