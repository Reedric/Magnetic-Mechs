using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEffectScript : MonoBehaviour
{
    //code for making the effect when the magnet is active move
    private float speed = 9.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
