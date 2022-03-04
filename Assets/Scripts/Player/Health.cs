using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //TODO Cambiar a script de stats en general
    public float health = 100f;
    public Text displayHealth;

    void Start()
    {
        displayHealth.text = "Health: " + health;
    }

    private void FixedUpdate()
    {
        displayHealth.text = "Health: " + health;
    }
}
