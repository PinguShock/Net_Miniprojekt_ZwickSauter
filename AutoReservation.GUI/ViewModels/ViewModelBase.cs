using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Extensions;
using AutoReservation.Common.Interfaces;
using AutoReservation.GUI.Factory;

namespace AutoReservation.GUI.ViewModels
{
    public abstract class ViewModelBase {

        public string saveWarningMessage = "Änderungen speichern? \nDies kann nicht rückgängig gemacht werden!";
        public string saveWarningWindowTitle = "Speichern bestätigen";
        public string removeWarningMessage = "Eintrag wirklich löschen? \nÄnderungen können nicht rückgängig gemacht werden!";
        public string removeWarningTitle = "Löschen bestätigen";

        protected ViewModelBase() {
        }


        #region PopWarnings
        public void showWarningMessage(string warning, string title) {
            MessageBox.Show(warning, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public MessageBoxResult showSecureSaveMessage() {
            return MessageBox.Show(saveWarningMessage, saveWarningWindowTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        }
        public bool showSecureDeleteMessage() {
            MessageBoxResult result = MessageBox.Show(removeWarningMessage, removeWarningTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes ? true : false;
        }
        #endregion


    }
}
