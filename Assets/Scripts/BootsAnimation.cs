using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsAnimation : MonoBehaviour
{
    public Transform footBone;
    void LateUpdate()
    {
        if (footBone != null)
        {
            transform.position = footBone.position;
            transform.rotation = footBone.rotation;
        }
    }
}
