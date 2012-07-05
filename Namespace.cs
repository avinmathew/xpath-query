using System.Text.RegularExpressions;

namespace XPathQuery
{
    public class Namespace
    {
        public string Prefix { get; set; }
        public string URI { get; set; }

        public Namespace(string prefix, string uri)
        {
            Prefix = prefix;
            URI = uri;
        }

        // Expects a string in the format of {prefix}="{uri}"
        public static Namespace FromString(string ns)
        {
            Regex regex = new Regex("(.*)=\"(.*)\"");
            var match = regex.Match(ns);
            string prefix = match.Groups[1].Value;
            string uri = match.Groups[2].Value;
            return new Namespace(prefix, uri);
        }
    }
}
