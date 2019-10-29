using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager enemymanager;

    public bool checkGen;
    public float expIncrease;

    public Enemy[] enemyToGen;

    [HideInInspector]
    public List<Enemy> enemy;
    //[HideInInspector]
    public List<int> enemyIndex;
    // [HideInInspector]
    public List<int> enemyLevel;

    public int quatityManagerEnemy;
    public int timeToGen;
    public void Awake()
    {
        enemymanager = this;
    }
    public void Start()
    {
        if (!checkGen)
        {
            Invoke("Gen", timeToGen);
        }
        else
        {
            Invoke("Gen1", timeToGen);
        }
        for (int i = 0; i < enemyToGen.Length; i++)
        {
            Enemy obj = Instantiate(enemyToGen[i], new Vector3(0, 0, 0), Quaternion.identity);
            obj.index = i;
            enemy.Add(obj);
            obj.transform.SetParent(GamePlay.gameplay.environment);
            obj.gameObject.SetActive(false);
        }
        ClearList();
        xPositionMin = -6;
        xPositionMax = -6;
    }
    //[HideInInspector]
    public int indexEnemy;
    //[HideInInspector]
    public bool checkFight;

    public bool test;

    [HideInInspector]
    public int dirScaleToGen;
    float expTmp = 15;
    [SerializeField]
    int levelAdd;

    public int amountGenTwoEnemy;

    private int indexGenTwoEnemy;
    [HideInInspector]
    public float xPositionMin;
    [HideInInspector]
    public float xPositionMax;

    public void Gen()
    {
        //dòng dưới để test gen item skill, test xong thì comment lại
        //GamePlay.gameplay.GenItemSkill();


        int rd = Random.Range(0, 100);
        if (rd < 60)
        {
            GenEnemy();
        }
        else
        {
            //GamePlay.gameplay.GenDie(CameraFollow.camerafollow.transform.position.x + 21);
            if (!checkGen)
            {
                Invoke("Gen", timeToGen + 5);
            }
            else
            {
                Invoke("Gen1", timeToGen + 5);
            }
        }
    }


    public void GenEnemy()
    {
        checkEndFight = false;
        expTmp = 0;
        GamePlay.gameplay.GenItemSkill();
        for (int i = 0; i < GamePlay.gameplay.hero.Length; i++)
        {
            Hero herotmp = GamePlay.gameplay.hero[i];
            if (herotmp.gameObject.activeInHierarchy)
            {
                expTmp += herotmp.property.exp * herotmp.property.level;
            }
        }

        expTmp *= Random.Range(0.5f, 1.5f);

        int indexTmp = (int)(expTmp / 18.5f);

        // sinh dot bien
        int mutation = 0;
        if (Random.Range(0, 100) > 70)
        {
            mutation = Random.Range(indexTmp, indexTmp * 2);
        }

        quatityManagerEnemy = indexEnemy = indexTmp + Random.Range(-indexTmp / 2, indexTmp / 2) + mutation;

        if (indexGenTwoEnemy < amountGenTwoEnemy)
        {
            indexGenTwoEnemy++;
            if (indexEnemy <= 0)
            {
                quatityManagerEnemy = indexEnemy = 1;
            }
            else
            {
                levelAdd = indexEnemy - 2;
                quatityManagerEnemy = indexEnemy = 2;
            }
        }
        else
        {
            if (indexEnemy <= 0)
            {
                quatityManagerEnemy = indexEnemy = 1;
            }
            //else if (indexEnemy > 5)
            //{
            //    levelAdd = indexEnemy - 5;
            //    quatityManagerEnemy = indexEnemy = 5;
            //}
            else if (indexEnemy > enemyToGen.Length)
            {
                levelAdd = indexEnemy - enemyToGen.Length;
                quatityManagerEnemy = indexEnemy = enemyToGen.Length;
            }
            else
            {
                levelAdd = 1;
            }
        }

        GamePlay.gameplay.checkCallTeamRadiant = false;

        if (Random.Range(0, 100) > 50)
        {
            dirScaleToGen = -1;
        }
        else
        {
            dirScaleToGen = 1;
        }


        int[] temp = new int[0];
        do
        {
            //int index = Random.Range(0, 5);
            int index = Random.Range(0, enemyToGen.Length);
            if (System.Array.FindIndex(temp, t => t == index) < 0)
            {
                System.Array.Resize(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = index;
            }
        }
        while (temp.Length < indexEnemy);

        float expMaxEnemy = 0;
        int maxLevelEnemy = -1;
        int indexMaxLevelEnemy = -1; ;


        for (int i = 0; i < indexEnemy; i++)
        {
            Enemy e = enemy.Find(a => a.index == temp[i]);
            int rd = Random.Range(-1, levelAdd);
            if (rd < 0)
            {
                rd = 0;
            }
            levelAdd -= rd;
            e.gameObject.SetActive(true);
            e.property.level = rd + 1;

            // tim enemy level cao nhat
            if (e.property.level * e.property.exp > expMaxEnemy)
            {
                expMaxEnemy = e.property.level * e.property.exp;
                maxLevelEnemy = e.property.level;
                indexMaxLevelEnemy = e.index;
            }

            //add list
            enemyIndex.Add(e.index);
            enemyLevel.Add(e.property.level);

            e.SetProperty();
            e.SetTranform();
            Debug.Log("<color=red>Enemy name is: </color>" + "<b>" + e.name + "</b>");

            //if (xPositionMax == -6)
            //{
            //    xPositionMax = e.transform.position.x;
            //}
            //else
            //{   if (e.transform.position.x > xPositionMax)
            //    {
            //        xPositionMax = e.transform.position.x;
            //    }
            //}
            //if (xPositionMin == -6)
            //{
            //    xPositionMin = e.transform.position.x - 3.5f;
            //}
            //else
            //{
            //    if (e.transform.position.x < xPositionMin)
            //    {
            //        xPositionMin = e.transform.position.x - 3.5f;
            //    }
            //}

        }
        // hien thi canh bao enemy level cao
        UIManager.ui.imgWarning.gameObject.SetActive(true);
        UIManager.ui.imgWarning.sprite = GamePlay.gameplay.warning[indexMaxLevelEnemy];
        UIManager.ui.textLevelWarning.text = maxLevelEnemy.ToString();

        if (dirScaleToGen > 0)
        {
            UIManager.ui.imgWarning.rectTransform.anchoredPosition = new Vector3(0, 50, 0);
        }
        else
        {
            UIManager.ui.imgWarning.rectTransform.anchoredPosition = new Vector3(0, -170, 0);
        }


        Invoke("OffWarning", 2.5f);


        // GamePlay.gameplay.checkCallTeamRadiant = false;
        //checkCallTeam = false;
    }

    public void OffWarning()
    {
        UIManager.ui.imgWarning.gameObject.SetActive(false);
    }

    public void Gen1()
    {
        checkEndFight = false;

        GamePlay.gameplay.GenItemSkill();

        int indexTmp = (int)(expTmp / 18.5f);

        quatityManagerEnemy = indexEnemy = indexTmp + Random.Range(-indexTmp / 2, indexTmp / 2);
        if (indexEnemy <= 0)
        {
            quatityManagerEnemy = indexEnemy = 1;
        }
        //else if (indexEnemy > 5)
        //{
        //    levelAdd = indexEnemy - 5;
        //    quatityManagerEnemy = indexEnemy = 5;
        //}
        else if (indexEnemy > enemyToGen.Length)
        {
            levelAdd = indexEnemy - enemyToGen.Length;
            quatityManagerEnemy = indexEnemy = enemyToGen.Length;
        }
        else
        {
            levelAdd = 1;
        }
        GamePlay.gameplay.checkCallTeamRadiant = false;
        if (Random.Range(0, 100) > 50)
        {
            dirScaleToGen = -1;
        }
        else
        {
            dirScaleToGen = 1;
        }
        int[] temp = new int[0];
        do
        {
            int index = Random.Range(0, 5);
            if (System.Array.FindIndex(temp, t => t == index) < 0)
            {
                System.Array.Resize(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = index;
            }
        }
        while (temp.Length < indexEnemy);

        for (int i = 0; i < indexEnemy; i++)
        {
            Enemy e = enemy.Find(a => a.index == temp[i]);
            int rd = Random.Range(-1, levelAdd);
            if (rd < 0)
            {
                rd = 0;
            }
            levelAdd -= rd;
            e.gameObject.SetActive(true);
            e.property.level = rd + 1;

            //add list
            enemyIndex.Add(e.index);
            enemyLevel.Add(e.property.level);

            e.SetProperty();
            e.SetTranform();
        }

        expTmp += expIncrease;
        // GamePlay.gameplay.checkCallTeamRadiant = false;
        //checkCallTeam = false;
    }

    public void CallTeamEnemy(Transform target)
    {
        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (enemy[i].gameObject.activeInHierarchy)
        //        {
        //            enemy[i].target = target;
        //            enemy[i].Move();
        //        }
        //    }
        for (int i = 0; i < enemyToGen.Length; i++)
        {
            if (enemy[i].gameObject.activeInHierarchy)
            {
                enemy[i].target = target;
                enemy[i].Move();
            }
        }
        checkFight = true;
    }
    //private bool checkCallTeam;
    private bool checkEndFight;
    public void EndFight()
    {
        checkEndFight = true;
        GamePlay.gameplay.numberOfEnemyBeKilled += enemyIndex.Count;

        if (GamePlay.gameplay.numberOfEnemyBeKilled >= GamePlay.gameplay.enemyBeKilled[PlayerPrefs.GetInt("levelFireShield")])
        {
            PlayerPrefs.SetInt("levelFireShield", PlayerPrefs.GetInt("levelFireShield") + 1);
            if (PlayerPrefs.GetInt("levelFireShield") >= 3)
            {
                PlayerPrefs.SetInt("levelFireShield", 3);
            }
        }

        if (checkFight)
        {
            checkFight = false;
            UpgradeLevelOrRespawn();
            GamePlay.gameplay.GoldDevil();
            ClearList();
        }

        for (int i = 0; i < enemyToGen.Length; i++)
        {
            Hero h = GamePlay.gameplay.hero[i];
            h.move.checkCalled = false;
            h.move.checkMove = true;
            h.checkFindEnemy = false;
            h.move.SetScale();
        }

        GamePlay.gameplay.SetActionDefaul();
        GamePlay.gameplay.OffEffectCurse();

        if (!checkGen)
        {
            Invoke("Gen", timeToGen);
        }
        else
        {
            Invoke("Gen1", timeToGen);
        }
        //GamePlay.gameplay.RemoveItemSkill();
        xPositionMin = -6;
        xPositionMax = -6;
    }
    public void UpgradeLevelOrRespawn()
    {
        //print(enemyIndex[0]);
        //print(enemyIndex.Count);
        for (int i = 0; i < enemyIndex.Count; i++)
        {
            Hero herotmp = GamePlay.gameplay.hero[enemyIndex[i]];

            float tmpchechNec;
            if (GamePlay.gameplay.checkNecroman[enemyIndex[i]] < 0)
            {
                tmpchechNec = 0.5f;
            }
            else
            {
                tmpchechNec = 1;
            }
            GamePlay.gameplay.currentExp[enemyIndex[i]] += GamePlay.gameplay.expGain * enemy[enemyIndex[i]].property.exp * enemyLevel[i] * tmpchechNec;

            GamePlay.gameplay.exp += (int)(GamePlay.gameplay.expGain * enemy[enemyIndex[i]].property.exp * enemyLevel[i] * tmpchechNec);

            if (GamePlay.gameplay.currentExp[enemyIndex[i]] >= herotmp.property.exp)
            {
                int leveltoUp = (int)(GamePlay.gameplay.currentExp[enemyIndex[i]] / herotmp.property.exp);
                GamePlay.gameplay.currentExp[enemyIndex[i]] = GamePlay.gameplay.currentExp[enemyIndex[i]] - leveltoUp * herotmp.property.exp;
                if (!herotmp.property.checkDie)
                {
                    // + level               
                    herotmp.property.LevelUp(leveltoUp);
                }
                else
                {
                    // respawn                   
                    herotmp.gameObject.SetActive(true);
                    herotmp.property.Respawn();
                    GamePlay.gameplay.checkResurr[enemyIndex[i]] = 1;
                    //if(leveltoUp -1 <= 1)
                    //{
                    //    leveltoUp = 2;
                    //}
                    herotmp.property.LevelUp(leveltoUp - 1);
                    // herotmp.move.SetTransform();
                    herotmp.property.SetHpDefaulf();
                }
            }
        }
    }
    [HideInInspector]
    public bool checkDontMove;

    public void ClearList()
    {
        enemyIndex.Clear();
        enemyLevel.Clear();
    }

    private int indexNoFight;
    public void EndNoFight()
    {
        if (!checkEndFight)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                enemy[i].gameObject.SetActive(false);
            }
            ClearList();
            //GamePlay.gameplay.RemoveItemSkill();
            if (!checkGen)
            {
                Invoke("Gen", timeToGen);
            }
            else
            {
                Invoke("Gen1", timeToGen);
            }
        }
        indexNoFight++;
        if (indexNoFight >= GamePlay.gameplay.xAirPunchToUpGrade[PlayerPrefs.GetInt("levelAirPunch")])
        {
            PlayerPrefs.SetInt("levelAirPunch", PlayerPrefs.GetInt("levelAirPunch") + 1);
            if (PlayerPrefs.GetInt("levelAirPunch") >= 3)
            {
                PlayerPrefs.SetInt("levelAirPunch", 3);
            }
        }
        xPositionMin = -6;
        xPositionMax = -6;
    }
}
