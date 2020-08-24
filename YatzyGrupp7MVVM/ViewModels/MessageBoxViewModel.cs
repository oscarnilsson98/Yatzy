using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YatzyGrupp7MVVM.Services;
using YatzyGrupp7MVVM.Views;
using YatzyGrupp7MVVM.Models;
using System.Threading;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class MessageBoxViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region SetMessage
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Message"));
                }

            }
        }

        public void LoadMessageBox()
        {
            message = StringHandler.Message;
            OnPropertyChanged(new PropertyChangedEventArgs("Message"));

            Music();

        }
        #endregion

        #region ButtonClick
        private RelayCommand _clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new RelayCommand(t => ChangeWindow()));
            }
        }

        private void ChangeWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            StartView startView = new StartView();
            GameViewModel gameViewModel = new GameViewModel();

            MusicEngine.StartStop();
            MusicEngine.ButtonSoundEffect();
            window.Close();
            gameViewModel.CloseCurrentWindow();
            startView.Show();
        }
        #endregion

        #region Music

        private void Music()
        {

            Thread t = new Thread(MusicEngine.StartStop);
            string filename = "Media/Sounds/EndGame.wav";
            MusicEngine.file = filename;
            t.Start();

        }

        #endregion
    }
}
