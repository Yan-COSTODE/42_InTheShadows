using System.IO;
using UnityEngine;

public class SaveManager : SingletonTemplate<SaveManager>
{
    private string savePath;

    public bool SaveExist => File.Exists(savePath);

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveData(LevelTemplateElem[] dataArray)
    {
        string jsonData = JsonUtility.ToJson(new LevelTemplateElemWrapper(dataArray));
        File.WriteAllText(savePath, jsonData);
        Debug.Log("Data saved at: " + savePath);
    }

    public void ResetData()
    {
        File.Delete(savePath);
    }
    
    public LevelTemplateElem[] LoadData()
    {
        if (SaveExist)
        {
            string jsonData = File.ReadAllText(savePath);
            LevelTemplateElemWrapper dataWrapper = JsonUtility.FromJson<LevelTemplateElemWrapper>(jsonData);
            return dataWrapper.Array;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }
}
