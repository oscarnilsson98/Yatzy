using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using YatzyGrupp7MVVM.Models;
using System.Windows;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using YatzyGrupp7MVVM.Services;
using System.Windows.Input;
using YatzyGrupp7MVVM.Views;
using YatzyGrupp7MVVM.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Media;
using NAudio.Wave;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class StartViewModel : INotifyPropertyChanged
    {
        public StartViewModel()
        {
        }

        public void LoadStart()
        {
            DbOperations.EndGameOlderThan2H();

            Music();
        }

        #region StartGame
        private RelayCommand _clickCommandWindow;
        public RelayCommand ClickCommandWindow
        {
            get
            {
                return _clickCommandWindow ?? (_clickCommandWindow = new RelayCommand(w => ChangeWindow(), w => CanExecute));
            }
        }

        public void ChangeWindow()
        {
            MusicEngine.ButtonSoundEffect();

            MusicEngine.StartStop();

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            LobbyView lobbyView = new LobbyView();

            lobbyView.Show();

            window.Close();
        }
        #endregion

        #region GoToLeaderboard
        private RelayCommand _clickCommandWindowLeaderboard;
        public RelayCommand ClickCommandWindowLeaderboard
        {
            get
            {
                return _clickCommandWindowLeaderboard ?? (_clickCommandWindowLeaderboard = new RelayCommand(w => ChangeWindowLeaderboard(), w => CanExecute));
            }
        }

        public void ChangeWindowLeaderboard()
        {
            MusicEngine.ButtonSoundEffect();

            MusicEngine.StartStop();

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            ScoreBoardView scoreBoardView = new ScoreBoardView();

            scoreBoardView.Show();

            window.Close();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region Music
        private void Music()
        {
            Thread t = new Thread(MusicEngine.StartStop);
            string filename = "Media/Sounds/SpireofLight.wav";
            MusicEngine.file = filename;
            t.Start();

            SoundIconSwitch();
        }
        private RelayCommand _clickCommandPlus;
        public RelayCommand ClickCommandPlus
        {
            get
            {
                return _clickCommandPlus ?? (_clickCommandPlus = new RelayCommand(t => IncreaseVolume()));
            }
        }
        private void IncreaseVolume()
        {
            MusicEngine.IncreaseVolume();
        }

        private RelayCommand _clickCommandMinus;
        public RelayCommand ClickCommandMinus
        {
            get
            {
                return _clickCommandMinus ?? (_clickCommandMinus = new RelayCommand(t => DecreaseVolume()));
            }
        }
        private void DecreaseVolume()
        {
            MusicEngine.DecreaseVolume();
        }

        private RelayCommand _clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new RelayCommand(t => Mute(), t => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        public void Mute()
        {
            MusicEngine.Mute();
            SoundIconSwitch();
        }

        private RelayCommand _clickCommandSoundMenu;
        public RelayCommand ClickCommandSoundMenu
        {
            get
            {
                return _clickCommandSoundMenu ?? (_clickCommandSoundMenu = new RelayCommand(t => SoundMenu()));
            }
        }
        private void SoundMenu()
        {
            MusicEngine.DrawerPlayerMethod();
        }

        private RelayCommand _clickCommandCollaps;
        public RelayCommand ClickCommandCollaps
        {
            get
            {
                return _clickCommandCollaps ?? (_clickCommandCollaps = new RelayCommand(t => Collaps()));
            }
        }
        private void Collaps()
        {
            MusicEngine.DrawerPlayerMethod();
        }

        private void SoundIconSwitch()
        {
            if (MusicEngine.isMute == true)
            {
                Sound_icon = new BitmapImage(new Uri("pack://application:,,,/Icons/SoundIcons/Mute.png", UriKind.RelativeOrAbsolute));
            }
            else if (MusicEngine.isMute == false)
            {
                Sound_icon = new BitmapImage(new Uri("pack://application:,,,/Icons/SoundIcons/VolumeMenu.png", UriKind.RelativeOrAbsolute));
            }
        }

        private BitmapImage soundIcon;
        public BitmapImage Sound_icon
        {
            get { return soundIcon; }
            set
            {
                soundIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sound_icon"));
            }
        }
        #endregion
    }
}
