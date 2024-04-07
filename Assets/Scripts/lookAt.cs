using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour
{
    [SerializeField]
    private Transform _lookingObj;

    void Update()
    {
        transform.LookAt(_lookingObj);
    }
}
