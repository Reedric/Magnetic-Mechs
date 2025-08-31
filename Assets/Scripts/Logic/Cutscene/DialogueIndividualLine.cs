using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIndividualLine
{
    //a class which holds all of the varaibles associated with each line of dialogue in a cutscene
    public string lineToLoad;
    public string speakerImage;
    public string speakerAudio;
    public float timeToPlay;
    public DialogueIndividualLine(string lineToLoad, string speakerImage, string speakerAudio, float timeToPlay)
    {
        this.lineToLoad = lineToLoad;
        this.speakerImage = speakerImage;
        this.speakerAudio = speakerAudio;
        this.timeToPlay = timeToPlay;
    }
}
