using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using gamification_backend.Game;
using Microsoft.AspNetCore.Mvc;

namespace gamification_backend.Controllers;

public class WebSocketController : Controller
{
    [Route("/ws")]
    [HttpGet]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async void Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var id = HttpContext.Session.GetInt32(GameController.SessionId);
        if (!id.HasValue)
        {
            Console.WriteLine("Could not find session id");
            return;
        }

        var prevTime = -1;
        while (true)
        {
            var running = GameManager.Instance().SessionIsRunning(id.Value);
            Message data;
            if (!running)
            {
                data = Message.CreateStateChange("Game is not running");
            }
            else
            {
                //Getting state from the session
                var time = GameManager.Instance().GetSessionTime(id.Value);
                if (time == prevTime) continue;
                prevTime = time;
                data = Message.CreateUpdate(time.ToString());
            }

            var json = JsonSerializer.Serialize(data);
            buffer = Encoding.UTF8.GetBytes(json);
            var size = Encoding.UTF8.GetByteCount(json);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, size),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }


    private class Message
    {
        private Type _type;

        private Message(Type type, string message)
        {
            _type = type;
            data = message;
        }

        public string type
        {
            get => _type.ToString();
        }

        public string data { get; set; }

        public static Message CreateUpdate(string data)
        {
            return new Message(Type.Update, data);
        }

        public static Message CreateStateChange(string data)
        {
            return new Message(Type.StateChange, data);
        }

        internal enum Type
        {
            Update,
            StateChange,
        }
    }
}