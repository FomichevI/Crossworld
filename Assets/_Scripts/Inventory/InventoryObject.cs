using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TypeItem {empty, equipment, loot}

public class InventoryObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool OnPlayer = false; //Айтем надет на персонаже или находится в инвентаре
    public TypeItem Type;
    private EquipmentItemData _equipment; //Только для экипировки
    //Визуальное отображение
    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;

    public void UseItem()
    {
        if (Type == TypeItem.equipment)
        {
            if (!OnPlayer)            
                Inventory.S.SetOnEquipment(_equipment);
            else
                Inventory.S.SetOffEquipment(_equipment);
        }
    }

    public void Initialization(TypeItem type, EquipmentItemData item) //Загрузка и отображение данных
    {
        Type = type;
        if (type != TypeItem.empty)
        {
            _equipment = item;
            if (_equipment.Rare == "rare")
                _bg.color = Color.blue;
            else if (_equipment.Rare == "usual")
                _bg.color = Color.gray;
            else if (_equipment.Rare == "unusual")
                _bg.color = Color.green;
            _icon.sprite = Resources.Load<Sprite>("_Images/Equipment/" + _equipment.Set + "/" + _equipment.Name);
        }
        else
        {
            _equipment = null;
            _bg.color = Color.gray;
            _icon.sprite = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FloatingPanel.S.Show(_bg, _icon, _equipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        FloatingPanel.S.Hide();
    }
}
