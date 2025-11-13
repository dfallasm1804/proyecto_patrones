using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour, IAtacar, IHandleTrigger
{
    
    [Header("Atacar")]
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private int _bulletSpeed;
    [SerializeField] private int _bulletDamage;
    public Collider2D attackUnlock;
    private bool hasAttack;

    
    // IAtacar
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && hasAttack)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 direction = (mousePos - transform.position).normalized;
        
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * _bulletSpeed;
        Destroy(bullet, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "AttackUnlock")
        {
            Debug.Log("Attack Unlock");
            hasAttack = true;
        }
    }
}
