using Domino.Token;

namespace Domino.Player;

public abstract class BasePlayer {
    string Name { public get; protected set; }
    IEnumerable<BaseToken> AvailableTokens { public get; protected set; }
}
