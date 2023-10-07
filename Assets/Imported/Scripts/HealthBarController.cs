using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private MicroBar hpBar;

    [SerializeField] bool randomDamageAndHeal;
    [SerializeField] float damageHealAmount;
    [SerializeField] Vector2 damageHealRandomRange;
    [SerializeField] bool enableDebugMessages;
    readonly float maxHP = 100;
    float hp = 100;
    void Start()
    {
        if (hpBar != null) hpBar.Initialize(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount)
    {
        // Deal damage
        hp -= amount;
        if (hp < 0) hp = 0;
        // destroy tank

        // Update HealthBar
        if (hpBar != null) hpBar.UpdateHealthBar(hp);
    }

    public void Heal(float amount)
    {
        // Heal
        hp += amount;
        if (hp > maxHP) hp = maxHP;
        // gain life

        // Update HealthBar
        if (hpBar != null) hpBar.UpdateHealthBar(hp);
    }
}
