using UnityEngine;

public class ActivateBeeBoss : MonoBehaviour
{
    //turns on the drone spawner when the player collides with it
    public BeeBossParentScript beeBossParentScript;
    public GameObject bossCanvas;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && beeBossParentScript != null)
        {
            bossCanvas.SetActive(true);
            beeBossParentScript.gameObject.SetActive(true);
            beeBossParentScript.Activate();
        }
    }
}
