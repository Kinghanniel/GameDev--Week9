using UnityEngine;

namespace GameDevWithhanniel.Player
{
    public class Player_GunRotation : MonoBehaviour
    {
        private Vector2 mousePosition;
        public Transform target; // Target position for the gun 
        public SpriteRenderer gunSpriteRenderer; // Reference to the gun's SpriteRenderer

        [SerializeField] private Player_Movement playerMovementRef; // Reference to check facing direction

        void Update()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunRotation();
            GunPosition();
        }

        void GunRotation()
        {
            // Calculate the direction vector from the gun to the mouse position
            Vector2 lookDirection = mousePosition - (Vector2)target.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            // Check player's facing direction
            if (playerMovementRef.facingRight)
            {
                // Player facing right
                transform.rotation = Quaternion.Euler(0, 0, angle);
                gunSpriteRenderer.flipY = false; // Ensure sprite is not flipped
            }
            else
            {
                // Player facing left
                transform.rotation = Quaternion.Euler(0, 0, angle);
                gunSpriteRenderer.flipY = true; // Flip the sprite vertically
            }
        }

        void GunPosition()
        {
            // Update the position of the gun to match the target
            transform.position = target.position;
        }
    }
}
