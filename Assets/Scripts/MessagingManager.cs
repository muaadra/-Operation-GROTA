using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class takes care of showing messages to the player like toast messages
/// or story messages. 
/// </summary>
public class MessagingManager : MonoBehaviour
{
    public BuildMessage confirmBuildMssg;
    public ToastMessage toastMssg;
    public StoryMessages_Level1 storyMssgs1;
    public StoryMessages_Level2 storyMssgs2;
    public StoryMessages_Level3 storyMssgs3;
    public static MessagingManager manager;

    //generic messages 
    public const string InsufficientFunds = "Insufficient Funds";

    private void Awake() {
        manager = this;
    }

    private void Start() {
        //show the story when the level starts
        int level = SceneManager.GetActiveScene().buildIndex + 1;

        //show introductory story messages
        if (level == 1) {
            storyMssgs1.showStory1();
        } else if (level == 2) {
            storyMssgs2.showStory1();
        }else if (level == 3) {
            storyMssgs3.showStory1();
        }

    }

    /// <summary>
    /// this shows a timed message (shows for x seconds)
    /// </summary>
    /// <param name="message"></param> the message text
    /// <param name="duration"></param> how long to show in seconds
    public void showToast(string message, float duration) {
        toastMssg.showToast(message, duration);
    }

    

}
