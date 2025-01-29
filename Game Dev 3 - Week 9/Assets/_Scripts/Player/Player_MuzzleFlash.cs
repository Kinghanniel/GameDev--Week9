using System.Collections;
using UnityEngine;

namespace GameDevWithhanniel.Player
{
    public class Player_MuzzleFlash : MonoBehaviour
    {


        public IEnumerator ReturnToThePool()
        {
            yield return new WaitForSeconds(1);

            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(ReturnToThePool());

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player");
            }
        }
    }
}
