using UnityEngine;

namespace CF.Enemy {
    class EnemyHealthController : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;
        private int currentHealth;
        private void Awake()
        {
            currentHealth = maxHealth;
        }
        public void TakeDamage(int damage)
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
        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        public int GetMaxHealth()
        {
            return maxHealth;
        }
    }
}

