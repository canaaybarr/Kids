using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using _Managers;


namespace SaveSystem
{
    public static class SaveSystem 
    {
        public static void GameSaveSystem(EX_GameManager upgrade)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Game.Kıds";
            FileStream stream = new FileStream(path,FileMode.Create);
            
        }
    }
}
