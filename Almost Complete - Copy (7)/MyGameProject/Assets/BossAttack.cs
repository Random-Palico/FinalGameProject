using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int atkDamage = 2;
    public int atkDamage2P = 2;

    public Vector3 attackOffset; // Offset for the first attack
    public Vector3 attackOffset2P; // Offset for the second attack
    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 pos = GetAttackPosition();

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<HealthBar>().TakeDamage(atkDamage);
        }
    }

    public void Attack2P()
    {
        Vector3 pos = GetAttackPosition2P();

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<HealthBar>().TakeDamage(atkDamage2P);
        }
    }

    private Vector3 GetAttackPosition()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        return pos;
    }

    private Vector3 GetAttackPosition2P()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset2P.x; // Use the new offset for the second attack
        pos += transform.up * attackOffset2P.y;
        return pos;
    }

    private void OnDrawGizmos()
    {
        // Draw the attack range for the first attack
        Gizmos.color = Color.red; // Color for the attack range
        Vector3 attackPosition = GetAttackPosition(); // Calculate the attack position
        Gizmos.DrawWireSphere(attackPosition, attackRange); // Draw a wire sphere for the range

        // Draw the position of the first attack
        Gizmos.color = Color.green; // Color for the attack position
        Gizmos.DrawSphere(attackPosition, 0.1f); // Draw a small sphere at the attack position

        // Draw the attack range for the second attack
        Gizmos.color = Color.blue; // Color for the second attack range
        Vector3 attackPosition2P = GetAttackPosition2P(); // Calculate the second attack position
        Gizmos.DrawWireSphere(attackPosition2P, attackRange); // Draw a wire sphere for the second attack range

        // Draw the position of the second attack
        Gizmos.color = Color.yellow; // Color for the second attack position
        Gizmos.DrawSphere(attackPosition2P, 0.1f); // Draw a small sphere at the second attack position
    }
}