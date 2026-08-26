using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public GameObject AxisGameObject;

    public Action<float> OnMirrorChange;
    
    void Awake()
    {
        Instance = this;
    }
}
