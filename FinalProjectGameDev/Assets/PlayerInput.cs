using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement playerCreature;
    public float inputHorizontal;


    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        
         if(Input.GetKey(KeyCode.D)){
            input.x += 1;
        }
         if(Input.GetKey(KeyCode.A)){
            input.x += -1;
        }
        if (Input.GetKey(KeyCode.W) && playerCreature.canJump == true)
        {
            playerCreature.Jump();
            playerCreature.canJump = false;
            
        }
        if (inputHorizontal > 0 && !playerCreature.facingRight){
            playerCreature.Flip();
        }
        if (inputHorizontal < 0 && playerCreature.facingRight){
            playerCreature.Flip();
        }
        

        playerCreature.MoveCreature(input);
        
    }
    

}
