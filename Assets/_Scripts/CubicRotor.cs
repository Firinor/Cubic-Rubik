using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CubicRotor : MonoBehaviour
{
    public PlayerInputHolder input;
    
    public Transform Rotor;
    public Transform Cubic;
    
    public Button UndoButton;
    public Button RedoButton;

    public float aminationSpeed = 2;
    public float aminationResetSpeed = 3;
    
    private List<CubicComand> commands = new(512);
    private int currentIndex = 0;
    private bool isUndo = false;

    private Coroutine coroutine;
    public CameraController CameraController;
    public inGameTimer Timer;

    private float offlineTime;
    public float offlineTimeLimit = 10f;
    public float offlineRotationSpeed = 1f;

    public List<Transform> CheckList;

    private void Awake()
    {
        input.onDrag += HandleTouchInput;
        offlineTime = offlineTimeLimit * 0.8f;
    }

    public void RotateCubic(string command)
    {
        if (Enum.TryParse<CubicComand>(command, out CubicComand newComand))
            commands.Add(newComand);
        else
            throw new Exception();
        
        if (commands.Count >= 512)
        {
            commands.RemoveRange(0, 256);
            currentIndex -= 256;
        }
    }

    private void HandleTouchInput(Vector2 obj)
    {
        offlineTime = 0;
    }
    
    private void Update()
    {
        OfflainRotation();
        
        if(commands.Count == currentIndex)
            return;
        
        if(coroutine is not null)
            return;
        
        coroutine = StartCoroutine(AnimateRotation(commands[currentIndex], CheckPlayerMove));
    }

    private void CheckPlayerMove()
    {
        currentIndex++;
        coroutine = null;
        if (IsSolved())
        {
            Timer.enabled = false;
        }
    }
    
    private void OfflainRotation()
    {
        if(offlineTime >= offlineTimeLimit)
            CameraController.HandleTouchInput(new Vector2(offlineRotationSpeed*Time.deltaTime, 0));
        else
            offlineTime += Time.deltaTime;
    }

    private IEnumerator AnimateRotationList(List<CubicComand> commands, Action onComplete = null)
    {
        foreach (var comand in commands)
        {
            yield return AnimateRotation(comand);
        }
        onComplete?.Invoke();
    }
    
    private IEnumerator AnimateRotation(CubicComand command, Action onComplete = null)
    {
        List<Transform> sideCubes = new(9);
        Rotor.rotation = Quaternion.identity;
        Quaternion start = Rotor.rotation;
        Quaternion target = Rotor.rotation;
        switch (command)
        {
            case CubicComand.BC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.y < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, -90, 0);
                break;
            case CubicComand.BCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.y < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 90, 0);
                break;
            case CubicComand.YC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.z < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 0, -90);
                break;
            case CubicComand.YCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.z < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 0, 90);
                break;
            case CubicComand.GC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.x > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(90, 0, 0);
                break;
            case CubicComand.GCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.x > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(-90, 0, 0);
                break;
            case CubicComand.OC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.x < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(-90, 0, 0);
                break;
            case CubicComand.OCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.x < -1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(90, 0, 0);
                break;
            case CubicComand.WC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.z > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 0, 90);
                break;
            case CubicComand.WCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.z > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 0, -90);
                break;
            case CubicComand.RC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.y > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, 90, 0);
                break;
            case CubicComand.RCC:
                for (int i = 0; i < Cubic.childCount; i++)
                    if (Cubic.GetChild(i).position.y > 1)
                        sideCubes.Add(Cubic.GetChild(i));
                target *= Quaternion.Euler(0, -90, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
        for(int i = 0; i < sideCubes.Count; i++)
            sideCubes[i].SetParent(Rotor, worldPositionStays: true);
        float elapsed = 0;
        while (elapsed < aminationSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / aminationSpeed;
            Rotor.rotation = Quaternion.Lerp(start, target, t);
            yield return null;
        }

        for (int i = 0; i < sideCubes.Count; i++)
        {
            Transform cube = sideCubes[i];
            cube.SetParent(Cubic, worldPositionStays: true);
            Vector3 position = cube.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            position.z = Mathf.Round(position.z);
            cube.position = position;
        }
        onComplete?.Invoke();
    }

    public void CubicButtonClick()
    {
        if (IsSolved())
        {
            ShaffleCubic();
        }
        else
        {
            Timer.enabled = false;
            commands = new(512);
            currentIndex = 0;
            StartCoroutine(ResetCubic());
        }
    }
    
    public IEnumerator ResetCubic()
    {
        float t = 0;
        List<Quaternion> rotations = new();
        for (int i = 0; i < CheckList.Count; i++)
            rotations.Add(CheckList[i].rotation);
        while (t < aminationResetSpeed)
        {
            t += Time.deltaTime;
            for (int i = 0; i < CheckList.Count; i++)
                CheckList[i].rotation = Quaternion.Lerp(rotations[i], Quaternion.identity, t/aminationResetSpeed);
            yield return null;
        }
        for (int i = 0; i < CheckList.Count; i++)
            CheckList[i].rotation = Quaternion.identity;
    }

    public void ShaffleCubic()
    {
        int randomCount = Random.Range(26, 44);
        string[] faces = { "Y", "W", "B", "R", "G", "O" };
        string[] modifiers = { "C", "CC", "2" };
    
        List<string> scramble = new List<string>();
        string lastFace = "";
    
        for (int i = 0; i < randomCount; i++)
        {
            string face;
            do
            {
                face = faces[Random.Range(0, faces.Length)];
            } while (face == lastFace);
        
            string modifier = modifiers[Random.Range(0, modifiers.Length)];
            scramble.Add(face + modifier);
            lastFace = face;
        }

        List<CubicComand> commands = new();
        foreach (var code in scramble)
        {
            if (code.EndsWith("2"))
            {
                var replace = code.Replace("2", "C");
                Enum.TryParse<CubicComand>(replace, out CubicComand newComand);
                commands.Add(newComand);
            }
            else
            {
                Enum.TryParse<CubicComand>(code, out CubicComand newComand);
                commands.Add(newComand);
            }
        }

        coroutine = StartCoroutine(AnimateRotationList(commands, onComplete: Timer.StartCubic));
    }
    public bool IsSolved()
    {
        foreach (var cube in CheckList)
            if (Quaternion.Angle(cube.rotation, Quaternion.identity) > 1)
                return false;
        
        return true;
    }

    private void OnDestroy()
    {
        input.onDrag -= HandleTouchInput;
    }
}
public enum CubicComand
{
    YC,
    YCC,
    WC,
    WCC,
    BC,
    BCC,
    RC,
    RCC,
    OC,
    OCC,
    GC,
    GCC
}