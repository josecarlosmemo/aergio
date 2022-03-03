using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");        
        float vertical = Input.GetAxis("Vertical");

        //print(horizontal);
        transform.Translate(
            5 * horizontal * Time.deltaTime, 
            5 * vertical * Time.deltaTime, 
            0,
            Space.World
        );
        
    }
}
