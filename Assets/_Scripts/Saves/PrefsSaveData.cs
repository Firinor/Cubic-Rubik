using System;
using UnityEngine;

[Serializable]
public class PrefsSaveData : SaveData
{
    public Vector3[] cubicPosition;
    public Quaternion[] cubicRotation;
    public string timerTime;

    public override Vector3[] CubicPosition
    {
        get => cubicPosition;
        set => cubicPosition = value;
    }
    public override Quaternion[] CubicRotation
    {
        get => cubicRotation;
        set => cubicRotation = value;
    }
    public override string TimerTime
    {
        get => timerTime;
        set => timerTime = value;
    }
    
    public override void FirstLoad()
    {
        var data = SaveLoadSystem<PrefsSaveData>.Load("Player", new ());
        cubicPosition = data.CubicPosition;
        cubicRotation = data.CubicRotation;
        timerTime = data.TimerTime;
    }

    public override void ResetProgress()
    {
        var data = new PrefsSaveData();
        cubicPosition = data.CubicPosition;
        cubicRotation = data.CubicRotation;
        timerTime = data.TimerTime;
        Save();
    }

    public override void Save()
    {
        SaveLoadSystem<PrefsSaveData>.Save("Player", this);
    }
}