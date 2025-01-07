using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public int price;
    public string itemName;
    public bool canDealDamage; // New property
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;
    public AttributesToChange attributesToChange = new AttributesToChange();
    public int amountToChangeAttributes;

    public bool UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            HealthBar playerHealth = GameObject.Find("MainCharacter").GetComponent<HealthBar>();
            if(playerHealth.health == playerHealth.maxHealth)
            {
                return false;
            }
            else
            {
                playerHealth.Health(amountToChangeStat);
                return true;
            }
        }
        return false;
    }
    public void UseItemDamage()
    {
        if (attributesToChange == AttributesToChange.damage)
        {
            PlayerAttack playerAttack = GameObject.Find("MainCharacter").GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                Debug.Log($"Adding damage: {amountToChangeAttributes}");
                playerAttack.AddDamage(amountToChangeAttributes);
            }
        }
    }
    public enum StatToChange
    {
        health,
        aman
    };

    public enum AttributesToChange
    {
        damage,
        achan
    };
}
