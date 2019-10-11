using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
  
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public int[] XToUpgradeAirPunch;
    public int[] XDiamondToUpgradeShield;
    public int[] XToUpgradeThunder;
    public int[] XDiamondToUpgradeBloodlust;
    public int[] XDiamondToUpgradeCurse;
    public int[] XDiamondToUpgradeArmorPercing;
    public int[] XDiamondToUpgradeWind;
    public int[] XToUpgradeResurrection;
    public int[] XToUpgradeFireShield;
    public int[] XToUpgradeGoldDevil;
    public int[] XToUpgradeBigTroop;
    public int[] XToUpgradeArchNecromancer;
}
