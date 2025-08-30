using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIndividualLine
{
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
