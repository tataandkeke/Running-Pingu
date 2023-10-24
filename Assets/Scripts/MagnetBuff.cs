using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBuff : MonoBehaviour
{
    public bool isMagnetOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isMagnetOn = true;
        }
    }
}
