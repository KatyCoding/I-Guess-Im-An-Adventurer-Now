using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public CharacterInputController PlayerController;
    [SerializeField] private float speedModifier = 2;
    private Vector2 velocity;
    void Start()
    {
        PlayerController.onMoveLeft = OnMoveLeft;
        PlayerController.onMoveRight = OnMoveRight;
        PlayerController.onStopMoving = OnStopMoving; 
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
}
