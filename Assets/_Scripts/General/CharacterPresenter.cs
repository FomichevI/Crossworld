using UnityEngine;

public class CharacterPresenter : MonoBehaviour //�������� �� ����� �������� ���� ��������� � ������������ ���� �������� � ����������� �� ���� ������
{
    [SerializeField] private SkinnedMeshRenderer _helmetMr;
    [SerializeField] private SkinnedMeshRenderer _cuirassMr;
    [SerializeField] private SkinnedMeshRenderer _beardMr;
    [SerializeField] private SkinnedMeshRenderer _hairMr;
    [SerializeField] private SkinnedMeshRenderer _bootsMr;
    [SerializeField] private SkinnedMeshRenderer _bracersMr;
    [SerializeField] private SkinnedMeshRenderer _headMr;
    [SerializeField] private SkinnedMeshRenderer _greavesMr;
    [SerializeField] private SkinnedMeshRenderer _shoulderPadsMr;
    [SerializeField] private SkinnedMeshRenderer _weaponRightMr;
    [SerializeField] private SkinnedMeshRenderer _weaponLeftMr;
    [SerializeField] private SkinnedMeshRenderer _shildMr;
    [SerializeField] private SkinnedMeshRenderer _twoHandedWeaponMr;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        LoadLook();
    }

    public void LoadLook()
    {
        EquipmentItemData[] inventoryItems = PlayerDataLoader.S.GetEquip();
        SkinnedMeshRenderer currentItem = null;
        //���������� ��� �������� ���������� + ������ � ������. ��������� �� ��������� ������
        _headMr.gameObject.SetActive(true);
        _beardMr.gameObject.SetActive(true);
        _helmetMr.gameObject.SetActive(true);
        _hairMr.gameObject.SetActive(true);
        _shoulderPadsMr.gameObject.SetActive(true);
        _weaponRightMr.gameObject.SetActive(false);
        _shildMr.gameObject.SetActive(false);
        _weaponLeftMr.gameObject.SetActive(false);
        _twoHandedWeaponMr.gameObject.SetActive(false);
        foreach (EquipmentItemData item in inventoryItems)
        {
            //�������� ������� �������
            if (item.Type == "helmet") { currentItem = _helmetMr; }
            else if (item.Type == "bracers") { currentItem = _bracersMr; }
            else if (item.Type == "shoulderPads") { currentItem = _shoulderPadsMr; }
            //����������, ����� ������ ��������
            else if (item.Type == "weaponRight")
            {
                if (item.Class == "oneHanded")
                {
                    currentItem = _weaponRightMr;
                    currentItem.gameObject.SetActive(true);
                }
                else
                {
                    currentItem = _twoHandedWeaponMr;
                    currentItem.gameObject.SetActive(true);
                }
            }            
            else if (item.Type == "weaponLeft")
            {
                if (item.Class == "oneHanded")
                {
                    currentItem = _weaponLeftMr;
                    currentItem.gameObject.SetActive(true);
                }
                else
                {
                    currentItem = _shildMr;
                    currentItem.gameObject.SetActive(true);
                }
            }
            else if (item.Type == "cuirass") { currentItem = _cuirassMr; }
            else if (item.Type == "greaves") { currentItem = _greavesMr; }
            else if (item.Type == "boots") { currentItem = _bootsMr; }
            else if (item.Type == "shirt") { continue; }

            if (item.Name != "none")//���� ���� �������� �� ������, �� ���������� ����������� �������
            {
                currentItem.sharedMesh = Resources.Load<Mesh>("_Models/Characters/Set/" + item.Set + "/" + item.Type);
                currentItem.material = Resources.Load<Material>("_Models/Characters/Set/" + item.Set + "/SetMaterial");
                if (item.Type == "helmet") //�������� ������, ��� ��������� ������ � ����������� �� ������ �����
                {
                    if (item.Class == "helf") { _hairMr.gameObject.SetActive(false); }
                    else if (item.Class == "full")
                    {
                        _hairMr.gameObject.SetActive(false);
                        _headMr.gameObject.SetActive(false);
                        _beardMr.gameObject.SetActive(false);
                    }
                }
            }
            else//� ��������� ������ ���� ��������� ����������� ��������, ���� ���������� �����������
            {
                if (currentItem == _helmetMr || currentItem == _shoulderPadsMr || currentItem == _weaponRightMr || currentItem == _weaponLeftMr || currentItem == _twoHandedWeaponMr
                    || currentItem == _shildMr)
                {
                    currentItem.gameObject.SetActive(false);
                }
                else
                {
                    currentItem.sharedMesh = Resources.Load<Mesh>("_Models/Characters/Set/simple/" + item.Type);
                    currentItem.material = Resources.Load<Material>("_Models/Characters/Set/simple/SetMaterial");
                }
            }
        }

        //������ �������� � ����������� �� ��������� ������
        if (_twoHandedWeaponMr.gameObject.activeSelf)
            _animator.SetTrigger("TwoHandedType");
        else if (_weaponLeftMr.gameObject.activeSelf)
            _animator.SetTrigger("DualType");
        else
            _animator.SetTrigger("ShildType");
    }
}
