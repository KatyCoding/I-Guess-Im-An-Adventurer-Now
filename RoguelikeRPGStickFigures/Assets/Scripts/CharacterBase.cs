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
        combatantRef.combatant.OnDeath += (a) => { PlayerController.animController.TriggerDeath(); };

    }

    // Update is called once per frame
    void Update()
    {
        combatantRef.combatant.EntityTransformRef.position += new Vector3(velocity.x,velocity.y,0) * Time.deltaTime * speedModifier;
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
        switch (animData.param.type)
        {
            case AnimatorControllerParameterType.Trigger:
                PlayerController.animController.CharacterAnimator.SetTrigger(animData.param.name);
                break;
            case AnimatorControllerParameterType.Bool:
                PlayerController.animController.CharacterAnimator.SetBool(animData.param.name, true);
                break;
        }
        
        //var anim = animator.GetCurrentAnimatorStateInfo(0);
    }

}
