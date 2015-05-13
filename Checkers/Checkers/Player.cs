using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Player
    {
        public string Name { get; set; }
        public bool IsTurn { get; set; }
        public string Display { get; set; }
        /// <summary>
        /// Returns +1 for player1, and -1 for player2
        /// </summary>
        public int YDirection { get; set; }
        public bool IsPlayerX { get; set; }


        public string KingDisplay { get; set; }
    }
}
