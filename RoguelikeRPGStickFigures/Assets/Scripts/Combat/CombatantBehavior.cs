using System.Collections;
using UnityEngine;

public class CombatantBehavior : MonoBehaviour
{
    public Transform ProjectileSpawnPoint;
    public Combatant combatant;
    private bool projectileSpawnTrigger;
    private bool projectileFireTrigger;
    private void Awake()
    {
        combatant = new Combatant(combatant);
        combatant.OnAttackTriggered += SpawnProjectile;
    }
    public void TriggerDamageOutput()
    {
        combatant.TriggerDamage();
    }

    public void SpawnProjectile(AttackDataScriptableObject attack, Combatant target)
    {
        StartCoroutine(WaitForProjectileTrigger(attack, target));
    }

    private IEnumerator WaitForProjectileTrigger(AttackDataScriptableObject attack, Combatant target)
    {
        yield return new WaitUntil(()=>projectileSpawnTrigger);
        var projectile = Instantiate<BaseProjectile>(attack.ProjectilePrefab, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
        projectile.OnProjectileHit += TriggerDamageOutput;
        var velocity = (target.EntityTransformRef.position - projectile.transform.position).normalized * 6;
        projectile.Initialize(velocity,target,ProjectileSpawnPoint);
        projectileSpawnTrigger = false;
        yield return new WaitUntil(()=>projectileFireTrigger);
        projectile.FireProjectile();
        projectileFireTrigger = false;
    }

    public void InstantiateProjectile()
    {
        projectileSpawnTrigger = true;
    }

    public void FireProjectile()
    {
        projectileFireTrigger = true;
    }
}

