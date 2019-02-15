using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteTest : MonoBehaviour {

    Wiimote remote;
    public Vector3 motion;
    public Vector3 offset;
    float lastFramex;
    float lastFramey;
    float lastFramez;

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
        //   remote.RumbleOn = true; // Enabled Rumble
        //  remote.SendStatusInfoRequest (); // Requests Status Report, encodes Rumble into input report
        remote.Accel.CalibrateAccel (WiimoteApi.AccelCalibrationStep.A_BUTTON_UP);

        //   float[] accelerationOffsetArray = remote.Accel.GetCalibratedAccelData ();
        //   offset = new Vector3 (accelerationOffsetArray[1], accelerationOffsetArray[0], accelerationOffsetArray[2]);

        Invoke ("StopRumble", 1f);

    }

    void StopRumble () {
        //    remote.RumbleOn = false; // Disabled Rumble
        //    remote.SendStatusInfoRequest ();
        // Requests Status Report, encodes Rumble into input report
    }
    void FinishedWithWiimotes () {
        foreach (Wiimote remote in WiimoteManager.Wiimotes) {
            WiimoteManager.Cleanup (remote);
        }
    }

    void Update () { // called once per frame (for example)

        if (Input.GetKeyDown ("a")) {
            remote.Accel.CalibrateAccel (WiimoteApi.AccelCalibrationStep.A_BUTTON_UP);
        }
        if (Input.GetKeyDown ("b")) {
            remote.Accel.CalibrateAccel (WiimoteApi.AccelCalibrationStep.EXPANSION_UP);
        }
        if (Input.GetKeyDown ("c")) {
            remote.Accel.CalibrateAccel (WiimoteApi.AccelCalibrationStep.LEFT_SIDE_UP);
        }

        int ret;
        do {

            ret = remote.ReadWiimoteData ();
            float[] accelerationDataArray = remote.Accel.GetCalibratedAccelData ();

            /*          float y = (float) Math.Round ((double) accelerationDataArray[0], 2);
                     float x = (float) Math.Round ((double) accelerationDataArray[1], 2);
                     float z = (float) Math.Round ((double) accelerationDataArray[2], 2);

                     if (y == lastFramey) {
                         y = 0f;
                     }
                     if (x == lastFramex) {
                         x = 0f;
                     }
                     if (z == lastFramez) {
                         z = 0f;
                     }
                     //    Vector3 temporaryMotion = new Vector3 (accelerationDataArray[1], accelerationDataArray[2], accelerationDataArray[0]);
                     motion = new Vector3 (x, y, z);

                     lastFramex = x;
                     lastFramey = y;
                     lastFramez = z;*/

            Vector3 motionCurrent = new Vector3 ((float) Math.Round ((double) accelerationDataArray[0], 2) * -1f, (float) Math.Round ((double) accelerationDataArray[2], 2), (float) Math.Round ((double) accelerationDataArray[1], 2) * -1f);
            motion = motionCurrent + offset;

        } while (ret > 0); // ReadWiimoteData() returns 0 when nothing is left to read.  So by doing this we continue to

    }

}