using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UISelection : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private TMP_Text levelButton;

    private void Start()
    {
        homeButton.onClick.AddListener(Home);
    }

    private void OnDestroy()
    {
        homeButton.onClick.RemoveAllListeners();
    }

    public void SetActive(bool _status)
    {
        gameObject.SetActive(_status);
    }

    public void SetLevel(int _level)
    {
        if (_level < 0)
            _level = 0;
        
        levelButton.text = $"Score: {_level * 1000}";
    }
    
    private void Home()
    {
        GameManager.Instance.LaunchMenu();
    }
}
