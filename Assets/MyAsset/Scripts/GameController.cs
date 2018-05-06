using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject BallPrefab, BallTracePrefab;
    public GameObject DebugLinePrefab;
    public GameObject Racket;
    public GameObject RacketCenter;
    public GameObject RacketTracePrefab;
    public GameObject RacketTraceHitPrefab;
    GameObject BallInstance;
    BallController ball_controller;
    //    RackerController racketController;

    public float TraceDisplayTime;
    public UnityEngine.Video.VideoPlayer VideoController;
    float start_time;
    float start_trace_time;
    public float playbackspeed = 3.0f;

    //
    float inplay_start_time;
    float trace_start_time;

    class MyTrace
    {
        public Vector3 ballPos;
        public Vector3 racketPos;
        public Quaternion racketDir;
        public Vector3 racketCenterPos;
        public float time;
    }

    List<GameObject> racketTrace;
    List<GameObject> debugLineTrace;
    List<MyTrace> Trace;

    // for PlayBack
    GameObject playbackRacket;
    GameObject playbackBall;
    GameObject playbackLine;
    

    enum State  { Serving, Served,  InPlay, TraceDisplaying, WaitingServe };
    State state ;

    // Use this for initialization
    void Start () {
        StartServe();
        Trace = new List<MyTrace>();
        racketTrace = new List<GameObject>();
 //       debugLineTrace  new List<GameObject>();
    }

    // Update is called once per frame9
    void Update()
    {


        //        Debug.Log("Update time :" + Time.deltaTime);
        float time = Time.time - start_time;

        if (state == State.Serving) // just started
        {
            // Start video and set state to 1
            StartVideo();
            Racket.GetComponent<RackerController>().RestartRacket();

            state = State.Served;
        }
        else if (state == State.Served) // video is playing
                             // check timer and when 
        {
            if (time >= 2.6f)
            {
                BallInstance = Instantiate(BallPrefab, new Vector3(18.38f, 2.71f, -12.8f), new Quaternion ( 0,0,0,0));
                ball_controller = BallInstance.GetComponent<BallController>();
                PlayBallHit();
                state = State.InPlay;
                inplay_start_time = Time.time;
            }
        }
        else if (state == State.InPlay) // ball is alive
        {
            // Get Ball Center
            Vector3 ballPos = new Vector3(0,0,0);
            Quaternion ballDir = new Quaternion(0,0,0,0);
            BallInstance.GetComponent<BallController>().GetPosition(ref ballPos, ref ballDir);

            // Get Racket Model
            Vector3 racketPos = new Vector3(0, 0, 0);
            Quaternion racketDir = new Quaternion(0, 0, 0, 0);;
            Racket.GetComponent<RackerController>().GetPosition(ref racketPos, ref racketDir);

            // Get Racket Center
            Vector3 racketCenterPos = RacketCenter.GetComponent<Transform>().position;

            /*            GameObject debugLine = Instantiate(DebugLinePrefab);
                        LineRenderer lr = debugLine.GetComponent<LineRenderer>();
                        lr.SetPosition(0, ballPos);
                        lr.SetPosition(1, racketPos);

                        racketTrace.Add(debugLine);
            */
            MyTrace tr = new MyTrace();
            tr.ballPos= ballPos;
            tr.racketPos = racketPos;
            tr.racketDir = racketDir;
            tr.racketCenterPos = racketCenterPos;
            tr.time = Time.time - inplay_start_time;
            Trace.Add(tr);

            //            traceRacketIfNecesary();
        }
        else if (state == State.TraceDisplaying) // ball is alive
        {
            if (Trace.Count == 0)
            {
                FinishPlayback();
            }
            else
            {
                MyTrace tr = Trace[0]; // 先頭のデータを取り出す
                if ((Time.time - start_trace_time) >= tr.time * playbackspeed) // time to go?
                {

                    MoveGameObject(playbackRacket, tr.racketPos);
                    RotateGameObject(playbackRacket, tr.racketDir);
                    MoveGameObject(playbackBall, tr.ballPos);

                    LineRenderer lr = playbackLine.GetComponent<LineRenderer>();
                    lr.SetPosition(0, tr.ballPos);
                    lr.SetPosition(1, tr.racketCenterPos);

                    Trace.RemoveAt(0);
                }
            }
        }
        else if (state == State.WaitingServe) // 
        {
            destroyRacketTraces();
            StartServe();
        }

    }

    void FixedUpdate()
    {
//        Debug.Log("FixedUpdate time :" + Time.deltaTime);
    }

    void traceRacketIfNecesary()
    {

        if ( shouldTraceRacket())
        {
            GameObject racket = GameObject.Find("Racket");
            if (racket != null)
            {
                Vector3 pos = racket.GetComponent<Transform>().position;
                Quaternion dir = racket.GetComponent<Transform>().rotation;
                racketTrace.Add(CreateRacketTrace(pos, dir));
            }
        }

    }

    public void SetBallOut()
    {
        Debug.Log("GameController:SetBallOut()");
        StartPlayback();

    }

    public void PlayBallHit()
    {
        Debug.Log("GameController: PlayBallHit()");
        ball_controller.PlayHitSound();
    }

    public void TraceHitRacket()
    {
        GameObject racket = GameObject.Find("Racket");
        if (racket != null)
        {
            Vector3 pos = racket.GetComponent<Transform>().position;
            Quaternion dir = racket.GetComponent<Transform>().rotation;
            racketTrace.Add(Instantiate(RacketTraceHitPrefab, pos, dir));
        }
    }

    public void StartServe()
    {
        Debug.Log("GameController:StartServe() ");
        start_time = Time.time;
        state = State.Serving;
    }

    public void StartVideo()
    {
        VideoController.Play();
    }

    public GameObject CreateBallTrace(Vector3 pos)
    {
        return ( Instantiate( BallTracePrefab, pos, new Quaternion(0, 0, 0, 0)) );
    }

    public bool shouldTraceRacket()
    {
        float z = BallInstance.GetComponent<Transform>().position.z;
        return ((z > 4) && (z < 8));
    }

    public GameObject CreateRacketTrace(Vector3 pos, Quaternion dir)
    {
        return (Instantiate(RacketTracePrefab, pos, dir ));
    }

    void StartPlayback()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        playbackRacket = Instantiate(RacketTracePrefab );
        playbackBall = Instantiate(BallTracePrefab);
        playbackLine = Instantiate( DebugLinePrefab) ;

        ball_controller.destroyTraces();

        state = State.TraceDisplaying;
        start_trace_time = Time.time;
    }

    void FinishPlayback()
    {
        Destroy( playbackRacket );
        Destroy( playbackBall );
        Destroy( playbackLine );

        state = State.WaitingServe;
    }

    public void destroyRacketTraces()
    {
        foreach (GameObject go in racketTrace)
        {
            Destroy(go);
        }
        racketTrace.Clear();
    }
        
    void MoveGameObject( GameObject obj, Vector3 pos)
    {
        Transform tr = obj.GetComponent<Transform>();
        tr.position = pos;
        return;
    }

    void RotateGameObject(GameObject obj, Quaternion dir)
    {
        Transform tr = obj.GetComponent<Transform>();
        tr.rotation = dir;
        return;
    }
}
