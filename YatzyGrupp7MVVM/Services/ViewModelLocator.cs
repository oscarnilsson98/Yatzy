using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatzyGrupp7MVVM.ViewModels;


namespace YatzyGrupp7MVVM.Services
{
    public class ViewModelLocator
    {
        private static StartViewModel startViewModel = new StartViewModel();
        private static LobbyViewModel lobbyViewModel = new LobbyViewModel();
        private static GameViewModel gameViewModel = new GameViewModel();
        private static LeaderBoardViewModel leaderBoardViewModel = new LeaderBoardViewModel();
        private static MessageBoxViewModel messageBoxViewModel = new MessageBoxViewModel();
        private static RulesViewModel rulesViewModel = new RulesViewModel();

        public static StartViewModel StartViewModel
        {
            get
            {
                return startViewModel;
            }
        }
        public static LobbyViewModel LobbyViewModel
        {
            get
            {
                return lobbyViewModel;
            }
        }
        public static GameViewModel GameViewModel
        {
            get
            {
                return gameViewModel;
            }
        }
        public static LeaderBoardViewModel LeaderBoardViewModel
        {
            get
            {
                return leaderBoardViewModel;
            }
        }
        public static MessageBoxViewModel MessageBoxViewModel
        {
            get
            {
                return messageBoxViewModel;
            }
        }
        public  static RulesViewModel RulesViewModel
        {
            get
            {
                return rulesViewModel;
            }
        }
    }
}
