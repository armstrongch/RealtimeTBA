using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;

namespace RoadTrip
{
    public partial class Game
    {
        private void SaveGame()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //<savedata>
            XmlNode rootNode = xmlDoc.CreateElement("savedata");
            xmlDoc.AppendChild(rootNode);

            //<player>
            XmlNode playerNode = xmlDoc.CreateElement("player");

            XmlAttribute playerName = xmlDoc.CreateAttribute("name");
            playerName.Value = Player.Name;
            playerNode.Attributes.Append(playerName);

            //<currentLocation>
            XmlNode currentLocation = xmlDoc.CreateElement("currentLocation");
            currentLocation.InnerText = Player.CurrentLocation.Name;
            playerNode.AppendChild(currentLocation);

            //<attributeList>
            XmlNode skills = xmlDoc.CreateElement("skills");
            for (int i = 0; i < Player.SkillValues.Count; i += 1)
            {
                XmlNode skill = xmlDoc.CreateElement("skill");

                XmlAttribute attributeType = xmlDoc.CreateAttribute("type");
                SKILLS type = Player.SkillValues.Keys.ElementAt(i);
                attributeType.Value = type.ToString();
                skill.Attributes.Append(attributeType);

                XmlAttribute attributeValue = xmlDoc.CreateAttribute("value");
                attributeValue.Value = Player.SkillValues[type].ToString();
                skill.Attributes.Append(attributeValue);

                skills.AppendChild(skill);
            }
            playerNode.AppendChild(skills);

            rootNode.AppendChild(playerNode);

            //<locationsList>
            XmlNode locationList = xmlDoc.CreateElement("locationList");
            rootNode.AppendChild(locationList);

            foreach (Location location in Locations)
            {
                //<location locationName="Name">
                XmlNode locationNode = xmlDoc.CreateElement("location");

                XmlAttribute locationName = xmlDoc.CreateAttribute("locationName");
                locationName.Value = location.Name;
                locationNode.Attributes.Append(locationName);

                //<description>
                XmlNode description = xmlDoc.CreateElement("description");
                description.InnerText = location.Description;
                locationNode.AppendChild(description);

                //<itemList>
                XmlNode itemList = xmlDoc.CreateElement("itemList");
                foreach (string itemName in location.GetItemNames())
                {
                    //<item>Item Name</item>
                    XmlNode item = xmlDoc.CreateElement("item");
                    item.InnerText = itemName;
                    itemList.AppendChild(item);
                }
                locationNode.AppendChild(itemList);

                //<exitList>
                XmlNode exitList = xmlDoc.CreateElement("exitList");
                foreach (string name in location.GetExitNames())
                {
                    //<exit exitName="Name">Location Name</exitName>
                    XmlNode exit = xmlDoc.CreateElement("exit");
                    XmlAttribute exitName = xmlDoc.CreateAttribute("exitName");
                    exitName.Value = name;
                    exit.Attributes.Append(exitName);
                    exit.InnerText = location.GetLocationNameFromExitName(name);
                    exitList.AppendChild(exit);
                }
                locationNode.AppendChild(exitList);

                locationList.AppendChild(locationNode);
            }

            xmlDoc.Save(GameFilePath);

        }

        private List<Location> LoadLocationList(XmlDocument xmlDoc)
        {
            List<Location> locationList = new List<Location>();

            XmlNode locationNodeList = xmlDoc.SelectSingleNode("savedata").SelectSingleNode("locationList");

            foreach (XmlNode locationNode in locationNodeList.ChildNodes)
            {
                string locationName = locationNode.Attributes.GetNamedItem("locationName").Value;
                string locationDesc = locationNode.SelectSingleNode("description").InnerText;

                List<Item> itemList = new List<Item>();
                foreach (XmlNode itemNode in locationNode.SelectSingleNode("itemList").ChildNodes)
                {
                    itemList.Add(ItemFactory.GenerateItem(itemNode.InnerText));
                }

                Dictionary<string, string> exitList = new Dictionary<string, string>();
                foreach (XmlNode exitNode in locationNode.SelectSingleNode("exitList").ChildNodes)
                {
                    exitList.Add(exitNode.Attributes.GetNamedItem("exitName").Value, exitNode.InnerText);
                }


                Location location = new Location(locationName, locationDesc, itemList, exitList);
                locationList.Add(location);
            }
            return locationList;
        }

        Player LoadPlayer(XmlDocument xmlDoc)
        {
            XmlNode playerNode = xmlDoc.SelectSingleNode("savedata").SelectSingleNode("player");
            string playerName = playerNode.Attributes.GetNamedItem("name").Value;
            string currentLocationName = playerNode.SelectSingleNode("currentLocation").InnerText;
            Location currentLocation = Locations.Where(x => x.Name == currentLocationName).FirstOrDefault();
            Player player = new Player(currentLocation, playerName);

            XmlNode skillsNode = playerNode.SelectSingleNode("skills");
            foreach (XmlNode skillvalue in skillsNode.ChildNodes)
            {
                string skillType = skillvalue.Attributes["type"].Value;
                string skillValue = skillvalue.Attributes["value"].Value;
                player.UpdateSkillValue(skillType, int.Parse(skillValue));
            }
            return player;
        }
    }
}
