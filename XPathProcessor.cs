using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace XPathQuery
{
    public static class XPathProcessor
    {
        public static XPathProcessorResult Process(string xmlFile, string xpath, IEnumerable<Namespace> namespaces)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XPathNavigator nav = doc.CreateNavigator();

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            foreach (Namespace ns in namespaces)
                nsMgr.AddNamespace(ns.Prefix, ns.URI);

            XPathExpression expr = XPathExpression.Compile(xpath, nsMgr);

            object result;
            if (expr.ReturnType == XPathResultType.NodeSet)
                result = doc.SelectNodes(xpath, nsMgr);
            else
                result = nav.Evaluate(expr);

            XPathProcessorResult processorResult = new XPathProcessorResult(result, expr.ReturnType);

            return processorResult;
        }
    }
}
