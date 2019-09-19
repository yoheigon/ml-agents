using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class HatCubeAgent : Agent
{
    public float m_Speed = 10f;            
    public float m_TurnSpeed = 180f;       

    public bool contact;    

    public GameObject myLaser;

    private Rigidbody rb;

    private Vector3 m_LaserScale = new Vector3(0.3f, 0.01f, 4f);
    private Vector3 m_LaserLength = new Vector3(0.0f, 0.0f, 2.5f);

    private Vector3 ident = new Vector3(0f, 0f, 0f);

    private string m_ForwardAxisName;     
    private string m_StrafeAxisName;     
    private string m_TurnAxisName;         
    private string m_LaserAxisName;         

    private float m_CollisionTimer;    

    private RayPerception3D rayPer;

    private float m_ForwardInputValue;    
    private float m_StrafeInputValue;    
    private float m_TurnInputValue;        
    private float m_LaserInputValue;        

    public int score;        


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception3D>();
        score = 0;
        contact = false;

    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        rb = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception3D>();
        score = 0;
        contact = false;

        
        //agentRb = GetComponent<Rigidbody>();
        //Monitor.verticalOffset = 1f;
        //myArea = area.GetComponent<BananaArea>();
        //rayPer = GetComponent<RayPerception3D>();
        //myAcademy = FindObjectOfType<BananaAcademy>();

    //    SetResetParameters();
    }

    void ExecuteAction()
    {
        Move();
        Turn();
        Shoot();
            
                
            
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
       // if (contact)
       //     m_ForwardInputValue = Mathf.Min(m_ForwardInputValue, 0);
        
        Vector3 forward = transform.forward * m_ForwardInputValue * m_Speed * Time.deltaTime;
        Vector3 strafe = transform.right * m_StrafeInputValue * m_Speed * Time.deltaTime;
        Vector3 movement = strafe + forward;
        if (contact)
            movement.z = Mathf.Min(movement.z, 0);
            //m_ForwardInputValue = Mathf.Min(m_ForwardInputValue, 0);

        if (m_LaserInputValue != 1)
            rb.MovePosition(rb.position + movement);
        //rb.AddForce(new Vector3(m_ForwardInputValue, 0.0f, m_StrafeInputValue) * m_Speed);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        
        rb.MoveRotation(rb.rotation * turnRotation);
    }   

    private void Shoot()
    {
        if (m_LaserInputValue > 0.0f)
        {
            myLaser.transform.localScale = m_LaserScale;
            myLaser.transform.localPosition = m_LaserLength;
            //Vector3 position = transform.TransformDirection(RayPerception3D.PolarToCartesian(25f, 90f));
            //Debug.DrawRay(transform.position, position, Color.red, 0f, true);
        }
        else
        {
            myLaser.transform.localScale = ident;// new Vector3(0f, 0f, 0f);
            myLaser.transform.localPosition = ident;// new Vector3(0f, 0f, 0f);

        }    
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            contact = true;
            m_CollisionTimer = 1f;
        }
    }

    void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            m_CollisionTimer = 1f;
        }
    }
    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            contact = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);                
            SetReward(1f);
            score++;
        }

    }

    public override void AgentReset()
    {
        
        gameObject.transform.localPosition = new Vector3(0f, .5f, -16f);
        //gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        gameObject.transform.rotation = Quaternion.identity;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
        score = 0;
    }

    public override void CollectObservations()
    {
        //position and rotation 7 observations
        AddVectorObs(gameObject.transform.localPosition);
        AddVectorObs(gameObject.transform.rotation);

        //raycasts 10 rays * (3 classes + IDK + distance) = 50 observations 
        float rayDistance = 50f;
        //float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        //float[] rayAngles = { 0f, 50f, 100f, 150f, 200f, 250f, 300f };
        //float[] rayAngles = { 225, 255f, 285f, 315f, 90f, 60f, 120f};
        float[] rayAngles = { 160f, 180f, 200f, 340f, 0f, 20f, 50f, 60f, 70f, 80f, 90f, 100f, 110f, 120f, 130f,  270f, 255f, 240f, 285f, 300f};
        string[] detectableObjects = { "UnbreakableSphere", "BreakableSphere", "Coin"};
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, .5f, .5f));
        if (m_CollisionTimer <= 0)
            contact = false;


    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var forwardAxis = (int)vectorAction[0];
        var rightAxis = (int)vectorAction[1];
        var rotateAxis = (int)vectorAction[2];
        var shootAxis = (int)vectorAction[3];

       
        m_ForwardInputValue = 0;
        m_StrafeInputValue = 0;
        m_TurnInputValue = 0;
        m_LaserInputValue = 0;
        int currentScore = score;
        switch (forwardAxis)
        {
            case 1:
                m_ForwardInputValue = 1;
                break;
            case 2:
                m_ForwardInputValue = -1;
                break;
        }
        switch (rightAxis)
        {
            case 1:
                m_StrafeInputValue = 1;
                break;
            case 2:
                m_StrafeInputValue = -1;
                break;
        }
        switch (rotateAxis)
        {
            case 1:
                m_TurnInputValue = 1;
                break;
            case 2:
                m_TurnInputValue = -1;
                break; 
        }
        switch (shootAxis)
        {
            case 1:
                m_LaserInputValue = 1;
                break;
        }

        ExecuteAction();

        if (rb.transform.localPosition.y < -10)
        {
            gameObject.SetActive(false);
            SetReward(-1f);
            Done();
        }

        else {
            SetReward(.01f); //survival
        }
    }
}
