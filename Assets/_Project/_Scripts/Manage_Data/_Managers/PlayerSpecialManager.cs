using UnityEngine;
using System.Collections.Generic;
using CF.Player;

namespace CF.Data {
    public class PlayerSpecialManager : MonoBehaviour
    {
        #region Singelton
        public static PlayerSpecialManager Instance;

        private void Awake() {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Interface

        private List<SpecialData> playerSpecials = null;

        public List<SpecialData> PlayerSpecials {
            get => playerSpecials == null ? LoadPlayerSpecials() : playerSpecials;
            private set {
                playerSpecials = value;
            }
        }

        public SpecialData GetSpecialForIdx(int idx) {
            return PlayerSpecials[idx];
        }

        public bool IsSpecialOwnedByIdx(int idx) {
            var specials_owned = DataController.LoadPlayerSpecialsOwned();
            return specials_owned.Contains(PlayerSpecials[idx].name);
        }

        public bool IsSpecialEquipedByIdx(int idx) {
            var special_equiped = DataController.LoadPlayerSpecial();
            return PlayerSpecials[idx].name == special_equiped.name;
        }

        #endregion

        #region Load Data

        private List<SpecialData> LoadPlayerSpecials()
        {
            List<SpecialData> specials = new List<SpecialData>();
            
            var specialAssets = Resources.LoadAll("PlayerSpecials", typeof(SpecialData));

            foreach (var asset in specialAssets)
            {
                var special = (SpecialData) asset;
                specials.Add(special);
            }

            playerSpecials = specials;
            return specials;
        }

        #endregion
    }

}
