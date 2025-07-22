using UnityEngine;

namespace CF.Enemy {
    public class EnemyMovementController : MonoBehaviour
    {
        private EnemyContext context;
        private float[] laneAnchorPoints { get; set; }
        private float moveSpeed => context.enemyData.MoveSpeed;

        private int targetLaneIndex = -1;
        private bool isMoving = false;
        private bool isInterrupted = false;

        

        private void Awake()
        {
            context = GetComponent<EnemyContext>();
            laneAnchorPoints = SetupScene.Current.laneAnchorPoints;
        }

        public bool MoveToLane(int laneIndex)
        {
            float targetX = laneAnchorPoints[laneIndex];
            Vector3 pos = transform.position;
            // TODO: apply status effects like slowing here
            float realMoveSpeed = moveSpeed;

            pos.x = Mathf.MoveTowards(pos.x, targetX, realMoveSpeed * Time.deltaTime);
            transform.position = pos;

            return Mathf.Abs(pos.x - targetX) < 0.01f;
        }

        public void InterruptMovement()
        {
            isInterrupted = true;
            isMoving = false;
        }

        public void ResumeMovement()
        {
            if (targetLaneIndex >= 0)
            {
                isMoving = true;
                isInterrupted = false;
            }
        }

        public bool IsAtLane(int laneIndex)
        {
            float targetX = laneAnchorPoints[laneIndex];
            return Mathf.Abs(transform.position.x - targetX) < 0.01f;
        }

        public int GetNearestLaneIndex()
        {
            float closestDistance = float.MaxValue;
            int closestLaneIndex = -1;
            for (int i = 0; i < laneAnchorPoints.Length; i++)
            {
                float distance = Mathf.Abs(transform.position.x - laneAnchorPoints[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLaneIndex = i;
                }
            }
            return closestLaneIndex;
        }
    }
}