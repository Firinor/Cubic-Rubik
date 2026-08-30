using System;
using UnityEngine;

[Serializable]
public abstract class SaveData
{
    public abstract Vector3[] CubicPosition { get; set; }
    public abstract Quaternion[] CubicRotation { get; set; }
    public abstract string TimerTime { get; set; }

    public abstract void FirstLoad();
    
    public abstract void ResetProgress();
    public abstract void Save();
    
    public static SaveData GetPlayer()
    {
#if IS_YANDEX
        return new YGSaveData();
#else
        return new PrefsSaveData();
#endif
    }
}