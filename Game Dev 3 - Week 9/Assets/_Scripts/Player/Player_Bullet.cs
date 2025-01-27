using GameDevWithhanniel.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevWithhanniel.Player
{
    public class Player_Bullet : MonoBehaviour
    {

        private void Start()
        {
            // Ignore the collision between the bullet and player at the start
            int playerLayer = LayerMask.NameToLayer("Player");
            int bulletLayer = gameObject.layer;

            Physics2D.IgnoreLayerCollision(playerLayer, bulletLayer);
        }

        private void Update()
        {
            StartCoroutine(deactivatebullet());  
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // To get where specifically I have collided 
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 collisionPoint = contact.point; // Add a cool muzzleflash in collision
                float randomRange = Random.Range(0.5f, 1.5f);

                // Only spawn a muzzle flash at the collision point if it's not already deactivated
                var flashObj = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.MuzzleFlash);
                if (flashObj != null)
                {
                    flashObj.transform.localScale = new Vector3(randomRange, randomRange, randomRange);
                    flashObj.transform.position = collisionPoint;

                    var flashScript = flashObj.GetComponent<Player_MuzzleFlash>();
                    StartCoroutine(flashScript.ReturnToThePool());
                }
            }
            // Returns the bullet object to the available pool
            gameObject.SetActive(false);

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player");
            }
        }
        IEnumerator deactivatebullet()
        {
            yield return new WaitForSeconds(4);
            gameObject.SetActive(false); 
        }
    }
}


