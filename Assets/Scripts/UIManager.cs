using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager ui;
    public Text textGold;

    public Image[] imgSkill;
    public Button[] btnSkill;

    public Text textWarning;

    public Button[] options;
    void Awake()
    {
        ui = this;
    }
    public GameObject swipeToStart;
    private void Start()
    {
        SetTextGold();
    }
    public void SetTextGold()
    {
        textGold.text = GamePlay.gameplay.gold.ToString();
    }
    public void CantJmp()
    {
        textWarning.text = "UNABLE TO JUMP HERE";
        textWarning.enabled = true;
        Invoke("OffWarning", 0.75f);
    }
    public void CantFlip()
    {
        textWarning.text = "UNABLE TO FLIP HERE";
        textWarning.enabled = true;
        Invoke("OffWarning", 0.75f);
    }
    public void OffWarning()
    {
        CancelInvoke("OffWarning");
        textWarning.enabled = false;
    }
    [HideInInspector]
    public int amountSkill;
    int j;
    public void AddSkill(int i)
    {
        if (amountSkill < 5)
        {
            for (j = 0; j < 5; j++)
            {
                if (imgSkill[j].enabled == false)
                {
                    break;
                }
            }
            if (j < 5)
            {
                amountSkill++;
                btnSkill[j].onClick.RemoveAllListeners();
                if (i == 0)
                {
                    btnSkill[j].onClick.AddListener(AirPunch);
                }
                else if (i == 1)
                {
                    btnSkill[j].onClick.AddListener(Shield);
                }
                else if (i == 2)
                {
                    btnSkill[j].onClick.AddListener(Thunder);
                }
                else if (i == 3)
                {
                    btnSkill[j].onClick.AddListener(Bloodlust);
                }
                else if (i == 4)
                {
                    btnSkill[j].onClick.AddListener(Curse);
                }
                else if (i == 5)
                {
                    btnSkill[j].onClick.AddListener(ArmorPiercing);
                }
                else if (i == 6)
                {
                    btnSkill[j].onClick.AddListener(Wind);
                }
                else if (i == 7)
                {
                    btnSkill[j].onClick.AddListener(Resurrection);
                }
                else
                {
                    btnSkill[j].onClick.AddListener(FireShield);
                }
                imgSkill[j].sprite = GamePlay.gameplay.sprSkill[i];
                imgSkill[j].enabled = true;
                if (j == 0)
                {
                    btnSkill[j].onClick.AddListener(delegate () { RemoveSkill(0); });
                }
                else if (j == 1)
                {
                    btnSkill[j].onClick.AddListener(delegate () { RemoveSkill(1); });
                }
                else if (j == 2)
                {
                    btnSkill[j].onClick.AddListener(delegate () { RemoveSkill(2); });
                }
                else if (j == 3)
                {
                    btnSkill[j].onClick.AddListener(delegate () { RemoveSkill(3); });
                }
                else
                {
                    btnSkill[j].onClick.AddListener(delegate () { RemoveSkill(4); });
                }
            }
        }
    }
    void AirPunch()
    {
        GamePlay.gameplay.AirPunch();
    }
    void Shield()
    {
        GamePlay.gameplay.Shield();
    }
    void Thunder()
    {
        GamePlay.gameplay.Thunder();
    }
    void Bloodlust()
    {
        GamePlay.gameplay.Bloodlust();
    }
    void Curse()
    {
        GamePlay.gameplay.Curse();
    }
    void ArmorPiercing()
    {
        GamePlay.gameplay.ArmorPiercing();
    }
    void Wind()
    {
        GamePlay.gameplay.Wind();
    }
    void Resurrection()
    {
        GamePlay.gameplay.Resurrection();
    }
    void FireShield()
    {
        GamePlay.gameplay.FireShield();
    }

    public void RemoveSkill(int k)
    {
        imgSkill[k].enabled = false;
        amountSkill--;
    }
    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Share()
    {
        FBAnalyticManager.instance.ShareOnFB();
    }
    public void Rate()
    {

    }

    public GameObject gameover;
    public Text textHighScore;
    public Text textScore;
    public Text textExp;
    public Text textCoin;

    public void ResetGameIfWatchVideoAds()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            foreach (var obj in options)
            {
                obj.gameObject.SetActive(true);
            }
            options[0].onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Menu");
            });
            options[1].onClick.AddListener(() =>
            {
                AdmobManager.instance.DisplayVideoAd();
                foreach (var obj in options)
                {
                    obj.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        ////test
        //if (Application.internetReachability != NetworkReachability.NotReachable)
        //{
        //    AdmobManager.instance.DisplayBanner();
        //    Debug.Log("Show Ad here");
        //}
        //endtest
        Time.timeScale = 0;
        GamePlay.gameplay.score = GamePlay.gameplay.gold + (int)CameraFollow.camerafollow.transform.position.x;
        textScore.text = GamePlay.gameplay.score.ToString();
        if (GamePlay.gameplay.score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", GamePlay.gameplay.score);
        }
        textHighScore.text = PlayerPrefs.GetInt("highScore").ToString();

        textExp.text = "+" + GamePlay.gameplay.exp.ToString() + " exp";
        PlayerPrefs.SetInt("exp", PlayerPrefs.GetInt("exp") + GamePlay.gameplay.exp);

        int expToLevelUp = 0;
        // level hien thi se lon hon luu tri 1
        if (PlayerPrefs.GetInt("level") == 0)
        {
            expToLevelUp = 1000;
        }
        else
        {
            expToLevelUp = 2000 * (int)Mathf.Pow(2, PlayerPrefs.GetInt("level"));
        }

        if (PlayerPrefs.GetInt("exp") >= expToLevelUp)
        {
            PlayerPrefs.SetInt("exp", 0);
            LevelUp();
        }
        else
        {
            gameover.SetActive(true);
        }
        textCoin.text = GamePlay.gameplay.gold.ToString();

        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("coin") + GamePlay.gameplay.gold);


    }
    public GameObject levelUp;
    public Text textLevel;
    public Text textCoinLevelUp;
    public Text textDiamondLevelUp;

    public void LevelUp()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        textLevel.text = (PlayerPrefs.GetInt("level") + 1).ToString();

        textCoinLevelUp.text = (PlayerPrefs.GetInt("level") * 1000).ToString();
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("coin") + GamePlay.gameplay.gold);

        textDiamondLevelUp.text = PlayerPrefs.GetInt("level").ToString();
        PlayerPrefs.SetInt("diamond", PlayerPrefs.GetInt("diamond") + GamePlay.gameplay.gold);

        levelUp.SetActive(true);
    }
    public void Collect()
    {
        levelUp.SetActive(false);
        gameover.SetActive(true);
    }

    public Image imgWarning;
    public Text textLevelWarning;
}
