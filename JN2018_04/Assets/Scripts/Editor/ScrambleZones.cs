using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrambleZones : MonoBehaviour {

    [MenuItem ("Tools/Scramble Zones")]
    private static void NewMenuOption () {
        GameObject[] zones = GameObject.FindGameObjectsWithTag ("zone");
        int[] originalZoneOrder = new int[zones.Length];

        for (int i = 0; i < originalZoneOrder.Length; i++) {
            originalZoneOrder[i] = zones[i].GetComponent<Zone> ().zoneID;

        }

        int[] newZoneOrder = originalZoneOrder;

        for (int t = 0; t < newZoneOrder.Length; t++) {
            int tmp = newZoneOrder[t];
            int r = Random.Range (t, newZoneOrder.Length);
            newZoneOrder[t] = newZoneOrder[r];
            newZoneOrder[r] = tmp;
        }

        for (int z = 0; z < zones.Length; z++) {
            zones[z].GetComponent<Zone> ().zoneID = newZoneOrder[z];
            zones[z].name = "Zone " + newZoneOrder[z].ToString ();

        }

    }

}