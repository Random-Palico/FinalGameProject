using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTakeDamage
{
    void TakeDamage (int damage, Vector2 force, GameObject instigator);
}
