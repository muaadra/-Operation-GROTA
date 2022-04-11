using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class is for the stories in level 2. example: the story board that 
/// shows at the beginning of the level or when you complete all objectives
/// </summary>

public class StoryMessages_Level2 : StoryMessages
{

    public HeatTimer heatWaveTimer;

    /// <summary>
    /// this method creates a story board for the level
    /// create a list of StoryMessage and add what you want to show as pop up message
    /// </summary>
    public void showStory1() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Crash report!",

            "General!\n\nYour transport ship was shot down by enemy drones!\n\n" +
            "Luckily most of the crew survived, but you'll have to make the rest of the journey on foot.",

            "Communicatons officer",
            communicationOfficer
        ));

        msgs.Add(new StoryMessage(
            "Crash report!",

            "\nWe crashed in a forest just south of their main base.\n\n" +
            "Unfortunately, this area is already infested with aliens.\n\n",

            "Communicatons officer",
            communicationOfficer
        ));

        msgs.Add(new StoryMessage(
            "Crash report!",

            "\nOur geologists report that this area is rich in resources that " +
            "you can use to rebuild your army on the way.",

            "Communicatons officer",
            communicationOfficer
        ));

        msgs.Add(new StoryMessage(
            "Crash report!",

            "\nBe careful out there general, there are many more aliens in this forest " +
            "than there were in the desert. You may want to construct some fast vehicles " +
            "to scout ahead before moving your whole army in.",

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
    public void showStory2() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Excellent work General!",

            "The hivemind leader is just inside.",
            "Communicatons officer",
            Commander
        ));

        msgs.Add(new StoryMessage(
            "Proceed",

            "Are you ready to proceed?",

            "Communicatons officer",
            communicationOfficer
        ) {
            //after the player clicks OK, what method do you want to call
            messageEvent = goToNextLevel
        });

        currentStory = msgs;
        currentStoryCount = 0;
        showStory("Next", "Ok");
    }


    /// <summary>
    /// go to the next level
    /// </summary>
    private void goToNextLevel() {
        LevelManager.manager.goToNextLevel();
    }
}
