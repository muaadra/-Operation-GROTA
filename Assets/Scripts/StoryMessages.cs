using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class creates the story messages that appear at the begining of the level
/// To use this: See StoryMessages_Level1 class for an example
/// </summary>

public class StoryMessages : MonoBehaviour
{
    public Text title;
    public Text message;
    public Text nameOfSender; //from commander or scientist...
    public Text nextButton;
    public Image senderThumbnail;
    public Image BG;
    [HideInInspector] public Color32 bgOrigColor;
    [HideInInspector] public Color32 bgObjectiveColor;

    public Sprite communicationOfficer;
    public Sprite Commander;

    [HideInInspector] public List<StoryMessage> currentStory;
    [HideInInspector] public int currentStoryCount;
    public delegate void StoryMessageSpecialFunction();

    private string nextBttnText;
    private string lastBttnText;

    private void Awake() {
        bgOrigColor = BG.color;
        bgObjectiveColor = new Color32(20, 111, 103, 255);
    }


    /// <summary>
    /// this method is called by subclass when a story is created, it goes through a list
    /// of StoryMessage objects an show each object as a slide of the of the story
    /// </summary>
    /// <param name="nextBttnText"></param> the text to show on the "next" button
    /// <param name="lastBttnText"></param> the text on the button of the last stor
    public void showStory(string nextBttnText, string lastBttnText) {
        this.nextBttnText = nextBttnText;
        this.lastBttnText = lastBttnText;
        LevelManager.manager.UIActive = true;

        //if on last section of story and player clicked "ok", then hide message
        if (currentStoryCount >= currentStory.Count || currentStory == null) {
            currentStoryCount = 0;
            currentStory = null;
            LevelManager.manager.UIActive = false;
            gameObject.SetActive(false);
            return;
        }

        //if it is the last section of the story, then show "Ok" button
        if (currentStoryCount == currentStory.Count - 1) {
            nextButton.text = lastBttnText;
        } else {
            nextButton.text = nextBttnText;
        }

        gameObject.SetActive(true);

        //show story section on UI
        title.text = currentStory[currentStoryCount].title;
        message.text = currentStory[currentStoryCount].message;
        nameOfSender.text = currentStory[currentStoryCount].nameOfSender;
        senderThumbnail.sprite = currentStory[currentStoryCount].thumbnailOfSender;


        //change color of background, if this is an objective
        if (currentStory[currentStoryCount].title == "Objective") {
            BG.color = bgObjectiveColor;
        } else {
            BG.color = bgOrigColor;
        }

        currentStoryCount++;
    }


    /// <summary>
    /// when player clicks on "next" button to show next slide of the story
    /// </summary>
    public void nextStorySectionOnClick() {
        //run the methos associated with the message (if exists)
        if (currentStory[currentStoryCount - 1].messageEvent != null) {
            currentStory[currentStoryCount - 1].messageEvent();
        }
        MusicManager.manager.playClick(1);
        showStory(nextBttnText, lastBttnText);
    }


    /// <summary>
    /// the story 'slide' or 'message' object
    /// </summary>
    public class StoryMessage {
        public string title;
        public string message;
        public string nameOfSender; //from commander or scientist...
        public Sprite thumbnailOfSender;
        public StoryMessageSpecialFunction messageEvent = null;

        public StoryMessage(string title, string message, string nameOfSender,
            Sprite thumbnailOfSender) {
            this.title = title;
            this.message = message;
            this.nameOfSender = nameOfSender;
            this.thumbnailOfSender = thumbnailOfSender;
        }
    }
}
