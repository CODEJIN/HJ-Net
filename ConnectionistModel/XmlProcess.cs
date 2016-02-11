using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public static class XMLProcess
{
    public static XmlNode CreateNode(XmlDocument xmlDoc, string nodeName, string xmlData)
    {
        XmlNode node = xmlDoc.CreateElement("", nodeName, "");
        node.InnerXml = xmlData;

        return node;
    }
}