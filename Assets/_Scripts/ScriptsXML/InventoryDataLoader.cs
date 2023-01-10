using UnityEngine;
using System.Xml;

public class InventoryDataLoader : MonoBehaviour
{
    static public InventoryDataLoader S;
    private XmlDocument _inventoryDataXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/InventoryData.xml";
    }

    public EquipmentItemData[] GetEquipment() //Возвращает весь список предметов экипировки
    {
        _inventoryDataXml = new XmlDocument();
        _inventoryDataXml.Load(_path);
        XmlNode xml = _inventoryDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNodeList itemsNodeList = equipmentNode.SelectNodes("item");
        EquipmentItemData[] items = new EquipmentItemData[itemsNodeList.Count];
        for (int i = 0; i < itemsNodeList.Count; i++)
        {
            items[i] = new EquipmentItemData(itemsNodeList[i].Attributes["name"].Value, itemsNodeList[i].Attributes["type"].Value, itemsNodeList[i].Attributes["set"].Value);
        }
        return items;
    }
}
