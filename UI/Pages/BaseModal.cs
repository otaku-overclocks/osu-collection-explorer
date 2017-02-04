using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.UI.Pages
{
    public enum ModalFinishType
    {
        Succes,
        Cancelled
    }

    public class BaseModal : BasePage
    {
        public Action<ModalFinishType> Callback { get; set; }

        protected BaseModal()
        {
        }

        protected BaseModal(Action<ModalFinishType> closeCallback)
        {
            Callback = closeCallback;
        }

        public virtual void Close(ModalFinishType type = ModalFinishType.Succes)
        {
            Callback?.Invoke(type);
            MainWindow.OpenDialog(null);
        }
    }
}
