// Source: https://github.com/berhir/Fluxor.Selectors

namespace Fluxor.Selectors;

public interface ISelector<TResult> {
    TResult Select( IStore state );
}
