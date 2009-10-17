using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using OsoFramework.Management;
using System.ServiceModel.Description;
using System.Windows.Controls.Primitives;
using OsoFramework;
using OsoFrameworkManager.Management;
using System.Windows.Threading;

namespace OsoFrameworkManager
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        WebRobotItemCollection robots = new WebRobotItemCollection();
        ServiceClient client;
      

        public MainPage()
        {
            InitializeComponent();
        }

       
        private void LoadRobotManagement_Click(object sender, RoutedEventArgs e)
        {
            
            robots = client.GetRobotItems();
            
            this.WebRobotList.ItemsSource = robots;
            if (robots.Count < 1)
            {
                this.StreamLogOutput.Text +=
                    String.Format("[{0}] [{1}] - {2}", DateTime.Now, "Manager", "OsoFx Service is down");
            }
        }



        private void WebRobotList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.RobotActions.Visibility = Visibility.Visible;
        }

        
        private void StartStopRobotCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var currentItem = (robots.DefaultView.CurrentItem as WebRobotItem);
            e.CanExecute = (currentItem != null);

          
        }

        private void StartStopRobotCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            var currentItem = (robots.DefaultView.CurrentItem as WebRobotItem);
            if (e.OriginalSource is ToggleButton)
            {
                var button = (ToggleButton)e.OriginalSource;
                if (button.IsChecked.HasValue)
                {
                    if (button.IsChecked.Value)
                    {

                        client.Start(currentItem.Name);
                    }
                    else
                    {
                        client.Stop(currentItem.Name);
                    }
                }
                else
                {
                    client.Stop(currentItem.Name);
                }
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceClient(this.RobotManagementUrl.Text);

            Action<WebRobotItem, WebRobotManagementStatus> UpdateRobotStatus = (r,status) =>
                {
                    if (status == WebRobotManagementStatus.ERROR)
                    {
                        r.IsToggleChecked = null;
                    }
                    if (status == WebRobotManagementStatus.STOPPED)
                    {
                        r.IsToggleChecked = false;
                    }
                };
            WebRobotLogServiceLoader.OpenService(typeof(WebManagementLogService));
            WebManagementLogService.OnWriteLogEvent += 
                (log => this.StreamLogOutput.Text += log );

            WebManagementLogService.OnWebRobotStatus +=
                (name, status) => 
                {
                    
                    if (name == "all")
                    {
                        foreach (var robot in this.robots)
                        {
                            UpdateRobotStatus(robot, status);
                        }
                    }
                    else
                    {
                        var robot =
                        (from r in this.robots
                         where r.Name == name
                         select r).FirstOrDefault();

                        UpdateRobotStatus(robot, status);
                    }
                };

        }

        private void CreateRobotScript_Click(object sender, RoutedEventArgs e)
        {
            client.AddRobotScript(this.RobotName.Text, this.ScriptingCodeTextCode.Text);
            LoadRobotManagement_Click(sender, e);
        }

    }
}
