using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi
{
    internal class Farm
    {
        private int _deadPets = 0;
        private int _alivePets = 0;
        private int _avgFeedingPeriod = 0;
        private int _avgThirstQuenchingPeriod = 0;
        private int _avgHappinessDaysCount = 0;
        private int _avgAge = 0;

        public List<Pet> _pets { get; private set; } = new List<Pet>();

        public void AddPet(Pet pet)
        {
            _pets.Add(pet);
        }
        public void UpdateState()
        {
            foreach (Pet pet in _pets)
            {
                pet.UpdateState();
            }

            _deadPets = _pets.Count(p => p._isDead == true);
            _alivePets = _pets.Count(p => p._isDead == false);
        }
        public void Clear()
        {
            _pets.Clear();
            UpdateState();
        }
    }
}
