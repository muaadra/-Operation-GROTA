using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// this class is for the stories in level 1. example: the story board that 
/// shows at the beginning of the level or when you complete all objectives
/// </summary>

public class StoryMessages_Level1 : StoryMessages 
{


    public HeatTimer heatWaveTimer;

    /// <summary>
    /// this method creates a story board for the level
    /// create a list of StoryMessage and add what you want to show as pop up message
    /// </summary>
    public void showStory1() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Welcome",

            "Hello General,\n\nThe aliens have taken control of several major cities and are " +
            "rapidly expanding. They have already polluted 40% of the Earth's oceans, and have " +
            "done extreme damage...",

            "Commander",
            Commander
            ));

        msgs.Add(new StoryMessage(
            "Message",

            "\n...to our ecosystems.The damage they've done has caused this desert to experience intense heat waves. " +
            "If we don't stop them soon, the heat waves will only get worse.",

            "Commander",
            Commander
        ));


        msgs.Add(new StoryMessage(
            "Message", 
            "\nYour mission is to stop the alien invasion.\n\nAt any cost...",

            "Commander",
            Commander
        ));

        msgs.Add(new StoryMessage(
            "Message",

            "General!\n\nAccording to our meteorologists, we only have 25 mintues before the start of the next heat wave.\n\n" +
            "We must begin constructing...",

            "Communicatons officer",
            communicationOfficer
        ));

        msgs.Add(new StoryMessage(
            "Objective",

            "... and reinforcing our base as soon as possible. Our buildings must achieve level 3 to withstand the incoming heat wave" +
            "\n\nYou must complete all objectives."
,

            "Communicatons officer",
            communicationOfficer
        ));

         msgs.Add(new StoryMessage(
            "Message",

            "Find your objectives and build menu on the bottom left.\n\n" +
            "Your resources are on the top left of the screen.\n\n" + 
            "The next heat wave timer is on top"
,

            "Communicatons officer",
            communicationOfficer
        ) {
            //after the player clicks OK, what method do you want to call
            messageEvent = eventsAfterStory1  
        });

        currentStory = msgs;
        currentStoryCount = 0;
        showStory("Next", "Ok"); //show this story
    }

    /// <summary>
    /// this method creates a story board for the level
    /// create a list of StoryMessage and add what you want to show as pop up message
    /// </summary>
    public void showStory2() {
        List<StoryMessage> msgs = new List<StoryMessage>();

        msgs.Add(new StoryMessage(
            "Congratulations!",

            "Great work General!\n\nOur army is now ready to defeat the hivemind.\n\n" +
            "An aerial transport ship is heading your way to transport you to their base.",

            "Commander",
            Commander
        ));

        msgs.Add(new StoryMessage(
            "Message",

            "General!, we are ready.\n\nHit the 'complete' button to start the transportation" +
            " of our forces",

            "Communicatons officer",
            communicationOfficer
        ) {
            //after this message is shown, what method do you want to call
            messageEvent = completeLevel1
        });
      

        currentStory = msgs;
        currentStoryCount = 0;
        showStory("Next", "Complete"); //show this story
    }

    /// <summary>
    /// go to the next level
    /// </summary>
    private void completeLevel1() {
        LevelManager.manager.goToNextLevel();
    }


    /// <summary>
    /// show the animation under objectives buttion to get player attention
    /// and start heat wave
    /// </summary>
    private void eventsAfterStory1() { 
        heatWaveTimer.startTimer();
        LevelManager.manager.objectivesFlashingBg.SetActive(true);
    }
    
}
