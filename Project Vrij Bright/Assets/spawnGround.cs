using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGround : MonoBehaviour {

    public GameObject groundTile;
    public float groundWidth;
    public float groundHeight;

    private float spawnSpoint = 0;
    public int spawnRate;

    public float verticalSpacing;
    public float horizontalSpacing;
    public float amountOfSpacings;
    public int amountOfSpacingsVertical;
    public float obstacleAmountOfSpacing;
    public float obstacleSpacingHorizontal;
    public float bottom;
    public GameObject eraser;
    public GameObject[] obstacles;
    public GameObject[] obstaclesGuardian;

    private void Start() {
        bottom =  groundTile.GetComponent<BoxCollider2D>().bounds.extents.x / 2;
        Debug.Log(bottom + "groundTile");
        groundWidth = groundTile.GetComponent<BoxCollider2D>().bounds.size.x;
        groundHeight = groundTile.GetComponent<BoxCollider2D>().bounds.size.y;

        eraser = GameObject.FindGameObjectWithTag("Eraser");
       
        horizontalSpacing =  (groundWidth / amountOfSpacings)/2;
        obstacleSpacingHorizontal = groundWidth / obstacleAmountOfSpacing;
        
        for (int i = 0; i < amountOfSpacings; i++) {
        Debug.Log(horizontalSpacing * i);

            if (i > 2) {
                SpawnObstacle(obstacles[Random.Range(0, obstacles.Length)], (i * horizontalSpacing) + spawnSpoint);
                }
            }
        for (int x = 0; x < amountOfSpacingsVertical; x++) {
            SpawnObstacleGuardian((x + 1 * verticalSpacing) + spawnSpoint, 5);
            Debug.Log("spawned object");
            }
        spawnSpoint = groundWidth;
        SpawnGround();

        }

    private void Update() {
        if (eraser.transform.position.x > spawnSpoint - (groundWidth * 4)) {
            SpawnGround();
            }
        }

    private void SpawnObstacles(float spawnPoint) {
        if ( Random.value > 0.5f) {
            return;
            }
        else if (Random.value > 0.4) {
            SpawnObstacle(obstacles[0], spawnPoint);
            }
        }

    //spawn physics
    private void SpawnObstacle(GameObject _obstacle, float _spawnPoint) {
        GameObject obstacle = Instantiate(_obstacle);
        obstacle.transform.position = new Vector3(_spawnPoint, groundTile.transform.position.y, 0);
        obstacle.name = _obstacle.name;
        obstacle.transform.parent = this.gameObject.transform;
        }
    
    //spawn obstacles up = 1 down = -1;
    private void SpawnObstacleGuardian( float _spawnPoint, int _amount = 1, int upOrDown = 1) {
        int realAmount = Random.Range(0, _amount);
        for (int i = 0; i < realAmount; i++) {
           
            GameObject _obstacle = Instantiate(obstaclesGuardian[Random.Range(0, obstaclesGuardian.Length)]);
            float height = (i * _obstacle.GetComponent<BoxCollider2D>().bounds.size.y) * upOrDown;
            _obstacle.transform.position = new Vector3(_spawnPoint, (bottom + (i * height)), 0);
            _obstacle.name = _obstacle.name;
            _obstacle.transform.parent = this.gameObject.transform;
            }
        }

    private void SpawnGround() {
        Debug.Log("spawned");
        for (int i = 0; i < spawnRate; i++) {
            GameObject tile = Instantiate(groundTile);
            tile.transform.position = new Vector3(spawnSpoint, groundTile.transform.position.y, 0);
            tile.name = "groundTile";
            tile.transform.parent = this.gameObject.transform;
            spawnSpoint += groundWidth;
            for (int j = 0; j < amountOfSpacings; j++) {
                if (Random.value > 0.33f) {
                    SpawnObstacle(obstacles[Random.Range(0, obstacles.Length)], (j * horizontalSpacing) + spawnSpoint);
                    }
                }
            for (int x = 0; x < obstacleAmountOfSpacing; x++) {
                SpawnObstacleGuardian( (x * verticalSpacing) + spawnSpoint, amountOfSpacingsVertical);
                Debug.Log("spawned object");
                }
            }

        }

    }
