using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryInterface : MonoBehaviour //Отвечает за отображение всех данных на сцене игтерфейса
{
    [SerializeField] private GameObject _inventoryEquipmentContent; //Контент, содержащий экипировку
    [SerializeField] private GameObject _inventoryObjectPrefab; //Префаб элемента экипировки
    [Header("Элементы экипировки персонажа")]
    [SerializeField] private InventoryObject _helmetEquipped;
    [SerializeField] private InventoryObject _bracersEquipped;
    [SerializeField] private InventoryObject _shoulderPadsEquipped;
    [SerializeField] private InventoryObject _weaponRightEquipped;
    [SerializeField] private InventoryObject _weaponLeftEquipped;
    [SerializeField] private InventoryObject _cuirassEquipped;
    [SerializeField] private InventoryObject _greavesEquipped;
    [SerializeField] private InventoryObject _bootsEquipped;
    [SerializeField] private InventoryObject _shirtEquipped;
    [SerializeField] private PlayerBar _playerBar;
    [Header("Тексты характеристик персонажа")]
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _strengthText;
    [SerializeField] private TextMeshProUGUI _agilityText;
    [SerializeField] private TextMeshProUGUI _parryText;
    [SerializeField] private TextMeshProUGUI _rageText;
    [SerializeField] private TextMeshProUGUI _survivabilityText;
    [SerializeField] private TextMeshProUGUI _defenseText;
    [SerializeField] private TextMeshProUGUI _penetrationText;

    private void OnEnable()
    {
        Inventory.RefreshInventoryEvent.AddListener(RefreshInventory);
        Inventory.RefreshInventoryEvent.AddListener(RefreshCharacteristics);
    }

    private void RefreshCharacteristics()
    {
        Characteristics charac = Inventory.S.Characteristics;
        _hpText.text = charac.MaxHp.ToString();
        _damageText.text = charac.TotalMinDamage.ToString() + "-" + charac.TotalMaxDamage.ToString();
        _strengthText.text = charac.Strength.ToString();
        _agilityText.text = charac.Agility.ToString();
        _parryText.text = charac.Parry.ToString();
        _rageText.text = charac.Rage.ToString();
        _survivabilityText.text = charac.Survivability.ToString();
        _defenseText.text = charac.Defense.ToString();
        _penetrationText.text = charac.Penetration.ToString();
        _playerBar.RefreshHpBar(charac.MaxHp);
    }

    private void RefreshInventory()
    {
        //Обновляем окно инвентаря
        foreach (Transform child in _inventoryEquipmentContent.transform) Destroy(child.gameObject);
        EquipmentItemData[] inventoryItems = InventoryDataLoader.S.GetEquipment();
        foreach (EquipmentItemData item in inventoryItems)
        {
            //Выбор типа
            TypeItem type = TypeItem.equipment;
            if (item.Type == "none")
                type = TypeItem.empty;
            GameObject iconGo = Instantiate<GameObject>(_inventoryObjectPrefab, _inventoryEquipmentContent.transform);

            iconGo.GetComponent<InventoryObject>().Initialization(type, item);
        }
        //Обновляем окно персонажа
        inventoryItems = PlayerDataLoader.S.GetEquip();
        foreach (EquipmentItemData item in inventoryItems)
        {
            TypeItem type;
            if (item.Name != "none")
                type = TypeItem.equipment;
            else
                type = TypeItem.empty;
            if (item.Type == "helmet") { _helmetEquipped.Initialization(type, item); }
            else if (item.Type == "bracers") { _bracersEquipped.Initialization(type, item); }
            else if (item.Type == "shoulderPads") { _shoulderPadsEquipped.Initialization(type, item); }
            else if (item.Type == "weaponRight")
            {
                _weaponRightEquipped.Initialization(type, item);
                if (item.Class == "twoHanded")
                    _weaponLeftEquipped.Initialization(type, item);
            }
            else if (item.Type == "weaponLeft") { _weaponLeftEquipped.Initialization(type, item); }
            else if (item.Type == "cuirass") { _cuirassEquipped.Initialization(type, item); }
            else if (item.Type == "greaves") { _greavesEquipped.Initialization(type, item); }
            else if (item.Type == "boots") { _bootsEquipped.Initialization(type, item); }
            else if (item.Type == "shirt") { _shirtEquipped.Initialization(type, item); }
        }
    }
    public void LoadMapScene()
    {
        PlayerPrefs.SetInt("SceneToLoad", 0);
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnDisable()
    {
        Inventory.RefreshInventoryEvent.RemoveListener(RefreshInventory);
        Inventory.RefreshInventoryEvent.RemoveListener(RefreshCharacteristics);
    }
}
