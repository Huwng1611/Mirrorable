using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {

    bool check;
    public Animator anim;
    void Update () {
		if(GamePlay.gameplay.captain.transform.position.x >= transform.position.x - 3)
        {
            if (!check)
            {
                check = true;
                anim.Play("Idle");
                Invoke("Fall", 0.4f);
            }
        }
        if (checkFall)
        {
            if (dir < 0)
            {
                if (transform.position.y >= 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * speed * dir, 0);
                }
                else
                {
                    checkFall = false;
                }
            }
            else
            {
                if (transform.position.y <= 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * speed * dir, 0);
                }
                else
                {
                    checkFall = false;
                }
            }
        }
	}
    public int dir;
    public float speed;
    bool checkFall;
    public void Fall()
    {
        checkFall = true;
    }
}
