using System;
using TMPro;
using UnityEngine;

public class inGameTimer : MonoBehaviour
{
    public TextMeshProUGUI text;

    private TimeSpan time;

    public void StartCubic()
    {
        if(enabled)
            return;
        
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
        text.text = "<mspace=0.6em>" + ((int)time.TotalMinutes).ToString("D2") + "</mspace>" +
                    ":<mspace=0.6em>" + time.Seconds.ToString("D2") + "</mspace>" +
                    "<size=60%><mspace=0.54em>." + time.Milliseconds.ToString("D3") + "</mspace></size>";
    }
}
