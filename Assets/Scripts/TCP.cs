using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Linq;
using Oculus.Interaction;
using System.IO;
using System.Collections;


public class TCP : MonoBehaviour
{

    private string HOST;
    private int PORT;
    private TcpListener server;
    private TcpClient client;
    private NetworkStream netStream;
    public string receivedData = string.Empty;
    private float[] sentData;
    public int fbModality = 0;
    private volatile bool isRunning = true;
    private DateTime startTime;

    private string filePath;
    async void Start()
    {
        HOST = "172.16.157.70";
        PORT = 12345;
        await StartServerAsync();
    }

    void Update()
    {


    }

    private async Task StartServerAsync()
    {

        try
        {
            server = new TcpListener(IPAddress.Parse(HOST), PORT);
            server.Start();
            Debug.Log($"Server started at {HOST}:{PORT}. Waiting for client...");
            client = await server.AcceptTcpClientAsync();
            netStream = client.GetStream();
            Debug.Log("Client connected.");
            startTime = DateTime.Now;
            _ = ReceiveDataAsync();
            _ = SendDataAsync();
        }
        catch (SocketException ex)
        {
            Debug.LogError($"SocketException: {ex.Message}");
            Cleanup();
        }

    }
    
    private async Task ReceiveDataAsync()
    {
        try
        {
            byte[] lengthBuffer = new byte[4];

            while (isRunning)
            {

                if (netStream != null && netStream.CanRead)
                {
                    // First, read the length prefix (4 bytes)
                    int lengthBytesRead = await netStream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                    Debug.Log($"lengthBytesRead: {lengthBytesRead}");
                    if (lengthBytesRead < 4) break; // If we can't read 4 bytes for length, break out or handle error

                    // Convert the length prefix to an integer (big-endian)
                    int messageLength = BitConverter.ToInt32(lengthBuffer.Reverse().ToArray(), 0);

                    byte[] dataBuffer = new byte[messageLength];
                    int totalBytesRead = 0;

                    // Now read the actual JSON data in chunks
                    while (totalBytesRead < messageLength)
                    {

                        int bytesRead = await netStream.ReadAsync(dataBuffer, totalBytesRead, messageLength - totalBytesRead);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        totalBytesRead += bytesRead;
                    }

                    if (totalBytesRead == messageLength)
                    {
                        try
                        {
                            string jsonData = Encoding.UTF8.GetString(dataBuffer, 0, totalBytesRead);
                            var receivedJson = JsonConvert.DeserializeObject<ReceivedData>(jsonData);
                            if (receivedJson != null)
                            {
                                fbModality = receivedJson.fbModality;
                            }

                            Debug.Log($"Received JSON: {jsonData}");
                        }
                        catch (JsonException ex)
                        {
                            Debug.LogError($"JSON Decode Error: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in ReceiveDataAsync: {ex.Message}");
        }
    }

    private async Task SendDataAsync()
    {
        try
        {

            while (isRunning && netStream != null && netStream.CanWrite)
            {
                float timestamp = (float)Math.Round((DateTime.Now - startTime).TotalSeconds, 5);
                var dataToSend = BuildSentData();
                await SendJsonAsync(netStream, dataToSend);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SendData Exception: {ex.Message}");
        }
    }

    private SentData BuildSentData()
    {
        return new SentData
        {
            timestamp = (float)Math.Round((DateTime.Now - startTime).TotalSeconds, 5),
        };
    }

    private async Task SendJsonAsync(NetworkStream netStream, SentData dataToSend)
    {
        string jsonData = JsonConvert.SerializeObject(dataToSend) + "\n";
        byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);

        await netStream.WriteAsync(jsonDataBytes, 0, jsonDataBytes.Length);
    }

    private void Cleanup()
    {
        isRunning = false;
        netStream?.Close();
        client?.Close();
        server?.Stop();
        Debug.Log("Server stopped and resources cleaned up.");
    }

    private void OnDestroy()
    {
        Cleanup();
    }


}

[Serializable]
public class SentData
{
    public float timestamp = 0f;
}


[Serializable]
public class ReceivedData
{
    public int fbModality;
}