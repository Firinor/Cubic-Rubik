using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubicRotor : MonoBehaviour
{
    public Transform Rotor;
    public Transform Cubic;
    
    public Button UndoButton;
    public Button RedoButton;

    public float aminationSpeed = 2;
    
    private List<CubicComand> commands = new(512);
    private int currentIndex = 0;
    private bool isUndo = false;

    private Coroutine coroutine;

    public void RotateCubic(string comand)
    {
        if (Enum.TryParse<CubicComand>(comand, out CubicComand newComand))
            commands.Add(newComand);
        else
            commands.Add(CubicComand.YC);
        
        if (commands.Count >= 512)
        {
            commands.RemoveRange(0, 256);
            currentIndex -= 256;
        }
    }

    private void Update()
    {
        if(commands.Count == currentIndex)
            return;
        
        if(coroutine is not null)
            return;


        coroutine = StartCoroutine(AnimateRotation(commands[currentIndex]));
    }
    
    private IEnumerator AnimateRotation(CubicComand command)
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
        currentIndex++;
        coroutine = null;
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