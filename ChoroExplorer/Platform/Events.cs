namespace ChoroExplorer.Platform {
    internal class Events {
        public class DisplayExplorerRequest {
            public string	Target { get; }

            public DisplayExplorerRequest( string target ) {
                Target = target;
            }
        }
    }
}
