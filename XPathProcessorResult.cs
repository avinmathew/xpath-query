using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace XPathQuery
{
    public class XPathProcessorResult
    {
        public string Result
        {
            get
            {
                if (!string.IsNullOrEmpty(StringResult))
                    return StringResult;

                StringBuilder sb = new StringBuilder();
                foreach (XmlNode node in XMLNodeList)
                {
                    StringBuilder sbLocal = new StringBuilder();
                    using (StringWriter stringWriter = new StringWriter(sbLocal))
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                    {
                        xmlTextWriter.Formatting = Formatting.Indented;
                        node.WriteTo(xmlTextWriter);
                    }
                    sb.AppendLine(sbLocal.ToString());
                }

                return sb.ToString();
            }
        }
        public int Count
        {
            get
            {
                if (!string.IsNullOrEmpty(StringResult))
                    return 1;
                else
                    return XMLNodeList.Count;
            }
        }

        private string StringResult { get; set; }
        private XmlNodeList XMLNodeList { get; set; }

        public XPathProcessorResult(object value, XPathResultType resultType)
        {
            switch (resultType)
            {
                case XPathResultType.NodeSet:
                    XMLNodeList = (XmlNodeList)value;
                    break;
                case XPathResultType.Boolean:
                case XPathResultType.Number:
                case XPathResultType.String:
                    StringResult = value.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unexpected XPathResultType");
            }
            
        }

    }
}
