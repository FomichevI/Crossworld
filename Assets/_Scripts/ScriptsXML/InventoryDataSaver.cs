using UnityEngine;
using System.Xml;

public class InventoryDataSaver : MonoBehaviour
{
    static public InventoryDataSaver S;
    private XmlDocument _inventoryDataXml;
    private string _path;

    void Awake()
    {
        S = this;
            _path = Application.dataPath + "/StreamingAssets/Data/InventoryData.xml";
    }

    public void AddEquipmentItem(EquipmentItemData item) //Добавляет в инвентарь новый предмет
    {
        _inventoryDataXml = new XmlDocument();
        _inventoryDataXml.Load(_path);
        XmlNode xml = _inventoryDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");

        //Создаем новый элемент
        XmlElement newItemElem = _inventoryDataXml.CreateElement("item");
        XmlAttribute nameAtt = _inventoryDataXml.CreateAttribute("name");
        XmlText nameText = _inventoryDataXml.CreateTextNode(item.Name);
        XmlAttribute typeAtt = _inventoryDataXml.CreateAttribute("type");
        XmlText typeText = _inventoryDataXml.CreateTextNode(item.Type);
        XmlAttribute setAtt = _inventoryDataXml.CreateAttribute("set");
        XmlText setText = _inventoryDataXml.CreateTextNode(item.Set);
        nameAtt.AppendChild(nameText);
        typeAtt.AppendChild(typeText);
        setAtt.AppendChild(setText);
        newItemElem.Attributes.Append(nameAtt);
        newItemElem.Attributes.Append(typeAtt);
        newItemElem.Attributes.Append(setAtt);
        equipmentNode.AppendChild(newItemElem);

        _inventoryDataXml.Save(_path);
    }

    public void RemoveEquipmentItem(EquipmentItemData item) //Удаляет предмет из инвентаря
    {
        _inventoryDataXml = new XmlDocument();
        _inventoryDataXml.Load(_path);
        XmlNode xml = _inventoryDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNodeList itemsNodeList = equipmentNode.SelectNodes("item");
        foreach(XmlNode node in itemsNodeList)
        {
            if(node.Attributes["name"].Value == item.Name)
            {
                node.ParentNode.RemoveChild(node);
                break;
            }
        }
        _inventoryDataXml.Save(_path);
    }
}
