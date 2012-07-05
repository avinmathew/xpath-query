using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathQuery
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtXMLFile_TextChanged(null, null);
            txtXPath_TextChanged(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Namespace> namespaces = new List<Namespace>();
                foreach (var line in txtNamespaces.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    namespaces.Add(Namespace.FromString(line));

                txtNamespaceCount.Text = string.Format("{0} namespace{1}", namespaces.Count, namespaces.Count == 1 ? string.Empty : "s");

                XPathProcessorResult result = XPathProcessor.Process(txtXMLFile.Text, txtXPath.Text, namespaces);
                txtResult.Text = result.Result;
                txtResultCount.Text = string.Format("{0} result{1}", result.Count, result.Count == 1 ? string.Empty : "s");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error processing XPath", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void txtXMLFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!File.Exists(txtXMLFile.Text))
            {
                txtXMLFile.Style = (Style)this.FindResource("ErrorTextBoxStyle");
                return;
            }
            XDocument doc;
            try
            {
                doc = XDocument.Load(txtXMLFile.Text);
            }
            catch
            {
                txtXMLFile.Style = (Style)this.FindResource("ErrorTextBoxStyle");
                return;
            }

            XPathNavigator nav = doc.CreateNavigator();
            nav.MoveToFollowing(XPathNodeType.Element);
            IDictionary<string, string> ns = nav.GetNamespacesInScope(XmlNamespaceScope.All);

            if (ns.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var pair in ns)
                {
                    if (pair.Key == string.Empty)
                        sb.AppendLine(string.Format("default=\"{0}\"", pair.Value));
                    else
                        sb.AppendLine(string.Format("{0}=\"{1}\"", pair.Key, pair.Value));
                }
                txtNamespaces.Text = sb.ToString().TrimEnd();
            }

            txtXMLFile.Style = (Style)this.FindResource("TextBoxStyle");
        }

        private void txtXMLFile_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                txtXMLFile.Text = droppedFilePaths[0]; // Only use first file
            } 
        }

        private void txtXMLFile_PreviewDrag(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }


        private void txtXPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XPathNavigator nav = doc.CreateNavigator();
            try
            {
                XPathExpression expr = nav.Compile(txtXPath.Text);
            }
            catch (XPathException)
            {
                txtXPath.Style = (Style)this.FindResource("ErrorTextBoxStyle");
                return;
            }

            txtXPath.Style = (Style)this.FindResource("TextBoxStyle");
        }
    }
}
