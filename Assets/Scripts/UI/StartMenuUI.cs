using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button SelectLevelButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(PlayButton_OnClick);
        QuitButton.onClick.AddListener(QuitButton_OnClick);
        SettingsButton.onClick.AddListener(SettingsButton_OnClick);
        SelectLevelButton.onClick.AddListener(SelectLevelButton_OnClick);
    }

    private void PlayButton_OnClick()
    {
        Loader.Load(Loader.Scene.Main);
    }

    private void QuitButton_OnClick()
    {
        Application.Quit();
    }

    private void SettingsButton_OnClick()
    {

    }

    private void SelectLevelButton_OnClick()
    {

    }
}
