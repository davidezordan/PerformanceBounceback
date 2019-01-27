using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCheck : MonoBehaviour {
    void Start ()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (!obj.isStatic)
            {
                DebugManager.Info(String.Format("Not static: {0}", obj.name));
            }
        }
    }
}
