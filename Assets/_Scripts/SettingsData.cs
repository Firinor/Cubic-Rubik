using System;

[Serializable]
public class SettingsData
{
    public bool isPlayerLanguage = false;
    public bool IsTimer;
    public bool IsAxis;
    public float MirrorValue;
    public float SFXValue = .2f;
    public string Language = "en";
}