using UnityEngine;
using UnityEngine.Networking;

public class BugReporter : MonoBehaviour
{
    public void SendBugReport()
    {
        string subject = "Rubik's Cube Game";
        
        string body = "Hello Firinor!";
        
        string encodedSubject = UnityWebRequest.EscapeURL(subject);
        string encodedBody = UnityWebRequest.EscapeURL(body);
        
        Application.OpenURL($"mailto:zachesovtm.unitydeveloper@yandex.ru?subject={encodedSubject}&body={encodedBody}");
    }
}