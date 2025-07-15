using System.Collections;
using UnityEngine;
using CF.Data;

namespace CF.Player {
public class LaneController : MonoBehaviour
{
    public bool debug;

    [SerializeField]
    private TouchInputHandler inputHandler;

    private float[] laneAnchorPoints;

    private float moveSpeed;

    #region Lane Variables
    public int CurrentLane { get; private set; } = 1;

    [SerializeField]
    private float timeBetweenInput = 0.2f;
    private float curTimeBetweenInput;
    public bool canMove { get; private set; }

    #endregion

    private Queue moveCommandQueue;
    private MoveCommand currentMoveCommand = null;

    [HideInInspector]
    public bool locked;

    #region Unity Functions

    private void Awake()
    {
        currentMoveCommand = new MoveCommand(Vector2.zero);
        currentMoveCommand.isExecuted = true;
        moveCommandQueue = new Queue();

        inputHandler.OnEndTouch += RegisterMoveCommand;

    }

    private void Start()
    {
        laneAnchorPoints = SetupScene.Current.laneAnchorPoints;
        curTimeBetweenInput = 0f;
    }

    private void Update()
    {
        if (curTimeBetweenInput <= 0f)
        {
            canMove = true;
        }
        else
        {
            curTimeBetweenInput -= Time.deltaTime;
            canMove = false;
        }

        if (locked) moveSpeed = 0;
    }

    #endregion

    public void LoadPlayerData(PlayerData data)
    {
        moveSpeed = data.MoveSpeed;
    }

    #region Private Functions

    private void RegisterMoveCommand(Vector2 _input) 
    {
        MoveCommand moveCommand = new MoveCommand(_input);
        moveCommandQueue.Enqueue(moveCommand);
        Log("Enqueued Move Command");

        UseMoveCommand();
    }

    private void UseMoveCommand() 
    {
        if (moveCommandQueue.Count > 0 && currentMoveCommand.isExecuted)
        {
            currentMoveCommand = (MoveCommand)moveCommandQueue.Dequeue();
            MovePlayer(currentMoveCommand.playerInput);
            Log("Used MoveCommand");
        }
    }
    
    #region Move Functions
    private void MovePlayer(Vector2 _input) 
    {
        curTimeBetweenInput = timeBetweenInput;

        StopCoroutine("MovePlayerTowardsPosition");
        transform.position = new Vector3(laneAnchorPoints[CurrentLane], transform.position.y, 0);

        CurrentLane = InputToLane(_input);

        Vector3 _pos = new Vector3(laneAnchorPoints[CurrentLane], transform.position.y, 0);
        var coroutine = MovePlayerTowardsPosition(_pos);

        StartCoroutine(coroutine);
    }

    private int InputToLane(Vector2 _input) 
    {
        int xInput = (int) Mathf.Clamp(CurrentLane + _input.x, 0, laneAnchorPoints.Length - 1);
        return xInput;
    }

    private IEnumerator MovePlayerTowardsPosition(Vector3 _pos) 
    {
        while (!(Vector2.Distance(_pos, transform.position) < 0.1f))
        {
            float concreteSpeed = (moveSpeed * (ScreenSize.GetScreenToWorldWidth * 0.1f)) * Time.deltaTime;
            transform.Translate(Vector3.Normalize(_pos - transform.position) * concreteSpeed);
            yield return null;
        }

        transform.position = _pos;

        currentMoveCommand.isExecuted = true;

        UseMoveCommand();

    }

    #endregion

    #endregion

    #region Log

    private void Log(string _message)
    {
        if (!debug) return;
        Debug.Log("[Lane Controller]: " + _message);
    }

    private void LogWarning(string _message) 
    {
        if (!debug) return;
        Debug.Log("[Lane Controller]: " + _message);
    }

    #endregion 
}
}