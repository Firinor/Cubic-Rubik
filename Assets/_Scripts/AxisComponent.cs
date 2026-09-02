using UnityEngine;

public class AxisComponent : MonoBehaviour
{
    public PlayerInputHolder input;
    public float distance;
    
    private void Start()
    {
        input.onDrag += OnDrag;
        input.onZoom += DistanceChange;
        OnDrag(Vector2.zero, Vector2.zero);
    }

    private void DistanceChange(float obj)
    {
        distance = 0.5f - Camera.main!.transform.localPosition.z;
    }

    private void OnDrag(Vector2 delta, Vector2 point)
    {
        gameObject.SetActive(Vector3.Distance(transform.position, Camera.main.transform.position) > distance);
    }
    
    private void OnDestroy()
    {
        input.onZoom -= DistanceChange;
        input.onDrag -= OnDrag;
    }
    
}
