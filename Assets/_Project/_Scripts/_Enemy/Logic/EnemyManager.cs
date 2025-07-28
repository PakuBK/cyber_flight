using UnityEngine;
using CF.Player;
using CF.UI;
using CF.Data;

namespace CF.Enemy {
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        public EnemyData[] enemys;

        [SerializeField]
        private GameObject player;

        private EnemyStateMachine stateMachine;

        private EnemyContext context;
    

        private int currentEnemy = 0;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            GameEvents.Current.onEnemyDeath += TransitionToNewEnemy;
        }

        private void Initialize()
        {
            stateMachine = GetComponent<EnemyStateMachine>();
            context = GetComponent<EnemyContext>();

            Sprite playerSprite = player.GetComponent<InteractionHandler>().data.Icon; // May break, gets current used Sprite.
            //UIController.current.ShowVersusUI(playerSprite, enemys[currentEnemy].Icon, enemys[currentEnemy].Name);

        }

        private void TransitionToNewEnemy()
        {
            //brain.gameObject.SetActive(false);

            HandleReward();

            Sprite playerSprite = player.GetComponent<InteractionHandler>().data.Icon; // May break, gets current used Sprite.

            currentEnemy++;

            if (currentEnemy == enemys.Length)
            {
                currentEnemy = 0;
            }

            UIController.current.ShowVersusUI(playerSprite, enemys[currentEnemy].Icon, enemys[currentEnemy].Name);

            Invoke("SetupEnemy", 2f); // this doesn't seem like a good solutuin
        }

        private void SetupEnemy()
        {
            //brain.ResetPos();

            //brain.gameObject.SetActive(true);
            //brain.LoadEnemy(enemys[currentEnemy]);
        }

        private void HandleReward()
        {
            var enemy = enemys[currentEnemy];
            int shardsAdded = Random.Range(enemy.ShardsMin, enemy.ShardsMax);

            int fragmentAmount = enemy.FragmentAmount;

            int[] fragment = new int[5];

            fragment[(int)enemy.FragmentTier] = fragmentAmount;


            UIController.current.UpdateShardText(shardsAdded);

            UIController.current.ShowFragmentText(enemy.FragmentTier, fragmentAmount);

            DataController.AddCurrency(shardsAdded, fragment);

            SetupScene.Current.shardsThisSession += shardsAdded;
            SetupScene.Current.enemysThisSession += 1;

        }

        public EnemyData GetCurrentEnemyData()
        {
            return enemys[currentEnemy];
        }
    }
}