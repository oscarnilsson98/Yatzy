using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using YatzyGrupp7MVVM.Data;
using YatzyGrupp7MVVM.Models;
using YatzyGrupp7MVVM.Services;
using YatzyGrupp7MVVM.Views;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class LeaderBoardViewModel : INotifyPropertyChanged
    {
        public LeaderBoardViewModel()
        {

        }

        public async void LoadLeaderBoard()
        {
            Music();

            await InitAsync();
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region ScoreBoards

        async Task InitAsync()
        {
            var task1 = BindDataClassic();
            var task2 = BindDataControlled();
            var task3 = BindDataMostWins();
            var task4 = BindDataMostPlayed();
            await Task.WhenAll(task1, task2, task3, task4);
        }

        private ObservableCollection<Player> playersOnScoreboardClassic = new ObservableCollection<Player>();
        private ObservableCollection<Player> playersOnScoreboardControlled = new ObservableCollection<Player>();
        private ObservableCollection<Player> playersOnScoreboardMostWins = new ObservableCollection<Player>();
        private ObservableCollection<Player> playersOnScoreboardMostPlayed = new ObservableCollection<Player>();

        private void ClearData()
        {
            playersOnScoreboardClassic.Clear();
            playersOnScoreboardControlled.Clear();
            playersOnScoreboardMostWins.Clear();
            playersOnScoreboardMostPlayed.Clear();
        }

        #region Classic

        public ObservableCollection<Player> PlayersOnScoreboardClassic
        {
            get
            {
                return playersOnScoreboardClassic;
            }

            set
            {
                playersOnScoreboardClassic = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersOnScoreboardClassic"));
            }
        }

        private Player _selectedPlayer;
        private ObservableCollection<Player> _selectedPlayers = new ObservableCollection<Player>();


        public ObservableCollection<Player> SelectedPlayers
        {
            get { return _selectedPlayers; }

            set
            {
                if (_selectedPlayers == value)
                {
                    return;
                }

                _selectedPlayers = value;

            }
        }

        private bool FindDuplicates()
        {

            foreach (var p in SelectedPlayers)
            {
                if (p == SelectedPlayer)
                {
                    return false;
                }
            }

            return true;
        }

        public Player SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;

            }
        }

        private async Task BindDataClassic()
        {
            List<Player> players = new List<Player>();

            await Task.Run(() => players = DbOperations.GetHighestScoreLatest7DaysClassic());

            ObservableCollection<Player> _playersOnScoreboard = new ObservableCollection<Player>(players as List<Player>);

            int p = 0;
            int i = 0;

            foreach (object o in _playersOnScoreboard)
            {
                if (playersOnScoreboardClassic.Count <= 4)
                {
                    playersOnScoreboardClassic.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else if (_playersOnScoreboard[i].Rank == p)
                {
                    playersOnScoreboardClassic.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Player_id;
                    i++;
                }
                else
                {
                    break;
                }

            }
        }

        #endregion

        #region Controlled
        public ObservableCollection<Player> PlayersOnScoreboardControlled
        {
            get
            {
                return playersOnScoreboardControlled;
            }

            set
            {
                playersOnScoreboardControlled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersOnScoreboardControlled"));
            }
        }

        private async Task BindDataControlled()
        {
            List<Player> players = new List<Player>();

            await Task.Run(() => players = DbOperations.GetHighestScoreLatest7DaysStyrd());

            ObservableCollection<Player> _playersOnScoreboard = new ObservableCollection<Player>(players as List<Player>);
            int p = 0;
            int i = 0;
            foreach (object o in _playersOnScoreboard)
            {
                if (playersOnScoreboardControlled.Count <= 4)
                {
                    playersOnScoreboardControlled.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else if (_playersOnScoreboard[i].Rank == p)
                {
                    playersOnScoreboardControlled.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else
                {
                    break;
                }

            }
        }


        #endregion

        #region Mostplayed
        public ObservableCollection<Player> PlayersOnScoreboardMostPlayed
        {
            get
            {
                return playersOnScoreboardMostPlayed;
            }

            set
            {
                playersOnScoreboardMostPlayed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersOnScoreboardMostPlayed"));
            }
        }

        private async Task BindDataMostPlayed()
        {
            List<Player> players = new List<Player>();

            await Task.Run(() => players = DbOperations.GetMostPlayed());

            ObservableCollection<Player> _playersOnScoreboard = new ObservableCollection<Player>(players as List<Player>);


            int i = 0;
            int p = 0;
            foreach (object o in _playersOnScoreboard)
            {

                if (playersOnScoreboardMostPlayed.Count <= 4)
                {
                    playersOnScoreboardMostPlayed.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else if (_playersOnScoreboard[i].Rank == p)
                {
                    playersOnScoreboardMostPlayed.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else
                {
                    break;

                }
            }
        }
        #endregion

        #region Mostwins
        public ObservableCollection<Player> PlayersOnScoreboardMostWins
        {
            get
            {
                return playersOnScoreboardMostWins;
            }

            set
            {
                playersOnScoreboardMostWins = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayersOnScoreboardMostWins"));
            }
        }

        private async Task BindDataMostWins()
        {
            List<Player> players = new List<Player>();

            await Task.Run(() => players = DbOperations.GetMostWins());

            ObservableCollection<Player> _playersOnScoreboard = new ObservableCollection<Player>(players as List<Player>);
            int p = 0;
            int i = 0;

            foreach (object o in _playersOnScoreboard)
            {
                if (playersOnScoreboardMostWins.Count <= 4)
                {
                    playersOnScoreboardMostWins.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else if (_playersOnScoreboard[i].Rank == p)
                {
                    playersOnScoreboardMostWins.Add(_playersOnScoreboard[i]);
                    p = _playersOnScoreboard[i].Rank;
                    i++;
                }
                else
                {
                    break;

                }
            }
        }

        #endregion

        #endregion

        #region GoBackToStart
        private RelayCommand _clickCommandWindowPlay;
        public RelayCommand ClickCommandWindowPlay
        {
            get
            {
                return _clickCommandWindowPlay ?? (_clickCommandWindowPlay = new RelayCommand(w => ChangeWindowPlay(), w => CanExecute));
            }
        }

        public void ChangeWindowPlay()
        {
            MusicEngine.ButtonSoundEffect();

            MusicEngine.StartStop();

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            StartView startView = new StartView();

            startView.Show();

            window.Close();
        }
        public bool CanExecute
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Music
        public void Music()
        {
            Thread t = new Thread(MusicEngine.StartStop);
            string filename = "Media/Sounds/Leaderboard.wav";
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
