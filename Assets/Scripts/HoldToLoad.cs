using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldToLoad : MonoBehaviour
{
    private float _holdDuration = 1f;
    private float _holdTimer = 0;
    private bool _holding = false;

    public static event Action OnHoldComplete;

    // Update is called once per frame
    void Update()
    {
        if (_holding)
        {
            _holdTimer += Time.deltaTime;

            if (_holdTimer >= _holdDuration)
            {
                OnHoldComplete.Invoke();
                ResetHold();
            }
        }        
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _holding = true;
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        _holdTimer = 0f;
        _holding = false;
    }
}
