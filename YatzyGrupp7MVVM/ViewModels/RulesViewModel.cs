using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YatzyGrupp7MVVM.Services;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class RulesViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region ButtonClick
        private RelayCommand _clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new RelayCommand(t => CloseWindow()));
            }
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            window.Close();
        }
        #endregion

        public string SystemHelp
        {
            get
            {
                return ("För att ändra ljudinställningar klicka på ljudikonen uppe i högra hörnet \nFör att avsluta ett spel tryck på dörrikonen uppe i högra hörnet \nFör att sätta på eller stänga av hjälpande text i spel tryck på HJÄLP uppe i högra hörnet. \nSpel som är oavslutade efter 2 timmar avslutas och raderas.");
            }
        }

        public string GameHelp
        {
            get
            {
                return (" För att spara/hålla på tärningarna till nästa kast så klicka på tärningarna.\n För att välja poäng så klickar du i den gröna rutan. \n För att stryka en runda så klickar du i en grön ruta med 0 poäng.\n För att spela mot datorn - lägg till spelare som börjar på Bot i användarnamnet");
            }
        }

        public string GameRulesClassic
        {
            get
            {
                return ("Spelaren väljer själv helt fritt vilken kategori man vill satsa på för att få poäng.\nI klassisk Yatzy utfaller tillkommer en bonus om poängsumman på övre halvan blir 63 eller högre.");
            }
        }

        public string GameRulesStyrd
        {
            get
            {
                return ("Spelaren är tvungen att välja kategorierna i ordningsföljd. \nStyrd Yatzy är svårare och därför får man bonus om poängsumman på övre halvan är 42 eller högre.");
            }
        }

        public string GameRulesCombos
        {
            get
            {
                return ("Ettor: 5\nTvåor: 10\nTreor: 15\nFyror: 20\nFemmor: 25\nSexor: 30\nEtt par: 12(2 lika)\nTvå par: 22(2st 2 lika)\nTretal: 18(3a lika)\nFyrtal: 24(4a lika)\nLiten stege: 15(1, 2, 3, 4 och 5.)\nStor stege: 20(2, 3, 4, 5 och 6.)\nKåk: 28(Exempelvis 6, 6, 6, 4 och 4.)\nChans: 30(samtliga tärningsprickar räknas samman)\nYatzy: 50(samtliga tärningar är likadana)");
            }
        }
       
        
    }
}
