using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void Click(string method)
    {
        GameBehaviour.instance.Invoke(method, 0);
    }
}