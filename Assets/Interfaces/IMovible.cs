using UnityEngine;
using UnityEngine.InputSystem;

public interface IMovible
{
    public void Move(InputAction.CallbackContext context);
}
