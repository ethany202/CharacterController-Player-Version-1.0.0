using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public float maxHealth = 100f;
    public Image healthBar;
    public float characterCurrentHealth;
    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        characterCurrentHealth = player.GetHealth();
        healthBar.fillAmount = characterCurrentHealth / maxHealth;
    }
}
