using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    GameController gameController;
    GameObject ballTracePrefab;
    public float xMin = -10;
    public float xMax = 10;
    public float yMin = -100;
    public float yMax = 100;
    public float zMin = -20;
    public float zMax = 20;
    public float initialSpeedX = -10;
    public float initialSpeedY = -10;
    public float initialSpeedZ = -10;
    public float maxTimeToLive = 10.0f;
    bool ballIsLive;
    List<GameObject> ballTrace;

    Vector3 initialPos;
    float birthTime;
    AudioSource sound=null;

    // Use this for initialization
    void Start()
    {
        Debug.Log("New ball is instantiated");
        StartBall();
    }

    // Update is called once per frame
    void Update()
    {
        float x, y, z;
        Debug.Log("Ball:Update");
        if (!ballIsLive)
        {
            return;
        }

        Transform trans = GetComponent<Transform>();
        x = trans.position.x;
        y = trans.position.y;
        z = trans.position.z;

        ballTrace.Add(gameController.CreateBallTrace(new Vector3(x, y, z)));
        
        if (( Time.time > birthTime + maxTimeToLive ) || (x < xMin) || (x > xMax) || (y < yMin) || (y > yMax) || (z < zMin) || (z > zMax))
        {
            Debug.Log("CheckReset: now to call GameController.SetBallOut");
            gameController.SetBallOut();
            ballIsLive = false;

            trans.position = new Vector3(-100, -100, -100);
            Destroy(gameObject, .5f);
        }

    }

    public void StartBall()
    {
        Debug.Log("Ball:Start ");

        sound = GetComponent<AudioSource>();

        gameController = FindObjectOfType<GameController>();

//        Transform trans = GetComponent<Transform>();
//        trans.position = initialPos;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(initialSpeedX, initialSpeedY, initialSpeedZ);

        ballTrace = new List<GameObject>();


        birthTime = Time.time;
        ballIsLive = true;
    }

    public void PlayHitSound()
    {
        if ( sound == null)
            sound = GetComponent<AudioSource>();

        sound.PlayOneShot(sound.clip);
    }

    public void destroyTraces()
    {
        foreach (GameObject go in ballTrace)
        {
            Destroy( go);
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

