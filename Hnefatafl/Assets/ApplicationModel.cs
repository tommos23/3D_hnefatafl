using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class ApplicationModel
    {
        public enum GameType {Player_V_Player, AI_V_Player, AI_V_AI};
        static public GameType gameType = GameType.Player_V_Player;
    }
}
