using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public CharacterInputController PlayerController;
    [SerializeField] private float speedModifier = 2;
    [SerializeField] private CombatantBehavior combatantRef;

    private Vector2 velocity;
    void Start()
    {
        PlayerController.onMoveLeft += OnMoveLeft;
        PlayerController.onMoveRight += OnMoveRight;
        PlayerController.onStopMoving += OnStopMoving;
        combatantRef.combatant.OnAttackTriggered += TriggerAttack;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(velocity.x,velocity.y,0) * Time.deltaTime * speedModifier;
    }

    void OnMoveRight(float val)
    {
        velocity.x = val;
    }
    void OnMoveLeft(float val)
    {
        velocity.x = -val;
    }
    void OnStopMoving()
    {
        velocity = Vector2.zero;
    }
    public void TriggerAttack(AttackDataScriptableObject attack)
    {

        var animData = combatantRef.combatant.CombatData.GetAnimationInfo(attack);
        switch (animData.ParameterType)
        {
            case CombatantData.AttackDataAnimOverrideWrapper.AnimationParameterType.TRIGGER:
                PlayerController.animController.CharacterAnimator.SetTrigger(animData.AnimationParameter);
                break;
            case CombatantData.AttackDataAnimOverrideWrapper.AnimationParameterType.BOOL:
                PlayerController.animController.CharacterAnimator.SetBool(animData.AnimationParameter, true);
                break;
        }
        //var anim = animator.GetCurrentAnimatorStateInfo(0);
    }

}
