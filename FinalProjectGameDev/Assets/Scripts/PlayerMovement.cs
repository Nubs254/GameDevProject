using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float jumpForce = 10f;
    public bool facingRight = true;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void MoveCreature(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    public void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

   public bool IsGrounded()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitMovingPlatform = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("MovingPlatforms"));
        
        return hitGround.collider != null || hitMovingPlatform.collider != null;
    }

    public bool IsTouchingWall()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x + 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hitLeft = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x + 0.1f, LayerMask.GetMask("Ground"));
        return (hitRight.collider != null || hitLeft.collider != null);
    }

    public bool CanJump()
    {
        bool grounded = IsGrounded();
        bool touchingWall = IsTouchingWall();

        // Allow jumping only if both grounded and touching a wall
        if(grounded&&touchingWall){
            return true;
        }else if((grounded==false)&&(touchingWall==true)){
            return false;
        }else if((grounded==true)&&(touchingWall==false)){
            return true;
        }else if((grounded==false)&&(touchingWall=false)){
            return false;
        }
        return false;
    }

    private void WhenGrounded(Transform ground){
        transform.SetParent(ground);
    }
}
