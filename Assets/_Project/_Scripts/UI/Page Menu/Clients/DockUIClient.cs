using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CF.Data;

namespace CF.UI {
public class DockUIClient : MonoBehaviour
{

    #region Singelton
    public static DockUIClient Instance = null;

    private void Awake()
    {
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

    #region Ship Page Variables

    private int currentPage = 0;
    [SerializeField]
    private Image ShipIcon;
    [SerializeField]
    private Image ShipBackDrop;
    [SerializeField]
    private Sprite[] RarityPanelSprites;
    [SerializeField]
    private RectTransform BackBar;
    [SerializeField]
    private RectTransform HealthBar;
    [SerializeField]
    private RectTransform SpeedBar;
    [SerializeField]
    private TextMeshProUGUI ShipName;

    private float MAX_SPEED = 25;
    private float MAX_HEALTH = 10;

    #endregion

    #region Currency Variables

    [Header("Currency Variables")]

    [SerializeField]
    private TextMeshProUGUI Tier1;
    [SerializeField]
    private TextMeshProUGUI Tier2;
    [SerializeField]
    private TextMeshProUGUI Tier3;
    [SerializeField]
    private TextMeshProUGUI Tier4;
    [SerializeField]
    private TextMeshProUGUI Tier5;
    [SerializeField]
    private TextMeshProUGUI Shards;

    #endregion
    private void Start()
    {
        // Fix this // fix what?
        MAX_HEALTH = PlayerShipManager.Instance.GetMaxHealth();
        MAX_SPEED = PlayerShipManager.Instance.GetMaxSpeed();

        LoadShipPage(currentPage);
        LoadCurrencyUI();
    }

    private void Update() {
        if (Application.platform == RuntimePlatform.Android && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PageController.instance.CloseFullPage();
        }
    }

    #region Ship Page
    public void NextShipPage()
    {
        currentPage++;
        LoadShipPage(currentPage);

    }

    public void BackShipPage()
    {
        currentPage--;
        LoadShipPage(currentPage);
    }

    public void SelectShip()
    {
        
        var ship = PlayerShipManager.Instance.PlayerShips[currentPage];

        if (DataController.IsShipOwned(ship.name))
        {
            DataController.SavePlayerShip(ship); // saves ship in the current ship data slot
            Debug.Log("BEEP - You own this ship.");
        }
        else
        {
            Debug.Log("MEEEP - You don't own this ship.");
        }
        

    }

    private void LoadShipPage(int page)
    {
        
        var ships = PlayerShipManager.Instance.PlayerShips;

        if (page > ships.Count - 1)
        {
            currentPage = 0;
            page = 0;
        }
        else if (page < 0)
        {
            currentPage = ships.Count - 1;
            page = currentPage;
        }

        var ship = ships[page];

        ShipIcon.sprite = ship.Icon;

        var barWidth = BackBar.gameObject.GetComponentInParent<RectTransform>().rect.width;

        HealthBar.sizeDelta = new Vector2(barWidth * ((float)(ship.Health) / MAX_HEALTH), HealthBar.sizeDelta.y);
        SpeedBar.sizeDelta = new Vector2(barWidth * ((float)(ship.MoveSpeed) / MAX_SPEED), SpeedBar.sizeDelta.y);

        ShipName.text = ship.DisplayName;

        ShipBackDrop.sprite = RarityPanelSprites[ship.Rarity];

        if (DataController.IsShipOwned(ship.name)) // turn ship icon in a black silhouet
        {
            ShipIcon.color = Color.white;
        }
        else
        {
            ShipIcon.color = Color.black;
        } 
    }

    #endregion

    #region Currency UI

    private void LoadCurrencyUI()
    {
        var currency = DataController.LoadCurrency();
        Tier1.text = currency.FragmentsTier1.ToString();
        Tier2.text = currency.FragmentsTier2.ToString();
        Tier3.text = currency.FragmentsTier3.ToString();
        Tier4.text = currency.FragmentsTier4.ToString();
        Tier5.text = currency.FragmentsTier5.ToString();

        Shards.text = currency.Shards.ToString();
    }

    #endregion

    private void LoadBackground()
    {
        SceneLoader.LoadUIBackground();
    }
}
}
