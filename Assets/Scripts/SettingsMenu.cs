using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject menu;
    public Sprite speakeOn;
    public Sprite speakeOff;
    public Image allSounds;
    public Image BGMusicImg;
    private AudioSource BGMusicAS;

    private void Start() {
        BGMusicAS = MusicManager.manager.BGMusicAS;
        LevelManager.manager.settingsMenu = this;
    }

    /// <summary>
    /// to mute/unmute all sounds by setting AudioListener.volume = 0
    /// </summary>
    public void muteAllSounds() {
        float vol = AudioListener.volume;
        
        //toggle mute/unmute
        if (vol > 0) {
            AudioListener.volume = 0;
            allSounds.sprite = speakeOff;
        } else {
            AudioListener.volume = 1;
            allSounds.sprite = speakeOn;
        }    
    }

    /// <summary>
    /// to mute background music
    /// </summary>
    public void muteBackGroundMusic() {
        bool active = BGMusicAS.isActiveAndEnabled;
        
        //set speaker image based on bg music status
        if (active) {
            BGMusicImg.sprite = speakeOff;
        } else {
            BGMusicImg.sprite = speakeOn;
        }

        //toggle bg music
        BGMusicAS.enabled = !active;
    }

    /// <summary>
    /// exit the game
    /// </summary>
    public void exitGame() {
        Application.Quit();
        print("game Exited");
    }

    /// <summary>
    /// restart the level
    /// </summary>
    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// show/hide settings window
    /// </summary>
    public void showHideWindow() {
        menu.SetActive(!menu.activeSelf);
        LevelManager.manager.UIActive = menu.activeSelf;
    }

    private void Update() {
        //show settings window when 'escape' key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            showHideWindow();
        }
    }
}
