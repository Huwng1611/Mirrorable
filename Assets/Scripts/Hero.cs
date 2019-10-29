using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    //[HideInInspector]
    public Property property;
    //[HideInInspector]
    public Move move;

    public Animator anim;

    public GameObject[] skillEffect;

    public Material mat;
    public int index;
    void Awake()
    {
        property = gameObject.GetComponent<Property>();
        move = gameObject.GetComponent<Move>();
    }
    // [HideInInspector]
    public Collider2D[] tempCollider;
    //[HideInInspector]
    public int layerEnemy;
    //[HideInInspector]
    public BoxCollider2D boxCheckGround;
    private void Start()
    {
        layerEnemy = LayerMask.GetMask("Dire");
        boxCheckGround = transform.GetChild(1).GetComponent<BoxCollider2D>();
        PlayAnim("run");
        if (index >= 4)
        {
            LongRange = GamePlay.gameplay.width;
        }
        ColorUp();
    }
    public void ColorUp()
    {
        mat.color = new Vector4(1, 1, 1, 1);
    }
    public void ColorDown()
    {
        mat.color = GamePlay.gameplay.colorBod;
    }
    public void PlayAnim(string ani)
    {
        anim.Play(ani);
    }
    public void OFFBox()
    {
        boxCheckGround.enabled = false;
        property.coll.enabled = false;
    }
    public void OnBox()
    {
        boxCheckGround.enabled = true;
        property.coll.enabled = true;
    }
    public float LongRange;
    public float ShortRange;

    //[HideInInspector]
    public bool checkFindEnemy;
    private void Update()
    {
        if (!property.checkDie)
        {
            if (property.enemy == null)
            {
                if (!checkFindEnemy)
                {
                    //range xa
                    if (!GamePlay.gameplay.checkShortRange)
                    {
                        if ((tempCollider = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + move.dirScale * 0.65f), new Vector2(LongRange, 0.5f), 0, layerEnemy)).Length > 0)
                        {
                            property.enemy = tempCollider[Random.Range(0, tempCollider.Length)].GetComponent<Property>();
                            property.enemy.listFighter.Add(property);
                            property.Attack();
                            if (property.enemy != null)
                                GamePlay.gameplay.CallTeamRadiant(property.enemy.transform);

                            EnemyManager.enemymanager.CallTeamEnemy(transform);
                            move.checkCalled = false;
                            PlayAnim("atk");
                            checkRun = false;
                        }
                        else
                        {
                            if (!checkRun)
                            {
                                move.ContinueMove();
                                checkRun = true;
                                PlayAnim("run");
                            }
                        }
                    }
                    else
                    {
                        //range gan
                        if ((tempCollider = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + move.dirScale * 0.65f), new Vector2(ShortRange, 0.5f), 0, layerEnemy)).Length > 0)
                        {
                            property.enemy = tempCollider[Random.Range(0, tempCollider.Length)].GetComponent<Property>();
                            property.enemy.listFighter.Add(property);

                            move.checkCalled = false;
                            property.Attack();
                            if (property.enemy != null)
                                GamePlay.gameplay.CallTeamRadiant(property.enemy.transform);
                            EnemyManager.enemymanager.CallTeamEnemy(transform);

                            PlayAnim("atk");
                            checkRun = false;
                        }
                        else
                        {
                            if (!checkRun)
                            {
                                move.ContinueMove();
                                checkRun = true;
                                PlayAnim("run");
                            }
                        }
                    }
                }
            }
            else
            {
                // skill FireShield
                FireShield();

                move.StopMove();
            }

            if (index == 2)
            {
                if ((tempCollider = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + move.dirScale * 0.65f), new Vector2(LongRange, 0.5f), 0, layerEnemy)).Length <= 0)
                {
                    move.checkMove = true;
                    property.enemy = null;
                }
            }
        }
    }

    private bool checkRun;

    //FireShield
    //[HideInInspector]
    public bool checkFireShield;
    public void OnFireShield(float amount)
    {
        CancelInvoke("OFFFireShield");
        Invoke("OnShield", 0.5f);
        property.sprSkill[3].enabled = true;
        skillEffect[3].SetActive(true);
    }
    public void OnShield()
    {
        checkFireShield = true;
        Invoke("OFFFireShield", 10);
    }
    public void FireShield()
    {
        if (checkFireShield)
        {
            if (Mathf.Abs(transform.position.x - property.enemy.transform.position.x) < 2)
            {
                if (!property.enemy.checkDie && GamePlay.gameplay.killEnemyFireShield > 0)
                {
                    property.enemy.level = 0;
                    property.enemy.currentHp = 0;
                    property.enemy.SetHealthBar();
                    property.enemy.SetTextLevel();
                    property.enemy.Die();
                    GamePlay.gameplay.killEnemyFireShield--;
                    if (GamePlay.gameplay.killEnemyFireShield <= 0)
                    {
                        CancelInvoke("OFFFireShield");
                        OFFFireShield();
                    }
                }
            }
        }
    }

    public void OFFFireShield()
    {
        Invoke("OFFFireShield", 10);
        checkFireShield = false;
        for (int i = 0; i < GamePlay.gameplay.hero.Length; i++)
        {
            GamePlay.gameplay.hero[i].property.sprSkill[3].enabled = false;
            GamePlay.gameplay.hero[i].skillEffect[3].SetActive(false);
        }

    }
}
