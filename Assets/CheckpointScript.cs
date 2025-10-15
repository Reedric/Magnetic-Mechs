using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [Header("Components")]
    private MultiSceneVariables multiSceneVariables;
    private GameObject player;
    [Header("variable")]
    public int checkpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        multiSceneVariables = GameObject.FindGameObjectWithTag("MultiSceneVariables").GetComponent<MultiSceneVariables>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (multiSceneVariables != null && player != null && checkpoint != 0)
        {
            if(multiSceneVariables.getCheckpoint() == checkpoint)
            {
                float myX = transform.position.x;
                float myY = transform.position.y;
                player.transform.position = new Vector3(myX,myY, player.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(multiSceneVariables.getCheckpoint());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (multiSceneVariables != null && checkpoint != 0)
            {
                multiSceneVariables.setCheckpoint(checkpoint);
            }
        }

    }
}
