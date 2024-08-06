using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmallGoblinBehavior : MonoBehaviour
{
    public Transform EntityTransform;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Vector2 velocity = new Vector2();
    [SerializeField] private CombatantBehavior combatantRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatantRef.combatant.OnAttackTriggered += TriggerAttack;
        combatantRef.combatant.OnDeath += (a)=> {TriggerDeath();};
    }
    private float timer = 2;
    // Update is called once per frame
    void Update()
    {
        if (CombatMaster.instance.IsInCombat)
        {
            if (velocity.x != 0)
            {
                velocity.x = 0;
                animator.SetBool("Moving", false);
            }
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 2;
            velocity = new Vector2(Random.Range(-1, 2), 0);
        }
        EntityTransform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
        if (velocity.x > 0)
        {
            animator.SetBool("Moving", true);
            spriteRenderer.flipX = true;
        }
        else if (velocity.x < 0)
        {
            animator.SetBool("Moving", true);
            spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("Moving", false);
            spriteRenderer.flipX = false;
        }
    }
    public void TriggerAttack(AttackDataScriptableObject attack)
    {
/*
        var animData = combatantRef.combatant.CombatData.GetAnimationInfo(attack);
        switch(animData.ParameterType)
            {
            case CombatantData.AttackDataAnimOverrideWrapper.AnimationParameterType.TRIGGER:
                animator.SetTrigger(animData.AnimationParameter);
                break;
            case CombatantData.AttackDataAnimOverrideWrapper.AnimationParameterType.BOOL:
                animator.SetBool(animData.AnimationParameter, true);
                break;
        }
*/
        //var anim = animator.GetCurrentAnimatorStateInfo(0);

    }

    private void TriggerDeath()
    {
        animator.SetTrigger("Death");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        //TODO add multiple combatants in some encounters
        if (other.CompareTag("Player"))
        {
            List<Combatant> combatants = new List<Combatant>();
            combatants.AddRange(CombatMaster.GetPlayerTeam());
            combatants.Add(combatantRef.combatant);
            CombatMaster.instance.EnterCombat(combatants);
            
        }
        
    }
}
