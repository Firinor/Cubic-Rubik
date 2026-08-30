using UnityEngine;

public class MirrorComponent : MonoBehaviour
{
    public void Initialize()
    {
        SettingsManager.Instance.OnMirrorChange += OnMirrorChange;
    }

    private void OnMirrorChange(float distance)
    {
        gameObject.SetActive(distance > 0.5);

        distance = distance > .5f ? distance : .5f;
        
        Vector3 position = transform.localPosition;
        position.x = Mathf.Approximately(position.x, 0) ? 0 : position.x > 0 ? distance : -distance;
        position.y = Mathf.Approximately(position.y, 0) ? 0 : position.y > 0 ? distance : -distance;
        position.z = Mathf.Approximately(position.z, 0) ? 0 : position.z > 0 ? distance : -distance;
        transform.localPosition = position;
    }
    
    private void OnDestroy()
    {
        SettingsManager.Instance.OnMirrorChange -= OnMirrorChange;
    }
}
