using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInputHolder input;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 2f;
    private bool isCameraRotation;
    private bool isFlick;
    public Vector2 MinMaxCameraZoom;
    public float ZoomSpeed;
    [SerializeField] private CubicRotor rotor;
    private Vector3 dragStartPoint;
    private Transform dragStartTransform;
    public void Start()
    {
        Physics.queriesHitBackfaces = false;
        input.onStartDrag += HandleTouchStartInput;
        input.onDrag += HandleTouchInput;
        input.onZoom += CameraDistance;
    }

    private void HandleTouchStartInput(Vector2 point)
    {
        Ray ray = Camera.main!.ScreenPointToRay(point);
        RaycastHit hit;
        
        isFlick = false;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("SideCenter"))
            {
                isCameraRotation = true;
            }
            else
            {
                isCameraRotation = false;
                dragStartTransform = hit.transform;
                dragStartPoint = hit.point;
                //Debug.Log(hit.point);
            }
        }
        else
        {
            isCameraRotation = true;
        }
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

    public void HandleTouchInput(Vector2 delta, Vector2 point)
    {
        if (isCameraRotation)
        {
            Quaternion rotation = Quaternion.Euler(delta.y * rotationSpeed, -delta.x * rotationSpeed, 0);
            target.rotation *= rotation;
        }
        else if(!isFlick)
        {
            Ray ray = Camera.main!.ScreenPointToRay(point);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            
            if(hit.transform == dragStartTransform 
               || hit.collider.CompareTag("SideCenter"))
                return;

            Vector3 slideVector = hit.transform.position - dragStartTransform.position;
            //Debug.Log(slideVector + " " + slideVector.magnitude + " flick:" + isFlick);
            if(slideVector.magnitude > 2.5f)
                return;
            
            isFlick = true;
            if (Mathf.Abs(dragStartPoint.z - 3) < 0.1f)//White side
            {
                if (!Mathf.Approximately(slideVector.x, 0))
                {
                    if(dragStartTransform.position.y < -1)
                        rotor.RotateCubic(slideVector.x < 0 ? "OC" : "OCC");
                    else if(dragStartTransform.position.y > 1)
                        rotor.RotateCubic(slideVector.x > 0 ? "RC" : "RCC");
                }
                else if(!Mathf.Approximately(slideVector.y, 0))
                {
                    if(dragStartTransform.position.x < -1)
                        rotor.RotateCubic(slideVector.y > 0 ? "BC" : "BCC");
                    else if(dragStartTransform.position.x > 1)
                        rotor.RotateCubic(slideVector.y < 0 ? "GC" : "GCC");
                }
            }
            else if (Mathf.Abs(dragStartPoint.z + 3) < 0.1f)//Yellow side
            {
                if (!Mathf.Approximately(slideVector.x, 0))
                {
                    if(dragStartTransform.position.y < -1)
                        rotor.RotateCubic(slideVector.x > 0 ? "OC" : "OCC");
                    else if(dragStartTransform.position.y > 1)
                        rotor.RotateCubic(slideVector.x < 0 ? "RC" : "RCC");
                }
                else if(!Mathf.Approximately(slideVector.y, 0))
                {
                    if(dragStartTransform.position.x < -1)
                        rotor.RotateCubic(slideVector.y < 0 ? "BC" : "BCC");
                    else if(dragStartTransform.position.x > 1)
                        rotor.RotateCubic(slideVector.y > 0 ? "GC" : "GCC");
                }
            }
            else if (Mathf.Abs(dragStartPoint.y + 3) < 0.1f)//Orange side
            {
                if (!Mathf.Approximately(slideVector.x, 0))
                {
                    if(dragStartTransform.position.z < -1)
                        rotor.RotateCubic(slideVector.x < 0 ? "YC" : "YCC");
                    else if(dragStartTransform.position.z > 1)
                        rotor.RotateCubic(slideVector.x > 0 ? "WC" : "WCC");
                }
                else if(!Mathf.Approximately(slideVector.z, 0))
                {
                    if(dragStartTransform.position.x < -1)
                        rotor.RotateCubic(slideVector.z > 0 ? "BC" : "BCC");
                    else if(dragStartTransform.position.x > 1)
                        rotor.RotateCubic(slideVector.z < 0 ? "GC" : "GCC");
                }
            }
            else if (Mathf.Abs(dragStartPoint.y - 3) < 0.1f)//Red side
            {
                if (!Mathf.Approximately(slideVector.x, 0))
                {
                    if(dragStartTransform.position.z < -1)
                        rotor.RotateCubic(slideVector.x > 0 ? "YC" : "YCC");
                    else if(dragStartTransform.position.z > 1)
                        rotor.RotateCubic(slideVector.x < 0 ? "WC" : "WCC");
                }
                else if(!Mathf.Approximately(slideVector.z, 0))
                {
                    if(dragStartTransform.position.x < -1)
                        rotor.RotateCubic(slideVector.z < 0 ? "BC" : "BCC");
                    else if(dragStartTransform.position.x > 1)
                        rotor.RotateCubic(slideVector.z > 0 ? "GC" : "GCC");
                }
            }
            else if (Mathf.Abs(dragStartPoint.x + 3) < 0.1f)//Blue side
            {
                if (!Mathf.Approximately(slideVector.y, 0))
                {
                    if(dragStartTransform.position.z < -1)
                        rotor.RotateCubic(slideVector.y > 0 ? "YC" : "YCC");
                    else if(dragStartTransform.position.z > 1)
                        rotor.RotateCubic(slideVector.y < 0 ? "WC" : "WCC");
                }
                else if(!Mathf.Approximately(slideVector.z, 0))
                {
                    if(dragStartTransform.position.y < -1)
                        rotor.RotateCubic(slideVector.z < 0 ? "OC" : "OCC");
                    else if(dragStartTransform.position.y > 1)
                        rotor.RotateCubic(slideVector.z > 0 ? "RC" : "RCC");
                }
            }
            else if (Mathf.Abs(dragStartPoint.x - 3) < 0.1f)//Green side
            {
                if (!Mathf.Approximately(slideVector.y, 0))
                {
                    if(dragStartTransform.position.z < -1)
                        rotor.RotateCubic(slideVector.y < 0 ? "YC" : "YCC");
                    else if(dragStartTransform.position.z > 1)
                        rotor.RotateCubic(slideVector.y > 0 ? "WC" : "WCC");
                }
                else if(!Mathf.Approximately(slideVector.z, 0))
                {
                    if(dragStartTransform.position.y < -1)
                        rotor.RotateCubic(slideVector.z > 0 ? "OC" : "OCC");
                    else if(dragStartTransform.position.y > 1)
                        rotor.RotateCubic(slideVector.z < 0 ? "RC" : "RCC");
                }
            }
        }
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
        input.onStartDrag -= HandleTouchStartInput;
        input.onDrag -= HandleTouchInput;
    }
}