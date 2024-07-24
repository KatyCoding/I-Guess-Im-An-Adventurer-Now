using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator CharacterAnimator;
    public SpriteRenderer sprite;
    [Header("Anim Triggers")]   
    public string IsWalkingRightAnimCondition;
    public string IsWalkingLeftAnimCondition;
    public void Start()
    {
        
    }
    public void SetIsWalkingRight(bool isWalkingRight)
    {
        CharacterAnimator.SetBool(IsWalkingRightAnimCondition, isWalkingRight);
        
    }
    public void SetIsWalkingLeft(bool isWalkingLeft)
    {
        CharacterAnimator.SetBool(IsWalkingLeftAnimCondition, isWalkingLeft);
       
    }
    public void OnStopMoving()
    {
        SetIsWalkingLeft(false);
        SetIsWalkingRight(false);
    }
    public void OnWalkRight(float speed)
    {
        SetIsWalkingRight(speed > 0);
    }
    public void OnWalkLeft(float speed)
    {
        SetIsWalkingLeft(speed > 0);
    }
    public void FlipXOFF()
    {
        sprite.flipX = false;
    }
    public void FlipXON()
    {
        sprite.flipX = true;
    }
}
