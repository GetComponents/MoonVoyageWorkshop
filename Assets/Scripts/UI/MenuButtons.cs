using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    GameObject Settings;

    [SerializeField]
    GameObject continueButton;
    bool settingsOpen;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.GetInt("LevelIndex") == 0)
        {
            Destroy(continueButton);
        }
        DefaultPreferences();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    private void DefaultPreferences()
    {
        if (Settings.TryGetComponent<SettingsManager>(out SettingsManager sm))
        {
            sm.DefaultPreferences();
        }
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        SoundManager.Instance.PlaySFX(eSFX.EUIContinueGame, this.gameObject);
        Debug.Log("LoadingTheGame");
        SceneManager.LoadScene("LevelScene");
    }

    public void NewGame()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        SoundManager.Instance.PlaySFX(eSFX.EUIContinueGame, this.gameObject);
        Debug.Log("LoadingTheGame");
        PlayerPrefs.SetInt("LevelIndex", 0);
        PlayerPrefs.SetInt("StarCount", 0);
        PlayerPrefs.SetFloat("Timer", 0);
        SceneManager.LoadScene("StartGameScene");
    }

    public void QuitGame()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        Application.Quit();
    }

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        SoundManager.Instance.PlaySFX(eSFX.EUIOpenSettingsJingle, this.gameObject);
        settingsOpen = true;
        Settings.SetActive(true);
        if (Settings.TryGetComponent<SettingsManager>(out SettingsManager sm))
        {
            sm.OpenSettings();
        }
    }

    public void CloseSettings()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUICloseSettingsJingle, this.gameObject);
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        settingsOpen = false;
        Settings.SetActive(false);
    }

    public void OpenCredits()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIOpenSettingsJingle, this.gameObject);
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        SceneManager.LoadScene("Credits");
    }

    public void GoToMainMenu()
    {
        SoundManager.Instance.PlaySFX(eSFX.EUIButtonPress, this.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
