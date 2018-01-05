using EMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using UIClient.PubSub;
using UIClient.View;
using UIClient.ViewModel;

namespace UIClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			
			DataContext = new MainWindowViewModel();
			InitializeComponent();

            //string message = string.Format("Network Model Service Test Client is up and running...");
            //Console.WriteLine(message);
            //CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            //message = string.Format("Result directory: {0}", Config.Instance.ResultDirecotry);
            //Console.WriteLine(message);
            //CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            try
            {
                AlarmsEventsSubscribeProxy.Instance.Subscribe();
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to AlarmsEvents Publisher Service! \n {0}", e.Message);
            }

        }
		
	}
}
