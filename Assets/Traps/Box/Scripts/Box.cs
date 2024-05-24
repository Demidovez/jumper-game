using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public void OnDestroyObject()
    {
        Destroy(gameObject);
    }
}
