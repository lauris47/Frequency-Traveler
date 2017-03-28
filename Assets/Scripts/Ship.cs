﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //public Vector3 myRotation;
    public int drag;
    public static bool drowning, shipReset;
    //public bool drown;
    bool drowningInWater;
    Collision collision;
    private float shipDrownTime, timer;
    public int lives;

    void Awake()
    {
        lives = GameManager.instance.getLives();
    }

    void Update()
    {
        timer = Time.time;

        if (drowning && shipDrownTime + 3 < timer)
        {
            respawnShip();
        }
    }

    void LateUpdate()
    {
        //drowning = drown;
        //To move the ship down the wave
        if (!drowning && collision != null && collision.gameObject.tag == "Wave")
        {

            //To controll ship with buttons, this overrides ship naturaly sliding down the wave
            if (Input.GetKey(KeyCode.D))
            {
                //ORG
                GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 5, 0.2f), 0, 0);


            }
            if (Input.GetKey(KeyCode.A))
            {

                //GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, -5, 0.2f), 0, 0);
                GetComponent<Rigidbody>().velocity = gameObject.transform.right*-1;
            }
        }


        //Drown 
        if (transform.localEulerAngles.z < 295 && transform.localEulerAngles.z > 45 && !drowning || collision != null && collision.gameObject.tag == "Rock" && !drowning)
        {
            if(collision != null && collision.gameObject.tag == "Rock" && !drowning)
            {
                GameManager.instance.setScore(-50);
            }
            drowning = true;
            gameObject.GetComponent<Rigidbody>().drag = drag * 4;
            shipDrownTime = timer;
        }
        else
        {
            //GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().drag = drag;
        }

        /*
        if (drowning)
        {
            if (collision != null && collision.gameObject.tag == "Wave")
            {
                //gameObject.GetComponent<Rigidbody>().drag = drag * 4;
                //gameObject.GetComponent<Rigidbody>().angularDrag = 1;
            }

        }
        else
        {
            foreach (Collider colider in GetComponents<Collider>())
            {
                colider.enabled = true;
            }
        }

        */

        //Respawn ship
        if (transform.position.y < -7 || transform.position.x < -11 || transform.position.x > 10 || Input.GetKey(KeyCode.R))
        {
            respawnShip();
        }
        else
        {
            shipReset = false;
        }
    }

    void respawnShip()
    {
            transform.position = new Vector3(-6.87f, 2f, 0.7f);
            transform.rotation = Quaternion.identity;
            drowning = false;
            shipReset = true;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //drown = false;
    }

    void OnCollisionEnter(Collision collision)
    {

        this.collision = collision;


        if (!Input.GetKeyDown(KeyCode.D) || !Input.GetKeyDown(KeyCode.A))
        {
            if (!drowning && collision != null && collision.gameObject.tag == "Wave")
            {

                //transform.forward()
                //print(" Rotation " + transform.localEulerAngles.z);
                
                if (transform.localEulerAngles.z > 295)
                {
                    // ORG // GetComponent<Rigidbody>().velocity = new Vector3((360 - transform.localEulerAngles.z) / 10, 0, 0);
                    GetComponent<Rigidbody>().velocity = gameObject.transform.right  * 2;
                }
                if (transform.localEulerAngles.z < 45)

                {
                    // ORG // GetComponent<Rigidbody>().velocity = new Vector3((transform.localEulerAngles.z - 45) / 20, 0, 0);
                    GetComponent<Rigidbody>().velocity = gameObject.transform.right * -1 * 2;
                }
            }
        }


        if (drowning && collision.gameObject.tag == "Wave")
        {
            //Sink slowly, by setting drag to very high when in water and drowning

            if (collision.collider.bounds.Contains(transform.position))
            {
                //print("point is inside collider");
            }

            gameObject.GetComponent<Rigidbody>().drag = drag * 2;
            gameObject.GetComponent<Rigidbody>().angularDrag = 0.4f;
            print("Drownging and colliding with " + collision.gameObject);
        }

        /*
        if (collision.gameObject.tag == "Rock")
        {
            drowning = true;
            gameObject.GetComponent<Rigidbody>().drag = drag * 4;
        }
        */


            /*
            if(drowningInWater)
            for (int i = 0; i < 120; i++) {
                    if (collision.collider.bounds.Contains())
                    {

                    }
            }*/

            //Sink slowly, by setting drag to very high when in water and drowning
            /*
            if (collision.collider.bounds.Contains(transform.position))
            {
                print("point is inside collider");
            }

            if (collision.gameObject.tag == "Bird")
            {



            }
            */
            // }

        }
}
