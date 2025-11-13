using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IHandleTrigger
{
    private int _bulletDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.TakeDamage(_bulletDamage);
            
            Destroy(gameObject);
        }
    } 
}
