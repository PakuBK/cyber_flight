using UnityEngine;

namespace CF.Enemy {
    class EnemyHealthController : MonoBehaviour
    {
        private EnemyContext context;

        private float maxHealth = 100;
        private float currentHealth;

        private void Awake()
        {
            context = GetComponent<EnemyContext>();
        }
        private void Start()
        {
            InitializeHealth();
        }

        public void InitializeHealth()
        {
            maxHealth = context.enemyData.Health;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            // Handle enemy death logic here
            Debug.Log("Enemy has died.");
            gameObject.SetActive(false); // Deactivate the enemy object
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
        public float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}

