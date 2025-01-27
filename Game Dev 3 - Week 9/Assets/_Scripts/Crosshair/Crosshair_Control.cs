using UnityEngine;

namespace GameDevWithhanniel.Crosshair
{
    public class Crosshair_Control : MonoBehaviour
    {
        private Vector2 mousePosition;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;     //Sets the cursor to be invisible

        }

        // Update is called once per frame
        void Update()
        {
            SetCrosshairPosition();

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = false;     //Sets the cursor to be invisible

            }
        }

        private void SetCrosshairPosition()
        {
            //Gets the mouse poistion in world points
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Moves the position of whatever this script is in to the mouse position
            transform.position = mousePosition;
        }


    }
}

