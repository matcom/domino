using System.Collections;
using Domino.Board;
using Domino.Player;

namespace Domino.Game;

public abstract class BaseGame {
    BaseBoard GameBoard { public get; protected set; }
    IEnumerable<BasePlayer> Players { public get; protected set; }
}
