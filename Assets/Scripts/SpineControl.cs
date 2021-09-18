using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineControl : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(transform.root.GetComponent<Player>().spineAngle, 0f, 0f);
    }
}

