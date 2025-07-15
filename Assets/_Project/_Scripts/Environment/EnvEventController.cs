using System.Collections.Generic;
using UnityEngine;

namespace CF.Environment {
public class EnvEventController : MonoBehaviour
{
    public List<Sprite> Sprites;

    public GameObject ObstaclePrefab;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector2 movementVector;

    [SerializeField]
    private bool randomSize;

    [SerializeField]
    private float minSize, maxSize;

    [Header("Settings")]

    [SerializeField]
    private float duration;
    [SerializeField]
    private float intervall;

    [SerializeField] [Range(0,100)]
    private float chanceForEvent;

    [SerializeField]
    private float checkTime;

    private float startTime, intervallStartTime;

    private float checkStartTime;

    private bool isRunning;

    private void Update()
    {

        if (Time.time > checkStartTime + checkTime && !isRunning)
        {
            checkStartTime = Time.time;
            if (chanceForEvent >= Random.Range(0f, 100f))
            {
                var lanes = SetupScene.Current.laneAnchorPoints;
                int[] possiblesMoves = { 0, 2 };
                EnterEnvEvent(lanes[possiblesMoves[Random.Range(0, possiblesMoves.Length)]]);
            }
        }

        if (isRunning)
        {
            EnvEvent();
        }

    }

    private void Start()
    {
        checkStartTime = Time.time;
    }


    private void EnterEnvEvent(float xPos)
    {

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        startTime = Time.time;
        isRunning = true;
    }

    private void EnvEvent()
    {
        if (Time.time > intervallStartTime + intervall)
        {
            SpawnObstacle();
            intervallStartTime = Time.time;
        }

        if (Time.time > startTime + duration)
        {
            checkStartTime = Time.time;
            isRunning = false;
        }
    }

    private void SpawnObstacle()
    {
        Vector3 _pos = transform.position;

        float _laneWidth = Mathf.Abs(SetupScene.Current.laneAnchorPoints[2] - SetupScene.Current.laneAnchorPoints[1]);
        float _offset = Random.Range(-_laneWidth * 0.25f, _laneWidth * 0.25f);

        _pos += new Vector3(_offset, 0, 0);

        float size = Random.Range(minSize, maxSize);

        Sprite sprite = Sprites[Random.Range(0, Sprites.Count)];

        ObjectPooler.Current.InstantiateSpaceTrash(ObstaclePrefab, _pos, size, sprite, speed);
    }

}
}