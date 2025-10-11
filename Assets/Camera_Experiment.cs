using UnityEngine;

public class Camera_Experiment : MonoBehaviour
{
    public Transform player;
    public float cursorInfluence = 0.5f;
    public float cusorClamp = 0.5f;
    public float smoothSpeed = 0.2f;
    private void LateUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Assuming a 2D game, set z to 0
        Vector3 directionToCursor = (mouseWorldPos - player.position);
        if (directionToCursor.magnitude > cusorClamp)
        {
            directionToCursor = directionToCursor.normalized * cusorClamp;
            mouseWorldPos = player.position + directionToCursor;
        }
        Vector3 targetPosition = Vector3.Lerp(player.position, mouseWorldPos, cursorInfluence);
        targetPosition.z = 0f;
        transform.position = Vector3.Lerp(transform.position,targetPosition,smoothSpeed*Time.deltaTime);


    }
}
