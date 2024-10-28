using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    [SerializeField] private Material finishedMaterial;
    [SerializeField] private Material unfinishedMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject ui;
    [SerializeField] private TMP_Text queryText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button launchButton;
    private int iLevel;
    private bool bStatus;

    public bool Status => bStatus;

    private void Start()
    {
        SetUIStatus(false);
    }

    private void OnDestroy()
    {
        launchButton.onClick.RemoveAllListeners();
    }

    public void Init(bool _status, string _query, float _timer, int _level)
    {
        SetStatus(_status);
        SetData(_query, _timer);
        iLevel = _level;
        launchButton.onClick.AddListener(SelectTile);
    }

    private void SetStatus(bool _status)
    {
        meshRenderer.material = _status ? finishedMaterial : unfinishedMaterial;
        bStatus = _status;
    }

    public void SetUIStatus(bool _status)
    {
        ui.SetActive(_status);
    }

    private void SetData(string _query, float _timer)
    {
        queryText.text = _query;

        if (_timer == 0.0f)
            timerText.text = "--:--";
        else
        {
            int _time = Mathf.RoundToInt(_timer);
            int _seconds = _time % 60;
            int _minutes = _time / 60;
            timerText.text = $"{_minutes:00}:{_seconds:00}";
        }
    }

    public void SelectTile()
    {
        GameManager.Instance.LaunchLevel(iLevel);
    }
}
