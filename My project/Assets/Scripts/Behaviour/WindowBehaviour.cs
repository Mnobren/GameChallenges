using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        Close();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
}
