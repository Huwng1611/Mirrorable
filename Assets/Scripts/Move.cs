using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public Transform targetFollow;

    public bool checkCaptain;
    public bool checkEnd;
    //kiem tra xem dang o tren hay o duoi duong chay
    //[HideInInspector]
    public bool checkDown;

    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
    public Transform checkGround;
    //[HideInInspector]
    public bool notAtEg;
    //[HideInInspector]
    public bool checkMove = true;
    public Hero herotmp;
    public Property property;

    public void Awake()
    {
        dirScale = 1;
        herotmp = GetComponent<Hero>();
        property = GetComponent<Property>();
    }

    /// <summary>
    /// cần sửa cái này & những chỗ reference đến nó
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetFollow(Transform target)
    {
        targetFollow = target;
        //if (!target.gameObject.GetComponent<Property>().checkDie)
        //{
        //    targetFollow = target;
        //}
        //else
        //{
        //    SetTargetFollow(target.GetComponent<Move>().targetFollow);
        //}
    }

    public void SetTransform()
    {
        int dirTmp = (int)GamePlay.gameplay.captain.transform.localScale.y;
        if (GamePlay.gameplay.captain.move.checkDown)
        {
            checkDown = true;
        }
        else
        {
            checkDown = false;
        }
        transform.localScale = new Vector3(1, dirTmp, transform.localScale.z);
        checkChangeGravity = true;
        //transform.position = new Vector3(CameraFollow.camerafollow.transform.position.x - GamePlay.gameplay.width / 3, 0.38f * dirTmp, transform.position.z);
        //transform.position = GamePlay.gameplay.captain.transform.position;
        if (gameObject.name.Contains("Drman"))
        {
            transform.position = new Vector3(GamePlay.gameplay.captain.transform.position.x, GamePlay.gameplay.captain.transform.position.y + 1f, GamePlay.gameplay.captain.transform.position.z);
        }
        else
        {
            transform.position = GamePlay.gameplay.captain.transform.position;
        }
    }
    public void SetScale()
    {
        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
    [HideInInspector]
    public bool checkStart;
    public void OnEnable()
    {
        if (!checkStart)
        {
            checkStart = true;
        }
        else
        {
            indexAction = GamePlay.gameplay.indexAction;
        }
    }
    void Update()
    {
        if (!property.checkDie)
        {
            if (checkMove)
            {
                if (checkCaptain)
                {
                    if (!EnemyManager.enemymanager.checkFight)
                    {
                        transform.position = new Vector3(transform.position.x + Time.deltaTime * GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                        notAtEg = Physics2D.OverlapCircle(checkGround.position, 1f, GamePlay.gameplay.layerGround);
                    }
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetFollow.transform.position.x, transform.position.y, 0), ref velocity, smoothTime);
                }
                if (!EnemyManager.enemymanager.checkFight)
                {
                    if (GamePlay.gameplay.action[indexAction] != 0)
                    {
                        if (transform.position.x > GamePlay.gameplay.xToAction[indexAction])
                        {
                            if (GamePlay.gameplay.action[indexAction] == 1)
                            {
                                Jump();
                            }
                            else if (GamePlay.gameplay.action[indexAction] == -1)
                            {
                                UpOrDown();
                            }

                        }
                    }
                }
            }
            if (checkCalled)
            {
                notAtEg = Physics2D.OverlapCircle(checkGround.position, 1f, GamePlay.gameplay.layerGround);
                if (notAtEg)
                {
                    if (property.enemy == null)
                    {
                        if (dir == -1)
                        {
                            if (transform.position.x > target.position.x)
                                transform.position = new Vector3(transform.position.x + dir * Time.deltaTime * GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                        }
                        else
                        {
                            if (transform.position.x < target.position.x)
                                transform.position = new Vector3(transform.position.x + dir * Time.deltaTime * GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                        }
                    }
                }
                else
                {
                    if (checkCaptain)
                    {
                        transform.position = new Vector3(transform.position.x + Time.deltaTime * GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, targetFollow.position, ref velocity, smoothTime);
                    }
                }
            }
            if (checkChangeGravity)
            {
                //if (!EnemyManager.enemymanager.checkFight)
                //{
                herotmp.OFFBox();
                //}
                notAtEg = Physics2D.OverlapCircle(checkGround.position, 1f, GamePlay.gameplay.layerGround);
                if (transform.position.y * transform.localScale.y < 0)
                {
                    herotmp.OnBox();
                    transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
                    checkChangeGravity = false;
                }
                if (checkDown)
                {
                    if (transform.position.y < 0)
                    {
                        rb.gravityScale = -1 * GamePlay.gameplay.gravity;
                        checkChangeGravity = false;
                        herotmp.OnBox();
                        herotmp.ColorDown();
                    }
                }
                else
                {
                    if (transform.position.y > 0)
                    {
                        rb.gravityScale = 1 * GamePlay.gameplay.gravity;
                        checkChangeGravity = false;
                        herotmp.OnBox();
                        herotmp.ColorUp();
                    }
                }
            }
        }
        else
        {
            notAtEg = Physics2D.OverlapCircle(checkGround.position, 0.5f, GamePlay.gameplay.layerGround);
            if (!checkDown)
            {
                if (transform.position.y <= 0)
                {
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                if (transform.position.y >= 0)
                {
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }


    //[HideInInspector]
    public bool checkChangeGravity;
    // [HideInInspector]
    public bool checkCalled;
    // [HideInInspector]
    public Transform target;
    public int dir;
    //[HideInInspector]
    public bool checkCall;

    public void MoveIfCalled()
    {
        if (!checkCall)
        {
            if (gameObject.activeInHierarchy)
            {
                checkCalled = true;
                checkMove = false;
                if (transform.position.x > target.position.x)
                {
                    dir = -1;
                }
                else
                {
                    dir = 1;
                }
                if (target.localScale.y > 0)
                {
                    if (checkDown)
                    {
                        herotmp.PlayAnim("flip");
                        herotmp.ColorUp();
                    }
                    checkDown = false;
                }
                else
                {
                    if (!checkDown)
                    {
                        herotmp.PlayAnim("flip");
                        herotmp.ColorDown();
                    }
                    checkDown = true;
                }
                transform.localScale = new Vector3(dir, target.localScale.y, 1);
                herotmp.property.textLevel.transform.localScale = new Vector3(1, target.localScale.y, 1);
                //herotmp.property.collCheckGround.enabled = true;
                // herotmp.property.coll.enabled = true;
                dirScale = (int)target.localScale.y;
                checkChangeGravity = true;
            }
        }
    }

    public void ContinueMove()
    {
        herotmp.property.textLevel.transform.localScale = new Vector3(1, herotmp.property.textLevel.transform.localScale.y, 1);
        checkMove = true;
    }
    public void StopMove()
    {
        checkMove = false;
    }
    //[HideInInspector]
    public int indexAction;

    [HideInInspector]
    public bool checkAction;

    public Rigidbody2D rb;


    public void Jump()
    {
        int dir;
        if (!checkDown)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        rb.velocity = new Vector2(0, GamePlay.gameplay.jumpHigh * dir);
        herotmp.PlayAnim("jump");
        NextAction();
    }
    [HideInInspector]
    public int dirScale = 1;
    public void UpOrDown()
    {
        if (checkDown)
        {
            dirScale = 1;
            checkDown = false;
            //herotmp.ColorUp();
        }
        else
        {
            dirScale = -1;
            checkDown = true;
            //herotmp.ColorDown();
        }
        herotmp.PlayAnim("flip");
        herotmp.property.SetScaleTextLevel(dirScale);
        transform.localScale = new Vector3(1, dirScale, 1);
        checkChangeGravity = true;
        //rb.gravityScale *= -1;
        NextAction();
    }
    public void NextAction()
    {
        if (checkEnd)
        {
            GamePlay.gameplay.action[indexAction] = 0;
            GamePlay.gameplay.indexAction++;
            //if (GamePlay.gameplay.indexAction >= 5)
            if (GamePlay.gameplay.indexAction >= GamePlay.gameplay.hero.Length)
            {
                GamePlay.gameplay.indexAction = 0;
            }

        }
        indexAction++;
        //if (indexAction >= 5)
        if (indexAction >= GamePlay.gameplay.hero.Length)
        {
            indexAction = 0;
        }
        //if (checkEnd)
        //{
        //    if (GamePlay.gameplay.checkResurrection)
        //    {
        //        GamePlay.gameplay.SetActionDefaul();
        //        GamePlay.gameplay.checkResurrection = false;
        //    }
        //}

    }


}
