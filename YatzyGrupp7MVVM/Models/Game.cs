using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatzyGrupp7MVVM.Models
{
    public class Game
    {     
        public int Game_Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public GameType Gametype { get; set; }
    }
}
