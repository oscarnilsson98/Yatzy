using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using YatzyGrupp7MVVM.Models;
using YatzyGrupp7MVVM.Services;
using System.Collections.ObjectModel;
using YatzyGrupp7MVVM.Data;
using Npgsql;
using YatzyGrupp7MVVM.Views;
using System.Windows;
using System.Windows.Threading;
using NAudio.Wave;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class LobbyViewModel : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        #region Variables       

        private GameType _selectedGameType;
        private Player _selectedPlayer;
        private Player newPlayer = new Player();
        private Player _selectedPlayerToRemove = new Player();

        private string _firstName;
        private string _lastName;
        private string _nickName;
        private string _searchInput;

        private RelayCommand _startGameCommand;
        private RelayCommand _addPlayerCommand;
        private RelayCommand _registerPlayerCommand;
        private RelayCommand _removePlayerCommand;
        private RelayCommand _clearSearchCommand;


        private List<Player> players = new List<Player>();
        private List<GameType> _gametypes = new List<GameType>();
        private List<Player> _busyplayers = new List<Player>();
        private ObservableCollection<Player> _selectedPlayers = new ObservableCollection<Player>();
        private ObservableCollection<Player> _availablePlayers = new ObservableCollection<Player>();
        private ObservableCollection<Player> _busyPlayers = new ObservableCollection<Player>();
        private ObservableCollection<GameType> _gameTypes = new ObservableCollection<GameType>();




        #endregion

        #region Properties       

        public bool HelperActive { get; set; }

        public ObservableCollection<Player> Selected_players
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

        public ObservableCollection<Player> Busy_players
        {
            get
            {
                return _busyPlayers;
            }
            set
            {
                if (_busyPlayers != value)
                {
                    _busyPlayers = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Busy_players"));
                }
            }
        }

        public ObservableCollection<Player> Available_players
        {
            get
            {
                return _availablePlayers;
            }
            set
            {
                if (_availablePlayers != value)
                {
                    _availablePlayers = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Available_players"));
                }
            }
        }

        public ObservableCollection<GameType> Game_types
        {
            get
            {
                return _gameTypes;

            }

            set
            {
                if (_gameTypes != value)
                {
                    _gameTypes = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Game_types"));
                }

            }
        }

        public GameType Selected_game_type
        {
            get
            {
                return _selectedGameType;
            }
            set
            {

                _selectedGameType = value;
                StartGameCommand.RaiseCanExecuteChanged();
                PropertyChanged(this, new PropertyChangedEventArgs("Selected_game_type"));

            }

        }

        public Player Selected_player
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;
                AddPlayerCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Selected_player"));

            }
        }

        public Player Selected_player_to_remove
        {
            get
            {
                return _selectedPlayerToRemove;
            }
            set
            {
                _selectedPlayerToRemove = value;
                RemovePlayerCommand.RaiseCanExecuteChanged();
                StartGameCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Selected_player_to_remove"));

            }
        }

        public string Firstname
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
                RegisterPlayerCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Firstname"));
            }
        }

        public string Lastname
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                RegisterPlayerCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Lastname"));
            }
        }

        public string Nickname
        {
            get
            {
                return _nickName;
            }

            set
            {
                _nickName = value;
                ValidateNickName(_nickName);
                RegisterPlayerCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Nickname"));

            }
        }

        public string Search_input
        {
            get { return _searchInput; }
            set
            {
                _searchInput = value;
                FilterPlayers(_searchInput);
                ClearSearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("Search_input"));

            }
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

        #region Commands

        public RelayCommand StartGameCommand
        {
            get
            {
                if (_startGameCommand == null)
                {
                    _startGameCommand = new RelayCommand(p => { StartGame(); }, p => IsGameAllowed());
                }
                return _startGameCommand;
            }
            set
            {
                _startGameCommand = value;
            }
        }

        public RelayCommand AddPlayerCommand
        {
            get
            {
                if (_addPlayerCommand == null)
                    _addPlayerCommand = new RelayCommand(add => { AddPlayer(); }, add => IsAddPlayerAvailable());
                return _addPlayerCommand;
            }
            set
            {
                _addPlayerCommand = value;
            }
        }

        public RelayCommand RegisterPlayerCommand
        {
            get
            {
                if (_registerPlayerCommand == null)
                    _registerPlayerCommand = new RelayCommand(add => { RegisterPlayer(); }, add => IsRegisterAllowed());
                return _registerPlayerCommand;
            }
            set
            {
                _registerPlayerCommand = value;
            }
        }

        public RelayCommand RemovePlayerCommand
        {
            get
            {
                if (_removePlayerCommand == null)
                    _removePlayerCommand = new RelayCommand(add => { RemovePlayer(); }, add => IsRemovalApproved());
                return _removePlayerCommand;
            }

            set
            {
                _removePlayerCommand = value;
            }

        }

        public RelayCommand ClearSearchCommand
        {
            get
            {
                if (_clearSearchCommand == null)
                {
                    _clearSearchCommand = new RelayCommand(clear => { OnClearSearch(); }, clear => IsResetSearchAllowed());
                }
                return _clearSearchCommand;
            }
            set
            {
                _clearSearchCommand = value;
            }

        }

        #endregion

        #region Methods

        #region On Page Load Methods

        // LÄGG TILL TRY CATCH för PostgresException ex?
        public async void LoadLobby()
        {

            await InitAsync();
            ClearSelectedPlayers();
            _selectedPlayers = new ObservableCollection<Player>();
        }


        async Task InitAsync()
        {
            var task1 = GetAvailablePlayers();
            var task2 = GetBusyPlayers();
            var task3 = GetGameTypes();
            await Task.WhenAll(task1, task2, task3);
        }

        private async Task GetAvailablePlayers()
        {
            await Task.Run(() => players = DbOperations.GetAvailablePlayers());
            Available_players = new ObservableCollection<Player>(players);
        }

        private async Task GetBusyPlayers() //https://www.wpf-tutorial.com/listview-control/listview-grouping/ information hämtad härifrån
        {
            await Task.Run(() => _busyplayers = DbOperations.PlayersInActiveGame());
            Busy_players = new ObservableCollection<Player>(_busyplayers);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Busy_players);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Game_id");
            view.GroupDescriptions.Add(groupDescription);
        }

        private async Task GetGameTypes()
        {
            await Task.Run(() => _gametypes = DbOperations.GetGametypes());
            Game_types = new ObservableCollection<GameType>(_gametypes);
        }


        #endregion

        #region Boolean Methods

        private bool IsResetSearchAllowed()
        {
            if (!string.IsNullOrWhiteSpace(Search_input))
                return true; 
          
            return false;
        }

        private bool IsGameAllowed()
        {
            if (Selected_players.Count >= 2 && Selected_game_type != null)
            {
                return true;
            }

            else if (Selected_players.Count == 4)
            {
                return false;
            }

            else if (Selected_player_to_remove == null)
            {
                return false;
            }

            else
            {
                return false;
            }

        }

        private bool IsRemovalApproved()
        {
            if (Selected_players.Count == 0)
            {
                return false;
            }
            return true;
        }

        private bool IsRegisterAllowed()
        {
            Thread.Sleep(10); // Async Await??
            if (string.IsNullOrWhiteSpace(Nickname) ||  _validationErrors.Count != 0)
            {
                return false;
            }
            return true;
        }

        private bool IsAddPlayerAvailable()
        {
            if (Selected_players.Count == 4)
            {
                return false;
            }

            else
            {
                return Selected_player != null;
            }
        }

        private bool IsPlayerAlreadyAdded()
        {
            if (Selected_players.Count == 0)
            {
                return true;
            }

            foreach (var p in Selected_players)
            {
                if (p.Player_id == Selected_player.Player_id)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreAllPlayersBots()
        {
            int playerCount = 0;

            foreach (var p in Selected_players)
            {
                var s = p.Nickname.ToLower();

                if (s.StartsWith("bot"))
                {
                    playerCount++;
                }
            }

            if (playerCount == Selected_players.Count)
            {
                return true;
            }

            return false;
        }

        #endregion

        private void ClearSelectedPlayers()
        {
            Selected_players.Clear();
        }

       

        private void StartGame()
        {
            MusicEngine.ButtonSoundEffect();
            // kod som startar och lagrar spel i databas

            try
            {
                if (AreAllPlayersBots()==false)
                {
                    Game game = new Game();
                    DbOperations.CreateNewGameId(Selected_game_type.Gametype_id);
                    game = DbOperations.GetGameId();

                    foreach (var player in Selected_players)
                    {
                        DbOperations.StartNewGame(game.Game_Id, player.Player_id);
                    }

                    ActiveGame.SetActiveGame(Selected_players, game.Game_Id, game.Start, game.End, Selected_game_type);

                    if (HelperActive == true)
                    {
                        ActiveGame.Helper = true;
                    }
                    else if (HelperActive ==  false)
                    {
                        ActiveGame.Helper = false;
                    }

                    MusicEngine.StartStop();

                    ChangeWindow();

                    MusicEngine.ButtonSoundEffectGameStart();
                }


                else
                {
                    MessageBox.Show("Det är inte tillåtet att starta ett spel med enbart botar");
                }

            }

            catch (PostgresException ex)
            {               
                MessageBox.Show(ex.Message.ToString());
            }

        }

       

        private void OnClearSearch()
        {
            Search_input = null;
            Available_players = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
            PropertyChanged(this, new PropertyChangedEventArgs("Available_players"));
        }

        private void ClearRegisterPlayerArea()
        {
            _lastName = "";
            _firstName = "";
            _nickName = "";
            OnPropertyChanged(new PropertyChangedEventArgs("Firstname"));
            OnPropertyChanged(new PropertyChangedEventArgs("Nickname"));
            OnPropertyChanged(new PropertyChangedEventArgs("Lastname"));
        }

        private void AddRegisterPlayerInSelectedPlayers()
        {

            newPlayer = DbOperations.GetPlayerByNickname(_nickName);

            if (Selected_players.Count < 4)
            {
                _selectedPlayers.Add(newPlayer);
            }

            else
            {

                players.Add(newPlayer);
                Available_players = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
                PropertyChanged(this, new PropertyChangedEventArgs("Available_players"));

            }
            OnPropertyChanged(new PropertyChangedEventArgs("Selected_players"));
           

        }

        public void AddPlayer()
        {
            //Kod som lägger till spelare för spel                                          

            if (IsPlayerAlreadyAdded() == true && IsAddPlayerAvailable() == true)
            {

                _selectedPlayers.Add(Selected_player);
                StartGameCommand.RaiseCanExecuteChanged();
                AddPlayerCommand.RaiseCanExecuteChanged();
                PropertyChanged(this, new PropertyChangedEventArgs("Selected_players"));

                
                players.Remove(Selected_player);
                Available_players = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
                OnPropertyChanged(new PropertyChangedEventArgs("Available_players"));


            }

            else if (Selected_players.Count == 4)
            {
                MessageBox.Show("Du kan inte lägga till fler spelare \nmax antalet spelare har redan uppnåtts");
            }

            else
            {
                MessageBox.Show("Spelaren är redan tillagd för spel");
            }

        }

        private void RegisterPlayer()
        {

            try
            {
                //Lägger in ny spelare i databas och sedan hämtar in alla nya tillgängliga spelare
                DbOperations.AddNewPlayer(Firstname, Nickname, Lastname);
               // players = DbOperations.GetAvailablePlayers();
                AddRegisterPlayerInSelectedPlayers();               
                SetTimerNewPlayer();
                ClearRegisterPlayerArea();
            }

            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }

        private ObservableCollection <Player> UpdateList(ObservableCollection<Player> _availablePlayers)
        {
           
            
                _availablePlayers = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
                OnPropertyChanged(new PropertyChangedEventArgs("Available_players"));
                      
            return _availablePlayers;
        }

        private void FilterPlayers(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {              
                _availablePlayers = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
                OnPropertyChanged(new PropertyChangedEventArgs("Available_players"));
            }

            else
            {

                _availablePlayers = new ObservableCollection<Player>(players.Where(c => c.Nickname.ToLower().Contains(searchInput.ToLower())));
                UpdateList(_availablePlayers);   
                
            }
        }

        public void RemovePlayer()
        {
            players.Add(Selected_player_to_remove);           
            _availablePlayers = new ObservableCollection<Player>(players.OrderBy(x => x.Nickname).ToList());
            Selected_players.Remove(Selected_player_to_remove);
           PropertyChanged(this, new PropertyChangedEventArgs("Available_players"));         
        }

        private void ChangeWindow()
        {

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            GameView gameView = new GameView();

            gameView.Show();

            window.Close();
        }



        #endregion

        #region NewPlayersVisible
        private string newPlayerVisible = "Hidden";
        public string NewPlayerVisible
        {
            get { return newPlayerVisible; }
            set
            {
                newPlayerVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewPlayerVisible"));
            }
        }

        DispatcherTimer dispatcherTimerNewPlayer = new DispatcherTimer();
        private void SetTimerNewPlayer()
        {
            newPlayerVisible = "Visible";
            OnPropertyChanged(new PropertyChangedEventArgs("NewPlayerVisible"));
            dispatcherTimerNewPlayer.Interval = new TimeSpan(0, 0, 0, 2, 0);
            dispatcherTimerNewPlayer.Start();
            dispatcherTimerNewPlayer.Tick += new EventHandler(dispatcherTimerUpdatePlayer_Tick);
        }

        protected void dispatcherTimerUpdatePlayer_Tick(object sender, EventArgs e)
        {
            newPlayerVisible = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("NewPlayerVisible"));
            dispatcherTimerNewPlayer.Stop();
        }
        #endregion

        #region Music
        public void Music()
        {
            Thread t = new Thread(MusicEngine.StartStop);
            string filename = "Media/Sounds/LobbyMusic.wav";
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
                return _clickCommand ?? (_clickCommand = new RelayCommand(t => Mute()));
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

        #region BackBtn
        private RelayCommand _clickCommandBack;
        public RelayCommand ClickCommandBack
        {
            get
            {
                return _clickCommandBack ?? (_clickCommandBack = new RelayCommand(t => GoBack()));
            }
        }

        private void GoBack()
        {
            MusicEngine.ButtonSoundEffect();

            MusicEngine.StartStop();

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            StartView startView = new StartView();

            startView.Show();
            Selected_players.Clear();
            window.Close();
        }
        #endregion

        #region Rules
        private RelayCommand _openRulesCommand;
        public RelayCommand OpenRulesCommand
        {
            get
            {
                return _openRulesCommand ?? (_openRulesCommand = new RelayCommand(w => OpenRules()));
            }
        }

        private void OpenRules()
        {
            RulesView rulesView = new RulesView();

            rulesView.Show();
        }
        #endregion

        #region NickNameValidation
        // källa Magnus Montin https://blog.magnusmontin.net/2013/08/26/data-validation-in-wpf/ Läs mer om data-validation varianter här.

        private readonly ValidateNickname _validateNickname = new ValidateNickname(); // INotifyDataErrorInfo
        private readonly Dictionary<string, ICollection<string>> // INotifyDataErrorInfo
            _validationErrors = new Dictionary<string, ICollection<string>>(); // INotifyDataErrorInfo

        public string Error { get { return null; } } // IDataErrorInfo
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>(); // IDataErrorInfo

        private async void ValidateNickName(string nickname)
        {
            const string propertyKey = "Nickname";
            ICollection<string> validationErrors = null;
            /* Call service asynchronously */
            bool isValid = await Task<bool>.Run(() =>
            {
                return _validateNickname.ValidateNickName(nickname, out validationErrors);
            })
            .ConfigureAwait(false);

            if (!isValid)
            {
                /* Update the collection in the dictionary returned by the GetErrors method */
                _validationErrors[propertyKey] = validationErrors;
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);

            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                /* Remove all errors for this property */
                _validationErrors.Remove(propertyKey);
                /* Raise event to tell WPF to execute the GetErrors method */
                RaiseErrorsChanged(propertyKey);
            }
        }


        #region IDataErrorInfo
        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Nickname":
                        if (string.IsNullOrWhiteSpace(Nickname))
                            result = "Användarnamn kan inte vara tomt";

                        break;
                }
                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);

                OnPropertyChanged(new PropertyChangedEventArgs("ErrorCollection"));
                return result;
            }
        }
        #endregion
        #region INotifyDataErrorInfo members
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }
        #endregion

        #endregion

        
    }
}


