using System.Windows;
using System.Windows.Controls;
using osu_collection_manager.UI.Pages;

namespace osu_collection_manager.UI.Windows
{
    public abstract class BaseNavigationWindow : Window
    {
        public abstract void OpenPage(BasePage page);

        public abstract void OpenDialog(BaseModal dialog);
    }
}