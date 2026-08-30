using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;
   [SerializeField]
   private AudioConfig config;
   [SerializeField]
   private List<AudioSource> audioPool;
   private int soundIndex;

   private const float buttonClickDelay = .1f;
   private DateTime lastButtonClick;

   private AudioSource scoresSource;
   
   private void Awake()
   {
      Instance = this;
      lastButtonClick = DateTime.Now;
   }
   
   public void PlayButtonClick(Vector3 position = default)
   {
      if((DateTime.Now - lastButtonClick).TotalSeconds < buttonClickDelay)
         return;
      
      lastButtonClick = DateTime.Now;
      
      Play(position, config.ButtonClick, isPriority: true);
   }
   public void PlayFlick(Vector3 position = default)
   {
      Play(position, config.CubeFlick, isPriority: true);
   }
   
   public void PlayWin(Vector3 position = default)
   {
      Play(position, config.Win, isPriority: true);
   }
   
   public void Play(Vector3 position, ClipSettings clipData, bool isPriority = false, float volumeMultiplier = 1)
   {
      AudioSource source = audioPool.FirstOrDefault(a => !a.gameObject.activeSelf);

      if (source is null)
      {
         source = audioPool[soundIndex];
      }

      soundIndex++;
      soundIndex %= audioPool.Count;

      source.gameObject.SetActive(true);
      source.transform.position = position;
      source.pitch = 1 + Random.Range(-0.05f, 0.05f);
      source.volume = clipData.Volume * volumeMultiplier;
      
      source.PlayOneShot(clipData.Clip);

      StartCoroutine(DisableAudioSource(source));
   }

   private IEnumerator DisableAudioSource(AudioSource source)
   {
      while (source.isPlaying)
      {
         yield return null;
      }
      
      source.gameObject.SetActive(false);
   }
}
