using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    int n_gullimam = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot($"C:\\Users\\stanv\\OneDrive\\Bureaublad\\Gullie\\mam{n_gullimam}.png", 1);
            n_gullimam++;
        }       
    }
}
