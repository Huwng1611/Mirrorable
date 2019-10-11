using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaLan : MonoBehaviour {

    public Animator anim;
    public float speed;
    public int dir;
    public CircleCollider2D coll1;
    public CircleCollider2D coll2;
	void Start () {
		
	}
    public bool checkOff;
    private void Update()
    {
        if(!checkOff)
            transform.position = new Vector3(transform.position.x + dir * speed * Time.deltaTime, transform.position.y, 0);
    }
    public void Off()
    {
        checkOff = true;
        coll1.enabled = false;
        coll2.enabled = false;
    }
}
