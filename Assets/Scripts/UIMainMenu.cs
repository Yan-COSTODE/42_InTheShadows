using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button testButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        startButton.onClick.AddListener(Play);
        testButton.onClick.AddListener(Test);
        resetButton.onClick.AddListener(ResetButton);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        testButton.onClick.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    public void SetActive(bool _status)
    {
        gameObject.SetActive(_status);
        resetButton.interactable = SaveManager.Instance.SaveExist;
    }

    private void ResetButton()
    {
        SaveManager.Instance.ResetData();
        resetButton.interactable = false;
    }
    
    private void Play()
    {
        GameManager.Instance.LaunchSelection(false);
    }

    private void Test()
    {
        GameManager.Instance.LaunchSelection(true);
    }

    private void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
