using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : MonoBehaviour
{
    [SerializeField]
    private GameManager GM;
    [SerializeField]
    private BoxCollider Chick_BC;
    private void Awake()
    {
        GM= GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision Chick_BC)
    {
        if(Chick_BC.collider.tag=="Grass"|| Chick_BC.collider.tag == "Tomato"|| Chick_BC.collider.tag == "Cabbage")
        {
            Destroy(Chick_BC.gameObject);
            GM.CreateMeal();
        }
    }
}
