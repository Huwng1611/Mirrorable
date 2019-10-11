using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public SpriteRenderer[] itemTopBehind;
    public SpriteRenderer[] itemTop;

    public SpriteRenderer[] itemBodBehind;
    public SpriteRenderer[] itemBod;

    public void Start()
    {
      
    }
    public void SetTransform(Transform obj,float y)
    {
        obj.localPosition = new Vector3(Random.Range(-10.5f,10.5f),y,0);
    }
}
