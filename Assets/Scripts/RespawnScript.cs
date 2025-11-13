using System;
using System.Collections;
using Interfaces;
using UnityEngine;

public class RespawnScript : MonoBehaviour, ICheckpointMemento, IGroundcheck, IHandleTrigger
{
    [SerializeField] private GameObject _player;
    
    [SerializeField] private Transform _groundCheckPos;
    private Vector2 _groundCheckSize = new Vector2(0.8f, 0.1f);
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    
    private Vector2 _RespawnPosition;
    
    public static event Action OnLoadRespawn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RunSetRespawn());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded();
    }

    IEnumerator RunSetRespawn()
    {
        for (;;)
        {
            if (_isGrounded)
            {
                SetRespawn();
                Debug.Log("Respawn set");
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void IsGrounded()
    {
        if (Physics2D.OverlapBox(_groundCheckPos.position, _groundCheckSize, 0, _groundLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void SetRespawn()
    {
        _RespawnPosition = new Vector2(_groundCheckPos.position.x, _groundCheckPos.position.y);
    }

    private void LoadRespawn()
    {
        _player.transform.position = _RespawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "RespawnCollider")
        {
            Debug.Log("Respawn");
            LoadRespawn();
        }
    }
}
