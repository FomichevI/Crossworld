using System.Collections.Generic;
using UnityEngine;
using System.Xml;


public class ItemDataLoader : MonoBehaviour
{
    static public ItemDataLoader S;
    private XmlDocument _itemDataXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/ItemsData.xml";
    }

    public void SetAllCharacteristics(EquipmentItemData item) //Загружает характеристики предмета
    {
        if (item.Set != "none")
        {
            _itemDataXml = new XmlDocument();
            _itemDataXml.Load(_path);
            XmlNode xml = _itemDataXml.SelectSingleNode("xml");
            XmlNode setsNode = xml.SelectSingleNode("sets");
            XmlNode currentSetNode = setsNode.SelectSingleNode(item.Set);
            XmlNodeList itemsNodeList = currentSetNode.SelectNodes("item");
            foreach (XmlNode node in itemsNodeList)
            {
                if (node.Attributes["name"].Value == item.Name)
                {
                    item.Level = int.Parse(node.Attributes["level"].Value);
                    if (node.Attributes["maxDamage"] != null) item.MaxDamage = int.Parse(node.Attributes["maxDamage"].Value);
                    else item.MaxDamage = 0;
                    if (node.Attributes["minDamage"] != null) item.MinDamage = int.Parse(node.Attributes["minDamage"].Value);
                    else item.MinDamage = 0;
                    if (node.Attributes["class"] != null)
                        item.Class = node.Attributes["class"].Value;
                    item.Parry = int.Parse(node.Attributes["parry"].Value);
                    item.Penetration = int.Parse(node.Attributes["penetration"].Value);
                    item.Rage = int.Parse(node.Attributes["rage"].Value);
                    item.Rare = node.Attributes["rare"].Value;
                    item.Strength = int.Parse(node.Attributes["strength"].Value);
                    item.Survivability = int.Parse(node.Attributes["survivability"].Value);
                    item.Agility = int.Parse(node.Attributes["agility"].Value);
                    item.Defense = int.Parse(node.Attributes["defense"].Value);

                    break;
                }
            }
        }
    }

    public List<EquipmentItemData> GetAllItems(string rare, int minLvl, int maxLvl) //Возвращает все предметы, подходящие под заданные условия
    {
        List<EquipmentItemData> items = new List<EquipmentItemData>();
        _itemDataXml = new XmlDocument();
        _itemDataXml.Load(_path);
        XmlNode xml = _itemDataXml.SelectSingleNode("xml");
        XmlNode setsNode = xml.SelectSingleNode("sets");
        XmlNodeList setsNodeList = setsNode.ChildNodes;
        for (int i = 0; i < setsNodeList.Count; i++)
        {
            XmlNode itemNode = setsNodeList[i].SelectSingleNode("item");
            if (itemNode.Attributes["rare"].Value != rare) { continue; }
            else
            {
                XmlNodeList itemsNodeList = setsNodeList[i].SelectNodes("item");
                foreach (XmlNode node in itemsNodeList)
                {
                    if (int.Parse(node.Attributes["level"].Value) >= minLvl && int.Parse(node.Attributes["level"].Value) <= maxLvl)
                    {
                        EquipmentItemData item = new EquipmentItemData(node.Attributes["name"].Value, node.Attributes["type"].Value,
                            node.Attributes["set"].Value);
                        items.Add(item);
                    }
                }
            }
        }
        return items;
    }
}
