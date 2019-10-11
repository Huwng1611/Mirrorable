using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public static MapManager mapManager;
    public GameObject[] map;
    //[HideInInspector]
    public float xToGenMap = 42;
    public void Awake()
    {
        mapManager = this;
    }
    public void Start()
    {
       
    }
    void Update () {
		if(CameraFollow.camerafollow.transform.position.x > xToGenMap - 21)
        {
            GenMap();
        }
	}
    private int index = 0;
    public void GenMap()
    {      
        xToGenMap += 21f;
        map[index].transform.position = new Vector3(xToGenMap, 0, 0);
        index++;
        if (index >= 6)
        {
            index = 0;
        }
        //if (mapOlder == null)
        //{
        //    GameObject obj = Instantiate(ground[0], new Vector3(xToGenMap, 0, 0), Quaternion.identity);
        //    mapOlder = mapBetween;
        //    mapBetween = mapBetween2;
        //    mapBetween2 = currentMap;
        //    currentMap = obj;
        //    obj.transform.SetParent(transform);          
        //}
        //else
        //{
        //    GameObject obj = mapOlder;
        //    mapOlder = mapBetween;
        //    mapBetween = mapBetween2;
        //    mapBetween2 = currentMap;
        //    currentMap = obj;
        //    obj.transform.position = new Vector3(xToGenMap, 0, 0);
        //}        
    }  
}
