using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform tr = GetComponent<Transform>();

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Left pressed");
            Vector3 move = new Vector3(0.03f, 0, 0);
            tr.Translate(move);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Right pressed");
            Vector3 move = new Vector3(-0.03f, 0, 0);
            tr.Translate(move);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up pressed");
            Vector3 move = new Vector3(0, 0, -0.03f);
            tr.Translate(move);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Down pressed");
            Vector3 move = new Vector3(0, 0, 0.03f);
            tr.Translate(move);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A pressed for rotate left");
            tr.Rotate(Vector3.up);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S pressed for rotate right");
            tr.Rotate(-Vector3.up);

        }
    }

    public void GetPosition(ref Vector3 pos, ref Quaternion dir)
    {
        Transform transform = GetComponent<Transform>();

        pos = transform.position;
        dir = transform.rotation;

        return;
    }
}