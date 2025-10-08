using Cinemachine;
using UnityEngine;

public class BeeBossParentScript : MonoBehaviour
{
    [Header("components")]
    public BeeBossScript beeBossScript;
    private Rigidbody2D myRigidbody2D;
    public CinemachineVirtualCamera myVirtualCamera;
    public CeilingLaserScript ceilingLaserScript;
    [Header("variables")]
    private bool beeBossActive;
    private float speed = 3f;
    
    void Awake()
    {
        beeBossActive = false;
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (beeBossActive)
        {
            Vector2 moveHorizontal = Vector2.right * speed;
            myRigidbody2D.MovePosition(myRigidbody2D.position + moveHorizontal * Time.fixedDeltaTime);
        }
    }
    public void Activate()
    {
        beeBossActive = true;
        if(beeBossScript != null) beeBossScript.activateBoss();
        myVirtualCamera.Follow = gameObject.transform;
    }
}
