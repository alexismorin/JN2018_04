using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteTest : MonoBehaviour {

    Wiimote remote;
    public Vector3 motion;

    void Start () {
        InitWiimotes ();
    }

    void OnApplicationQuit () {
        FinishedWithWiimotes ();
    }

    void InitWiimotes () {
        WiimoteManager.FindWiimotes (); // Poll native bluetooth drivers to find Wiimotes
        foreach (Wiimote wiiRemote in WiimoteManager.Wiimotes) {
            remote = wiiRemote;
            print ("detected wiimote");
        }
        remote.SendPlayerLED (true, false, false, false); // LEDs will look like this: [* - - *]
        remote.SendDataReportMode (InputDataType.REPORT_BUTTONS_ACCEL);
        remote.RumbleOn = true; // Enabled Rumble
        remote.SendStatusInfoRequest (); // Requests Status Report, encodes Rumble into input report
        Invoke ("StopRumble", 1f);

    }

    void StopRumble () {
        remote.RumbleOn = false; // Disabled Rumble
        remote.SendStatusInfoRequest (); // Requests Status Report, encodes Rumble into input report
    }
    void FinishedWithWiimotes () {
        foreach (Wiimote remote in WiimoteManager.Wiimotes) {
            WiimoteManager.Cleanup (remote);
        }
    }

    void Update () { // called once per frame (for example)

        int ret;
        do {

            ret = remote.ReadWiimoteData ();
            float[] accelerationDataArray = remote.Accel.GetCalibratedAccelData ();
            motion = new Vector3 (accelerationDataArray[1], accelerationDataArray[0], accelerationDataArray[2]);
        } while (ret > 0); // ReadWiimoteData() returns 0 when nothing is left to read.  So by doing this we continue to

    }

}