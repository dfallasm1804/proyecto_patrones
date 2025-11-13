using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;

    private int _maxHealth = 3;
    private int _currentHealth;
    private Rigidbody2D _rb;
    private bool isGrounded;
    private bool shouldJump;
    
    public int damage = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = _maxHealth;
        
        _player = GameObject.Find("PlayerCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        // is grounded?
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundLayer);
        
        // player direction
        float direction = Mathf.Sign(_player.transform.position.x - transform.position.x);
        
        // player above
        bool isPlayerAbove =  Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << _player.gameObject.layer);

        if (isGrounded)
        {
            //chase player
            _rb.linearVelocity = new Vector2(direction * _speed, _rb.linearVelocity.y);
            
            //jump if gap && no ground
            //else if player && platform above
            
            //if ground
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, _groundLayer);
            
            //if gap
            RaycastHit2D gapInFront = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, _groundLayer);
            
            // if platform above
            RaycastHit2D playerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, _groundLayer);

            if (!gapInFront.collider && !groundInFront.collider)
            {
                shouldJump = true;
            } else if (isPlayerAbove && playerAbove.collider)
            {
                shouldJump = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
              
            Vector2 direction = (_player.transform.position - transform.position).normalized;
              
            Vector2 jumpDirection = direction * _jumpForce;
              
            _rb.AddForce(new Vector2(jumpDirection.x, _jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
