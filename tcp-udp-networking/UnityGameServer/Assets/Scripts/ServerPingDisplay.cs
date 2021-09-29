using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

public static class ServerPingDisplay 
{
    public static double clientPing;
    public static int clientId;

    static readonly Stopwatch stopwatch = new Stopwatch();

    static ServerPingDisplay()
    {
        stopwatch.Start();
    }

    static double LocalTime()
    {
        return stopwatch.Elapsed.TotalSeconds;
    }
    /*
    public static void SendResponse()
    {
        ServerSend.ServerPong(clientId, clientPing, LocalTime());
    }
    */

}
