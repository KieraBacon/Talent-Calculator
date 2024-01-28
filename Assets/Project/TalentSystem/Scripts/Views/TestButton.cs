using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using SkillSystem.ScriptableObjects;

namespace SkillSystem.UI
{
    public class TestButton : MonoBehaviour
    {
        [SerializeField] private ScriptableObject _talent;
        
        public Talent DeserializeFromFile(string name)
        {
            Talent talent = JsonConvert.DeserializeObject<Talent>(Read(Path(name)));
            Debug.Log(talent);
            return talent;
        }

        public void SerializeToFile(string name)
        {
            File.WriteAllText(Path(name), JsonConvert.SerializeObject(_talent));
        }
        
        public string Path(string filename)
        {
            string result = System.IO.Path.Combine(Application.dataPath, filename);
            return result;
        }

        public string Read(string path)
        {
            if (!File.Exists(path)) return "";
            string result = File.ReadAllText(path, Encoding.UTF8);
            return result;
        }

        public void Save(string name)
        {
            _talent = TalentObject.Instantiate(DeserializeFromFile(name));
        }
    }
}