using System;
using TagInterfacesSpace;
using UnityEngine;

public class Box : MonoBehaviour, IDestructible, IBox
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void DestroyObject()
    {
        _animator.SetBool("IsDestroyed",  true);
    }

    public void OnFinallyDestroyObject()
    {
        Destroy(gameObject);
    }
}