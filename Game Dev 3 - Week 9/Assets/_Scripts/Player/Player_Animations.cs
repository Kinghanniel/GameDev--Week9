using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevWithNey.Player
{

    public class Player_Animations : MonoBehaviour
    {

        Animator anim;
        Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));   //Will allow the animation to go to the movement one and back to idle. The Mathf.Abs will turn the number 
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                anim.SetTrigger("landed");
            }
        }


    }

}

