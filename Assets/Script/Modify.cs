using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour
{
    Vector2 rot;
    public float speed = 0.1f;
    float LockZ = 0;

    // Update is called once per frame
#if UNITY_STANDALONE
   

    void Start()
    {
        Debug.Log("PC");
    }

    void Update()
    {

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
    }

#endif


#if UNITY_IOS

    Vector3 MovementVector = new Vector3(0, 0, 0);
    RaycastHit Penis_Hit;

    void Start()
    {
        Debug.Log("Ipad");
    }

    void Update()
    {
        //MovementVector += Physics.gravity;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (Input.GetTouch(0).position.x < Screen.width / 2)
            {
                MovePlayer();
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (Input.GetTouch(0).position.x > Screen.width / 2)
            {
                RotatePlayer();
            }
        }

        if (!this.GetComponent<CapsuleCollider>().enabled)
        {
            if (Physics.Raycast(this.transform.position, Vector3.down, out Penis_Hit, 10.0f))
            {
                if (Penis_Hit.collider.gameObject.tag == "Chunk")
                {
                    this.GetComponent<CapsuleCollider>().enabled = true;
                }
            }
        }


    }

    void MovePlayer()
    {
        MovementVector.x = Input.GetTouch(0).deltaPosition.x * Time.deltaTime;
        MovementVector.z = Input.GetTouch(0).deltaPosition.y * Time.deltaTime;
        transform.Translate(MovementVector);
        Debug.Log("MOVES");
    }

    void RotatePlayer()
    {
        Quaternion RotateFuckingUnity = Quaternion.Euler(-Input.GetTouch(0).position.y, Input.GetTouch(0).position.x, LockZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, RotateFuckingUnity, Time.deltaTime);
        transform.rotation = RotateFuckingUnity;
    }


#endif

    //#if UNITY_EDITOR


    //    Touch touch;

    //    void Start()
    //    {
    //        Debug.Log("PENIS");
    //    }

    //    void Update()
    //    {
    //        Vector3 MovementVector = new Vector3(0, 0, 0);

    //        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    //        {
    //            if (touch.position.x < Screen.width / 2)
    //            {

    //                MovementVector.x = Input.GetTouch(0).deltaPosition.x * Time.deltaTime;
    //                MovementVector.y = Input.GetTouch(0).deltaPosition.y * Time.deltaTime;
    //                transform.Translate(MovementVector);
    //                Debug.Log("MOVES");
    //            }
    //        }

    //        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    //        {
    //            Debug.Log(string.Format("X:{0}, Y:{1}", Input.GetTouch(0).deltaPosition.x, Input.GetTouch(0).deltaPosition.y));

    //            if (Input.GetTouch(0).position.x > Screen.width / 2)
    //            {
    //                Quaternion RotateFuckingUnity = Quaternion.Euler(-Input.GetTouch(0).position.y, Input.GetTouch(0).position.x, 0);
    //                //transform.Rotate(new Vector3(Input.GetTouch(0).position.y, Input.GetTouch(0).position.x) * Time.deltaTime);
    //                transform.rotation = Quaternion.Slerp(transform.rotation, RotateFuckingUnity, Time.deltaTime*2);

    //            }
    //        }

    //    }




    //#endif

}
