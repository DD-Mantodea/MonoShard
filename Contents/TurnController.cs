using Mantodea.Contents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents
{
    public class TurnController
    {
        public static bool PlayerTurn = true;

        public static void EndPlayerTurn()
        {
            PlayerTurn = false;
        }

        public static void EndNPCTurn()
        {
            PlayerTurn = true;
        }
    }
}
