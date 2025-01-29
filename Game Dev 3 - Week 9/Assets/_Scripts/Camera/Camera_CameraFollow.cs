using UnityEngine;

namespace GameDevWithhanniel.CameraStuff
{



    public class Camera_CameraFollow : MonoBehaviour
    {
        /// <summary>
        /// Code for the smooth follow from: https://www.youtube.com/watch?v=_QnPY6hw8pA
        /// </summary>

        public Transform targetToFollow;        //This will be to store the camera target
        public Vector3 cameraOffset;            //This will make sure the camera does not overlap with the character
        public float smoothSpeed = 10f;         //To give a bit of delay to the camera. It will feel better.


        //Variables for the creation of boundaries
        public Vector3 minValue, maxValue;


        //To make sure it the camera does not jitter when the player moves
        private void FixedUpdate()
        {
            FollowTarget();
        }
        private void FollowTarget()
        {
            //To make sure that the camera will not overlap with the player
            Vector3 desiredPosition = targetToFollow.position + cameraOffset;


            Vector3 boundPosition = new Vector3(                            //We will create a new Vector 3 to set the bounds
                Mathf.Clamp(desiredPosition.x, minValue.x, maxValue.x),     //Clamp the new x value between the max x and min x
                Mathf.Clamp(desiredPosition.y, minValue.y, maxValue.y),     //Clamp the new y value between the max y and min y
                Mathf.Clamp(desiredPosition.z, minValue.z, maxValue.z));    //Clamp the new z value between the max z and min z



            //Will make the camera follow smoothly with some delay instead of being locked to the target
            //We have changed this to use the boundPosition as a target instead ot the desiredPosition
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, boundPosition, smoothSpeed * Time.deltaTime);
            //Actually moves the camera to the smoothedPosition Vector3 location
            transform.position = smoothedPosition;
        }
    }
}

