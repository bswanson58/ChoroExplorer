// Source: https://github.com/berhir/Fluxor.Selectors

namespace Fluxor.Selectors;

public class SelectorResult<TResult> {
    public TResult Result { get; set; }

    public SelectorResult( TResult result ) {
        Result = result;
    }
}
