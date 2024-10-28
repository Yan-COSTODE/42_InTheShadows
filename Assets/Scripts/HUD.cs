using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text query;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private Image progressBar;
    [SerializeField] private Button backButton;
    private LevelTemplate levelTemplate;
    private bool bActive = false;

    private void Start()
    {
        levelTemplate = GameManager.Instance?.LevelTemplate;
        backButton.onClick.AddListener(GoBack);
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        if (!bActive)
            return;

        SetTimerText(levelTemplate.Timer);
        progressBar.fillAmount = Easing.Ease(levelTemplate.Progress / 100.0f, EEasing.EASE_IN_QUART);
    }

    private void SetTimerText(float _s)
    {
        int _timer = Mathf.RoundToInt(_s);
        int _seconds = _timer % 60;
        int _minutes = _timer / 60;
        timer.text = $"{_minutes:00}:{_seconds:00}";
    }

    public void SetActive(bool _status)
    {
        gameObject.SetActive(_status);
        bActive = _status;

        if (_status)
            query.text = levelTemplate.GetQuery();
    }

    private void GoBack()
    {
        GameManager.Instance.LaunchSelection(null);
        GameManager.Instance.UIManager.IrisWipe.OnOutFinished += GameManager.Instance.LevelTemplate.Reset;
    }
}
