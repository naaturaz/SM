﻿using UnityEngine;

public class HUDFPS : MonoBehaviour
{
    // Attach this to a GUIText to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5 frames.

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private GUIText guiText;
    private int guiCount;
    private Vector3 guiTextPos = new Vector3(0.7f, .9f, 0);

    private GameObject guiClone;
    private static string _message;
    private static string _messageLong;

    public static string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    public GUIText GuiText
    {
        get { return guiText; }
        set { guiText = value; }
    }

    public static string MessageLong
    {
        get { return _messageLong; }
        set { _messageLong = value; }
    }

    private void Start()
    {
        guiCount = 0;

        if (guiCount == 0)
        {
            guiText = Instantiate(Resources.Load("Prefab/GUI/Helper/GuiText_FPSHelp", typeof(GUIText)), guiTextPos, Quaternion.identity) as GUIText;

            guiCount++;
        }

        if (!guiText)
        {
            Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    private void Update()
    {
        FPSCounter();
    }

    private static float savedFps = 0;

    private void FPSCounter()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            savedFps = fps;

            string format = System.String.Format("{0:F2} FPS", fps);
            guiText.text = format + "." + _message;

            if (fps < 30)
                guiText.material.color = Color.yellow;
            else
                if (fps < 10)
                guiText.material.color = Color.red;
            else
                guiText.material.color = Color.green;
            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    internal static float FPS()
    {
        return savedFps;
    }
}