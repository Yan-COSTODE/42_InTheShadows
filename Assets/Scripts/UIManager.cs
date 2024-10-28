using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HUD hud;
    [SerializeField] private UIMainMenu mainMenu;
    [SerializeField] private UIIrisWipe irisWipe;
    [SerializeField] private UISelection selection;

    public UIIrisWipe IrisWipe => irisWipe;
    public UISelection Selection => selection;
    
    private void Start()
    {
        GoToMainMenu();
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        hud.SetActive(false);
        selection.SetActive(false);
    }

    public void GoToSelection()
    {
        HideAll();
        selection.SetActive(true);
    }
    
    public void GoToHUD()
    {
        HideAll();
        hud.SetActive(true);
    }

    public void GoToMainMenu()
    {
        HideAll();
        mainMenu.SetActive(true);
    }
}
