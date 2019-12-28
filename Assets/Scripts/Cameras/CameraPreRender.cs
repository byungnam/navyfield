using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraPreRender : MonoBehaviour
{
    public delegate void PreRenderEvent();
    public static PreRenderEvent onPreRender;

    void OnPreRender()
    {
        if (onPreRender != null)
        {
            onPreRender();
        }
    }
}