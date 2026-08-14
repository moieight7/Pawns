using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResolutionManager
{
    private static Resolution[] resolutions;
    private static List<Resolution> filteredResolutions;

    private static RefreshRate currentRefreshRate;
    private static int currentResIndex = 0;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
                filteredResolutions.Add(resolutions[i]);
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + "y" + filteredResolutions[i].refreshRateRatio + "Hz";
            options.Add(resOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                currentResIndex = i;
        }

        DebugLogConsole.AddCommand("res_print", "Prints every full-screen resolution that the monitor supports.", PrintAllResolutions);
        DebugLogConsole.AddCommand<int>("res_set", "Sets the screen resolution to the i-th resolution supported by the monitor.", SetResolution);
    }

    public static void SetResolution(int resolutionIndex)
    {
        Resolution res = filteredResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, true);
    }

    public static void PrintAllResolutions()
    {
        int i = 0;
        foreach (Resolution resolution in filteredResolutions)
        {
            Debug.Log(resolution.width + "x " + resolution.height + "y " + resolution.refreshRateRatio + "Hz " + i++);
        }
    }
}
