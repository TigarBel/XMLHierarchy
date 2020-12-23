using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using XMLHierarchy.XMLParserLogic;

namespace XMLHierarchy
{
  /// <summary>
  /// Логика взаимодействия для MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Запуск основного окна программы
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Событие при нажатии кнопка "Start"
    /// </summary>
    /// <param name="sender">Объект кнопки</param>
    /// <param name="e">Событие</param>
    private async void ButtonStart_Click(object sender, RoutedEventArgs e)
    {
      try {
        //https://raw.githubusercontent.com/kizeevov/elcomplusfiles/main/config.xml
        //https://raw.githubusercontent.com/kizeevov/elcomplusfiles/main/tree.xml
        //D:\Downloads\work1.xml
        //D:\Downloads\work2.xml
        XDocument document = await new XMLParser().GetDocumentAsync(textBoxURI.Text);

        if (!treeViewXMLH.Items.IsEmpty) {
          treeViewXMLH.Items.Clear();
        }

        TreeViewItem treeViewItem = new TreeViewItem() { Header = document.Declaration };
        treeViewXMLH.Items.Add(treeViewItem);

        TreeViewItem root = new TreeViewItem() { Header = document.Root.Name.ToString() };
        if (document.Root.HasAttributes) {
          TreeViewItem attributesItem = new TreeViewItem() { Header = "Attributes" };

          foreach (XAttribute attribute in document.Root.Attributes()) {
            attributesItem.Items.Add(new TreeViewItem() { Header = attribute.ToString() });
          }
          root.Items.Add(attributesItem);
        }
        treeViewXMLH.Items.Add(root);

        LoadItemTree(document.Root, root);

      } catch (Exception exception) {
        MessageBox.Show($"Message :{exception.Message} ", "Exception Caught!");
      }
    }
    /// <summary>
    /// Чтение иерархии документа и её запись в элемент TreeViewItem
    /// </summary>
    /// <param name="root">Корень документа</param>
    /// <param name="treeViewItem">Элемент отображения иерархии</param>
    private void LoadItemTree(XElement root, TreeViewItem treeViewItem)
    {
      foreach (XElement element in root.Elements()) {
        TreeViewItem item;
        string name = $"{element.Name.LocalName.ToString()} {element.Name.Namespace.ToString()}";

        if (element.HasAttributes) {
          item = new TreeViewItem() { Header = name };
          TreeViewItem attributesItem = new TreeViewItem() { Header = "Attributes" };

          foreach (XAttribute attribute in element.Attributes()) {
            attributesItem.Items.Add(new TreeViewItem() { Header = attribute.ToString() });
          }
          item.Items.Add(attributesItem);

        } else {
          if(element.Value.ToString() != "") {
            item = new TreeViewItem() { Header = $"{name} = {element.Value.ToString()}" };
          } else {
            item = new TreeViewItem() { Header = name };
          }
        }
        treeViewItem.Items.Add(item);

        LoadItemTree(element, item);
      }
    }
  }
}
