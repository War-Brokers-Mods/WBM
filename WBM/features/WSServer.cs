namespace WBM
{
    public partial class WBM
    {
        private WebSocketSharp.Server.WebSocketServer server;
        private ushort serverPort = 24601;
        private Data.SerializableData data = new Data.SerializableData();

        private void setupWSSever()
        {
            server = new WebSocketSharp.Server.WebSocketServer($"ws://127.0.0.1:{this.serverPort}");
            server.AddWebSocketService<WSJSONService>("/json");
            server.Start();
        }

        private void destroyWSSever()
        {
            // properly stop websocket server
            this.server.Stop();
        }
    }

    public class WSJSONService : WebSocketSharp.Server.WebSocketBehavior
    {
    }
}
