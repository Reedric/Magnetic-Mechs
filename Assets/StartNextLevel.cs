using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNextLevel : MonoBehaviour
{
    public string levelToLoad = "fill in here";
    public float timeToWait = 1.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3) StartCoroutine(StartTutorialStage4()) ;
    }
    public IEnumerator StartTutorialStage4()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(levelToLoad);
    }
}
