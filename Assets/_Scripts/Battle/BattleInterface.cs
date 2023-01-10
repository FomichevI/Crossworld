using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleInterface : MonoBehaviour
{
    public static BattleInterface S;
    //***********Временно, пока скилы игрока еще в разработке***********
    public Button[] SkillButtons;
    public Image[] SkillFrames;

    [SerializeField] private MobBar _enemyBar;
    [SerializeField] private PlayerBar _playerBar;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _winLootContent;
    [SerializeField] private TextMeshProUGUI _expValueText;
    private BattleData _battleData;

    [SerializeField] private GameObject _hitDirectionsButtons;


    private void OnEnable()
    {
        BattleController.ShowHitButtonsEvent.AddListener(ShowHitDirectionButtons);
        BattleController.FinishFightEvent.AddListener(FinishFight);
    }

    void Awake()
    {
        if (S == null)
            S = this;
        _battleData = Resources.Load<BattleData>("ScriptableObjects/BattleData");
    }
    private void Start()
    {
        BattleController.CompleteHitEvent.AddListener(RefreshHitBars);
        RefreshHitBars();
    }
    public void ShowHitDirectionButtons()
    {
        _hitDirectionsButtons.SetActive(true);
    }
    public void RefreshHitBars() //Для первого обновления хитбаров
    {
        _enemyBar.RefreshHpBar(_battleData.Enemy.Characteristics.MaxHp);
        _playerBar.RefreshHpBar(_battleData.Player.Characteristics.MaxHp);
    }
    public void RefreshHitBars(int damage, bool forPlayer) //Отобразить новое состояние здоровья на хитбарах
    {
        if (_battleData.TypeOfCurrentHit != TypeOfHit.miss)
        {
            if (forPlayer)
            {
                BattleCharacter player = _battleData.Player;
                if ((player.CurrentHp) > 0)
                    _playerBar.RefreshHpBar(player.CurrentHp, player.Characteristics.MaxHp);
                else
                    _playerBar.RefreshHpBar(0, player.Characteristics.MaxHp);
            }
            else
            {
                BattleMob mob = _battleData.Enemy;
                if (mob.CurrentHp > 0)
                    _enemyBar.RefreshHpBar(mob.CurrentHp, mob.Characteristics.MaxHp);
                else
                    _enemyBar.RefreshHpBar(0, mob.Characteristics.MaxHp);
                RefreshFilledCombo();
            }
        }
    }
    public void FinishFight(bool isWin)
    {
        if (isWin)
        {
            _winPanel.SetActive(true);
        }
        else
        { 
            _losePanel.SetActive(true);
        }
        Loot loot = _battleData.Loot;
        if (loot != null)
        {
            _expValueText.text = loot.Experience.ToString();
            if (loot.Items != null)
            {
                GameObject _inventoryObjectPrefab = Resources.Load<GameObject>("Prefabs/Interface/InventoryObject");
                foreach (EquipmentItemData item in loot.Items)
                {
                    GameObject iconGo = Instantiate<GameObject>(_inventoryObjectPrefab, _winLootContent.transform);
                    iconGo.GetComponent<InventoryObject>().Initialization(TypeItem.loot, item);
                }
            }
        }
    }

    //Методы для кнопок удара (использует только игрок)
    public void MakeHitUp()
    {
        BattleController.MakeHitPlayer?.Invoke(HitDirection.up);
        _hitDirectionsButtons.SetActive(false);
    }
    public void MakeHitMidle()
    {
        BattleController.MakeHitPlayer?.Invoke(HitDirection.midle);
        _hitDirectionsButtons.SetActive(false);
    }
    public void MakeHitDown()
    {
        BattleController.MakeHitPlayer?.Invoke(HitDirection.down);
        _hitDirectionsButtons.SetActive(false);
    }
    public void UseSkill(int number)
    {
        if (_hitDirectionsButtons.activeSelf)
        {
            int filledCombo = _battleData.FilledCombo;
            BattleCharacter player = _battleData.Player;
            if (filledCombo - number >= 0 && player.Skills[number - 1].HitName != "")
            {
                BattleController.S.ChangeFilledCombo(-number);
                if (filledCombo - number < 2)
                    SetSkillUnactive(2);
                player.UseSkill(number);
                _hitDirectionsButtons.SetActive(false);
                RefreshFilledCombo();
            }
        }
    }
    private void RefreshFilledCombo() //Обновить цвет комбо иконок
    {
        int currentValue = _battleData.FilledCombo;
        for (int i = 0; i < 7; i++)
        {
            if (i+1 <= currentValue) SkillFrames[i].color = Color.green;
            else SkillFrames[i].color = Color.white;
        }
        if (currentValue >= 2)
            SetSkillActive(2);
    } 

    public void SetSkillActive(int skillNumber)
    {
        SkillButtons[skillNumber - 1].interactable = true;
    }
    public void SetSkillUnactive(int skillNumber)
    {
        SkillButtons[skillNumber - 1].interactable = false;
    }

    public void LoadMapScene()
    {
        PlayerPrefs.SetInt("SceneToLoad", 0);
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnDisable()
    {
        BattleController.CompleteHitEvent.RemoveListener(RefreshHitBars);
        BattleController.ShowHitButtonsEvent.RemoveListener(ShowHitDirectionButtons);
        BattleController.FinishFightEvent.RemoveListener(FinishFight);
    }
}
