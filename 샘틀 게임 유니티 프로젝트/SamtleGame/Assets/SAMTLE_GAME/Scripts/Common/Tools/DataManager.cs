using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MIT.SamtleGame.Tools
{
    [System.Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    public class DataManager
    {
        public static void JsonFileSave(object obj, string filePath, string fileName, FileMode mode = FileMode.Create)
        {
            string jsonData = JsonUtility.ToJson(obj);

            FileStream stream = new FileStream(string.Format("{0}/{1}.json", filePath, fileName), mode);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
        public static void JsonFileSave<T>(List<T> obj, string filePath, string fileName, FileMode mode = FileMode.Create)
        {
            string jsonData = JsonUtility.ToJson(new Serialization<T>(obj));

            FileStream stream = new FileStream(string.Format("{0}/{1}.json", filePath, fileName), mode);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();

            Debug.Log(string.Format("{0}/{1}.json 저장완료", filePath, fileName));
        }

         public static T JsonFileLoad<T>(string filePath, string fileName)
        {
            FileStream stream = new FileStream(string.Format("{0}/{1}.json", filePath, fileName), FileMode.Open);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            string jsonData = Encoding.UTF8.GetString(data);
            Debug.Log(string.Format("{0}/{1}.json 로드완료", filePath, fileName));

            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}
