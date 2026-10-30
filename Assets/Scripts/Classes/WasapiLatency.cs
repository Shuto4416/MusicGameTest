using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WasapiLatency
{
    [DllImport("LatencyEstimator",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern int WL_Start();

    [DllImport("LatencyEstimator",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern void WL_Stop();

    [DllImport("LatencyEstimator",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern double WL_GetOutputLatencyMs();

    [DllImport("LatencyEstimator",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern int WL_IsReady();

    public static int Start()
    {
        return WL_Start();
    }

    public static void Stop()
    {
        WL_Stop();
    }

    public static bool IsReady()
    {
        return WL_IsReady() != 0;
    }

    public static double GetLatencyMs()
    {
        return WL_GetOutputLatencyMs();
    }

    public static double GetLatencySeconds()
    {
        return GetLatencyMs() / 1000.0;
    }
}