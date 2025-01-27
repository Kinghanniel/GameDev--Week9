using UnityEngine;
using GameDevWithhanniel.CameraStuff;
using System.Collections;
using GameDevWithhanniel.DesignPattern;

namespace GameDevWithhanniel.Player
{
    public class Player_Shooting : MonoBehaviour
    {
        [SerializeField] Transform tipOfTheBarrel;
        [SerializeField] Transform ejectionPort;
        [SerializeField] float defaultRPM = 600f; // Default Rounds Per Minute
        [SerializeField] float minBulletSpeed = 10f; // Minimum bullet speed
        [SerializeField] float maxBulletSpeed = 50f; // Maximum bullet speed
        [SerializeField] float pushBackForce;
        [SerializeField] GameEvent bulletShot;
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] ParticleSystem sparks;

        private float currentRPM;
        private float fireRate; // Time between shots (calculated from RPM)
        private float bulletSpeed; // Calculated based on RPM
        private float lastFireTime;

        Player_Movement playerMovementRef;
        Rigidbody2D rb;

        private void Start()
        {
            Cursor.visible = false;
            playerMovementRef = GetComponent<Player_Movement>();
            rb = GetComponent<Rigidbody2D>();

            currentRPM = defaultRPM;
            UpdateFireRateAndBulletSpeed();
        }

        private void Update()
        {
            // Handle firing with cooldown
            if (Input.GetButton("Fire1") && Time.time >= lastFireTime + fireRate)
            {
                Fire();
                lastFireTime = Time.time; // Update the last fire time
            }
        }

        void Fire()
        {
            // Spawn the bullet
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletePool);

            if (spawnedBullet != null)
            {
                spawnedBullet.transform.position = tipOfTheBarrel.transform.position;
                RandomiseBulletSize(spawnedBullet);

                // Fire bullet
                Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
                FireBulletInRightDirection(bulletsRb);
            }

            // Pushback
            PushBack();

            // Raise the event
            bulletShot.Raise();

            // Fire ripple effect
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);

            // Handle muzzle flash and sparks
            MuzzleFlashLogic();
            sparks.Play();
        }

        void MuzzleFlashLogic()
        {
            var muzzleFlashObject = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.MuzzleFlash);
            if (muzzleFlashObject != null)
            {
                float randomValue = Random.Range(0.8f, 1.25f);
                muzzleFlashObject.transform.localScale = new Vector3(randomValue, randomValue, randomValue);

                muzzleFlashObject.transform.position = tipOfTheBarrel.position;
                muzzleFlashObject.transform.rotation = tipOfTheBarrel.rotation;

                var muzzleFlashScript = muzzleFlashObject.GetComponent<Player_MuzzleFlash>();
                StartCoroutine(muzzleFlashScript.ReturnToThePool());
            }
        }

        private void FireBulletInRightDirection(Rigidbody2D bulletsRb)
        {
            Vector2 shootDirection = tipOfTheBarrel.right;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bulletsRb.transform.rotation = Quaternion.Euler(0, 0, angle);

            bulletsRb.AddForce(shootDirection * bulletSpeed * 100); // Use calculated bulletSpeed
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

        // Adjust RPM and update fire rate and bullet speed
        public void ModifyRPM(float addedRPM, float duration)
        {
            StartCoroutine(BoostRPM(addedRPM, duration));
        }

        private IEnumerator BoostRPM(float addedRPM, float duration)
        {
            currentRPM += addedRPM;
            UpdateFireRateAndBulletSpeed(); // Recalculate fire rate and bullet speed
            yield return new WaitForSeconds(duration);
            currentRPM -= addedRPM;
            UpdateFireRateAndBulletSpeed(); // Revert to original values
        }

        private void UpdateFireRateAndBulletSpeed()
        {
            fireRate = 60f / currentRPM; // Convert RPM to time between shots
            bulletSpeed = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, currentRPM / 100f); // Scale bullet speed based on RPM
        }
    }
}
