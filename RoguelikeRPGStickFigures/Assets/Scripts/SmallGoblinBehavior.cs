using UnityEngine;

public class SmallGoblinBehavior : MonoBehaviour
{
    public Transform EntityTransform;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Vector2 velocity = new Vector2();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
}
