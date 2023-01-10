using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingPanel : MonoBehaviour
{
    public static FloatingPanel S;
    [SerializeField] private GameObject _child;
    private bool _isShowing = false;

    [SerializeField] private TextMeshProUGUI _descriptionText; //Описание предмета
    [SerializeField] private TextMeshProUGUI _nameItemText; //Название предмета
    [SerializeField] private TextMeshProUGUI _characteristicValueText; //Характеристики самого предмета
    [SerializeField] private TextMeshProUGUI _characteristicCompareText; //Сравнение характеристик предметов
    [Header("Настройки иконки")]
    [SerializeField] private Image _itemBg;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private RectTransform _bgFildTrans;

    private void Awake()
    {
        S = this;
    }
    public void Show(Image bg, Image icon, EquipmentItemData item)
    {
        if (item != null)
        {
            _itemBg.color = bg.color;
            _itemIcon.sprite = icon.sprite;
            _child.SetActive(true);
            _isShowing = true;
            GenerateEquipDescription(item);
        }
    }
    private void GenerateEquipDescription(EquipmentItemData equip)
    {
        string description = "";
        string charValue = "";
        string charCompare = "";
        EquipmentItemData wornItem = null;
        if (equip.Type == "weaponLeft")//Если надето двуручное оружие, то оружие для левой руки сравниваем с ним
        {
            EquipmentItemData rightWeapon = PlayerDataLoader.S.GetEquippedItem("weaponRight");
            if (rightWeapon.Class == "twoHanded")
                wornItem = PlayerDataLoader.S.GetEquippedItem("weaponRight");
            else
                wornItem = PlayerDataLoader.S.GetEquippedItem(equip.Type);
        }
        else
        {
            wornItem = PlayerDataLoader.S.GetEquippedItem(equip.Type);
        }
        //Первая строчка - уровень предмета
        int playerLevel = PlayerDataLoader.S.GetLevel();
        description += LanguageDataLoader.S.GetCharacteristic("level") + "\n\n";
        charValue += equip.Level + "\n\n";
        if (equip.Level <= playerLevel)
            charCompare += "<color=green>" + playerLevel + "</color>" + "\n\n";
        else
            charCompare += "<color=red>" + playerLevel + "</color>" + "\n\n";
        //Вторая строчка - урон
        if (equip.MinDamage != 0 || (equip.MinDamage - wornItem.MinDamage) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("minDamage") + "\n";
            charValue += equip.MinDamage + "\n";
            if (equip.MinDamage - wornItem.MinDamage < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.MinDamage - wornItem.MinDamage) + "</color>" + "\n";
            else if (equip.MinDamage - wornItem.MinDamage == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.MinDamage - wornItem.MinDamage) + "</color>" + "\n";

            if (equip.MaxDamage == 0 && (equip.MinDamage - wornItem.MinDamage) == 0)
            {
                description += "\n";
                charValue += "\n";
                charCompare += "\n";
            }
        }
        if (equip.MaxDamage != 0 || (equip.MaxDamage - wornItem.MaxDamage) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("maxDamage") + "\n\n";
            charValue += equip.MaxDamage + "\n\n";
            if ((equip.MaxDamage - wornItem.MaxDamage) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.MaxDamage - wornItem.MaxDamage) + "</color>" + "\n\n";
            else if (equip.MaxDamage - wornItem.MaxDamage == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.MaxDamage - wornItem.MaxDamage) + "</color>" + "\n\n";
        }
        //Последующие строчки - характеристики
        if (equip.Strength != 0 || (equip.Strength - wornItem.Strength) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("strength") + "\n";
            charValue += equip.Strength + "\n";
            if ((equip.Strength - wornItem.Strength) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Strength - wornItem.Strength) + "</color>" + "\n";
            else if (equip.Strength - wornItem.Strength == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Strength - wornItem.Strength) + "</color>" + "\n";
        }
        if (equip.Survivability != 0 || (equip.Survivability - wornItem.Survivability) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("survivability") + "\n";
            charValue += equip.Survivability + "\n";
            if ((equip.Survivability - wornItem.Survivability) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Survivability - wornItem.Survivability) + "</color>" + "\n";
            else if (equip.Survivability - wornItem.Survivability == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Survivability - wornItem.Survivability) + "</color>" + "\n";
        }
        if (equip.Rage != 0 || (equip.Rage - wornItem.Rage) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("rage") + "\n";
            charValue += equip.Rage + "\n";
            if ((equip.Rage - wornItem.Rage) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Rage - wornItem.Rage) + "</color>" + "\n";
            else if (equip.Rage - wornItem.Rage == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Rage - wornItem.Rage) + "</color>" + "\n";
        }
        if (equip.Agility != 0 || (equip.Agility - wornItem.Agility) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("agility") + "\n";
            charValue += equip.Agility + "\n";
            if ((equip.Agility - wornItem.Agility) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Agility - wornItem.Agility) + "</color>" + "\n";
            else if (equip.Agility - wornItem.Agility == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Agility - wornItem.Agility) + "</color>" + "\n";
        }
        if (equip.Parry != 0 || (equip.Parry - wornItem.Parry) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("parry") + "\n";
            charValue += equip.Parry + "\n";
            if ((equip.Parry - wornItem.Parry) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Parry - wornItem.Parry) + "</color>" + "\n";
            else if (equip.Parry - wornItem.Parry == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Parry - wornItem.Parry) + "</color>" + "\n";
        }
        if (equip.Defense != 0 || (equip.Defense - wornItem.Defense) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("defense") + "\n";
            charValue += equip.Defense + "\n";
            if ((equip.Defense - wornItem.Defense) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Defense - wornItem.Defense) + "</color>" + "\n";
            else if (equip.Defense - wornItem.Defense == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Defense - wornItem.Defense) + "</color>" + "\n";
        }
        if (equip.Penetration != 0 || (equip.Penetration - wornItem.Penetration) != 0)
        {
            description += LanguageDataLoader.S.GetCharacteristic("penetration") + "\n";
            charValue += equip.Penetration + "\n";
            if ((equip.Penetration - wornItem.Penetration) < 0)
                charCompare += "<color=red>" + Mathf.Abs(equip.Penetration - wornItem.Penetration) + "</color>" + "\n";
            else if (equip.Penetration - wornItem.Penetration == 0)
                charCompare += "\n";
            else
                charCompare += "<color=green>" + (equip.Penetration - wornItem.Penetration) + "</color>" + "\n";
        }

        _nameItemText.text = LanguageDataLoader.S.GetEquipment(equip.Set, equip.Name);
        _descriptionText.text = description;
        _characteristicValueText.text = charValue;
        _characteristicCompareText.text = charCompare;
    }
    public void Hide()
    {
        _child.SetActive(false);
        _isShowing = false;
    }

    private void Update()
    {
        if (_isShowing)
        {
            Vector2 newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Screen.width - newPos.x - _bgFildTrans.sizeDelta.x -20 < 0)
                newPos.x = Screen.width - _bgFildTrans.sizeDelta.x -20;
            newPos.x += 100;
            newPos.y -= 10;
            transform.position = newPos;
        }
        if (Input.GetMouseButtonDown(0))
            Hide();
    }
}
