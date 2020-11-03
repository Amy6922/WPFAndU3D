using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class Demo : MonoBehaviour
{
    public GameObject man;



    const int portNo = 500;
    private TcpClient _client;
    byte[] data;

    string Error_Message;

    void Start()
    {
        try
        {
            this._client = new TcpClient();
            this._client.Connect("127.0.0.1", portNo);
            data = new byte[this._client.ReceiveBufferSize];
            //SendMessage(txtNick.Text);
            SendMessage("Unity Demo Client is Ready!");
            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        }
        catch (Exception ex)
        {


        }
    }


    public void translateX(float x)

    {

        transform.Translate(new Vector3(x, 0, 0));

    }

    public void translateY(float y)

    {

        transform.Translate(new Vector3(0, y, 0));

    }


    public void translateZ(float z)

    {

        transform.Translate(new Vector3(0, 0, z));

    }



    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 150, 50), Error_Message);
    }


    public new void SendMessage(string message)
    {
        try
        {
            NetworkStream ns = this._client.GetStream();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            ns.Write(data, 0, data.Length);
            ns.Flush();
        }
        catch (Exception ex)
        {
            Error_Message = ex.Message;
            //MessageBox.Show(ex.ToString());
        }
    }
    public void ReceiveMessage(IAsyncResult ar)
    {
        try
        {
            //清空errormessage
            Error_Message = "";
            int bytesRead;
            bytesRead = this._client.GetStream().EndRead(ar);
            if (bytesRead < 1)
            {
                return;
            }
            else
            {
                Debug.Log(System.Text.Encoding.ASCII.GetString(data, 0, bytesRead));
                string message = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                switch (message)
                {
                    case "1":
                        translateX(1);
                        break;

                    case "2":
                        translateX(-1);
                        break;
                    case "3":
                        translateY(1);
                        break;
                    case "4":
                        translateY(-1);
                        break;
                    case "5":
                        translateZ(1);
                        break;
                    case "6":
                        translateZ(-1);
                        break;
                    default:
                        Error_Message = "unknown command";
                        break;


                }




            }
            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        }
        catch (Exception ex)
        {
            Error_Message = ex.Message;
        }
    }


    void OnDestroy()
    {

        this._client.Close();
    }




}
