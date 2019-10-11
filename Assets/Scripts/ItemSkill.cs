using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkill : MonoBehaviour {

    public int index;
    public Animator anim;
    public BoxCollider2D coll;
    public MeshRenderer meshvovannenlaphaitatnodi;
    // Use this for initialization

    private float outCamera;
	void OnEnable () {
        OnItemSkill();

        outCamera = 0.75f * GamePlay.gameplay.width;
    }
	
	public void OnItemSkill()
    {
        index = Random.Range(0, 9);
        meshvovannenlaphaitatnodi.enabled = false;
        anim.Play(index.ToString());
        check = false;
        coll.enabled = true;
        SetTransform();
    }
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public void Update()
    {       
        if(Mathf.Abs(transform.position.x - CameraFollow.camerafollow.transform.position.x) > GamePlay.gameplay.width * 1.5f)
        {
            GamePlay.gameplay.RemoveItemSkill();
        }
    }

    int dir;
    public void SetTransform()
    {
        transform.localScale = new Vector3(1, 1, 1);
        dir = 1;
        if (Random.Range(0, 100) > 50)
        {
            dir = -1;
        }
        transform.position = new Vector3(CameraFollow.camerafollow.transform.position.x + Random.Range(GamePlay.gameplay.width, GamePlay.gameplay.width + 5), Random.Range(0, GamePlay.gameplay.height - 1f) * dir, 0);
        transform.localScale = new Vector3(1, dir, 1);
    }
    private bool check;
    [HideInInspector]
    public int j;
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (!check)
        {
            if (coll.gameObject.tag == "Radiant")
            {
                if (UIManager.ui.amountSkill < 5)
                {
                    iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(CameraFollow.camerafollow.transform.position.x - GamePlay.gameplay.width, GamePlay.gameplay.height, 0), "time", 0.4f, "easetype", iTween.EaseType.easeInSine));
                    iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.3f,0.3f,0.3f), "time", 0.4f, "easetype", iTween.EaseType.easeInSine));
                    Invoke("AddSkill", 0.5f);
                }
                else
                {
                    anim.Play("10");
                }
                coll.enabled = false;
                check = true;
            }
        }
    }
    private void AddSkill()
    {
        UIManager.ui.AddSkill(index);
    }
    public void OFF()
    {
        check = false;
        gameObject.SetActive(false);
    }
    public void OnDisable()
    {
        Invoke("On", 1f);
    }
    public void On()
    {
        GamePlay.gameplay.GenItemSkill();
    }

}
