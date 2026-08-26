using UnityEngine;

public class AxisComponent : MonoBehaviour
{
    public PlayerInputHolder input;
    public float distance;
    
    private void Start()
    {
        input.onDrag += OnDrag;
        OnDrag(Vector2.zero);
    }

    private void OnDrag(Vector2 delta)
    {
        gameObject.SetActive(Vector3.Distance(transform.position, Camera.main.transform.position) > distance);
    }
    
    private void OnDestroy()
    {
        input.onDrag -= OnDrag;
    }
    
}
