using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement playerCreature;
    [SerializeField] Grappling_Gun grapplingGun;
    public float inputHorizontal;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform currentMovingPlatform;
    private Vector3 lastPlatformPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Check if the player is currently grappling
        bool isGrappling = grapplingGun.grappleRope.enabled;

        // Check if the player can jump
        bool canJump = playerCreature.CanJump();

        


        // Allow jumping only if not grappling and can jump
        if (Input.GetKeyDown(KeyCode.W) && (!isGrappling && canJump == true))
        {
            playerCreature.Jump();
        }

        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x += -1;
        }

        if (inputHorizontal > 0 && !playerCreature.facingRight)
        {
            playerCreature.Flip();
        }
        if (inputHorizontal < 0 && playerCreature.facingRight)
        {
            playerCreature.Flip();
        }

        playerCreature.MoveCreature(input);

        // Move with the moving platform if standing on one
        if (currentMovingPlatform != null)
        {
            Vector3 platformMovement = currentMovingPlatform.position - lastPlatformPosition;
            transform.position += new Vector3(platformMovement.x, 0f, 0f);
        }
    }


}
