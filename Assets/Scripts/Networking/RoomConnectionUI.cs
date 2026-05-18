using Unity.Netcode;
using UnityEngine;

namespace Ebala.Networking
{
    public class RoomConnectionUI : MonoBehaviour
    {
        [Header("Connection")]
        [SerializeField] private string address = "127.0.0.1";
        [SerializeField] private ushort port = 7777;

        private bool _connected;

        private void Start()
        {
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("NetworkManager is missing in scene.");
                enabled = false;
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 360, 250), "Online Session", GUI.skin.window);

            if (!_connected)
            {
                GUILayout.Label("Room mode: host/client via Unity Transport");
                address = LabeledTextField("Address", address);
                port = (ushort)LabeledIntField("Port", port);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Start Host", GUILayout.Height(32)))
                {
                    ConfigureTransport();
                    NetworkManager.Singleton.StartHost();
                }

                if (GUILayout.Button("Join Room", GUILayout.Height(32)))
                {
                    ConfigureTransport();
                    NetworkManager.Singleton.StartClient();
                }

                GUILayout.EndHorizontal();

                if (GUILayout.Button("Dedicated Server", GUILayout.Height(28)))
                {
                    ConfigureTransport();
                    NetworkManager.Singleton.StartServer();
                }
            }
            else
            {
                GUILayout.Label($"Connected players: {NetworkManager.Singleton.ConnectedClientsList.Count}");
                if (GUILayout.Button("Leave Session", GUILayout.Height(32)))
                {
                    NetworkManager.Singleton.Shutdown();
                    _connected = false;
                }
            }

            GUILayout.EndArea();
        }

        private void ConfigureTransport()
        {
            var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as Unity.Netcode.Transports.UTP.UnityTransport;
            if (transport == null)
            {
                Debug.LogError("UnityTransport is not assigned in NetworkManager.");
                return;
            }

            transport.SetConnectionData(address, port);
        }

        private void OnClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _connected = true;
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _connected = false;
            }
        }

        private static string LabeledTextField(string label, string value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(90));
            value = GUILayout.TextField(value, GUILayout.Width(220));
            GUILayout.EndHorizontal();
            return value;
        }

        private static int LabeledIntField(string label, int value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(90));
            var text = GUILayout.TextField(value.ToString(), GUILayout.Width(220));
            GUILayout.EndHorizontal();

            return int.TryParse(text, out var parsed) ? Mathf.Clamp(parsed, 1, 65535) : value;
        }
    }
}
