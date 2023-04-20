namespace ChoroExplorer.Platform {
    internal class Events {
        public class StatusEvent {
            public	string			Message { get; }
            public	bool			ExtendDisplay { get; set; }

            public StatusEvent( string message ) {
                Message = message;
            }
        }

        public class DisplayExplorerRequest {
            public string	Target { get; }

            public DisplayExplorerRequest( string target ) {
                Target = target;
            }
        }
    }
}
