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
    public float amountOfSpacingsVertical;

    public GameObject eraser;
    public GameObject[] obstacles;
    public GameObject[] obstaclesGuardian;

    private void Start() {
        groundWidth = groundTile.GetComponent<BoxCollider2D>().bounds.size.x;
        groundHeight = groundTile.GetComponent<BoxCollider2D>().bounds.size.y;

        eraser = GameObject.FindGameObjectWithTag("Eraser");
       
        horizontalSpacing = groundWidth / amountOfSpacings;
        
        for (int i = 0; i < amountOfSpacings; i++) {
        Debug.Log(horizontalSpacing * i);

            if (Random.value > 0.33f) {
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
        for (int i = 0; i < _amount; i++) {
            GameObject _obstacle = Instantiate(obstaclesGuardian[Random.Range(0, obstaclesGuardian.Length)]);
            float height = (i+1 * _obstacle.GetComponent<BoxCollider2D>().bounds.size.y) * upOrDown;
            _obstacle.transform.position = new Vector3(_spawnPoint, groundTile.transform.position.y + height, 0);
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
            for (int x = 0; x < amountOfSpacingsVertical; x++) {
                SpawnObstacleGuardian( (x+1 * verticalSpacing) + spawnSpoint, 5);
                Debug.Log("spawned object");
                }
            }

        }

    }
