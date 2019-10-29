using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPunch : MonoBehaviour
{

    public float speed;
    public BoxCollider2D coll;
    public int dirfly;
    private void OnEnable()
    {
        check = false;
        coll.enabled = false;
        Invoke("Fly", 0.375f);
    }
    void Fly()
    {
        check = true;
        coll.enabled = true;
    }
    private bool check;
    void Update()
    {
        if (check)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * speed * dirfly, transform.position.y, transform.position.z);
            if (dirfly == 1)
            {
                if (transform.position.x > CameraFollow.camerafollow.transform.position.x + GamePlay.gameplay.width)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (transform.position.x < CameraFollow.camerafollow.transform.position.x - GamePlay.gameplay.width)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    public bool checkTrigger;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Dire" && !checkTrigger)
        {
            GamePlay.gameplay.BeginAirPunch();
            checkTrigger = true;
        }
        else if (coll.gameObject.tag == "Die")
        {
            coll.GetComponent<Animator>().Play("OffAnim");
        }

    }
}
