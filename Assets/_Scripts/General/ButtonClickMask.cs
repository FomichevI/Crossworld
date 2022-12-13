using UnityEngine;
using UnityEngine.UI;

public class ButtonClickMask : MonoBehaviour //�������� �� ������ ������������� �����, ����� ������� ������� ������� ������
    //� ����������� �� ������������ �������
{
    [Range(0f, 1f)]
    public float AlphaLevel = 1f;
    private Image _bt;

    void Start()
    {
        _bt = gameObject.GetComponent<Image>();
        _bt.alphaHitTestMinimumThreshold = AlphaLevel;
    }
}
