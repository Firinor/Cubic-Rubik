using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Audio")]
public class AudioConfig : ScriptableObject
{
    [Header("Buttons")] 
    public ClipSettings ButtonClick;
    [Header("Cube")] 
    public ClipSettings CubeFlick;
    [Header("Win")] 
    public ClipSettings Win;
}