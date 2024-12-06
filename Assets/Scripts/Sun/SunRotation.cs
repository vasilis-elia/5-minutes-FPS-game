using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{  
    public float initialAngle = 0f;
    float angle = 0f;

    void Start()
    {
        angle = initialAngle;
    }

    // Update is called once per frame
    void Update()
    {    
        angle += 2f * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
}
