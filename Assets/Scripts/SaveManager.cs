using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;
    public bool hasLoaded;

    private void Awake()
    {
        instance = this;
        Load();
    }

    public void Save()
    {
        string dataPath = Application.persistentDataPath;

        var fSerializer = new XmlSerializer(typeof(SaveData));
        var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        fSerializer.Serialize(fStream, activeSave);
        fStream.Close();

        Debug.Log("Saved");
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;
        Debug.Log(dataPath);

        if(System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            var fSerializer = new XmlSerializer(typeof(SaveData));
            var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = fSerializer.Deserialize(fStream) as SaveData;
            fStream.Close();
            hasLoaded = true;
            Debug.Log("Loaded");
        }
    }

    public void SaveDelete()
    {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".save");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName = "save";
    public int unlocked = 0;
    public List<int> time = new List<int>();
    public float masterVol = 0;
    public float musicVol = 0;
    public float soundsVol = 0;
}
