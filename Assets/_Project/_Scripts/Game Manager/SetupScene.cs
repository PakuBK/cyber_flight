using UnityEngine;
using UnityEngine.U2D;

namespace CF {
public class SetupScene : MonoBehaviour
{
    public static SetupScene Current { get { return _current; } }
    public static SetupScene _current;


    [SerializeField]
    private Camera cam;

    private PixelPerfectCamera camPixel;

    [SerializeField]
    private Transform lane;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform currentEnemy;

    [SerializeField]
    private Transform[] singleLanes;

    public float[] laneAnchorPoints { get; private set; }

    public int shardsThisSession;
    public int enemysThisSession;

    private int numOfLanes;

    private float leftEdge, rightEdge;

    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }

        numOfLanes = singleLanes.Length;

        leftEdge = cam.ViewportToWorldPoint(new Vector3(0,0,0)).x;
        rightEdge = cam.ViewportToWorldPoint(new Vector3(1,1,0)).x;

        laneAnchorPoints = CalculateLanePoints();

        ScaleLaneSprite(lane);
        ScaleObject(player);
    }

    public float[] CalculateLanePoints()
    {
        float _length = rightEdge - leftEdge; // 30

        float _laneWidth = _length / numOfLanes; // 10

        float _laneMid = _laneWidth / 2; // 5

        float[] _anchorPoints = new float[numOfLanes];
        for (int i = 0; i < numOfLanes; i++)
        {
            float _laneEdge = _laneWidth * i;
            _anchorPoints[i] = _laneEdge + _laneMid;
        }

        float _centerPoint = _anchorPoints[numOfLanes / 2];

        float[] _playerPosLane = new float[numOfLanes];
        for (int j = 0; j < numOfLanes; j++)
        {
            _playerPosLane[j] = _anchorPoints[j] - _centerPoint;
        }

        return _playerPosLane;
    }

    private void ScaleLaneSprite(Transform _lane)
    {
        for (int i = 0; i < laneAnchorPoints.Length; i++)
        {
            singleLanes[i].position = new Vector3(laneAnchorPoints[i] + (0.1f * (i - 1)), ScreenSize.GetPixelScreenToWorldHeight / 2, 0);

            float _width = ScreenSize.GetPixelScreenToWorldWidth / numOfLanes;

            singleLanes[i].localScale = Vector3.one * Mathf.Abs(_width);
        }
    }

    public void ScaleObject(Transform _player)
    {
        float _height = ScreenSize.GetScreenToWorldHeight / 6f;

        _player.localScale = Vector3.one * _height;
    }

    public Transform GetCurrentEnemy() 
    {
        return currentEnemy;
    }

    public void SetEnemy(Transform _enemy)
    {
        currentEnemy = _enemy;
    }
}
}