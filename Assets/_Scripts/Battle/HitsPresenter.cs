using UnityEngine;
using TMPro;

public class HitsPresenter : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _yOffset;
    [SerializeField] private int _xOffset;
    [SerializeField] private TextMeshProUGUI[] _damageTexts;
    private float[] _damageTextsTimeLife;
    [SerializeField] private RectTransform _playerSpavnPoinTrans;
    [SerializeField] private RectTransform _enemySpavnPoinTrans;
    private BattleData _battleData;

    private void OnEnable()
    {
        BattleController.CompleteHitEvent.AddListener(ShowDamage);
    }
    private void Awake()
    {
        _battleData = Resources.Load<BattleData>("ScriptableObjects/BattleData");
    }
    private void Start()
    {
        _damageTextsTimeLife = new float[_damageTexts.Length];
        for (int i = 0; i < _damageTextsTimeLife.Length; i++)
            _damageTextsTimeLife[i] = _lifeTime;
        foreach (TextMeshProUGUI tmp in _damageTexts)
            tmp.gameObject.SetActive(false);
    }

    void FixedUpdate() //����������� ��� �������� ������ � ��������� ��, � ������� ���������� ���� �����
    {
        for (int i = 0; i < _damageTexts.Length; i++)
        {
            if (_damageTexts[i].gameObject.activeSelf)
            {
                _damageTextsTimeLife[i] -= 0.02f;
                if (_damageTextsTimeLife[i] <= 0)
                {
                    _damageTexts[i].gameObject.SetActive(false);
                    _damageTextsTimeLife[i] = _lifeTime;
                }
                else
                {
                    _damageTexts[i].rectTransform.position = new Vector3(_damageTexts[i].rectTransform.position.x, _damageTexts[i].rectTransform.position.y + _moveSpeed);
                }
            }
        }
    }

    public void ShowDamage(int damage, bool forPlayer)
    {
        TextMeshProUGUI newText = _damageTexts[1];
        foreach (TextMeshProUGUI tmp in _damageTexts)
            if (!tmp.gameObject.activeSelf)
            {
                newText = tmp;
                break;
            }

        if (_battleData.TypeOfCurrentHit == TypeOfHit.miss)
        {
            //������������� ���� 
            newText.color = Color.green;
            newText.outlineColor = Color.green;
            //������������� ��������
            newText.text = "������";
        }
        else if(_battleData.TypeOfCurrentHit == TypeOfHit.blocked)
        {
            //������������� ���� 
            newText.color = Color.blue;
            newText.outlineColor = Color.blue;
            //������������� ��������
            newText.text = "����\n" + damage.ToString();
        }
        else if (_battleData.TypeOfCurrentHit == TypeOfHit.criticalBlocked)
        {
            newText.color = Color.white;
            newText.outlineColor = Color.white;
            string t = "<color=blue>����</color>\n" + "<color=red>" + damage + "</color>";
            newText.text = t;
        }
        else if (_battleData.TypeOfCurrentHit == TypeOfHit.critical)
        {
            //������������� ���� 
            newText.color = Color.red;
            newText.outlineColor = Color.red;
            //������������� ��������
            newText.text = damage.ToString();
        }
        else
        {
            //������������� ���� 
            newText.color = Color.white;
            newText.outlineColor = Color.white;            
            //������������� ��������
            newText.text = damage.ToString();
        }
        //������������� �������
        if (!forPlayer)
        {
            newText.rectTransform.position = new Vector3(_enemySpavnPoinTrans.position.x + Random.Range(-_xOffset, _xOffset),
                _enemySpavnPoinTrans.position.y + Random.Range(-_yOffset, _yOffset));
        }
        else
        {
            newText.rectTransform.position = new Vector3(_playerSpavnPoinTrans.position.x + Random.Range(-_xOffset, _xOffset),
                _playerSpavnPoinTrans.position.y + Random.Range(-_yOffset, _yOffset));
        }
        newText.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        BattleController.CompleteHitEvent.RemoveListener(ShowDamage);
    }
}
