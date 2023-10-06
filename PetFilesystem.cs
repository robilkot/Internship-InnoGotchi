using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchi
{
    internal static class PetFilesystem
    {
        static void Save(in Pet pet, string filePath)
        {
            //File.WriteAllText(filePath, pet.ToString());
        }
        static Pet Load(string filePath)
        {
            string line;
            line = File.ReadAllText(filePath);
            return new Pet(line);
        }
    }
}