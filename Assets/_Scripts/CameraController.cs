using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInputHolder input;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 2f;
    public Vector3 startCameraRotation;

    public void Start()
    {
        input.onDrag += HandleTouchInput;
    }

    public void CameraToStart()
    {
        target.rotation = Quaternion.Euler(startCameraRotation);
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
            1 => Vector3.up,
            2 => Vector3.down,
            3 => Vector3.right,
            4 => Vector3.left,
            5 => Vector3.up*2,
            _ => Vector3.forward
        };
        
        target.rotation = Quaternion.Euler(sideVector*90);
    }
    
    private void OnDestroy()
    {
        input.onDrag -= HandleTouchInput;
    }
}