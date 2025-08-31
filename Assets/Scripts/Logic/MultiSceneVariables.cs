using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSceneVariables : MonoBehaviour
{
    //holds variables which are meant to persist across multiple scenes
    [Header("Multi Scene Variables")]
    public bool gamePadNotMouse = false;
    [Header("Singleton")]
    public static MultiSceneVariables multiSceneVariablesInstance;
    private void Awake()
    {
        if (multiSceneVariablesInstance != null && multiSceneVariablesInstance != this)
        {
            Destroy(this); 
        }
        else
        {
            multiSceneVariablesInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
