using System;
using System.Collections;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public Action OnProjectileHit;
    [HideInInspector] public Vector3 Velocity;
    [HideInInspector] public Combatant Target;
    private Transform preFiredFollowTransform;
    private bool isFired = false;

    public virtual void Initialize(Vector3 velocity, Combatant target, Transform followUntilFired)
    {
        Velocity = velocity;
        Target = target;
        preFiredFollowTransform = followUntilFired;
    }

    public virtual void FireProjectile()
    {
        isFired = true;
    }

    private void Update()
    {
        if (isFired)
        {
            transform.position += Velocity * Time.deltaTime;
            return;
        }

        transform.position = preFiredFollowTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var cb = other.GetComponent<CombatantBehavior>();
        if (cb == null)
            return;
        var combatant = cb.combatant;
        if (combatant == null || Target != combatant)
            return;
        OnProjectileHit?.Invoke();
        StartCoroutine(HoldForAFewFrames());
    }

    private IEnumerator HoldForAFewFrames()
    {
        for (int i = 0; i < 5; i++)
            yield return null;
        DestroyImmediate(gameObject);
    }
}