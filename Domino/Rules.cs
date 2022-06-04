namespace Domino.Rules;

public interface IRule {
    // Is next move valid
}

public interface IRuleSet<TRule> where TRule : IRule {

}
