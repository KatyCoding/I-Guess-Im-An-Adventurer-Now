using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterInputController : MonoBehaviour
{
    
    InputAction move;
    public Action<float> onMoveRight;
    public Action<float> onMoveLeft;
    public Action onStopMoving;
    [SerializeField] public CharacterAnimationController animController;
    private void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        if(animController!=null)
        {
            onMoveRight += animController.OnWalkRight;
            onMoveLeft += animController.OnWalkLeft;
            onStopMoving += animController.OnStopMoving;
        }
    }
    private void Update()
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        if (moveDirection.x > 0)
        {
            onMoveRight.Invoke(moveDirection.x);
        }
        else if (moveDirection.x < 0)
        {
            onMoveLeft.Invoke(Mathf.Abs(moveDirection.x));
        }
        else { onStopMoving.Invoke(); }
    }
}
