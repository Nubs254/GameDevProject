using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
   
    [SerializeField] float speed = 3f;
    [SerializeField] float jumpForce = 10f;
    public enum CreatureMovementType { tf, physics};
    [SerializeField] string creatureName = "Frogman";
    [SerializeField] Vector3 homePosition= Vector3.zero;
    public bool canJump = true;
    public bool facingRight = true;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -7){
            SceneManager.LoadScene("StartMenu");
        }
    }
    public void MoveCreature(Vector3 direction){
        transform.position += direction * Time.deltaTime *speed;
    }

    public void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;

        gameObject.transform.localScale = currentScale;
        
        facingRight = !facingRight;
    }
    public void Jump(){
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        
    }
    public void OnCollisionEnter2D(Collision2D collision){
    if(collision.gameObject.tag == "Ground"){
        canJump = true;
    }
}
}
