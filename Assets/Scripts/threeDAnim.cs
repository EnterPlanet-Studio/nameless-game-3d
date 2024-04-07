using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threeDAnim : MonoBehaviour
{    void Update()
    {
        transform.Rotate(
            20.0f * 2 * Time.deltaTime,
            -30.0f * 2 * Time.deltaTime, 
            10f * 2 * Time.deltaTime, 
            Space.Self
        );
    }
}
