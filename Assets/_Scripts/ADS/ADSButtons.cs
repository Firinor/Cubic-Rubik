using UnityEngine;
using UnityEngine.UI;

namespace FirYandexService
{
    public class ADSButtons : MonoBehaviour
    {
        [SerializeField] 
        private Button[] buttons;

#if IS_YANDEX
        private void Start()
        {
            if(FirYG2Service.instance == null)
                return;
            FirYG2Service.instance.SetButtons(buttons);
        }
#elif IS_GAMEMONETIZE
        private void Start()
        {
            if(GameMonetize.Instance == null)
                return;
            GameMonetize.Instance.SetButtons(buttons);
        }
#elif IS_MIRRA
        private void Start()
        {
            if(MirraService.Instance == null)
                return;
            MirraService.Instance.SetButtons(buttons);
        }
#else
        private void Start()
        {
            enabled = false;
            Destroy(gameObject);
        }
#endif
    }
}
