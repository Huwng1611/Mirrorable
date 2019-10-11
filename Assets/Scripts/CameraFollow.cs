using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow camerafollow;
    public Transform trnFollow;
    private void Awake()
    {
        camerafollow = this;
        tmp = 3.9f;
        float size = (Screen.width * 1125f) / (Screen.height * 2436);
        if(size <= 1)
        {
            size = 1 / size;
        }
        Camera.main.orthographicSize = 4.9f * size;

        GamePlay.gameplay.width = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        GamePlay.gameplay.height = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
    }
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    private float tmp;

    public Transform[] trn;
    void Update () {

        //transform.position = new Vector3(trnFollow.position.x + tmp , transform.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(trnFollow.position.x + tmp, transform.position.y, transform.position.z), ref velocity, smoothTime);
    }
}
