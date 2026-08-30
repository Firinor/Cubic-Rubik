using System;

[Serializable]
public class SettingsData
{
    public float MirrorValue;
    public bool IsAxis;
    public float SFXValue = .2f;
    public bool isPlayerLanguage = false;
    public string Language = "en";
}