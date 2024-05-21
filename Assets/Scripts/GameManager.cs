using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playMenu;
    [SerializeField]
    private GameObject _levelsMenu;
    [SerializeField]
    private GameObject _endlessMenu;
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private GameObject _creditsMenu;

    [SerializeField]
    private AudioSource _click;

    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField] 
    private GameObject _pausePanel;

    [SerializeField]
    private GameObject _main_canvas;
    [SerializeField]
    private GameObject _skin_canvas;

    [SerializeField]
    private GameObject _skin_camera;

    void Start ()
    {
        // do we have saved volume player prefs?
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            // set the mixer volume levels based on the saved player prefs
            mixer.SetFloat("MasterVolume", SaveManager.instance.activeSave.masterVol);
            mixer.SetFloat("SoundsVolume", SaveManager.instance.activeSave.soundsVol);
            mixer.SetFloat("MusicVolume", SaveManager.instance.activeSave.musicVol);
            SetSliders();
        }
        // otherwise just set the sliders
        else
        {
            SetSliders();
        }
}
    void SetSliders() {
        masterSlider.value = SaveManager.instance.activeSave.masterVol;
        sfxSlider.value = SaveManager.instance.activeSave.soundsVol;
        musicSlider.value = SaveManager.instance.activeSave.musicVol;
    }
    void WriteSettings() {
        SaveManager.instance.activeSave.masterVol = masterSlider.value;
        SaveManager.instance.activeSave.soundsVol = sfxSlider.value;
        SaveManager.instance.activeSave.musicVol = musicSlider.value;
    }

    public void SetMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        mixer.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSfxVolume()
    {
        mixer.SetFloat("SoundsVolume", sfxSlider.value);
    }

    public void Click() {_click.Play();}

    public void Play() {_playMenu.SetActive(true);}

    public void HidePlay() {_playMenu.SetActive(false);}

    public void Levels() {_levelsMenu.SetActive(true);}

    public void HideLevels() {_levelsMenu.SetActive(false);}

    public void Endless() {_endlessMenu.SetActive(true);}

    public void HideEndless() {_endlessMenu.SetActive(false);}

    public void Settings() {_settingsMenu.SetActive(true);}

    public void HideSettings() {
        WriteSettings();
        _settingsMenu.SetActive(false);
        SaveManager.instance.Save();
        }

    public void Credits() {_creditsMenu.SetActive(true);}

    public void HideCredits() {_creditsMenu.SetActive(false);}

    public void ToMain() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        }

    public void ToSkinsCanvas() {
        _skin_canvas.SetActive(true);
        _main_canvas.SetActive(false);
        _skin_camera.SetActive(true);
    }

    public void ToMainCanvas() {
        _skin_canvas.SetActive(false);
        _main_canvas.SetActive(true);
        _skin_camera.SetActive(false);
    }

    public void Quit() {Application.Quit();}

    public void Pause() {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
    }
    public void UnPause() {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

    public void LoadNext() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void LoadEndless() {
        SceneManager.LoadScene("endless");
        Time.timeScale = 1;
    }
}
