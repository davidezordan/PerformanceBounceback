using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedBallComponents {
    GameObject _ball;
    Rigidbody _rigidBody;
    public SpawnedBallComponents(GameObject ball)
    {
       _ball = ball;   
       _rigidBody = ball.GetComponent<Rigidbody>();
    }

    public GameObject GetBall { get { return _ball; } }

    public Rigidbody GetRigidBody { get { return _rigidBody; } }
}

public class BallSpawner : MonoBehaviour {
    private List<SpawnedBallComponents> pooledBalls; //the object pool
    public static BallSpawner current;

    public GameObject pooledBall; //the prefab of the object in the object pool
    public int ballsAmount = 40; //the number of objects you want in the object pool
    public static int ballPoolNum = 0; //a number used to cycle through the pooled objects

    private float cooldown;
    private float cooldownLength = 0.5f;

    void Awake()
    {
        current = this; //makes it so the functions in ObjectPool can be accessed easily anywhere
    }

    void Start()
    {
        pooledBalls = new List<SpawnedBallComponents>();
        for (int i = 0; i < ballsAmount; i++)
        {
            GameObject obj = Instantiate(pooledBall);
            obj.SetActive(false);

            pooledBalls.Add(new SpawnedBallComponents(obj));
        }
    }

    public SpawnedBallComponents GetPooledBall() {     
        ballPoolNum++;      
        
        if(ballPoolNum >= pooledBalls.Count) {         
            // Reset it to 0 if the number is larger than the number of objects we have in the pool.         
            ballPoolNum = 0;      
        }  
 
        if (pooledBalls[ballPoolNum].GetBall.activeSelf) {         
            foreach(var comp in pooledBalls) {             
                // If the object is not active...              
                if(!comp.GetBall.activeSelf) {                 
                    // return that one.
                    DebugManager.Info("Returning existing Ball");   
                    return comp;             
                } 
            }

            // If it gets down here, we have no available objects in the pool.         
            GameObject obj = Instantiate(pooledBall);
            var returnComponent = new SpawnedBallComponents(obj);
            pooledBalls.Add(returnComponent);       
            returnComponent.GetBall.SetActive(false);

            DebugManager.Info("Returning new Ball");

            return returnComponent;
        } else { 
            DebugManager.Info("Returning existing Ball");
            return pooledBalls[ballPoolNum];     
        }
    } 
   	
	void Update () {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            cooldown = cooldownLength;
            SpawnBall();
        }		
	}

    void SpawnBall()
    {
        var selected = BallSpawner.current.GetPooledBall();
        GameObject selectedBall = selected.GetBall;
        selectedBall.transform.position = transform.position;
        Rigidbody selectedRigidbody = selected.GetRigidBody;
        selectedRigidbody.velocity = Vector3.zero;
        selectedRigidbody.angularVelocity = Vector3.zero;
        selectedBall.SetActive(true);

        StartCoroutine(TemporaryBall(selectedBall)); 
    } 
 
public IEnumerator TemporaryBall(GameObject selectedBall) {     
    // After a period of time, destroy the ball.
    DebugManager.Info("Disposing ball in " + ballsAmount * cooldownLength);
    yield return new WaitForSeconds(ballsAmount * cooldownLength);     
    selectedBall.SetActive(false); } 
}