using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using OsoFramework.Management;
using System.ComponentModel;

namespace OsoFrameworkManager
{
    public class WebRobotItem : WebRobotDashboardItem, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        bool? isToggleChecked;

        public bool? IsToggleChecked
        {
            get
            {
                switch (this.Status)
                {
                    case OsoFramework.WebRobotManagementStatus.ERROR:
                        isToggleChecked = null;
                        break;
                    case OsoFramework.WebRobotManagementStatus.IDLE:
                        isToggleChecked = false;
                        break;
                    case OsoFramework.WebRobotManagementStatus.RUNNING:
                        isToggleChecked = true;
                        break;
                    case OsoFramework.WebRobotManagementStatus.STOPPED:
                        isToggleChecked = false;
                        break;
                }

                return isToggleChecked;
            }
            set
            {
                isToggleChecked = value;
                NotifyPropertyChanged("IsToggleChecked");
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }



        public static WebRobotItem Convert(WebRobotDashboardItem item)
        {
            WebRobotItem newItem = new WebRobotItem
                {
                    DatabaseConnectionString = item.DatabaseConnectionString,
                    Name = item.Name,
                    Status = item.Status
                };



            return newItem;
        }
    }
}
