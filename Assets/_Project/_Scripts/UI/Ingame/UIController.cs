using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CF.Data;

namespace CF.UI {
public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image Health;
    [SerializeField]
    private Image Shield;
    [SerializeField]
    private Image Special;
    [SerializeField]
    private Image EnemyHealth;

    [Header("Versus UI")]
    [SerializeField]
    private GameObject VersusUI;
    [SerializeField]
    private Image VersusPlayer;
    [SerializeField]
    private Image VersusEnemy;
    [SerializeField]
    private TextMeshProUGUI VersusTitle;

    [Header("Shard UI Text")]
    [SerializeField]
    private TextMeshProUGUI shardText;
    [SerializeField]
    private GameObject shardBox;

    [Header("Fragment Text")]
    [SerializeField]
    private GameObject FragmentBox;
    [SerializeField]
    private Image FragmentImage;
    [SerializeField]
    private TextMeshProUGUI FragmentTextAmount;
    [SerializeField]
    private List<Sprite> FragmentIcons;

    [Header("GAME OVER UI")]
    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField] private TextMeshProUGUI GO_ShardsText;
    [SerializeField] private TextMeshProUGUI GO_BossCountText;
    [SerializeField] private TextMeshProUGUI GO_Tier1Text;
    [SerializeField] private TextMeshProUGUI GO_Tier2Text;
    [SerializeField] private TextMeshProUGUI GO_Tier3Text;
    [SerializeField] private TextMeshProUGUI GO_Tier4Text;
    [SerializeField] private TextMeshProUGUI GO_Tier5Text;


    public static UIController current { get { return _current; } }
    public static UIController _current;

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
    }

    private void Start()
    {
        GameEvents.Current.onPlayerDeath += ShowGameOverUI;

    }

    public void UpdatePlayerHealth(int _health, int _maxHealth)
    {
        Health.fillAmount = (float) _health / (float) _maxHealth;
    }

    public void UpdatePlayerShield(float _amountLeft, float _maxAmount, bool on)
    {
        if (on)
        {
            Shield.fillAmount = 1 - (_amountLeft / _maxAmount);
        }
        else
        {
            Shield.fillAmount = _amountLeft / _maxAmount;
        }
        
    }

    public void UpdatePlayerSpecial(float _amountLeft, float _maxAmount)
    {
        Special.fillAmount = _amountLeft / _maxAmount;
    }

    public void UpdateEnemyHealth(float _amountLeft, float _maxAmount)
    {
        EnemyHealth.fillAmount = _amountLeft / _maxAmount;
    }

    private void ShowGameOverUI()
    {
        GameOverUI.SetActive(true);

        CurrencyToken token = DataController.LoadCurrency();

        int shardsBeforeSession = token.Shards - SetupScene.Current.shardsThisSession;

        StartCoroutine(IncreaseTextGradually(GO_ShardsText, shardsBeforeSession, token.Shards, 0.2f));

        GO_BossCountText.text = SetupScene.Current.enemysThisSession.ToString();

        GO_Tier1Text.text = token.FragmentsTier1.ToString();
        GO_Tier2Text.text = token.FragmentsTier2.ToString();
        GO_Tier3Text.text = token.FragmentsTier3.ToString();
        GO_Tier4Text.text = token.FragmentsTier4.ToString();
        GO_Tier5Text.text = token.FragmentsTier5.ToString();
    }

    public void ShowVersusUI(Sprite Player, Sprite Enemy, string enemyName)
    {
        VersusPlayer.sprite = Player;
        VersusEnemy.sprite = Enemy;

        VersusUI.SetActive(true);

        VersusUI.GetComponent<Animator>().Play("enter");

        VersusTitle.text = enemyName;

        Invoke("CloseVersusUI", 1.5f);
    }

    public void ShowVersusUI(Sprite enemyIcon, Sprite playerIcon, string enemyName, System.Action onComplete = null)
    {
        VersusPlayer.sprite = playerIcon;
        VersusEnemy.sprite = enemyIcon;

        VersusUI.SetActive(true);

        VersusUI.GetComponent<Animator>().Play("enter");

        VersusTitle.text = enemyName;

        // Play animation...
        // At the end of the animation:
        onComplete?.Invoke();
    }

    private void CloseVersusUI()
    {
        VersusUI.GetComponent<Animator>().Play("exit");
    }

    public void UpdateShardText(int amount)
    {
        shardBox.SetActive(true);

        StartCoroutine(IncreaseTextGradually(shardText, 0, amount, 0.1f));

        shardBox.GetComponent<Animator>().Play("entry");

        Invoke("CloseShardText", 2f);
    }

    private void CloseShardText()
    {
        shardBox.GetComponent<Animator>().Play("exit");
    }

    private IEnumerator IncreaseTextGradually(TextMeshProUGUI Text, int currentAmount, int finalAmount, float stepAmount)
    {
        while (currentAmount < finalAmount)
        {
            currentAmount++;

            Text.text = $"+{currentAmount.ToString()}";

            //TODO Add sound here

            yield return new WaitForSeconds(stepAmount);
        }
    }

    public void ShowFragmentText(BossFragment tier, int amount)
    {
        FragmentBox.SetActive(true);

        Sprite icon = FragmentIcons[(int)tier];

        FragmentImage.sprite = icon;

        FragmentTextAmount.text = "+" + amount.ToString();

        FragmentBox.GetComponent<Animator>().Play("entry");

        Invoke("HideFragmentText", 2f);
    }

    private void HideFragmentText()
    {
        FragmentBox.GetComponent<Animator>().Play("exit");
    }
}
}

