using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;

public class Property : MonoBehaviour {

    public int index;
    public int level;
    public float maxAttack;
    public float maxHp;
    public float maxArmorMelee;
    public float maxArmorRange;
    public float exp;

   [HideInInspector]
    public float currentAttack;
   [HideInInspector]
    public float currentHp;
   [HideInInspector]
    public float currentArmorMelee;
   [HideInInspector]
    public float currentArmorRange;
    //[HideInInspector]
    

    public SpriteRenderer healthBar;
    public TextMesh textLevel;

    public BoxCollider2D coll;
    public BoxCollider2D collCheckGround;

    private Hero heroTemp;
    private Enemy enemyTemp;
    public void Awake()
    {
        heroTemp = GetComponent<Hero>();
        enemyTemp = GetComponent<Enemy>();
    }
    public void Start()
    {
        Renderer mesh = textLevel.gameObject.GetComponent<Renderer>();
        mesh.sortingLayerName = healthBar.sortingLayerName;
        mesh.sortingOrder = healthBar.sortingOrder + 5;

        if (heroTemp != null)
        {
            index = heroTemp.index;
        }
        else
        {
            index = GetComponent<Enemy>().index;
        }
        DelayStart();

    }

    public void DelayStart()
    {
        SetDefault();
        checkIndexEnemy = false;
        Enable();
        checkOnEnable = true;
    }

    public void SetDefault()
    {
        currentArmorMelee = maxArmorMelee;
        currentArmorRange = maxArmorRange;
        currentAttack = maxAttack;
        SetTextLevel();
        SetHpDefaulf();
    }
    public void SetHpDefaulf()
    {
        currentHp = maxHp;
        SetHealthBar();
    }
    public void SetScaleTextLevel(int dir)
    {
        textLevel.transform.localScale = new Vector3(1, dir, 1);
    }
    public float damage;
    public void CaculatorDamage()
    {
        if (enemy != null)
        {
            if (index < 3)
            {
                if (currentAttack > enemy.currentArmorMelee)
                {
                    damage = (currentAttack - enemy.currentArmorMelee) * level;
                }
                else
                {
                    damage = level;
                }
            }
            else
            {
                if (currentAttack > enemy.currentArmorRange)
                {
                    damage = (currentAttack - enemy.currentArmorRange) * level;
                }
                else
                {
                    damage = level;
                }
            }
            if (damage <= level)
            {
                damage = level;
            }
        }      
    }
    // [HideInInspector]
    public Property enemy;
    public List<Property> listFighter;
    public void Attack()
    {
        StopAttack();      
        if (heroTemp != null)
        {
            heroTemp.move.checkCall = true;
            heroTemp.move.checkChangeGravity = false;
            heroTemp.OnBox();
        }      
        InvokeRepeating("AttackToEnemy", 0.5f, 0.5f);
    }
    public void AttackToEnemy()
    {
        int dirScale = 1;
        if (enemy != null)
        {        
            if (enemy.transform.position.x < transform.position.x)
            {
                dirScale = -1;
            }
            transform.localScale = new Vector3(dirScale, transform.localScale.y, transform.localScale.z);
        }

        if (index != 2)
        {
            if (enemy != null)
            {
                CaculatorDamage();
                if (index >= 3)
                {
                    GameObject arrowTmp;

                    GameObject objIns;
                    if (index == 3)
                    {                      
                        objIns = GamePlay.gameplay.arrow;
                    }
                    else
                    {
                        objIns = GamePlay.gameplay.arrow;
                    }
                    if (heroTemp != null)
                    {
                        arrowTmp = Instantiate(objIns, new Vector3(transform.position.x, transform.position.y + heroTemp.move.dirScale * 0.7f, 0), Quaternion.identity);
                    }
                    else
                    {
                        arrowTmp = Instantiate(objIns, new Vector3(transform.position.x, transform.position.y + enemyTemp.dirScale * 0.7f, 0), Quaternion.identity);

                    }
                    arrowTmp.transform.localScale = new Vector3(-dirScale, 1, 1);
                    float distanceTmp = Mathf.Abs(enemy.transform.position.x - transform.position.x);

                    float flyTime = 0.5f * (distanceTmp / GamePlay.gameplay.width);
                    if (heroTemp != null)
                    {
                        iTween.MoveTo(arrowTmp, iTween.Hash("position", new Vector3(enemy.transform.position.x + Random.Range(-0.2f,0.2f), enemy.transform.position.y + heroTemp.move.dirScale * 1, 0), "time", flyTime));
                    }
                    else
                    {
                        iTween.MoveTo(arrowTmp, iTween.Hash("position", new Vector3(enemy.transform.position.x + Random.Range(-0.2f, 0.2f), enemy.transform.position.y + enemyTemp.dirScale * 1, 0), "time", flyTime));
                    }
                    Destroy(arrowTmp, flyTime + 0.1f);

                    // co Delay mot khoang flyTime
                    Invoke("DelayAttack", flyTime);
                }
                else
                {
                    // k delay
                    DelayAttack();
                }
                
               
            }
        }
        // danh lan
        else
        {
            
            arrayEnemyToAttackAoe = new Collider2D[0];

            if (heroTemp != null)
            {
                arrayEnemyToAttackAoe = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + heroTemp.move.dirScale * 0.65f), new Vector2(heroTemp.ShortRange + 1 , 0.5f), 0, heroTemp.layerEnemy);
            }
            else
            {
                arrayEnemyToAttackAoe = Physics2D.OverlapBoxAll(new Vector3(transform.position.x, transform.position.y + enemyTemp.dirScale * 0.65f), new Vector2(enemyTemp.range + 1, 0.5f), 0, enemyTemp.layerEnemy);
            }

            if (arrayEnemyToAttackAoe.Length > 0)
            {
                for (int i = 0; i < arrayEnemyToAttackAoe.Length; i++)
                {
                    enemy = arrayEnemyToAttackAoe[i].GetComponent<Property>();
                    if (enemy != null)
                    {
                        CaculatorDamage();
                        enemy.TakeDamage(damage);
                        if (enemy != null)
                        {
                            if (enemy.currentHp <= 0 && enemy.level < 2)
                            {
                                enemy = null;
                                StopAttack();
                            }
                        }
                    }
                }
            }
            else
            {
                if (enemy != null)
                {
                    CaculatorDamage();
                    enemy.TakeDamage(damage);
                    if (enemy != null)
                    {
                        if (enemy.currentHp <= 0 && enemy.level < 2)
                        {
                            enemy = null;
                            StopAttack();
                        }
                    }
                    else
                    {
                        if (enemyTemp != null)
                        {
                            enemyTemp.checkFindEnemy = false;
                        }
                    }
                }
                else
                {
                   // print(gameObject.name);
                    if (heroTemp != null)
                    {
                        StopAttack();
                        heroTemp.move.ContinueMove();
                        heroTemp.PlayAnim("run");
                    }
                }
            }
        }
    }
    public void DelayAttack()
    {
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (enemy != null)
            {
                if (enemy.currentHp <= 0 && enemy.level < 2)
                {
                    enemy = null;
                    StopAttack();
                }
            }
            else
            {
                if (enemyTemp != null)
                {
                    enemyTemp.checkFindEnemy = false;
                }
            }
        }
    }
    public Collider2D[] arrayEnemyToAttackAoe;
    public void StopAttack()
    {
        CancelInvoke("AttackToEnemy");
    }
    public void TakeDamage(float amount)
    {
        if (amount <= currentHp)
        {
            currentHp -= amount;
            if (currentHp <= 0)
            {
                currentHp = 0;
                Die();
            }

        }
        else
        {
            int po = (int)(amount / maxHp);
            if (po >= level)
            {
                level = 1;
                SetTextLevel();
                currentHp = 0;
                Die();
            }
            else
            {
                LevelUp(-po);
                float mod = amount - maxHp * po;

                currentHp -= mod;
                if (currentHp <= 0)
                {
                    currentHp = 0;
                    Die();
                }
            }
        }
        GentextDamageTaken(amount);

        SetHealthBar();
    }
    //public List<TextDamage> listTextDamage;
    public void GentextDamageTaken(float amount)
    {
        int dir;
        if (heroTemp != null)
        {
            if (heroTemp.move.checkDown)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }
        }
        else
        {
            dir = enemyTemp.dirScale;
        }
        TextDamage textTmp = Instantiate(GamePlay.gameplay.textDamage, new Vector3(transform.position.x, textLevel.transform.position.y), Quaternion.identity);
        textTmp.damage = (int)amount;
        textTmp.dir = dir;
        if (gameObject.tag == "Radiant")
        {
            textTmp.color = GamePlay.gameplay.colorDamageRadiantTaken;
        }
        else if (gameObject.tag == "Dire")
        {
            textTmp.color = GamePlay.gameplay.colorDamageDireTaken;
        }
        //listTextDamage.Add(textTmp);
        //for(int i = 0; i < listTextDamage.Count; i++)
        //{
        //    listTextDamage[i].transform.position = new Vector3( textTmp.transform.position.x, textTmp.transform.position.y + 0.25f, textTmp.transform.position.z);
        //}
        //Invoke("RemoveListTextDamage", 1);
    }
    //public void RemoveListTextDamage()
    //{
    //    listTextDamage.Remove(listTextDamage[0]);
    //}
    public void SetHealthBar()
    {
        healthBar.size = new Vector2(currentHp / maxHp, healthBar.size.y);
    }
    public void SetTextLevel()
    {
        if (level <= 1)
        {
            level = 1;
           // Disable();
        }
        textLevel.text = level.ToString();
    }
    public void LevelUp(int amount)
    {
        level += amount;
        if(level > GamePlay.gameplay.maxLevelHero[index])
        {
            GamePlay.gameplay.maxLevelHero[index] = level;
        }
        SetTextLevel();
    }

    public void Die()
    {
        if (level > 1)
        {
            LevelUp(-1);
            SetHpDefaulf();
        }
        else
        {
            level = 0;
            Disable();
        }

    }
    bool checkOnEnable;
    public void OnEnable()
    {
        if (checkOnEnable)
        {
            if (heroTemp != null)
            {
                GamePlay.gameplay.heroAmount++;
                if(GamePlay.gameplay.heroAmount >= 5)
                {
                    GamePlay.gameplay.heroAmount = 5;
                }
            }
            if (checkRespawnToSetDefaulProperty)
            {
                checkRespawnToSetDefaulProperty = false;
            }
            else
            {
                SetDefault();
            }
            checkIndexEnemy = false;
            Enable();           
        }
        
        //Invoke("Enable", 0.05f);
    }
    public void Enable()
    {
        checkDie = false;
        // tiep tuc chay           
        checkIndexEnemy = false;

        if (GamePlay.checkStart)
        {            
            coll.enabled = true;
            collCheckGround.enabled = true;
            if (heroTemp != null)
            {
                for(int i =0; i<heroTemp.skillEffect.Length - 1; i++)
                {
                    heroTemp.skillEffect[i].SetActive(false);
                }
                heroTemp.skillEffect[4].SetActive(true);
                Invoke("OffSkillEffectRes", 1.6f);

                heroTemp.move.SetTransform();
                if (heroTemp.index < GamePlay.gameplay.captain.index)
                {
                    GamePlay.gameplay.SetCaptain(heroTemp);
                    enemy = null;
                }
                if (heroTemp.index > GamePlay.gameplay.lastHero.index)
                {
                    GamePlay.gameplay.SetLastHero(heroTemp);
                }
                if (heroTemp.index >= 0)
                {
                    int i = 1;
                    int j = 1;
                    if (i < 4)
                    {
                        if (heroTemp.index + i < GamePlay.gameplay.hero.Length)
                            while (!GamePlay.gameplay.hero[heroTemp.index + i].gameObject.activeInHierarchy)
                            {
                                i++;
                                if (heroTemp.index + i >= GamePlay.gameplay.hero.Length)
                                {
                                    break;
                                }
                            }
                    }
                    if (j < 4)
                    {
                        if (heroTemp.index - j < GamePlay.gameplay.hero.Length - 1 && heroTemp.index - j >= 0)
                            while (!GamePlay.gameplay.hero[heroTemp.index - j].gameObject.activeInHierarchy)
                            {
                                j++;
                                if (heroTemp.index - j >= GamePlay.gameplay.hero.Length - 1 || heroTemp.index - j < 0)
                                {
                                    break;
                                }
                            }
                    }
                    if (heroTemp.index - j < GamePlay.gameplay.hero.Length && heroTemp.index - j >= 0)
                        heroTemp.move.SetTargetFollow(GamePlay.gameplay.hero[heroTemp.index - j].transform);
                    if (heroTemp.index + i < GamePlay.gameplay.hero.Length)
                        GamePlay.gameplay.hero[heroTemp.index + i].move.SetTargetFollow(this.transform);
                }
                heroTemp.move.ContinueMove();
                heroTemp.PlayAnim("run");

                GamePlay.gameplay.numberOfHero++;
                
                if (GamePlay.gameplay.heroAmount >= 5)
                {
                    GamePlay.gameplay.heroAmount = 5;
                }
                if (GamePlay.gameplay.heroAmount > 0)
                {
                    EnemyManager.enemymanager.checkDontMove = false;
                }
                if (GamePlay.gameplay.numberOfHero >= GamePlay.gameplay.xHeroToUpGradeResurrection[PlayerPrefs.GetInt("levelResurrection")])
                {
                    PlayerPrefs.SetInt("levelResurrection", PlayerPrefs.GetInt("levelResurrection") + 1);
                    if (PlayerPrefs.GetInt("levelResurrection") >= 3)
                    {
                        PlayerPrefs.SetInt("levelResurrection", 3);
                    }
                }
            }
        }
        SetTextLevel();
        
        //SetScaleTextLevel();
        for(int i = 0; i < sprSkill.Length; i++)
        {
            sprSkill[i].enabled = false;
        }
        enemy = null;
      
    }

    public void OffSkillEffectRes()
    {
        heroTemp.skillEffect[4].SetActive(false);
    }
    //[HideInInspector]
    public bool checkDie;
    public void Disable()
    {
        if (!checkDie)
        {
            checkDie = true;
            
            for (int i = 0; i < listFighter.Count; i++)
            {
                listFighter[i].enemy = null;
            }
            level = 1;
            textLevel.text = level.ToString();
            currentHp = 0;
            SetHealthBar();
            listFighter.Clear();
            StopAttack();
            coll.enabled = false;
            collCheckGround.enabled = false;
           
            // la quan ta Radiant
            if (heroTemp != null)
            {
                heroTemp.PlayAnim("die");
                if(heroTemp.move.notAtEg)
                    heroTemp.move.rb.gravityScale = 0;
                GamePlay.gameplay.heroAmount--;
                if (GamePlay.gameplay.heroAmount <= 0)
                {
                    EnemyManager.enemymanager.checkDontMove = true;
                }
                heroTemp.move.rb.velocity = Vector2.zero;
                heroTemp.move.checkChangeGravity = false;
                
                // la quan ta Radiant
                if (heroTemp != null)
                {
                    // neu la captain
                    if (heroTemp.move.checkCaptain)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (!GamePlay.gameplay.hero[i].property.checkDie)
                            {
                                GamePlay.gameplay.SetCaptain(GamePlay.gameplay.hero[i]);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    Invoke("GameOver", 1);
                                }
                            }
                        }

                    }
                    // neu la lastHero
                    else if (heroTemp.move.checkEnd)
                    {
                        if (heroTemp.index > 0)
                        {
                            for (int i = 4; i >= 0; i--)
                            {
                                if (!GamePlay.gameplay.hero[i].property.checkDie)
                                {
                                    GamePlay.gameplay.SetLastHero(GamePlay.gameplay.hero[i]);
                                    break;
                                }
                            }
                        }
                    }

                    else if (heroTemp.index > 0 && heroTemp.index < 4)
                    {
                        int i = 1;
                        int j = 1;
                        if (i < 4)
                        {
                            while (GamePlay.gameplay.hero[heroTemp.index + i].property.checkDie)
                            {
                                i++;
                                if (heroTemp.index + i >= 4)
                                {
                                    break;
                                }
                            }
                        }
                        if (j < 4)
                        {
                            while (GamePlay.gameplay.hero[heroTemp.index - j].property.checkDie)
                            {
                                j++;
                                if (heroTemp.index - j <= 0)
                                {
                                    break;
                                }
                            }
                        }
                        if (heroTemp.index + i >= 0 && heroTemp.index + i <= 4 && heroTemp.index - j >= 0 && heroTemp.index - j <= 4)
                        {
                            GamePlay.gameplay.hero[heroTemp.index + i].move.SetTargetFollow(GamePlay.gameplay.hero[heroTemp.index - j].transform);
                        }
                    }
                }
                EndBloodlust();
                EndShield();
                EndWind();
                heroTemp.OFFFireShield();

            }
            else
            {
                Enemy e = GetComponent<Enemy>();
                e.PlayAnim("die");
                e.rb.gravityScale = 0;
                ////la quan dich Dire

                if (!checkIndexEnemy)
                {
                    checkIndexEnemy = true;
                    EnemyManager.enemymanager.indexEnemy--;

                    if (EnemyManager.enemymanager.indexEnemy <= 0)
                    {
                        EnemyManager.enemymanager.EndFight();
                    }

                    EnemyManager.enemymanager.quatityManagerEnemy--;
                }

                EndCurse();
                EndArmorPiercing();
            }
            Invoke("WaitToDisable", 1.1f);
        }

    }

    bool checkRespawnToSetDefaulProperty;
    public void Respawn()
    {
        checkRespawnToSetDefaulProperty = true;
        gameObject.SetActive(true);
        Enable();
        checkDie = false;
        CancelInvoke("WaitToDisable");
        heroTemp.move.SetTransform();
        if (heroTemp.move.checkDown)
        {
            SetScaleTextLevel(-1);
        }
        else
        {
            SetScaleTextLevel(1);
        }
    }

    public void WaitToDisable()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Hero heroTemp = GetComponent<Hero>();
        checkDie = true;
        level = 1;
      

        if (checkCurse)
        {
            cruseEffect.SetActive(false);
        }       
    }
    private bool checkIndexEnemy;

    public void GameOver()
    { 
        //UIManager.ui.GameOver();
        UIManager.ui.ResetGameIfWatchVideoAds();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Die")
        {
            Disable();
            Animator anim = coll.GetComponent<Animator>();
            if(anim != null)
                anim.Play("OffAnim");
            else
            {
                BoxCollider2D box = coll.GetComponent<BoxCollider2D>();
                if(box!=null)
                    box.enabled = false;
            }
        }      
        else if (coll.gameObject.tag == "CauDa")
        {
            coll.GetComponent<Animator>().Play("Fire");
            int dirCauDa = -1;
            if (!heroTemp.move.checkDown)
            {
                dirCauDa = 1;
            }                       
            heroTemp.move.rb.velocity = new Vector2(0, dirCauDa * 25);
            Invoke("Disable", 1);
        }
        else if (coll.gameObject.tag == "WaterFall" )
        {
            if (!checkWaterFall)
            {
                checkWaterFall = true;
                float damageByWaterFall = ((level - 1) * maxHp + currentHp) / 3;
                TakeDamage(damageByWaterFall);
            }
        }
    }
    bool checkWaterFall;
    private void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "WaterFall")
        {
            checkWaterFall = false;
        }
    }
  
    //private void OnParticleCollision(GameObject other)
    //{
        
    //}

    public void OnHoDoc()
    {
        InvokeRepeating("DamePerSecond", 0, 0.5f);
    }
    public void DamePerSecond()
    {
        TakeDamage(10);
    }
    public void OffHoDoc()
    {
        CancelInvoke("DamePerSecond");
    }










    // ---------------------- **SKILL** -------------------------
    public SpriteRenderer[] sprSkill;
    private int[] indexSkill = new int[5];
    
    //  Shield
    float armorShield;
    public void Shield(float amount)
    {
        CancelInvoke("EndShield");
        armorShield = amount;
        currentArmorMelee = maxArmorMelee + amount;
        Invoke("EndShield",10);
        sprSkill[0].enabled = true;
        heroTemp.skillEffect[0].SetActive(true);
    }
    public void EndShield()
    {
        CancelInvoke("EndShield");
        currentArmorMelee -= armorShield;
        sprSkill[0].enabled = false;
        heroTemp.skillEffect[0].SetActive(false);
    }

    // Bloodlust
    float damageBloodlust;
    public void Bloodlust(float amount)
    {
        CancelInvoke("EndBloodlust");
        damageBloodlust = amount;
        currentAttack = maxAttack + amount;
        Invoke("EndBloodlust", 10);
        sprSkill[1].enabled = true;
        heroTemp.skillEffect[1].SetActive(true);
        Invoke("OffEffectBloodlust",0.55f);
    }
    public void OffEffectBloodlust()
    {
        heroTemp.skillEffect[1].SetActive(false);
    }
    public void EndBloodlust()
    {
        CancelInvoke("EndBloodlust");
        currentAttack -= damageBloodlust;
        sprSkill[1].enabled = false;
    }

    //Cruse
    float cruse;
    private bool checkCurse;
    public GameObject cruseEffect;
    public void Curse(float amount)
    {
        CancelInvoke("EndCurse");
        cruseEffect.SetActive(true);
        cruse = maxAttack - maxAttack*amount;
        currentAttack = maxAttack * amount;       
        Invoke("EndCurse", 10);
        sprSkill[0].enabled = true;
        checkCurse = true;
    }

    public void EndCurse()
    {
        CancelInvoke("EndCurse");
        currentAttack += cruse;
        sprSkill[0].enabled = false;
        checkCurse = false;
        cruseEffect.SetActive(false);
    }

    //ArmorPiercing
    float armorpiercingRange;
    float armorpiercingMeLee;
    public GameObject skillEffect;
    public void ArmorPiercing(float time)
    {
        CancelInvoke("EndArmorPiercing");
        armorpiercingRange = currentArmorRange;
        armorpiercingMeLee = currentArmorMelee;
        currentArmorRange = 0;
        currentArmorMelee = 0;
        Invoke("EndArmorPiercing", time);
        sprSkill[1].enabled = true;
        skillEffect.SetActive(true);
        Invoke("OffSkillEffectArmorPiercing", 0.41f);
    }

    public void OffSkillEffectArmorPiercing()
    {
        skillEffect.SetActive(false);
    }
    public void EndArmorPiercing()
    {
        CancelInvoke("EndArmorPiercing");
        currentArmorRange += armorpiercingRange;
        currentArmorMelee += armorpiercingMeLee;
        sprSkill[0].enabled = false;
    }

    //FireShield
    float armorWind;
    public void Wind(float amount)
    {
        CancelInvoke("EndWind");
        heroTemp.skillEffect[2].SetActive(false);
        armorWind = amount;
        currentArmorRange += amount;
        Invoke("EndWind", 10);
        sprSkill[2].enabled = true;
        heroTemp.skillEffect[2].SetActive(true);
        //heroTemp.skillEffect[2].GetComponent<UnityArmatureComponent>().animation.Play("windShield");
        //Invoke("WaitAnimWind", 0.4f);
    }
    public void WaitAnimWind()
    {
        heroTemp.skillEffect[2].GetComponent<UnityArmatureComponent>().animation.Stop();
    }
    public void EndWind()
    {
        CancelInvoke("EndWind");
        currentArmorRange -= armorWind;
        sprSkill[2].enabled = false;
        heroTemp.skillEffect[2].SetActive(false);
    }

    //Resurrection
    public void Resurrection(float amount)
    {
        if (checkDie)
        {
            level = 1;
            Respawn();
            if (amount < maxHp)
            {
                currentHp = amount;
            }
            else
            {
                currentHp = maxHp;
            }          
        }
        else
        {
            if (amount < maxHp - currentHp)
            {
                currentHp += amount;
            }
            else
            {
                amount = amount - (maxHp - currentHp);
                currentHp = maxHp;
                if (level < GamePlay.gameplay.maxLevelHero[index])
                {
                    LevelUp(1);
                }
                if (level < GamePlay.gameplay.maxLevelHero[index])
                {
                    if (amount > 0)
                    {
                        int leveltmp = (int)(amount / maxHp);
                        amount = amount - maxHp * leveltmp;
                        if (leveltmp >= GamePlay.gameplay.maxLevelHero[index] - level)
                        {
                            leveltmp = GamePlay.gameplay.maxLevelHero[index] - level;
                        }
                        LevelUp(leveltmp);
                        currentHp = amount;
                    }
                }
            }
        }
        SetHealthBar();
        heroTemp.skillEffect[4].SetActive(true);
        Invoke("OffEffectResurrection",1.6f);
    }
    public void OffEffectResurrection()
    {
        heroTemp.skillEffect[4].SetActive(false);

    }
    
}
