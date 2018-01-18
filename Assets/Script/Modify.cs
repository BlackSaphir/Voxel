using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    Vector2 rot;
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit, new BlockAir());
            }

        }
        rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3,
                          rot.y + Input.GetAxis("Mouse Y") * 3);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");

#endif


#if UNITY_IPHONE


        int TouchID = -1;

        //Touch[]. myTouches = Input.touches;
        
        if (Input.touchCount > 0)
	    {
            foreach (Touch myTouch in Input.touches)
            {
                if (myTouch.phase == TouchPhase.Began)
                {
                    TouchID = myTouch.fingerId;
                }

                if (myTouch.phase != TouchPhase.Ended && myTouch.fingerId == TouchID)
                {
                    if (myTouch.position.x < Screen.width / 2) // Left side of screen
                    {
                        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                        transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
                    }

                    if (myTouch.position.x > Screen.width / 2) // Right side of screen
                    {

                    }
                }
            }
	    }



#endif

    }
}
