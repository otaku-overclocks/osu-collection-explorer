using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using osu_collection_manager.Managers;
using osu_collection_manager.Models;
using osu_collection_manager.UI.Pages;
using osu_collection_manager.UI.UserControls.Models;

namespace osu_collection_manager.Pages
{
    /// <summary>
    /// Interaction logic for SelectCollectionsPage.xaml
    /// </summary>
    public partial class SelectCollectionsPage : BaseMainPage
    {
        public enum SelectPurpose
        {
            Export,
            Import
        }

        public SelectPurpose Purpose;

        public SelectCollectionsPage(IEnumerable<Collection> collections, SelectPurpose purpose = SelectPurpose.Export) : base()
        {
            InitializeComponent();
            Purpose = purpose;
            foreach (var collection in collections)
            {
                CollectionsTreeView.Collections.Add(new CollectionHolder(collection, true));
            }
            BtnConfirm.Content = Purpose == SelectPurpose.Export ? "Export" : "Import";
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            if (Purpose == SelectPurpose.Export)
            {
                var selected = CollectionsTreeView.GetSelected();
                var saveFileDialog = new SaveFileDialog {Filter = "Collections file (*.osc)|*.osc"};
                if (saveFileDialog.ShowDialog() != true) return;
                var file = new CollectionsFile(selected);
                file.WriteToFile(saveFileDialog.FileName);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                _mainWindow.WindowContent.Content = null;
            }
        }
    }
}