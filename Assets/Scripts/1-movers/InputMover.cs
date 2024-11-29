using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This component moves its object when the player clicks the arrow keys.
 */
public class InputMover : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 10f;

    [SerializeField] string triggeringTagFloor;  // For the walls
    [SerializeField] string triggeringTagCeiling;  // For the walls
    [SerializeField] string triggeringTagRight;  // For the walls
    [SerializeField] string triggeringTagLeft;  // For the walls
    private bool HitFloor = false;
    private bool HitCeiling = false;
    private bool HitRight = false;
    private bool HitLeft = false;

    [SerializeField]
    InputAction move = new InputAction(
        type: InputActionType.Value, expectedControlType: nameof(Vector2));

    void OnEnable()
    {
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
    }

    void Update()
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;
        if (HitFloor && moveDirection.y < 0)
        {
            movementVector = new Vector3(moveDirection.x, 0, 0) * speed * Time.deltaTime;
        }
        if (HitCeiling && moveDirection.y > 0)
        {
            movementVector = new Vector3(moveDirection.x, 0, 0) * speed * Time.deltaTime;
        }
        if (HitRight)
        {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (HitLeft)
        {
            transform.position = new Vector3(10, transform.position.y, 0);
        }
        transform.position += movementVector;
        //transform.Translate(movementVector);
        // NOTE: "Translate(movementVector)" uses relative coordinates - 
        //       it moves the object in the coordinate system of the object itself.
        // In contrast, "transform.position += movementVector" would use absolute coordinates -
        //       it moves the object in the coordinate system of the world.
        // It makes a difference only if the object is rotated.
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == triggeringTagFloor && enabled)
        {
            HitFloor = true;
        }
        if (other.tag == triggeringTagCeiling && enabled)
        {
            HitCeiling = true;
        }
        if (other.tag == triggeringTagRight && enabled)
        {
            HitRight = true;
        }
        if (other.tag == triggeringTagLeft && enabled)
        {
            HitLeft = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == triggeringTagFloor && enabled)
        {
            HitFloor = false;
        }
        if (other.tag == triggeringTagCeiling && enabled)
        {
            HitCeiling = false;
        }
        if (other.tag == triggeringTagRight && enabled)
        {
            HitRight = false;
        }
        if (other.tag == triggeringTagLeft && enabled)
        {
            HitLeft = false;
        }
    }
}
