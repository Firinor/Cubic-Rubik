using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
#if IS_YANDEX
using YG;
#endif


public class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private SettingsManager settings;

    
    private SaveData player;
    
    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
#if IS_YANDEX
        yield return YG2.onGetSDKData;
#endif
        player = SaveData.GetPlayer();
        player.FirstLoad();
        
        settings.Initialize();
        
#if IS_YANDEX
        YG2.GameReadyAPI();
#endif
    }
}
