using System;
using TMPro;
using UnityEngine;

public class inGameTimer : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    private TimeSpan time;

    public void StartCubic()
    {
        time = TimeSpan.Zero;
        enabled = true;
    }

    public void SwitchTimer()
    {
        text.gameObject.SetActive(!text.gameObject.activeSelf);
    }
    
    void Update()
    {
        time += TimeSpan.FromSeconds(Time.deltaTime);
        text.text = time.ToString(@"mm\:ss");
    }
}
