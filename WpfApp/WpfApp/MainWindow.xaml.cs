using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Windows;
using System.Windows.Input;



namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        TcpServer WpfServer;
        int size_state = 0;
        public MainWindow()
        {
            InitializeComponent();


            this.Closed += MainWindow_Closed;
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;



            WpfServer = new TcpServer();
            WpfServer.StartServer();


        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            unityhost.Form1_FormClosed();


            WpfServer.QuitServer();

        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {

            unityhost.Form1_Deactivate();
        }

        void MainWindow_Activated(object sender, EventArgs e)
        {
            unityhost.Form1_Activated();
        }

        private void btn_normal_Click(object sender, RoutedEventArgs e)
        {
            if (size_state == 0)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1280;
                this.Height = 800;
                toNormalSize.Visibility = Visibility.Hidden;
                toMaxSize.Visibility = Visibility.Visible;

                size_state = 1;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                toNormalSize.Visibility = Visibility.Visible;
                toMaxSize.Visibility = Visibility.Hidden;
                size_state = 0;
            }

        }

        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {


            }

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("1");
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("2");
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("3");
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("4");
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("5");
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            WpfServer.SendMessage("6");
        }
    }
}
