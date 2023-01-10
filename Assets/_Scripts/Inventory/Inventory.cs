using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour //Отвечает за расчет всех данных и их передачу интарфейсу
{
    public static Inventory S;
    public static UnityEvent RefreshInventoryEvent = new UnityEvent(); 
    private Characteristics _characteristics; public Characteristics Characteristics { get { return _characteristics; } }
    [SerializeField] private CharacterPresenter _presenter;
    private GameObject _player;
    private UiAudio _uiAudio;

    private void Awake()
    {
        S = this;
        _characteristics = new Characteristics();
        _player = _presenter.gameObject;
        _uiAudio = GetComponent<UiAudio>();
    }

    private void Update()
    {
        //Вращение персонажа при зажаток стрелке на клавиатуре
        if (Input.GetKey(KeyCode.LeftArrow))
            _player.transform.Rotate(0,1,0);
        if (Input.GetKey(KeyCode.RightArrow))
            _player.transform.Rotate(0,-1,0);
    }

    private void Start()
    {
        _characteristics.SetCharacteristics("player");
        RefreshInventoryEvent?.Invoke();
    }

    public void SetOnEquipment(EquipmentItemData equip)
    {
        if (equip.Level <= PlayerDataLoader.S.GetLevel()) //Надеваем вещь только если она подходит по уровню
        {
            EquipmentItemData oldItem_1 = null;
            EquipmentItemData oldItem_2 = null; //В случае, когда нужно поменять 2 одноручных оружия на двуручное
            if (equip.Type == "weaponLeft" || equip.Type == "weaponRight")
            {
                EquipmentItemData onPlayer = null;
                if (equip.Type == "weaponLeft") //Проверка на случай, если меняем двуручное оружие на одноручное в левую руку
                {
                    onPlayer = PlayerDataLoader.S.GetEquippedItem("weaponRight");
                    if (onPlayer.Class != "twoHanded")
                        onPlayer = PlayerDataLoader.S.GetEquippedItem("weaponLeft");
                }
                else
                {
                    onPlayer = PlayerDataLoader.S.GetEquippedItem(equip.Type);
                }

                if (equip.Type == "weaponLeft" && onPlayer.Class == "twoHanded")//Если надеваем оружие в левую руку с надетой двуручкой
                {
                    oldItem_1 = PlayerDataLoader.S.GetEquippedItem("weaponRight");
                }
                else if (equip.Type == "weaponRight" && equip.Class == "twoHanded")//Если надеваем двуручку
                {
                    oldItem_1 = PlayerDataLoader.S.GetEquippedItem(equip.Type);
                    oldItem_2 = PlayerDataLoader.S.GetEquippedItem("weaponLeft");
                }
                else
                {
                    oldItem_1 = PlayerDataLoader.S.GetEquippedItem(equip.Type);
                }
            }
            else
            {
                oldItem_1 = PlayerDataLoader.S.GetEquippedItem(equip.Type);
            }

            //Снимаем старые вещи и надеваем новые
            if (oldItem_1 != null && oldItem_1.Name != "none")
            {
                PlayerDataSaver.S.SetNewItem(new EquipmentItemData(oldItem_1.Type));
                InventoryDataSaver.S.AddEquipmentItem(oldItem_1);
            } //Если применить SetOffEquipment, то возникает проблема с аниматором
            if (oldItem_2 != null && oldItem_2.Name != "none")
            {
                PlayerDataSaver.S.SetNewItem(new EquipmentItemData(oldItem_2.Type));
                InventoryDataSaver.S.AddEquipmentItem(oldItem_2);
            }
            PlayerDataSaver.S.SetNewItem(equip);
            InventoryDataSaver.S.RemoveEquipmentItem(equip);

            _uiAudio.PlaySetOn();
            RefreshAll();
        }
        else
        {
            _uiAudio.PlayCantUse();
        }
    }
    public void SetOffEquipment(EquipmentItemData equip) //Заменяем конкретную вещь на персонаже на пустую того же типа
                                                         //и добавляем не пустую вещь в инвентарь
    {
        PlayerDataSaver.S.SetNewItem(new EquipmentItemData(equip.Type));
        InventoryDataSaver.S.AddEquipmentItem(equip);
        _uiAudio.PlaySetOn();
        RefreshAll();
    }

    private void RefreshAll()
    {
        _characteristics.SetCharacteristics("player");
        RefreshInventoryEvent?.Invoke();
        _presenter.LoadLook();
    }
}
