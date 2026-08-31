using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInputHolder input;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 2f;
    public Vector2 MinMaxCameraZoom;
    public float ZoomSpeed;
    [SerializeField] private CubicRotor rotor;
    public void Start()
    {
        input.onDrag += HandleTouchInput;
        input.onZoom += CameraDistance;
    }

    private void CameraDistance(float delta)
    {
        Vector3 pos = Camera.main!.transform.localPosition;
        float distance = -pos.z;
        distance += delta * ZoomSpeed;
        distance = Mathf.Max(MinMaxCameraZoom.x, distance);
        distance = Mathf.Min(MinMaxCameraZoom.y, distance);
        pos.z = -distance;
        Camera.main!.transform.localPosition = pos;
    }

    public void HandleTouchInput(Vector2 delta)
    {
        enabled = true;
        Quaternion rotation = Quaternion.Euler(delta.y * rotationSpeed, -delta.x * rotationSpeed, 0);
        target.rotation *= rotation;
    }
    public void LookFromSide(int side)
    {
        Vector3 sideVector = side switch
        {
            1 => new Vector3(180,270,270),
            2 => new Vector3(180,90,90),
            3 => new Vector3(90,0,0),
            4 => new Vector3(270,0,180),
            5 => new Vector3(180,0,90),
            _ => new Vector3(0,0,270)
        };
        
        target.rotation = Quaternion.Euler(sideVector);
        input.InvokeOnDrag();
        rotor.ResetAutoRotationTimer();
    }
    
    private void OnDestroy()
    {
        input.onZoom -= CameraDistance;
        input.onDrag -= HandleTouchInput;
    }
}