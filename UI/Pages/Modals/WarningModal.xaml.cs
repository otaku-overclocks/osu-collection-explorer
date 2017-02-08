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

namespace osu_collection_manager.UI.Pages.Modals
{
    /// <summary>
    /// Interaction logic for WarningModal.xaml
    /// </summary>
    public partial class WarningModal : BaseModal
    {
        public string Meme { get; set; } = Constants.OSU_MEMES[0];

        public WarningModal(Action<ModalFinishType> closeCallback) : base(closeCallback) {
            InitializeComponent();
            var rnd = new Random().Next(0, Constants.OSU_MEMES.Length);
            Meme = Constants.OSU_MEMES[rnd];
        }

        public WarningModal()
        {
            InitializeComponent();
            var rnd = new Random().Next(0, Constants.OSU_MEMES.Length);
            Meme = Constants.OSU_MEMES[rnd];
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (Meme.Equals(TbMeme.Text.ToLower()))
            {
                Close();
                return;
            }
            TbMeme.Text = "";
        }

        public override void Close(ModalFinishType type = ModalFinishType.Succes)
        {
            Properties.Settings.Default.ShownWarning = true;
            Properties.Settings.Default.Save();
            base.Close(type);
        }
    }
}