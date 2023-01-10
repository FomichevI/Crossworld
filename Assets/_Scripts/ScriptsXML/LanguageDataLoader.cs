using UnityEngine;
using System.Xml;


public class LanguageDataLoader : MonoBehaviour
{
    static public LanguageDataLoader S;
    private XmlDocument _languageXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/RuLanguage.xml";
    }

    public string GetCharacteristic(string title) //Возвращает переведенное название конкретной характеристики
    {
        _languageXml = new XmlDocument();
        _languageXml.Load(_path);
        XmlNode xml = _languageXml.SelectSingleNode("xml");
        XmlNode characteristicsNode = xml.SelectSingleNode("characteristics");
        XmlNode currentCharNode = characteristicsNode.SelectSingleNode(title);
        return currentCharNode.Attributes["tc"].Value;
    }
    public string GetEquipment(string set, string title) //Возвращает переведенное название снаряжения
    {
        _languageXml = new XmlDocument();
        _languageXml.Load(_path);
        XmlNode xml = _languageXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNode setsNode = equipmentNode.SelectSingleNode("sets");
        XmlNode currentSetNode = setsNode.SelectSingleNode(set);
        XmlNode itemNode = currentSetNode.SelectSingleNode(title);
        return itemNode.Attributes["tc"].Value;
    }
}

