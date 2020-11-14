using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void OnCatch()
    {
        //some effect...
        Destroy(gameObject);
    }
}