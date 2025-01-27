using UnityEngine;
using GameDevWithNey.CameraStuff;
using GameDevWithNey.DesignPattern;


namespace GameDevWithNey.Player
{


    public class Player_Shooting : MonoBehaviour
    {
        [SerializeField] Transform tipOfTheBarrel;
        [SerializeField] Transform ejectionPort;
        [SerializeField] float bulletSpeed;
        Player_Movement playerMovementRef;
        Rigidbody2D rb;
        [SerializeField] float pushBackForce;
        [SerializeField] GameEvent bulletShot;
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] ParticleSystem sparks;

        private void Start()
        {
            Cursor.visible = false;
            playerMovementRef = GetComponent<Player_Movement>();
            rb = GetComponent<Rigidbody2D>();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }

        void Fire()
        {
            //Spawns the bullet
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletePool);

            //Make the bullet be in the right position
            if (spawnedBullet != null) spawnedBullet.transform.position = tipOfTheBarrel.transform.position;
   
            //Random bullet scale
            RandomiseBulletSize(spawnedBullet);
            //Fires the bullet
            Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
            FireBulletInRightDirection(bulletsRb);
            //Does a pushback
            PushBack();
            //Raises the event
            bulletShot.Raise();
            //Fires the ripple effect
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);
            MuzzleFlashLogic();
            //Plays the sparks particles
            sparks.Play();
        }

        void MuzzleFlashLogic()
        {
            //Muzzle flash code
            var muzzleFlashObject = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.MuzzleFlash);
            if (muzzleFlashObject != null)
            {
                float randomValue = Random.Range(0.8f, 1.25f);
                muzzleFlashObject.transform.localScale = new Vector3(randomValue, randomValue, randomValue);

                // Align the muzzle flash with the tip of the barrel
                muzzleFlashObject.transform.position = tipOfTheBarrel.position;
                muzzleFlashObject.transform.rotation = tipOfTheBarrel.rotation;

                var muzzleFlashScript = muzzleFlashObject.GetComponent<Player_MuzzleFlash>();
                StartCoroutine(muzzleFlashScript.ReturnToThePool());
            }
        }

        private void FireBulletInRightDirection(Rigidbody2D bulletsRb)
        {
            // Determine the direction of the shot based on the gun's rotation
            Vector2 shootDirection = tipOfTheBarrel.right; // Use the barrel's local right direction for the bullet's movement.

            // Rotate the bullet to face the shoot direction
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bulletsRb.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Apply force in the shoot direction
            bulletsRb.AddForce(shootDirection * bulletSpeed * 100);
        }


        private void RandomiseBulletSize(GameObject spawnedBullet)
        {
            float randomScaleValue = Random.Range(0.7f, 1.3f);
            spawnedBullet.transform.localScale = new Vector3(randomScaleValue, randomScaleValue, randomScaleValue);
        }

        public void PushBack()
        {
            if (playerMovementRef.facingRight == true)
            {
                rb.AddForce(Vector2.left * pushBackForce * 100);
            }
            else
            {
                rb.AddForce(Vector2.right * pushBackForce * 100);
            }
        }
    }


}

