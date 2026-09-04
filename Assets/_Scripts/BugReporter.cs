using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#if IS_YANDEX
using YG;
#endif

public class BugReporter : MonoBehaviour
{
    public Button Button;
    
    public void Initialize()
    {
#if IS_YANDEX
        YG2.onReviewSent += ReviewSent;

        if (YG2.reviewCanShow)
            Button.onClick.AddListener(YG2.ReviewShow);
        else
            Button.gameObject.SetActive(false);
#else
        Button.onClick.AddListener(SendBugReport);
#endif
    }

    public void SendBugReport()
    {
        string subject = "Rubik's Cube Game";
        
        string body = "Hello Firinor!";
        
        string encodedSubject = UnityWebRequest.EscapeURL(subject);
        string encodedBody = UnityWebRequest.EscapeURL(body);
        
        Application.OpenURL($"mailto:zachesovtm.unitydeveloper@yandex.ru?subject={encodedSubject}&body={encodedBody}");
    }
    
    private void ReviewSent(bool sent)
    {
        if (sent)
            Button.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        Button.onClick.RemoveAllListeners();
#if IS_YANDEX
        YG2.onReviewSent -= ReviewSent;
#endif
    }
}