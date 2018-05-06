using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackerController : MonoBehaviour {
    public GameController gc;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnCollisionEnter(Collision collision)
    {
// Tell GameController Ball is hit.
        gc.PlayBallHit();
        gc.TraceHitRacket();
       gameObject.GetComponent<MeshCollider>().enabled = false;

    }

    public void GetPosition(ref Vector3 pos, ref Quaternion dir)
    {
        Transform transform = GetComponent<Transform>();

        pos = transform.position;
        dir = transform.rotation;

        return;
    }

    public void RestartRacket()
    {
        gameObject.GetComponent<MeshCollider>().enabled = true;
    }

}
