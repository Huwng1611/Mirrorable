using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [HideInInspector]
    public Property property;

    [HideInInspector]
    public Rigidbody2D rb;

    [SerializeField]
    private float outCamera;

    public Material mat;
    void Awake()
    {
        property = gameObject.GetComponent<Property>();
        rb = gameObject.GetComponent<Rigidbody2D>();      
    }

    [HideInInspector]
    public Collider2D[] tempCollider;

    [HideInInspector]
    public int layerEnemy;

    public int index;
    private void Start()
    {
        layerEnemy = LayerMask.GetMask("Radiant");
        if( index >= 3)
        {
            range = GamePlay.gameplay.width;
        }
        outCamera = GamePlay.gameplay.width * 1.75f;
    }
    public void OnEnable()
    {       
        checkOutCamera = false;
        checkMove = false;
        checkFindEnemy = false;
        PlayAnim("idle");
    }
    public void SetTranform()
    {
        dirScale = EnemyManager.enemymanager.dirScaleToGen;
        if(dirScale > 0)
        {
            mat.color = new Vector4(1, 1, 1, 1);
        }
        else
        {
            mat.color = GamePlay.gameplay.colorBod;
        }
        rb.gravityScale = dirScale * GamePlay.gameplay.gravity;
        property.textLevel.transform.localScale = new Vector3(-1, dirScale, 1);
        transform.localScale = new Vector3(-1, dirScale, 1);
        transform.position = new Vector3(CameraFollow.camerafollow.transform.position.x + 20 + index * 0.75f, dirScale * 2, 0);
    }
    public void OnDisable()
    {
      
    }

    public Animator anim;
    public void PlayAnim(string ani)
    {
        anim.Play(ani);      
    }
    public void SetProperty()
    {        
        property.SetDefault();
    }
    [HideInInspector]
    public int dirScale;
    public float speed;
    public float range;

    //[HideInInspector]
    public bool checkFindEnemy;
    void Update()
    {
        if (!property.checkDie)
        {
            if (property.enemy == null)
            {
                if (!checkFindEnemy)
                {
                    if ((tempCollider = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + dirScale * 0.65f), new Vector2(range, 1), 0, layerEnemy)).Length > 0)
                    {                   
                        property.enemy = tempCollider[Random.Range(0, tempCollider.Length)].GetComponent<Property>();
                        if (property.enemy != null)
                        {
                            EnemyManager.enemymanager.CallTeamEnemy(property.enemy.transform);
                            GamePlay.gameplay.CallTeamRadiant(transform);
                            checkMove = false;                           
                            PlayAnim("atk");                          
                            property.Attack();                            
                            property.enemy.listFighter.Add(property);
                        }
                    }
                    else
                    {
                        if (checkMove)
                        {
                            //if (checkAnimRun)
                            //{
                                PlayAnim("run");
                               // checkAnimRun = false;
                            //}
                            if (dir == -1)
                            {
                               
                                if (transform.position.x > target.position.x)
                                {
                                    transform.position = new Vector3(transform.position.x + dir * Time.deltaTime * speed, transform.position.y, transform.position.z);                                   
                                }
                                else
                                {
                                    if (!EnemyManager.enemymanager.checkDontMove)
                                    {
                                        if (property.enemy == null)
                                        {
                                            //target.position = new Vector3(target.transform.position.x + dir, target.transform.position.y, target.transform.position.z);
                                            if (GamePlay.gameplay.heroAmount >= 1)
                                            {
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    if (GamePlay.gameplay.hero[i].gameObject.activeInHierarchy)
                                                    {
                                                        target = GamePlay.gameplay.hero[i].transform;
                                                        Move();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                checkFindEnemy = false;
                                                checkMove = false;
                                            }
                                        }

                                        checkAnimRun = true;
                                        //checkMove = false;
                                    }

                                }
                            }
                            else
                            {
                                if (transform.position.x < target.position.x)
                                {
                                    transform.position = new Vector3(transform.position.x + dir * Time.deltaTime * speed, transform.position.y, transform.position.z);
                                }
                                else
                                {
                                    if (!EnemyManager.enemymanager.checkDontMove)
                                    {
                                        if (property.enemy == null)
                                        {
                                            //target.position = new Vector3(target.transform.position.x + dir, target.transform.position.y, target.transform.position.z);
                                            if (GamePlay.gameplay.heroAmount >= 1)
                                            {
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    if (GamePlay.gameplay.hero[i].gameObject.activeInHierarchy)
                                                    {
                                                        target = GamePlay.gameplay.hero[i].transform;
                                                        Move();
                                                    }
                                                }
                                            }
                                            else
                                            {                                              
                                                checkFindEnemy = false;
                                                checkMove = false;
                                            }
                                        }
                                        
                                        checkAnimRun = true;
                                        //checkMove = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                checkMove = true;
            }

            if (transform.position.x < CameraFollow.camerafollow.transform.position.x - outCamera)
            {
                if (!checkOutCamera)
                {
                    checkOutCamera = true;
                    EnemyManager.enemymanager.quatityManagerEnemy--;
                    if (EnemyManager.enemymanager.quatityManagerEnemy <= 0)
                    {
                        EnemyManager.enemymanager.EndNoFight();
                    }
                }
            }
        }
    }
    private bool checkAnimRun;
    [HideInInspector]
    public bool checkOutCamera;
    //[HideInInspector]
    public Transform target;
    
    //[HideInInspector]
    public bool checkMove;

    public int dir;
    public void Move()
    {     
        checkMove = true;
        if (transform.position.x > target.position.x)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        property.textLevel.transform.localScale = new Vector3(dir, dirScale, 1);
        transform.localScale = new Vector3(dir, dirScale, 1);
    }

}
