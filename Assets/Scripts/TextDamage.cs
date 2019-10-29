using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamage : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Renderer mesh = textDamage.gameObject.GetComponent<MeshRenderer>();
        mesh.sortingLayerName = "BGabove";
        mesh.sortingOrder = 10;
        SetTextDamage();
        Destroy(gameObject, 1);
    }
    public void SetTextDamage()
    {
        textDamage.text = "-" + damage.ToString();
        textDamage.color = color;
    }
    [HideInInspector]
    public int damage;

    public TextMesh textDamage;
    [HideInInspector]
    public Color color;
    [HideInInspector]
    public float speed = 7.5f;
    [HideInInspector]
    public int dir = 1;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * speed * dir, transform.position.z);
    }
}
