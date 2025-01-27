using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevWithhanniel.Player
{

    public class Player_Movement : MonoBehaviour
    {
        /// <summary>
        /// Video I used to get a juicy movement: https://www.youtube.com/watch?v=USLp-4iwNnQ
        /// Video I used to get a nice jump: https://www.youtube.com/watch?v=RPdn3r_tqcM
        /// </summary>



        //---
        //References variables
        //---
        Rigidbody2D rb;

        //---
        [Header("Movement Control Variables")]
        //---
        [Range(1f, 5f)]
        public float speed;
        [Range(1f, 20f)]
        public float jumpStrenght;
        Vector2 movementValue;
        public bool facingRight = true;


        //---
        [Header("Movement Juice Variables")]
        //---
        public float maxSpeed = 7f;
        public float newLinearDrag = 4f;

        //----
        [Header("Ground Check Variables")]
        //---
        public bool amIGrounded = true;
        public LayerMask whatIsGround;
        public Transform raycastLeftPos, raycastRightPos;
        public float rayLenght;


        //---
        [Header("Jump juice Variables")]
        //
        public float gravityValue = 1f;
        public float fallMultiplier = 5f;



        //-------
        //Built In Methods
        //-------

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            GetMovementValue();

            if (Input.GetButtonDown("Jump") && amIGrounded)
            {
                Jump();
            }

            GroundCheck();
        }

        private void FixedUpdate()
        {
            HorizontalMovement();
            ModifyPhysics();
        }






        //-------
        //Custom Methods
        //-------
        private void GetMovementValue()
        {
            movementValue = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void HorizontalMovement()
        {
            rb.AddForce(Vector2.right * movementValue.x * speed * 10);

            CheckIfIShouldFlip();

            LimitPlayerVelocity();
        }

        private void LimitPlayerVelocity()
        {
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }

        private void CheckIfIShouldFlip()
        {
            if ((movementValue.x > 0 && !facingRight) || (movementValue.x < 0 && facingRight))
            {
                Flip();
            }
        }

        void Flip()
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        }


        void ModifyPhysics()
        {
            //Checks if we are changing direction
            bool changingDirection = (movementValue.x > 0 && rb.velocity.x < 0) || (movementValue.x < 0 && rb.velocity.x > 0);


            if (amIGrounded)
            {
                //This if else statement adjusts the linear drag to make the
                //movement feel nice.
                if (Mathf.Abs(movementValue.x) < 0.4f || changingDirection)
                {
                    rb.drag = newLinearDrag;
                }
                else
                {
                    rb.drag = 0;
                }

                //This is to adjust the gravity
                rb.gravityScale = 0;
            }
            else
            {
                //All this else statement is to make the jump feel juicier
                rb.gravityScale = gravityValue;
                rb.drag = newLinearDrag * 0.15f;

                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravityValue * fallMultiplier;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.gravityScale = gravityValue * (fallMultiplier / 2);
                }
            }


        }
        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);                        //Stops all vertical velocity
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);        //Adds the force required to jump
        }



        private void GroundCheck()
        {
            //Checks if the raycasts on the left and right are touching ground
            bool isTouchingOnTheLeft = Physics2D.Raycast(raycastLeftPos.position, Vector2.down, rayLenght, whatIsGround);
            bool isTouchingOnTheRight = Physics2D.Raycast(raycastRightPos.position, Vector2.down, rayLenght, whatIsGround);

            RaycastVisualDebug(isTouchingOnTheLeft, isTouchingOnTheRight);


            //If you do not put == it will execute if true. This check is checking if one OR the other is true
            if (isTouchingOnTheLeft || isTouchingOnTheRight)
            {
                amIGrounded = true;
            }
            else
            {
                amIGrounded = false;
            }
        }


        private void RaycastVisualDebug(bool isTouchingOnTheLeft, bool isTouchingOnTheRight)
        {
            //Draws them so we can see them. Also changes the color if we are not grounded to red.
            if (isTouchingOnTheLeft)
            {
                Debug.DrawRay(raycastLeftPos.position, Vector2.down * rayLenght, Color.green);
            }
            else
            {
                Debug.DrawRay(raycastLeftPos.position, Vector2.down * rayLenght, Color.red);
            }
            if (isTouchingOnTheRight)
            {
                Debug.DrawRay(raycastRightPos.position, Vector2.down * rayLenght, Color.green);
            }
            else
            {
                Debug.DrawRay(raycastRightPos.position, Vector2.down * rayLenght, Color.red);
            }
        }
    }


}
