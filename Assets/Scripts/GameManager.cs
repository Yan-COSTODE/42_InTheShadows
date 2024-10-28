using UnityEngine;

public class GameManager : SingletonTemplate<GameManager>
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelTemplate levelTemplate;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Player player;
    [SerializeField] private float fMenuFadeTime = 3.0f;
    [SerializeField] private float fLevelFadeTime = 1.0f;
    private bool bTest = false;

    public UIManager UIManager => uiManager;
    public LevelTemplate LevelTemplate => levelTemplate;
    public bool Test => bTest;

    public void LaunchSelection(bool? _test)
    {
        if (_test.HasValue)
            bTest = _test.Value;
        
        LevelTemplateElem[] _database = SaveManager.Instance.LoadData();
        
        if (_database != null)
            LevelTemplate.SetDatabase(_database);
        else
            LevelTemplate.SetDatabaseEmpty();
        
        if (bTest)
            levelTemplate.SetDatabaseTest();
        
        uiManager.IrisWipe.FadeOut(fMenuFadeTime);
        uiManager.IrisWipe.OnOutFinished += () =>
        {
            mapGenerator.DestroyMap();
            mapGenerator.GenerateMap(levelTemplate.Database);
            mainCamera.SetActive(true);
            uiManager.GoToSelection();
            uiManager.IrisWipe.FadeIn(fMenuFadeTime);
            uiManager.IrisWipe.OnInFinished += () => player.SetCanMove(true);
        };
    }
    
    public void LaunchLevel(int _level)
    {
        player.SetCanMove(false);
        uiManager.IrisWipe.FadeOut(fLevelFadeTime);
        uiManager.IrisWipe.OnOutFinished += () =>
        {
            if (levelTemplate.Setup(_level))
            {
                mainCamera.SetActive(false);
                uiManager.GoToHUD();
            }
            
            uiManager.IrisWipe.FadeIn(fLevelFadeTime);
        };
    }

    public void LaunchMenu()
    {
        player.SetCanMove(false);
        uiManager.IrisWipe.FadeOut(fMenuFadeTime);
        uiManager.IrisWipe.OnOutFinished += () =>
        {
            uiManager.GoToMainMenu();
            mapGenerator.DestroyMap();
            uiManager.IrisWipe.FadeIn(fMenuFadeTime);
        };
    }
    
    public void SaveGame()
    {
        if (!bTest)
            SaveManager.Instance.SaveData(LevelTemplate.Database);
    }
    
    public void QuitGame()
    {
        SaveGame();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}