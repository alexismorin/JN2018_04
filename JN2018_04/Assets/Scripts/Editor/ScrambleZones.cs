using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrambleZones : MonoBehaviour {

    [MenuItem ("Tools/Stock Save")]
    private static void StockSave () {
        PlayerPrefs.SetString ("guardsCaseCode", "5-7-2-23-43-20-35-12-53-28");
    }
    /*
        [MenuItem ("Tools/Scramble Zones")]
        private static void NewMenuOption () {
            GameObject[] zones = GameObject.FindGameObjectsWithTag ("zone");
            int[] originalZoneOrder = new int[zones.Length];

            for (int i = 0; i < originalZoneOrder.Length; i++) {
                originalZoneOrder[i] = i;
                print (originalZoneOrder[i]);

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
                print (newZoneOrder[z]);
            }

        } */

}