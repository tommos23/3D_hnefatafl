using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class ApplicationModel
    {

        // This is used by udpRecieve Handler to start/stop the listener thread.
        // Required as a global var due to scope issues with threads
        public static bool runThread = true;

        // Defines all possible gametypes that can be played, each of these should be available through splash screen
        public enum GameType {Player_V_Player, AI_V_Player, Player_V_AI, AI_V_AI};
        static public GameType gameType = GameType.Player_V_Player;

        // Track winner of game, default: no-one
        public int Winner = 0;
    }
}
