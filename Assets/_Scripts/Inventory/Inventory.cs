using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour //�������� �� ������ ���� ������ � �� �������� ����������
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
        //�������� ��������� ��� ������� ������� �� ����������
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
        if (equip.Level <= PlayerDataLoader.S.GetLevel()) //�������� ���� ������ ���� ��� �������� �� ������
        {
            EquipmentItemData oldItem_1 = null;
            EquipmentItemData oldItem_2 = null; //� ������, ����� ����� �������� 2 ���������� ������ �� ���������
            if (equip.Type == "weaponLeft" || equip.Type == "weaponRight")
            {
                EquipmentItemData onPlayer = null;
                if (equip.Type == "weaponLeft") //�������� �� ������, ���� ������ ��������� ������ �� ���������� � ����� ����
                {
                    onPlayer = PlayerDataLoader.S.GetEquippedItem("weaponRight");
                    if (onPlayer.Class != "twoHanded")
                        onPlayer = PlayerDataLoader.S.GetEquippedItem("weaponLeft");
                }
                else
                {
                    onPlayer = PlayerDataLoader.S.GetEquippedItem(equip.Type);
                }

                if (equip.Type == "weaponLeft" && onPlayer.Class == "twoHanded")//���� �������� ������ � ����� ���� � ������� ���������
                {
                    oldItem_1 = PlayerDataLoader.S.GetEquippedItem("weaponRight");
                }
                else if (equip.Type == "weaponRight" && equip.Class == "twoHanded")//���� �������� ��������
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

            //������� ������ ���� � �������� �����
            if (oldItem_1 != null && oldItem_1.Name != "none")
            {
                PlayerDataSaver.S.SetNewItem(new EquipmentItemData(oldItem_1.Type));
                InventoryDataSaver.S.AddEquipmentItem(oldItem_1);
            } //���� ��������� SetOffEquipment, �� ��������� �������� � ����������
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
    public void SetOffEquipment(EquipmentItemData equip) //�������� ���������� ���� �� ��������� �� ������ ���� �� ����
                                                         //� ��������� �� ������ ���� � ���������
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
