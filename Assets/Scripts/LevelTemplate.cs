using System;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    [SerializeField] private Transform objectPlaceholder;
    [SerializeField] private Transform roomPlaceholder;
    [SerializeField] private GameObject levelCamera;
    [SerializeField] private LevelTemplateElem[] database;
    [SerializeField] private LevelTemplateRoom[] room;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material outlineSelectedMaterial;
    private int iLevel;
    private ObjectRotated objectRotated;
    private float fTimer = 0.0f;
    private bool bPlaying = false;

    public float Timer => fTimer;
    public float Progress => objectRotated.Progress;
    public LevelTemplateElem[] Database => database;
    public Material OutlineMaterial => outlineMaterial;
    public Material OutlineSelectedMaterial => outlineSelectedMaterial;
    
    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (!bPlaying)
            return;
        
        fTimer += Time.deltaTime;
    }

    public void SetDatabase(LevelTemplateElem[] _database)
    {
        database = _database;
    }
    
    public bool Setup(int _level)
    {
        if (_level < 0 || _level >= database.Length)
        {
            Debug.LogWarning($"Cannot setup a room of level {_level}");
            return false;
        }

        LevelTemplateElem _elem = database[_level];
        SetRoom(_elem.Room);
        objectRotated = Instantiate(_elem.ObjectToFind, objectPlaceholder);
        objectRotated.OnObjectFinished += FinishGame;
        // objectRotated.SetSelected(true);
        levelCamera.SetActive(true);
        fTimer = 0.0f;
        bPlaying = true;
        iLevel = _level;
        return true;
    }

    public void Reset()
    {
        ResetRoom();
        levelCamera.SetActive(false);
        fTimer = 0.0f;
        bPlaying = false;
    }

    private void FinishGame()
    {
        if (!GameManager.Instance.Test)
            database[iLevel].SetResult(true, fTimer - database[iLevel].ObjectToFind.LerpTime);
        
        GameManager.Instance.SaveGame();
        GameManager.Instance.LaunchSelection(null);
        GameManager.Instance.UIManager.IrisWipe.OnOutFinished += Reset;
    }
    
    public string GetQuery()
    {
        if (iLevel >= 0 && iLevel < database.Length) 
            return database[iLevel].Query;
        
        Debug.LogWarning($"Cannot get a query of level {iLevel}");
        return string.Empty;
    }
    
    private void SetRoom(ELevelRoom _room)
    {
        int i;
        
        for (i = 0; i < room.Length; i++)
            if (room[i].Room == _room)
                break;
        
        if (i == room.Length)
        {
            Debug.LogWarning($"Cannot setup a room of {_room}");
            return;
        }
        
        ResetRoom();
        Instantiate(room[i].Prefab, roomPlaceholder);
    }

    private void ResetRoom()
    {
        for (int i = 0; i < roomPlaceholder.childCount; i++)
            Destroy(roomPlaceholder.GetChild(i).gameObject);
        
        for (int i = 0; i < objectPlaceholder.childCount; i++)
            Destroy(objectPlaceholder.GetChild(i).gameObject);
    }
    
    public void SetDatabaseTest()
    {
        for (int i = 0; i < database.Length; i++)
            database[i].SetTestMode();
    }

    public void SetDatabaseEmpty()
    {
        for (int i = 0; i < database.Length; i++)
            database[i].SetEmpty();
    }
}

[Serializable]
public struct LevelTemplateRoom
{
    [SerializeField] private ELevelRoom room;
    [SerializeField] private GameObject prefab;

    public ELevelRoom Room => room;
    public GameObject Prefab => prefab;
}

[Serializable]
public struct LevelTemplateElem
{
    [SerializeField] private ELevelRoom room;
    [SerializeField] private ObjectRotated objectToFind;
    [SerializeField] private string query;
    [SerializeField, HideInInspector] private bool bCompleted;
    [SerializeField, HideInInspector] private float fFastestTime;
    
    public ELevelRoom Room => room;
    public ObjectRotated ObjectToFind => objectToFind;
    public string Query => query;
    public bool Completed => bCompleted;
    public float FastestTime => fFastestTime;

    public LevelTemplateElem(ELevelRoom _room, ObjectRotated _objectToFind, string _query, bool _bCompleted,
        float _fFastestTime)
    {
        room = _room;
        objectToFind = _objectToFind;
        query = _query;
        bCompleted = _bCompleted;
        fFastestTime = _fFastestTime;
    }

    public void SetResult(bool _status, float _time)
    {
        bCompleted = _status;
        
        if (fFastestTime == 0.0f)
            fFastestTime = _time;
        else if (_time < fFastestTime)
            fFastestTime = _time; 
    }
    
    public void SetTestMode()
    {
        SetResult(true, 10.0f);
    }
    
    public void SetEmpty()
    {
        SetResult(false, 0.0f);
    }
}

[Serializable]
public struct LevelTemplateElemWrapper
{
    [SerializeField] private LevelTemplateElem[] array;

    public LevelTemplateElem[] Array => array;
    
    public LevelTemplateElemWrapper(LevelTemplateElem[] _array)
    {
        array = _array;
    }
}

[Serializable]
public enum ELevelRoom
{
    Base
}