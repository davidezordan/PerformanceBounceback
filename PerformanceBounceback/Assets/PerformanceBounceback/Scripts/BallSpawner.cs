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
    private List<SpawnedBallComponents> _pooledBalls;
    private float _cooldown;
    private float _cooldownLength = 0.5f;
    public static BallSpawner current;

    public GameObject pooledBallPrefab; 
    public int ballsAmount = 40; //the number of objects you want in the object pool
    public static int ballPoolNum = 0; //a number used to cycle through the pooled objects

    public bool IsSpawningActive = true;

    void Awake()
    {
        current = this; //makes it so the functions in ObjectPool can be accessed easily anywhere
    }

    void Start()
    {
        _pooledBalls = new List<SpawnedBallComponents>();
        for (int i = 0; i < ballsAmount; i++)
        {
            GameObject obj = Instantiate(pooledBallPrefab);
            obj.SetActive(false);

            _pooledBalls.Add(new SpawnedBallComponents(obj));
        }
    }

    public SpawnedBallComponents GetPooledBall() {     
        ballPoolNum++;      
        
        if(ballPoolNum >= _pooledBalls.Count) {         
            // Reset it to 0 if the number is larger than the number of objects we have in the pool.         
            ballPoolNum = 0;      
        }  
 
        if (_pooledBalls[ballPoolNum].GetBall.activeSelf) {         
            foreach(var comp in _pooledBalls) {             
                // If the object is not active...              
                if(!comp.GetBall.activeSelf) {                 
                    // return that one.
                    DebugManager.Info("Returning existing Ball");   
                    return comp;             
                } 
            }

            // If it gets down here, we have no available objects in the pool.         
            GameObject obj = Instantiate(pooledBallPrefab);
            var returnComponent = new SpawnedBallComponents(obj);
            _pooledBalls.Add(returnComponent);       
            returnComponent.GetBall.SetActive(false);

            DebugManager.Info("Returning new Ball");

            return returnComponent;
        } else { 
            DebugManager.Info("Returning existing Ball");
            return _pooledBalls[ballPoolNum];     
        }
    } 
   	
	void Update () {
        if (!IsSpawningActive) {
            return;
        }

        _cooldown -= Time.deltaTime;
        if(_cooldown <= 0)
        {
            _cooldown = _cooldownLength;
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
        DebugManager.Info("Disposing ball in " + ballsAmount * _cooldownLength);
        yield return new WaitForSeconds(ballsAmount * _cooldownLength);     
        selectedBall.SetActive(false); 
    }

    public void Initialize() {
        foreach(var ball in _pooledBalls) {
            ball.GetBall.SetActive(false);
        }

        IsSpawningActive = true;
    }
}