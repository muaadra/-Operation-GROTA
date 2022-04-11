using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class is for the stories in level 3. example: the story board that 
/// shows at the beginning of the level or when you complete all objectives
/// </summary>

public class StoryMessages_Level3 : StoryMessages {

    public HeatTimer heatWaveTimer;

    /// <summary>
    /// this method creates a story board for the level
    /// create a list of StoryMessage and add what you want to show as pop up message
    /// </summary>
    public void showStory1() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Infiltration!",
            "Excellent job General!\n\n" +
            "We have successfully infiltrated the enemy base.\n\n" +
            "Now you must destroy the leader of the hive mind",

            "Commander",
            Commander
        ));

        msgs.Add(new StoryMessage(
            "Resources",

            "We've detected a very high concentration of crystals in the area.\n\n" +
            "The aliens must have been hoarding them to construct their defences.",

            "Communicatons officer",
            communicationOfficer
        ));

         msgs.Add(new StoryMessage(
            "Forcefield",

            "It appears that the hivemind's leader is protected by an impenetrable forcefield.\n\n" +
            "The source appears to be three forcefield generators placed inside the field.\n\n",

            "Communicatons officer",
            communicationOfficer
        ));

        msgs.Add(new StoryMessage(
            "Forcefield",

            "Our physicists believe that EMPs should be able to penetrate the field and disable the generators. " +
            "If we can hit all three generators with EMPs at the same time, it should disable the field.",

            "Communicatons officer",
            communicationOfficer
        ) {
            //after the player clicks OK, what method do you want to call
            messageEvent = showObjectiveFlasingBG
        });

        currentStory = msgs;
        currentStoryCount = 0;
        showStory("Next", "Ok");
    }

    /// <summary>
    /// show the animation under objectives buttion to get player attention
    /// </summary>
    private void showObjectiveFlasingBG() {
        LevelManager.manager.objectivesFlashingBg.SetActive(true);
    }

    /// <summary>
    /// this method creates a story board for the level
    /// create a list of StoryMessage and add what you want to show as pop up message
    /// </summary>
    public void showFinalStory() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Congratulations!",

            "Excellent work general! By destroying the alien leader, their hivemind connection has been severed.\n\n" +
            "Without communication, the aliens have lost all organization abilities.\n\n" +
            "Earth is saved.",
            "Communicatons officer",
            Commander
        ) {
            //after the player clicks OK, what method do you want to call
            messageEvent = showSettingMenu
        });

        currentStory = msgs;
        currentStoryCount = 0;
        showStory("Next", "END");
    }

    /// <summary>
    /// show the settings menu
    /// </summary>
    private void showSettingMenu() {
        LevelManager.manager.settingsMenu.showHideWindow();
    }

}
