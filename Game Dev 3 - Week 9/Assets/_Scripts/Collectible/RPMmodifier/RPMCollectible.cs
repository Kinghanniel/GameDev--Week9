using GameDevWithhanniel.Player;
using UnityEngine;

public class RPMCollectible : MonoBehaviour
{
    public float rpmBoost = 200f; // Increase RPM
    public float duration = 5f; // Boost duration

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var shootingScript = other.GetComponent<Player_Shooting>();
            if (shootingScript != null)
            {
                shootingScript.ModifyRPM(rpmBoost, duration);
            }

            Destroy(gameObject); // Destroy collectible
        }
    }
}
