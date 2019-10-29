using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay gameplay;
    public static bool checkStart;

    public float speed;
    public float jumpHigh;
    public float gravity;
    /// <summary>
    /// check game over
    /// </summary>
    public bool isGameOver;
    [HideInInspector]
    public bool checkShortRange;
    public TextDamage textDamage;
    //[HideInInspector]
    public int indexAction;
    public Color colorDamageRadiantTaken;
    public Color colorDamageDireTaken;
    public Color colorBod;

    public Hero[] hero;

    /// <summary>
    /// xét con lợn hiện tại là con bay hay con dưới đất
    /// </summary>
    public PigController currentPig;
    /// <summary>
    /// có 2 pigs: pig0 = con dưới đất; pig1 = con bay
    /// </summary>
    public PigController[] pigs = new PigController[2];
    //[HideInInspector]
    public int layerGround;

    //[HideInInspector]
    public int numberOfHero;

    //[HideInInspector]
    public int score;
    //[HideInInspector]
    public int exp;

    public int heroAmount;
    //[HideInInspector]
    public int numberOfEnemyBeKilled;
    void Awake()
    {
        gameplay = this;

        layerGround = LayerMask.GetMask("Ground");

        BigTroop();
        SetPig();

        checkResurr = new int[hero.Length];
        maxLevelHero = new int[hero.Length];
        action = new int[hero.Length];
        xToAction = new float[hero.Length];
        for (int i = 0; i < maxLevelHero.Length; i++)
        {
            maxLevelHero[i] = 1;
        }
        checkNecroman = new int[hero.Length];
        currentExp = new float[hero.Length];
    }
    //[HideInInspector]
    public Vector3 trnTap;

    public Transform environment;
    public Hero captain;
    public Hero lastHero;
    //[HideInInspector]
    public float width;
    //[HideInInspector]
    public float height;

    private void Start()
    {
        Time.timeScale = 0;
        //Invoke("BigTroop", 0.06f);
        OnGoldDevil();
        ArchNecromancer();
        checkStart = true;


        PlayerPrefs.SetInt("gameAmount", PlayerPrefs.GetInt("gameAmount") + 1);
        if (PlayerPrefs.GetInt("gameAmount") >= xGametoUpGradeThunder[PlayerPrefs.GetInt("levelThunder")])
        {
            PlayerPrefs.SetInt("levelThunder", PlayerPrefs.GetInt("levelThunder") + 1);
            if (PlayerPrefs.GetInt("levelThunder") >= 3)
            {
                PlayerPrefs.SetInt("levelThunder", 3);
            }
        }
        Invoke("CheckBigTroop", 0.05f);

    }

    /// <summary>
    /// lấy ra số heros khi bắt đầu chơi
    /// </summary>
    //private void GetHeroAmountWhenStart()
    //{
    //    for (int i = 0; i < hero.Length; i++)
    //    {
    //        if (hero[i].gameObject.activeInHierarchy)
    //        {
    //            heroAmount++;
    //        }
    //    }
    //}

    /// <summary>
    /// xét con lợn chạy theo heroes
    /// </summary>
    private void SetPig()
    {
        if (PlayerPrefs.GetInt("levelThunder") > 2 || PlayerPrefs.GetInt("levelResurrection") > 2 || PlayerPrefs.GetInt("levelFireShield") > 2
            || PlayerPrefs.GetInt("levelAirPunch") > 2)
        {
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        else
        {
            pigs[0].gameObject.SetActive(true);
            pigs[1].gameObject.SetActive(false);
            currentPig = pigs[0];
        }
    }
    /// <summary>
    /// check đoàn quân
    /// </summary>
    private void CheckBigTroop()
    {
        if (PlayerPrefs.GetInt("levelBigTroop") < 10)
        {
            heroAmount = 3;
            if (PlayerPrefs.GetInt("levelBigTroop") >= 4)
            {
                damageThunder *= 10;
                hpResurrection *= 10;
            }
        }
        else if (PlayerPrefs.GetInt("levelBigTroop") < 20)
        {
            heroAmount = 4;
            if (PlayerPrefs.GetInt("levelBigTroop") == 4)
            {
                damageThunder *= 10;
                hpResurrection *= 10;
            }
            hero[3].move.SetTargetFollow(GamePlay.gameplay.hero[1].transform);
        }
        else if (PlayerPrefs.GetInt("levelBigTroop") < 50)
        {
            heroAmount = 5;
            if (PlayerPrefs.GetInt("levelBigTroop") == 4)
            {
                damageThunder *= 10;
                hpResurrection *= 10;
            }
        }
        else
        {
            heroAmount = 6;
        }
        Debug.Log("<color=cyan>BigTroop Level = " + PlayerPrefs.GetInt("levelBigTroop") + "</color>");
    }
    [HideInInspector]
    public bool checkCallTeamRadiant;
    /// <summary>
    /// gọi team hero
    /// </summary>
    /// <param name="target"></param>
    public void CallTeamRadiant(Transform target)
    {
        for (int i = 0; i < hero.Length; i++)
        {
            hero[i].move.target = target;
            hero[i].move.checkCall = false;
            hero[i].move.MoveIfCalled();
        }
        //if (!checkCallTeamRadiant)
        //{
        //    checkCallTeamRadiant = true;
        //    action[indexAction] = 0;
        //    if (indexAction > 0)
        //    {
        //        indexAction--;
        //    }
        //    else
        //    {
        //        indexAction = 4;
        //    }
        //    for (int i = 0; i < hero.Length; i++)
        //    {
        //        hero[i].move.indexAction--;             
        //    }
        //}
    }
    /// <summary>
    /// hành động mặc định của hero
    /// </summary>
    public void SetActionDefaul()
    {
        ix = 0;
        //for (int i = 0; i < 5; i++)
        for (int i = 0; i < hero.Length; i++)
        {
            hero[i].move.indexAction = 0;
            xToAction[i] = 0;
            action[i] = 0;
        }
    }
    // set hero di dau tien
    public void SetCaptain(Hero cap)
    {
        if (captain != null)
        {
            captain.move.checkCaptain = false;
        }
        captain = cap;
        cap.move.checkCaptain = true;
        Invoke("SetCameraFollow", 0.05f);
    }

    /// <summary>
    /// camera đi theo captain
    /// </summary>
    public void SetCameraFollow()
    {
        CameraFollow.camerafollow.trnFollow = captain.transform;
    }

    //set hero di cuoi cung
    public void SetLastHero(Hero last)
    {
        if (lastHero != null)
        {
            lastHero.move.checkEnd = false;
        }
        lastHero = last;
        last.move.checkEnd = true;
        //for (int i = hero.Length - 1; i >= 0; i--)
        //{
        //    if (hero[i].gameObject.activeInHierarchy && !hero[i].property.checkDie)
        //    {
        //        lastHero.move.SetTargetFollow(hero[i].transform);
        //    }
        //}
    }
    private float timeToTap;
    private bool checkTapDown = false;
    private bool checkTapUp = true;
    private bool checkTimeScaleStart;
    public void TapDown()
    {
        if (!checkTimeScaleStart)
        {
            UIManager.ui.swipeToStart.SetActive(false);
            checkTimeScaleStart = true;
            Time.timeScale = 1;
        }
        if (!checkTapDown)
        {
            checkTapDown = true;
            checkTapUp = false;
            if (captain.property.enemy == null)
            {
                if (captain.move.notAtEg)
                {
                    trnTap = Input.mousePosition;
                }
            }
        }
    }

    private bool checkUpOrDown;
    private bool checkJump;
    private bool checkAction;
    public void TapUp()
    {
        if (!checkTapUp)
        {
            checkAction = false;
            //if (captain.transform.position.x < EnemyManager.enemymanager.xPositionMin || lastHero.transform.position.x > EnemyManager.enemymanager.xPositionMax)
            //{
            //    checkUpOrDown = true;
            //}
            //else
            //{
            //    checkUpOrDown = false;
            //}

            //if (captain.transform.position.x < EnemyManager.enemymanager.xPositionMin - jumpHigh || lastHero.transform.position.x > EnemyManager.enemymanager.xPositionMax - jumpHigh)
            //{
            //    checkJump = true;
            //}
            //else
            //{
            //    checkJump = false;
            //}            
            if (Mathf.Abs(Input.mousePosition.y - trnTap.y) >= 100)
            {
                checkTapUp = true;
                if (captain.property.enemy == null)
                {
                    // luu hanh dong
                    // captain dang o duoi
                    if (captain.move.notAtEg)
                    {
                        if (captain.move.checkDown)
                        {
                            if (Input.mousePosition.y > trnTap.y)
                            {
                                UpOrDown();
                                checkAction = true;
                                //if (checkUpOrDown)
                                //{
                                //    UpOrDown();
                                //    checkAction = true;
                                //}
                                //else
                                //{
                                //    UIManager.ui.CantFlip();
                                //}
                            }
                            else
                            {
                                Jump();
                                checkAction = true;
                                //if (!checkJump)
                                //{
                                //    if(EnemyManager.enemymanager.dirScaleToGen >= 0)
                                //    {
                                //        checkJump = true;
                                //    }
                                //}

                                //if (checkJump)
                                //{
                                //    Jump();
                                //    checkAction = true;
                                //}
                                //else
                                //{
                                //    UIManager.ui.CantJmp();
                                //}

                            }
                        }
                        else
                        // captain dang o tren
                        {
                            if (Input.mousePosition.y > trnTap.y)
                            {
                                Jump();
                                checkAction = true;
                                //if (!checkJump)
                                //{
                                //    if (EnemyManager.enemymanager.dirScaleToGen <= 0)
                                //    {
                                //        checkJump = true;
                                //    }
                                //}

                                //if (checkJump)
                                //{
                                //    Jump();
                                //    checkAction = true;
                                //}
                                //else
                                //{
                                //    UIManager.ui.CantJmp();
                                //}
                            }
                            else
                            {
                                UpOrDown();
                                checkAction = true;
                                //if (checkUpOrDown)
                                //{
                                //    UpOrDown();
                                //    checkAction = true;
                                //}
                                //else
                                //{
                                //    UIManager.ui.CantFlip();
                                //}
                            }
                        }

                    }
                }
                if (checkAction)
                {
                    SaveXPosition();
                    Transfer();
                    Invoke("OnTap", 0.15f);
                }
                else
                {
                    OnTap();
                }

            }
            else
            {
                OnTap();
            }
        }
    }
    private void OnTap()
    {
        checkTapDown = false;
        checkTapUp = true;
    }


    //[HideInInspector]
    public float[] xToAction;/* = new float[5];*/
    //[HideInInspector]
    public int[] action /*= new int[5]*/;
    // jump = 1
    // 0 k lm gi
    // upordown = -1

    private int ix;
    public void SaveXPosition()
    {
        xToAction[ix] = captain.transform.position.x - 0.25f;
    }

    public void Jump()
    {
        action[ix] = 1;
    }
    public void UpOrDown()
    {
        action[ix] = -1;
    }

    // truyen hanh dong sang cho tung con
    public void Transfer()
    {
        ix++;
        if (ix >= hero.Length)
        {
            ix = 0;
        }
    }



    // ------------------ Skill ------------------
    // AirPunch
    [HideInInspector]
    public bool checkAirPunch;

    public int[] xAirPunchToUpGrade;
    public GameObject airPunchEffect;

    int dirfly;

    public void AirPunch()
    {
        float dirAirPunch = 0;
        if (captain.move.checkDown)
        {
            dirAirPunch = -1;
        }
        else
        {
            dirAirPunch = 1;
        }
        dirfly = 1;
        float xHeroMax = 0;
        float xDireMin = 0;
        if (EnemyManager.enemymanager.checkFight)
        {
            for (int i = 0; i < hero.Length; i++)
            {
                Enemy e = EnemyManager.enemymanager.enemy[i];
                Hero h = hero[i];
                if (h.gameObject.activeInHierarchy)
                {
                    if (xHeroMax == 0)
                    {
                        xHeroMax = h.transform.position.x;
                    }
                    else
                    {
                        if (h.transform.position.x > xHeroMax)
                        {
                            xHeroMax = h.transform.position.x;
                        }
                    }
                }
                if (e.gameObject.activeInHierarchy)
                {
                    if (xDireMin == 0)
                    {
                        xDireMin = e.transform.position.x;
                    }
                    else
                    {
                        if (e.transform.position.x < xDireMin)
                        {
                            xDireMin = e.transform.position.x;
                        }
                    }
                }
            }
        }
        if (xHeroMax > xDireMin)
        {
            dirfly = -1;
        }
        airPunchEffect.transform.position = new Vector3(CameraFollow.camerafollow.transform.position.x - 7 * dirfly, -0.5f * dirAirPunch, airPunchEffect.transform.position.z);
        airPunchEffect.transform.localScale = new Vector3(-0.4f * dirfly, 0.4f * dirAirPunch, 1);
        airPunchEffect.GetComponent<AirPunch>().dirfly = dirfly;
        airPunchEffect.GetComponent<AirPunch>().checkTrigger = false;
        airPunchEffect.gameObject.SetActive(true);
    }
    public void BeginAirPunch()
    {
        checkAirPunch = true;

        // level 1
        if (PlayerPrefs.GetInt("levelAirPunch") == 0)
        {
            for (int i = 0; i < hero.Length; i++)
            {
                Enemy e = EnemyManager.enemymanager.enemy[i];

                if (e.gameObject.activeInHierarchy)
                {
                    e.rb.velocity = new Vector2(width * 1.75f * dirfly, height * EnemyManager.enemymanager.dirScaleToGen);
                    e.property.enemy = null;
                    e.checkFindEnemy = true;
                    e.PlayAnim("beSkillAirpunch");
                }
            }
        }
        // level 2 ,3 ,4
        else if (PlayerPrefs.GetInt("levelAirPunch") >= 1)
        {
            for (int i = 0; i < hero.Length; i++)
            {
                Enemy e = EnemyManager.enemymanager.enemy[i];

                if (e.gameObject.activeInHierarchy)
                {
                    //level 3, level 4
                    bool checkPunch = false;
                    if (PlayerPrefs.GetInt("levelAirPunch") >= 2)
                    {
                        if (EnemyManager.enemymanager.enemyIndex.Count >= 1)
                        {
                            if (e.index == EnemyManager.enemymanager.enemyIndex[0])
                            {
                                Hero herotmp = hero[e.index];

                                float tmpchechNec;
                                if (checkNecroman[e.index] < 0)
                                {
                                    tmpchechNec = 0.5f;
                                }
                                else
                                {
                                    tmpchechNec = 1;
                                }
                                currentExp[e.index] += expGain * e.property.exp * e.property.level * tmpchechNec;

                                if (currentExp[e.index] >= herotmp.property.exp)
                                {
                                    int leveltoUp = (int)(currentExp[e.index] / herotmp.property.exp);
                                    currentExp[e.index] = currentExp[e.index] - leveltoUp * herotmp.property.exp;
                                    if (!herotmp.property.checkDie)
                                    {
                                        // + level               
                                        herotmp.property.LevelUp(leveltoUp);
                                    }
                                    else
                                    {
                                        // respawn
                                        herotmp.gameObject.SetActive(true);
                                        checkResurr[e.index] = 1;
                                        herotmp.property.LevelUp(leveltoUp - 1);
                                        herotmp.property.SetHpDefaulf();
                                    }
                                }

                                e.property.level = 0;
                                e.property.currentHp = 0;
                                e.property.SetHealthBar();
                                e.property.SetTextLevel();
                                e.property.Die();
                                numberOfEnemyBeKilled++;
                                checkPunch = true;
                            }
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("levelAirPunch") == 3)
                            {
                                if (EnemyManager.enemymanager.enemyIndex.Count >= 2)
                                {
                                    if (e.index == EnemyManager.enemymanager.enemyIndex[1])
                                    {
                                        Hero herotmp = hero[e.index];

                                        float tmpchechNec;
                                        if (checkNecroman[e.index] < 0)
                                        {
                                            tmpchechNec = 0.5f;
                                        }
                                        else
                                        {
                                            tmpchechNec = 1;
                                        }
                                        currentExp[e.index] += expGain * e.property.exp * e.property.level * tmpchechNec;

                                        if (currentExp[e.index] >= herotmp.property.exp)
                                        {
                                            int leveltoUp = (int)(currentExp[e.index] / herotmp.property.exp);
                                            currentExp[e.index] = currentExp[e.index] - leveltoUp * herotmp.property.exp;
                                            if (!herotmp.property.checkDie)
                                            {
                                                // + level               
                                                herotmp.property.LevelUp(leveltoUp);
                                            }
                                            else
                                            {
                                                // respawn
                                                herotmp.gameObject.SetActive(true);
                                                checkResurr[e.index] = 1;
                                                herotmp.property.LevelUp(leveltoUp - 1);
                                                herotmp.property.SetHpDefaulf();
                                            }
                                        }

                                        e.property.level = 0;
                                        e.property.currentHp = 0;
                                        e.property.SetHealthBar();
                                        e.property.SetTextLevel();
                                        e.property.Die();
                                        numberOfEnemyBeKilled++;
                                        checkPunch = true;
                                    }
                                }
                                pigs[0].gameObject.SetActive(false);
                                pigs[1].gameObject.SetActive(true);
                                currentPig = pigs[1];
                            }
                        }
                    }
                    if (!checkPunch)
                    {
                        e.rb.velocity = new Vector2(width * 3.5f * dirfly, height * EnemyManager.enemymanager.dirScaleToGen);
                        e.property.enemy = null;
                        e.checkFindEnemy = true;
                        e.PlayAnim("beSkillAirpunch");
                    }
                }
            }
            if (numberOfEnemyBeKilled >= enemyBeKilled[PlayerPrefs.GetInt("levelFireShield")])
            {
                PlayerPrefs.SetInt("levelFireShield", PlayerPrefs.GetInt("levelFireShield") + 1);
                if (PlayerPrefs.GetInt("levelFireShield") >= 3)
                {
                    PlayerPrefs.SetInt("levelFireShield", 3);
                }
            }
        }
        if (EnemyManager.enemymanager.checkFight)
        {
            EnemyManager.enemymanager.checkFight = false;
            for (int i = 0; i < hero.Length; i++)
            {
                Hero h = hero[i];
                if (h.gameObject.activeInHierarchy)
                {
                    h.move.checkCalled = false;
                    h.move.checkMove = true;
                    h.checkFindEnemy = true;
                    h.move.SetScale();
                    h.property.enemy = null;
                    h.PlayAnim("run");
                }
            }
            SetActionDefaul();
        }
        Invoke("EndAirPunch", 1f);
    }
    public void EndAirPunch()
    {
        // level 1
        for (int i = 0; i < hero.Length; i++)
        {
            Enemy e = EnemyManager.enemymanager.enemy[i];

            if (e.gameObject.activeInHierarchy)
            {
                e.checkFindEnemy = false;
                e.target = null;
                e.checkMove = false;
                e.PlayAnim("idle");
            }

            Hero h = hero[i];
            if (h.gameObject.activeInHierarchy)
            {
                h.checkFindEnemy = false;
                h.move.target = null;
            }
        }
        // level 2
        if (PlayerPrefs.GetInt("levelAirPunch") >= 1)
        {
            if (dirfly > 0)
            {
                EnemyManager.enemymanager.EndNoFight();
            }
        }

        checkAirPunch = false;
        EnemyManager.enemymanager.ClearList();
    }

    // Shield
    private float armorAddShield;
    public void Shield()
    {
        if (PlayerPrefs.GetInt("levelShield") == 0)
        {
            armorAddShield = 8;
            armorAddShield = 14;
        }
        else if (PlayerPrefs.GetInt("levelShield") == 1)
        {
            armorAddShield = 10;
            armorAddShield = 14;
        }
        else if (PlayerPrefs.GetInt("levelShield") == 2)
        {
            armorAddShield = 12;
            armorAddShield = 14;
        }
        else
        {
            armorAddShield = 14;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            if (hero[i].gameObject.activeInHierarchy)
                hero[i].property.Shield(armorAddShield);
        }
    }

    // Thunder
    float damageThunder;
    public int[] xGametoUpGradeThunder;
    public GameObject thunderEffect;
    public void Thunder()
    {
        if (PlayerPrefs.GetInt("levelThunder") == 0)
        {
            damageThunder = 100;
        }
        else if (PlayerPrefs.GetInt("levelThunder") == 1)
        {
            damageThunder = 200;
        }
        else if (PlayerPrefs.GetInt("levelThunder") == 2)
        {
            damageThunder = 400;
        }
        else
        {
            damageThunder = 1000;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        float xMax = 0;
        float xMin = 0;
        float dirthunder = 0;
        float dirDire = 0;
        for (int i = 0; i < hero.Length; i++)
        {
            Enemy e = EnemyManager.enemymanager.enemy[i];
            if (e.gameObject.activeInHierarchy)
            {
                if (xMin == 0f)
                {
                    xMin = e.transform.position.x;
                    dirDire = e.transform.localScale.y;
                }
                else
                {
                    if (xMin > e.transform.position.x)
                    {
                        xMin = e.transform.position.x;
                    }
                }

                if (xMax == 0f)
                {
                    xMax = e.transform.position.x;
                }
                else
                {
                    if (xMax < e.transform.position.x)
                    {
                        xMax = e.transform.position.x;
                    }
                }
            }
        }
        float xAverage = (xMax + xMin) / 2;
        if (captain.move.checkDown)
        {
            dirthunder = -1;
        }
        else
        {
            dirthunder = 1;
        }
        if (EnemyManager.enemymanager.quatityManagerEnemy <= 0)
        {
            xAverage = Random.Range(CameraFollow.camerafollow.transform.position.x, CameraFollow.camerafollow.transform.position.x + width);
            checkDestinationThunder = false;
        }
        else
        {
            if (dirDire == dirthunder)
            {
                checkDestinationThunder = true;
            }
            else
            {
                checkDestinationThunder = false;
            }
        }

        thunderEffect.transform.position = new Vector3(xAverage, 2.6f * dirthunder, thunderEffect.transform.position.z);
        thunderEffect.transform.localScale = new Vector3(thunderEffect.transform.localScale.x, 1.5f * dirthunder, thunderEffect.transform.localScale.z);
        thunderEffect.SetActive(true);
        Invoke("DamageThunder", 0.2f);
        Invoke("OffEffectThunder", 0.7f);
    }
    private bool checkDestinationThunder;
    public void DamageThunder()
    {
        if (checkDestinationThunder)
        {
            for (int i = 0; i < hero.Length; i++)
            {
                Enemy e = EnemyManager.enemymanager.enemy[i];
                if (e.gameObject.activeInHierarchy)
                {
                    e.property.TakeDamage(damageThunder);
                }
            }
        }
    }
    public void OffEffectThunder()
    {
        thunderEffect.SetActive(false);
    }

    // Bloodlust
    float damageAddBloodlust;
    public void Bloodlust()
    {
        if (PlayerPrefs.GetInt("levelBloodlust") == 0)
        {
            damageAddBloodlust = 6;
        }
        else if (PlayerPrefs.GetInt("levelBloodlust") == 1)
        {
            damageAddBloodlust = 8;
        }
        else if (PlayerPrefs.GetInt("levelBloodlust") == 2)
        {
            damageAddBloodlust = 10;
        }
        else
        {
            damageAddBloodlust = 12;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            if (hero[i].gameObject.activeInHierarchy)
                hero[i].property.Bloodlust(damageAddBloodlust);
        }
    }

    // Curse
    float curse;
    // public GameObject curseEffect;
    public void Curse()
    {
        //curseEffect.SetActive(false);
        if (PlayerPrefs.GetInt("levelCurse") == 0)
        {
            curse = 0.25f;
        }
        else if (PlayerPrefs.GetInt("levelCurse") == 1)
        {
            curse = 0.34f;
        }
        else if (PlayerPrefs.GetInt("levelCurse") == 2)
        {
            curse = 0.5f;
        }
        else
        {
            curse = 0.67f;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }

        for (int i = 0; i < hero.Length; i++)
        {
            Enemy e = EnemyManager.enemymanager.enemy[i];
            if (e.gameObject.activeInHierarchy)
            {
                e.property.Curse(curse);
            }
        }
        //float xMax = 0;
        //float xMin = 0;
        //float dirCurse = 0;
        //float dirDire = 0;
        //for (int i = 0; i < hero.Length; i++)
        //{
        //    Enemy e = EnemyManager.enemymanager.enemy[i];
        //    if (e.gameObject.activeInHierarchy)
        //    {
        //        if (xMin == 0f)
        //        {
        //            xMin = e.transform.position.x;
        //            dirDire = e.transform.localScale.y;
        //        }
        //        else
        //        {
        //            if (xMin > e.transform.position.x)
        //            {
        //                xMin = e.transform.position.x;
        //            }
        //        }

        //        if (xMax == 0f)
        //        {
        //            xMax = e.transform.position.x;
        //        }
        //        else
        //        {
        //            if (xMax < e.transform.position.x)
        //            {
        //                xMax = e.transform.position.x;
        //            }
        //        }

        //    }
        //}
        //float xAverage = (xMax + xMin) / 2;
        //if (captain.move.checkDown)
        //{
        //    dirCurse = -1;
        //}
        //else
        //{
        //    dirCurse = 1;
        //}
        //if (EnemyManager.enemymanager.quatityManagerEnemy <= 0)
        //{
        //    xAverage = Random.Range(CameraFollow.camerafollow.transform.position.x, CameraFollow.camerafollow.transform.position.x + width);
        //    checkDestinationCurse = false;
        //}
        //else
        //{
        //    if (dirDire == dirCurse)
        //    {
        //        checkDestinationCurse = true;
        //    }
        //    else
        //    {
        //        checkDestinationCurse = false;
        //    }
        //}

        //curseEffect.transform.position = new Vector3(xAverage, 0, curseEffect.transform.position.z);
        //curseEffect.transform.localScale = new Vector3(1, dirCurse, 1);
        //curseEffect.SetActive(true);
        //Invoke("OffEffectCurse", 15f);

        //if (checkDestinationCurse)
        //{
        //    for (int i = 0; i < hero.Length; i++)
        //    {
        //        Enemy e = EnemyManager.enemymanager.enemy[i];
        //        if (e.gameObject.activeInHierarchy)
        //        {
        //            e.property.Curse(curse);
        //        }
        //    }
        //}
    }
    private bool checkDestinationCurse;
    public void OffEffectCurse()
    {
        CancelInvoke("OffEffectCurse");
        //curseEffect.SetActive(false);
    }
    //Armor piercing
    float timeArmorPiercing;
    public void ArmorPiercing()
    {
        if (PlayerPrefs.GetInt("levelArmorPiercing") == 0)
        {
            timeArmorPiercing = 10;
        }
        else if (PlayerPrefs.GetInt("levelArmorPiercing") == 1)
        {
            timeArmorPiercing = 15;
        }
        else if (PlayerPrefs.GetInt("levelArmorPiercing") == 2)
        {
            timeArmorPiercing = 20;
        }
        else
        {
            timeArmorPiercing = 30;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            if (EnemyManager.enemymanager.enemy[i].gameObject.activeInHierarchy)
                EnemyManager.enemymanager.enemy[i].property.ArmorPiercing(timeArmorPiercing);
        }
    }

    //Fire Shield
    float armorWind;
    public void Wind()
    {
        if (PlayerPrefs.GetInt("levelWind") == 0)
        {
            armorWind = 15;
        }
        else if (PlayerPrefs.GetInt("levelWind") == 1)
        {
            armorWind = 25;
        }
        else if (PlayerPrefs.GetInt("levelWind") == 2)
        {
            armorWind = 35;
        }
        else
        {
            armorWind = 45;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            if (hero[i].gameObject.activeInHierarchy)
                hero[i].property.Wind(armorWind);
        }
    }

    // Resurrection
    float hpResurrection;
    //[HideInInspector]
    public int[] checkResurr;/* = new int[6];*/
    //[HideInInspector]
    public bool checkResurrection;
    //[HideInInspector]
    public int[] maxLevelHero;/* = new int[6] { 1, 1, 1, 1, 1, 1 };*/

    public int[] xHeroToUpGradeResurrection;
    public void Resurrection()
    {
        if (PlayerPrefs.GetInt("levelResurrection") == 0)
        {
            hpResurrection = 50;
        }
        else if (PlayerPrefs.GetInt("levelResurrection") == 1)
        {
            hpResurrection = 100;
        }
        else if (PlayerPrefs.GetInt("levelResurrection") == 2)
        {
            hpResurrection = 200;
        }
        else
        {
            hpResurrection = 500;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            if (hero[i].gameObject.activeInHierarchy)
            {
                if (!hero[i].property.checkDie)
                {
                    hero[i].property.Resurrection(hpResurrection);
                }
                else
                {
                    if (checkResurr[i] > 0)
                    {
                        hero[i].gameObject.SetActive(true);
                        hero[i].property.Respawn();
                        if (hpResurrection < hero[i].property.maxHp)
                        {
                            hero[i].property.currentHp = hpResurrection;
                            hero[i].property.level = 1;
                        }
                        else
                        {
                            //hpResurrection
                            int leveltmp = (int)(hpResurrection / hero[i].property.maxHp);
                            if (leveltmp >= maxLevelHero[i])
                            {
                                leveltmp = maxLevelHero[i];
                                hero[i].property.currentHp = hero[i].property.maxHp;
                            }
                            else
                            {
                                hero[i].property.currentHp = hpResurrection - hero[i].property.maxHp * leveltmp;
                            }
                            hero[i].property.level = leveltmp;
                        }
                        hero[i].property.SetHealthBar();
                        hero[i].property.SetTextLevel();
                    }
                }
            }
            else
            {
                if (checkResurr[i] > 0)
                {
                    hero[i].property.Respawn();
                    if (hpResurrection < hero[i].property.maxHp)
                    {
                        hero[i].property.currentHp = hpResurrection;
                        hero[i].property.level = 1;
                    }
                    else
                    {
                        //hpResurrection
                        int leveltmp = (int)(hpResurrection / hero[i].property.maxHp);
                        hero[i].property.currentHp = hpResurrection - hero[i].property.maxHp * leveltmp;
                        if (leveltmp >= maxLevelHero[i])
                        {
                            leveltmp = maxLevelHero[i];
                            hero[i].property.currentHp = hero[i].property.maxHp;
                        }
                        else
                        {
                            leveltmp = leveltmp + 1;
                        }
                        hero[i].property.level = leveltmp;
                    }
                    hero[i].property.SetHealthBar();
                    //hero[i].property.level = maxLevelHero[i];
                    hero[i].property.SetTextLevel();
                }
            }
        }
        checkResurrection = true;
        SetActionDefaul();
    }

    // FireShield
    [HideInInspector]
    public int killEnemyFireShield;

    public int[] enemyBeKilled;
    public void FireShield()
    {
        if (PlayerPrefs.GetInt("levelFireShield") == 0)
        {
            killEnemyFireShield = 1;
        }
        else if (PlayerPrefs.GetInt("levelFireShield") == 1)
        {
            killEnemyFireShield = 2;
        }
        else if (PlayerPrefs.GetInt("levelFireShield") == 2)
        {
            killEnemyFireShield = 3;
        }
        else
        {
            killEnemyFireShield = 4;
            pigs[0].gameObject.SetActive(false);
            pigs[1].gameObject.SetActive(true);
            currentPig = pigs[1];
        }
        for (int i = 0; i < hero.Length; i++)
        {
            hero[i].OnFireShield(killEnemyFireShield);
        }
    }

    //Gold Devil
    public int gold;
    int goldDevil;
    float timeGoldDevil;
    bool checkGoldDevil;
    public void OnGoldDevil()
    {
        checkGoldDevil = true;
        if (PlayerPrefs.GetInt("levelGoldDevil") == 0)
        {
            goldDevil = 10;
        }
        else if (PlayerPrefs.GetInt("levelGoldDevil") == 1)
        {
            goldDevil = 20;
        }
        else if (PlayerPrefs.GetInt("levelGoldDevil") == 2)
        {
            goldDevil = 30;
        }
        else
        {
            goldDevil = 40;
        }
    }
    public void OFFGoldDevil()
    {
        checkGoldDevil = false;
    }
    public void GoldDevil()
    {
        if (checkGoldDevil)
        {
            for (int i = 0; i < EnemyManager.enemymanager.enemyLevel.Count; i++)
            {
                gold += EnemyManager.enemymanager.enemyLevel[i] * goldDevil;
            }
        }
        UIManager.ui.SetTextGold();
    }

    //Big Troop
    //[HideInInspector]
    public int[] checkNecroman; /*= new int[5];*/
    public void BigTroop()
    {
        PlayerPrefs.SetInt("levelBigTroop", PlayerPrefs.GetInt("levelBigTroop") + 1);
        SetCaptain(hero[0]);
        if (PlayerPrefs.GetInt("levelBigTroop") < 10)
        {
            //if (PlayerPrefs.GetInt("levelBigTroop") < 4)
            //{
            //    pigs[0].gameObject.SetActive(true);
            //    pigs[1].gameObject.SetActive(false);
            //    currentPig = pigs[0];
            //}
            //else
            //{
            //    pigs[0].gameObject.SetActive(false);
            //    pigs[1].gameObject.SetActive(true);
            //    currentPig = pigs[1];
            //}
            checkResurr[0] = 1;
            hero[1].gameObject.SetActive(false);
            checkNecroman[1] = -1;
            hero[2].gameObject.SetActive(false);
            checkNecroman[2] = -1;
            checkResurr[3] = 1;
            hero[4].gameObject.SetActive(false);
            checkNecroman[4] = -1;
            SetLastHero(hero[5]);
            hero[3].move.targetFollow = captain.transform;
            hero[5].move.targetFollow = hero[3].transform;
            //numberOfHero = 2;
            numberOfHero = 3;
        }
        else if (PlayerPrefs.GetInt("levelBigTroop") < 20)
        {
            //if (PlayerPrefs.GetInt("levelBigTroop") < 4)
            //{
            //    pigs[0].gameObject.SetActive(true);
            //    pigs[1].gameObject.SetActive(false);
            //    currentPig = pigs[0];
            //}
            //else
            //{
            //    pigs[0].gameObject.SetActive(false);
            //    pigs[1].gameObject.SetActive(true);
            //    currentPig = pigs[1];
            //}
            checkResurr[0] = 1;
            checkResurr[1] = 1;
            hero[2].gameObject.SetActive(false);
            checkNecroman[2] = -1;
            checkResurr[3] = 1;
            hero[5].gameObject.SetActive(false);
            hero[3].move.targetFollow = hero[1].transform;
            checkNecroman[4] = -1;
            SetLastHero(hero[4]);
            numberOfHero = 4;
        }
        else if (PlayerPrefs.GetInt("levelBigTroop") < 50)
        {
            //if (PlayerPrefs.GetInt("levelBigTroop") < 4)
            //{
            //    pigs[0].gameObject.SetActive(true);
            //    pigs[1].gameObject.SetActive(false);
            //    currentPig = pigs[0];
            //}
            //else
            //{
            //    pigs[0].gameObject.SetActive(false);
            //    pigs[1].gameObject.SetActive(true);
            //    currentPig = pigs[1];
            //}
            checkResurr[0] = 1;
            checkResurr[1] = 1;
            checkResurr[2] = 1;
            checkResurr[3] = 1;
            hero[5].gameObject.SetActive(false);
            checkNecroman[4] = -1;
            SetLastHero(hero[4]);
            numberOfHero = 5;
        }
        else
        {
            //pigs[0].gameObject.SetActive(false);
            //pigs[1].gameObject.SetActive(true);
            //currentPig = pigs[1];
            SetLastHero(hero[5]);
            checkResurr[0] = 1;
            checkResurr[1] = 1;
            checkResurr[2] = 1;
            checkResurr[3] = 1;
            checkResurr[4] = 1;
            numberOfHero = hero.Length;
        }
    }

    //Arch-Necromancer
    // [HideInInspector]
    public float[] currentExp;/* = new float[5];*/

    [HideInInspector]
    public float expGain;
    public void ArchNecromancer()
    {
        PlayerPrefs.SetInt("levelArchNecromancer", 0);
        if (PlayerPrefs.GetInt("levelArchNecromancer") == 0)
        {
            expGain = 0.5f;
        }
        else if (PlayerPrefs.GetInt("levelArchNecromancer") == 1)
        {
            expGain = 0.6f;
        }
        else if (PlayerPrefs.GetInt("levelArchNecromancer") == 2)
        {
            expGain = 0.7f;
        }
        else
        {
            expGain = 1;
        }
    }

    public GameObject itemSkill;
    public void GenItemSkill()
    {
        //RemoveItemSkill();
        itemSkill.SetActive(true);
    }
    public void RemoveItemSkill()
    {
        itemSkill.SetActive(false);
    }
    public Sprite[] sprSkill;




    public Sprite[] warning;

    public GameObject[] dieTop;
    public GameObject[] dieBod;
    [HideInInspector]
    public GameObject[] die;

    int amountGenDie = 0;
    int upgradeSizeGenDie = 0;
    int aMax = 1;
    int bMax = 1;
    public void GenDie(float xToGen)
    {
        amountGenDie++;
        //print(amountGenDie);
        int a = Random.Range(2, 4);
        int b = Random.Range(2, 4);

        if (amountGenDie > upgradeSizeGenDie)
        {
            upgradeSizeGenDie += 10;
            aMax++;
            bMax++;
            if (aMax >= dieTop.Length)
            {
                aMax = dieTop.Length;
            }
            if (bMax >= dieBod.Length)
            {
                bMax = dieBod.Length;
            }
        }

        bool checkDungNham = false;
        if (a > 0)
        {
            float aToGen = 21 / a;
            for (int i = 0; i < a; i++)
            {
                int aGen = Random.Range(0, aMax);
                if (aGen == 0)
                {
                    checkDungNham = true;
                }
                //float y = 0.35f;
                //if (aGen == 2)
                //{
                //    y = 1.4f;
                //}
                GameObject obj = Instantiate(dieTop[aGen],
                    new Vector3(Random.Range(xToGen - 10.5f + aToGen * i, xToGen - 10.5f + aToGen * (i + 1)), dieTop[aGen].transform.position.y, 0), Quaternion.identity);
                ////dòng if này mục đích check cho die (chông, lửa phun,...) ko sinh ra dưới các cầu trên map
                //if (obj.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
                //{
                //    Destroy(obj);
                //    Debug.Log("<color = blue>Warning: Destroy die object.</color>");
                //}



                if (aGen == 1)
                {
                    int rdTmp = Random.Range(0, 11);
                    obj.GetComponent<Animator>().Play("ChongAnim", -1, (float)(rdTmp / 10f));
                }
                if (aGen == 5)
                {
                    obj.transform.position = new Vector3(obj.transform.position.x, height, 0);
                    obj.GetComponent<Ice>().speed = height;
                }
                Destroy(obj, 10);
            }
        }
        if (b > 0)
        {
            float bToGen = 21 / b;
            for (int i = 0; i < a; i++)
            {
                int bGen;
                if (checkDungNham)
                {
                    bGen = Random.Range(1, bMax);
                }
                else
                {
                    bGen = Random.Range(0, bMax);
                }
                GameObject obj = Instantiate(dieBod[bGen],
                    new Vector3(Random.Range(xToGen - 10.5f + bToGen * i, xToGen - 10.5f + bToGen * (i + 1)), dieBod[bGen].transform.position.y, 0), Quaternion.identity);
                //dòng if này mục đích check cho die (chông, lửa phun,...) ko sinh ra dưới các cầu trên map
                if (obj.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    Destroy(obj);
                    Debug.Log("<color = blue>Warning: Destroy die object.</color>");
                }
                if (bGen == 1)
                {
                    int rdTmp = Random.Range(0, 11);
                    obj.GetComponent<Animator>().Play("ChongAnim", -1, (float)(rdTmp / 10f));
                }
                Destroy(obj, 10);
            }
        }
    }

    public GameObject arrow;
}


