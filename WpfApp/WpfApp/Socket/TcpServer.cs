

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System;


namespace Demo_Song
{
    class TcpServer
    {
        //私有成员
        private static byte[] result = new byte[1024];
        private int myProt = 500;   //端口  
        static Socket serverSocket;
        static Socket clientSocket;

        Thread myThread;
        static Thread receiveThread;

        //属性

        public int port { get; set; }
        //方法


        
        internal void StartServer()
        {
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(10);    //设定最多10个排队连接请求  

            Debug.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
       
            //通过Clientsoket发送数据  
            myThread = new Thread(ListenClientConnect);
            myThread.Start();
            
        }


        internal void QuitServer()
        {

            serverSocket.Close();
            clientSocket.Close();
            myThread.Abort();
            receiveThread.Abort();


        }


        internal void SendMessage(string msg)
        {
                clientSocket.Send(Encoding.ASCII.GetBytes(msg));
        }



        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private static void ListenClientConnect()
        {
            while (true)
            {
                try
                {
                    clientSocket = serverSocket.Accept();
                    clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                    receiveThread = new Thread(ReceiveMessage);
                    receiveThread.Start(clientSocket);
                }
                catch (Exception)
                {

                }

            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    Debug.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                }
                catch (Exception ex)
                {
                    try
                    {
                        Debug.WriteLine(ex.Message);
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();
                        break;
                    }
                    catch (Exception)
                    {
                    }

                }
            }
        }
    }
}  











































//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net;
//using System.Net.Sockets;
//namespace Demo_Song
//{
//    class TcpServer
//    {



       
//    private Socket _LocationListenSocket;  //本地侦听服务
//    private String _ListenPort; //服务器侦听客户端联接的端口
//    private String _MaxClient; //最大客户端连接数
//    private String _Clients As New SortedList; //客户端队列
//    private String _ListenThread As Thread = Nothing ;//侦听线程
//    private String _ServerStart As Boolean = False ;//服务器是否已经启动
//    private String _RecvMax As Integer; //接收缓冲区大小
//#End Region

//#Region "事件"
//    ''' <summary>
//    ''' 客户端联接事件
//    ''' </summary>
//    ''' <param name="IP">客户端联接IP</param>
//    ''' <param name="Port">客户端联接端口号</param>
//    ''' <remarks></remarks>
//    public Event ClientConnected(ByVal IP As String, ByVal Port As String)
//    ''' <summary>
//    ''' 客户端断开事件
//    ''' </summary>
//    ''' <param name="IP">客户端联接IP</param>
//    ''' <param name="Port">客户端联接端口号</param>
//    ''' <remarks></remarks>
//    public Event ClientClose(ByVal IP As String, ByVal Port As String)
//    ''' <summary>
//    ''' 接收到客户端的数据
//    ''' </summary>
//    ''' <param name="value">数据</param>
//    ''' <param name="IPAddress">数据来源IP</param>
//    ''' <param name="Port">数据来源端口</param>
//    ''' <remarks></remarks>
//    public Event DataArrived(ByVal value As Byte(), ByVal Len As Integer, ByVal IPAddress As String, ByVal Port As String)
//    ''' <summary>
//    ''' 异常数据
//    ''' </summary>
//    ''' <param name="ex"></param>
//    ''' <remarks></remarks>
//    ''' 
//    public Event Exception(ByVal ex As Exception)
//    ''' <summary>
//    ''' 文件到达事件
//    ''' </summary>
//    ''' <param name="value"></param>
//    ''' <param name="Len"></param>
//    ''' <param name="IPAddress"></param>
//    ''' <param name="Port"></param>
//    ''' <remarks></remarks>

//    public Event FileArrived(ByVal value As Byte(), ByVal Len As Integer, ByVal IPAddress As String, ByVal Port As String)

//#End Region

//#Region "属性"
//    ''' <summary>
//    ''' 侦听服务是否已经启动
//    ''' </summary>
//    ''' <value></value>
//    ''' <returns></returns>
//    ''' <remarks></remarks>
//    public ReadOnly Property IsServerStart() As Boolean
//        Get
//            Return _ServerStart
//        End Get
//    End Property
//    ''' <summary>
//    ''' 用于获取或者设置服务器的端口号
//    ''' </summary>
//    ''' <value></value>
//    ''' <returns></returns>
//    ''' <remarks></remarks>
//    public Property port() As Double
//        Get
//            Return _ListenPort
//        End Get

//        Set(ByVal value As Double)
//            _ListenPort = value
//        End Set
//    End Property
//    ''' <summary>
//    ''' 最大连接数
//    ''' </summary>
//    ''' <value></value>
//    ''' <returns></returns>
//    ''' <remarks></remarks>
//    public Property maxclient() As Double
//        Get
//            Return _MaxClient
//        End Get

//        Set(ByVal value As Double)
//            _MaxClient = value
//        End Set
//    End Property
//    ''' <summary>
//    ''' 缓冲区大小
//    ''' </summary>
//    ''' <value></value>
//    ''' <returns></returns>
//    ''' <remarks></remarks>
//    public Property RecvMax() As Double
//        Get
//            Return _RecvMax
//        End Get

//        Set(ByVal value As Double)
//            _RecvMax = value
//        End Set
//    End Property

//#End Region

//#Region "方法"
//    ''' <summary>
//    ''' 实例　TCPServer
//    ''' </summary>
//    ''' <param name="Port">侦听客户端联接的端口号</param>
//    ''' <param name="MaxClient">最大可以联接的客户端数量</param>
//    ''' <param name="RecvMax">接收缓冲区大小</param>
//    ''' <param name="RecvSleep">接收线程睡眠时间</param>
//    ''' <remarks></remarks>
//    Sub setting(ByVal Port As String, ByVal MaxClient As Integer, ByVal RecvMax As Integer, ByVal RecvSleep As Integer)
//        Try
//            Dim strHostName As String = Dns.GetHostName()
//            _ListenPort = Port
//            _MaxClient = MaxClient
//            _RecvMax = RecvMax
//            Dim strServerHost As New IPEndPoint(IPAddress.Any, Int32.Parse(_ListenPort))
//            '建立TCP侦听
//            _LocationListenSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
//            _LocationListenSocket.Bind(strServerHost)
//            _LocationListenSocket.Listen(_MaxClient)
//            _LocationListenSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.AcceptConnection, 1)

//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//        End Try
//    End Sub

//    ''' <summary>
//    ''' 开始侦听服务
//    ''' </summary>
//    ''' <remarks></remarks>
//    public Sub StartServer()
//        _ServerStart = True
//        Try
//            _ListenThread = New Thread(New ThreadStart(AddressOf ListenClient))
//            _ListenThread.Name = "监听客户端主线程"
//            _ListenThread.Start()
//        Catch ex As Exception
//            If (Not _LocationListenSocket Is Nothing) Then
//                If _LocationListenSocket.Connected Then
//                    _LocationListenSocket.Close()
//                End If
//            End If
//            RaiseEvent Exception(ex)
//        End Try
//    End Sub

//    ''' <summary>
//    ''' 关闭侦听
//    ''' </summary>
//    ''' <remarks></remarks>
//    public Sub Close()
//        Try
//            _ServerStart = False
//            'CloseAllClient()
//            Thread.Sleep(5)
//            _ListenThread.Abort()
//            _LocationListenSocket.Close()
//            _ListenThread = Nothing
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//        End Try
//    End Sub

//    ''' <summary>
//    ''' 客户端侦听线程
//    ''' </summary>
//    ''' <remarks></remarks>
//    private Sub ListenClient()
//        Dim sKey As String
//        While (_ServerStart)
//            Try
//                If Not _LocationListenSocket Is Nothing Then
//                    Dim clientSocket As System.Net.Sockets.Socket
//                    clientSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
//                    clientSocket = _LocationListenSocket.Accept()
//                    If Not clientSocket Is Nothing Then
//                        Dim clientInfoT As IPEndPoint = CType(clientSocket.RemoteEndPoint, IPEndPoint)
//                        sKey = clientInfoT.Address.ToString & "\&" & clientInfoT.Port.ToString
//                        _Clients.Add(sKey, clientSocket)
//                        RaiseEvent ClientConnected(clientInfoT.Address.ToString, clientInfoT.Port.ToString) '举起有客户端联接的事件
//                        '启动客户端接收主线程，开始侦听并接收客户端上传的数据
//                        Dim lb As New ClientCommunication(_LocationListenSocket, clientSocket, Me)
//                        AddHandler lb.Exception, AddressOf WriteErrorEvent_ClientCommunication
//                        Dim thrClient As New Thread(New ThreadStart(AddressOf lb.serverThreadProc))
//                        thrClient.Name = "客户端接收线程,客户端" & clientInfoT.Address.ToString & ":" & clientInfoT.Port.ToString
//                        thrClient.Start()
//                    End If
//                End If
//            Catch ex As Exception
//                RaiseEvent Exception(ex)
//            End Try
//        End While
//    End Sub

//    private Sub WriteErrorEvent_ClientCommunication(ByVal ex As Exception)
//        RaiseEvent Exception(ex)
//    End Sub

//    public Sub CloseClient(ByVal IP As String, ByVal Port As String)
//        GetClientSocket(IP, Port).Close()
//        GetClientClose(IP, Port)
//    End Sub
//    'public Sub AlertNoticeClientAll(ByValDepartmentName As String, ByValLineName As String, ByValErrorCode As Integer)  
//    '    '#DepartmentName,LineName,AlertCodeValue.  
//    '    ' ''Dim mStr As String  
//    '    ' ''mStr = "#" &DepartmentName& "," &LineName& "," &ErrorCode
//    '    ' ''Dim SendByte() As Byte = System.Text.UTF8Encoding.Default.GetBytes(mStr)  
//    '    ' ''For Each sc As System.Net.Sockets.Socket In _ClientComputers.Values
//    '    ' ''    sc.Send(SendByte, SendByte.Length(), SocketFlags.None)  
//    '    ' ''Next  
//    'End Sub  
//    public Sub CloseAllClient()
//        For Each sc As System.Net.Sockets.Socket In _Clients.Values
//            '断开所有工作站的Socket连接。
//            Dim clientInfoT As IPEndPoint = CType(sc.RemoteEndPoint, IPEndPoint)
//            CloseClient(clientInfoT.Address.ToString, clientInfoT.Port.ToString)
//        Next
//    End Sub
//#Region "接收客户端的数据"

//    ''' <summary>
//    ''' 接收到客户端的数据-字节数组
//    ''' </summary>
//    ''' <param name="value">数据内容</param>
//    ''' <param name="Len">字节长度</param>
//    ''' <param name="IPAddress">发送该数据的IP地址</param>
//    ''' <param name="Port">发送该数据的端口号</param>
//    ''' <remarks></remarks>
//    private Sub GetData_Byte(ByVal value As Byte(), ByVal Len As Integer, ByVal IPAddress As String, ByVal Port As String)
//        Try
//            RaiseEvent DataArrived(value, Len, IPAddress, Port)
//            'Catch exx As Sockets.SocketException
//            '    CloseClient(IPAddress, Port)  
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//        End Try
//    End Sub

//    ''' <summary>
//    ''' 得到客户端断开或失去客户端联连事件
//    ''' </summary>
//    ''' <param name="IP">客户端联接IP</param>
//    ''' <param name="Port">客户端联接端口号</param>
//    ''' <remarks></remarks>
//    private Sub GetClientClose(ByVal IP As String, ByVal Port As String)
//        Try
//            If _Clients.ContainsKey(IP & "\&" & Port) Then
//                SyncLock _Clients.SyncRoot
//                    '_Clients.Item(IP & "\&" & Port)  
//                    _Clients.Remove(IP & "\&" & Port)
//                End SyncLock
//            End If
//            RaiseEvent ClientClose(IP, Port)
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//        End Try
//    End Sub
//#End Region


//#Region "向客户端发送数据"

//    ''' <summary>
//    ''' 向客户端发送信息
//    ''' </summary>
//    ''' <param name="value">发送的内容</param>
//    ''' <param name="IPAddress">IP地址</param>
//    ''' <param name="Port">端口号</param>
//    ''' <returns> Boolean</returns>
//    ''' <remarks></remarks>
//    public Function SendData(ByVal value As Byte(), ByVal IPAddress As String, ByVal Port As String) As Boolean
//        Try
//            Dim clientSocket As System.Net.Sockets.Socket
//            clientSocket = _Clients.Item(IPAddress & "\&" & Port)
//            clientSocket.Send(value, value.Length, SocketFlags.None)
//            Return True
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//            Return False
//        End Try
//    End Function
//    public Function SendFile(ByVal value As String, ByVal IPAddress As String, ByVal Port As String) As Boolean
//        Try
//            Dim clientSocket As System.Net.Sockets.Socket
//            clientSocket = _Clients.Item(IPAddress & "\&" & Port)
//            clientSocket.SendFile(value)
//            Return True
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//            Return False
//        End Try
//    End Function
//    public Function SendDataToAllClient(ByVal value As Byte()) As Boolean
//        Try
//            For Each clientSocket As System.Net.Sockets.Socket In _Clients.Values
//                clientSocket.Send(value, value.Length, SocketFlags.None)
//            Next
//            Return True
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//            Return False
//        End Try
//    End Function

//#End Region

//    ''' <summary>
//    ''' 得到客户端的Socket联接
//    ''' </summary>
//    ''' <param name="IPAddress">客户端的IP</param>
//    ''' <param name="Port">客户端的端口号</param>
//    ''' <returns>Socket联接</returns>
//    ''' <remarks></remarks>
//    private Function GetClientSocket(ByVal IPAddress As String, ByVal Port As String) As Socket
//        Try
//            Dim ClientSocket As Socket
//            ClientSocket = _Clients.Item(IPAddress & "\&" & Port)
//            Return ClientSocket
//        Catch ex As Exception
//            RaiseEvent Exception(ex)
//            Return Nothing
//        End Try
//    End Function
//#End Region

//    private Class ClientCommunication
//        public Event Exception(ByVal ex As Exception)

//        private ServerSocket As New System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
//        private myClientSocket As New System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
//        private myParentObject As TCPServer
//        private oldbytes() As Byte
//        private _IPAddress, _Port As String
//        private NclientInfoT As IPEndPoint = Nothing
//        private iLen As Integer
//        private allDone As New ManualResetEvent(False)
//        ''' <summary>
//        ''' 实例ClientCommunication类
//        ''' </summary>
//        ''' <param name="ServerSocket"></param>
//        ''' <param name="ClientSocket"></param>
//        ''' <param name="ParentObject"></param>
//        ''' <remarks></remarks>
//        public Sub New(ByVal ServerSocket As Socket, ByVal ClientSocket As Socket, ByVal ParentObject As TCPServer)
//            Me.ServerSocket = ServerSocket
//            myClientSocket = ClientSocket
//            myParentObject = ParentObject
//            NclientInfoT = CType(myClientSocket.RemoteEndPoint, IPEndPoint)
//            _IPAddress = NclientInfoT.Address.ToString
//            _Port = NclientInfoT.Port.ToString
//        End Sub

//        ''' <summary>
//        ''' 客户端通讯主线程
//        ''' </summary>
//        ''' <remarks></remarks>
//        public Sub serverThreadProc()
//            Try
//                Dim sb As New SocketAndBuffer
//                sb.Socket = myClientSocket
//                sb.Socket.BeginReceive(sb.Buffer, 0, sb.Buffer.Length, SocketFlags.None, AddressOf ReceiveCallBack, sb)
//                'allDone.WaitOne()
//            Catch ex As Exception
//                RaiseEvent Exception(ex)
//            End Try
//        End Sub

//        ''' <summary>
//        ''' socket异步接收回调函数
//        ''' </summary>
//        ''' <param name="ar"></param>
//        ''' <remarks></remarks>
//        private Sub ReceiveCallBack(ByVal ar As IAsyncResult)
//            Dim sb As SocketAndBuffer
//            allDone.Set()
//            sb = CType(ar.AsyncState, SocketAndBuffer)
//            Try
//                If sb.Socket.Connected Then
//                    iLen = sb.Socket.EndReceive(ar)
//                    If iLen > 0 Then
//                        ReDim oldbytes(iLen - 1)
//                        Array.Copy(sb.Buffer, 0, oldbytes, 0, iLen)
//                        myParentObject.GetData_Byte(oldbytes, oldbytes.Length, _IPAddress, _Port)

//                        sb.Socket.BeginReceive(sb.Buffer, 0, sb.Buffer.Length, SocketFlags.None, AddressOf ReceiveCallBack, sb)
//                    Else
//                        If (Not myClientSocket Is Nothing) Then
//                            If myClientSocket.Connected Then
//                                myClientSocket.Close()
//                            Else
//                                myClientSocket.Close()
//                            End If
//                            myClientSocket = Nothing
//                            If Not NclientInfoT Is Nothing Then
//                                myParentObject._Clients.Remove(_IPAddress & "\&" & _Port)
//                                myParentObject.GetClientClose(_IPAddress, _Port)
//                            End If
//                        End If
//                    End If
//                End If
//            Catch ex As Exception
//                If (Not myClientSocket Is Nothing) Then
//                    If myClientSocket.Connected Then
//                        myClientSocket.Close()
//                    Else
//                        myClientSocket.Close()
//                    End If
//                    myClientSocket = Nothing
//                    If Not NclientInfoT Is Nothing Then
//                        myParentObject._Clients.Remove(_IPAddress & "\&" & _Port)
//                        myParentObject.GetClientClose(_IPAddress, _Port)
//                    End If
//                End If
//                RaiseEvent Exception(ex)
//            End Try
//        End Sub

//        ''' <summary>
//        ''' 异步操作socket缓冲类
//        ''' </summary>
//        ''' <remarks></remarks>
//        private Class SocketAndBuffer
//            public Socket As System.Net.Sockets.Socket
//            public Buffer(8192) As Byte
//        End Class
//    End Class
























//    }







//}
