using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiimoteObjectMover : MonoBehaviour {

    public WiimoteTest wiimoteMotionReceiver;
    Vector3 gameMotion;
    public float moveSpeed = 1f;
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate () {
        gameMotion = wiimoteMotionReceiver.motion;
        transform.Translate (gameMotion * Time.deltaTime * moveSpeed, Space.World);
        transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity, smoothTime);
    }
}