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
using UIClient.View;

namespace UIClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			
			DataContext = this;
			InitializeComponent();

			//string message = string.Format("Network Model Service Test Client is up and running...");
			//Console.WriteLine(message);
			//CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			//message = string.Format("Result directory: {0}", Config.Instance.ResultDirecotry);
			//Console.WriteLine(message);
			//CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

		}

		private List<ModelCode> getSelectedProp()
		{
			List<ModelCode> retList = new List<ModelCode>();

			foreach (var item in PropertiesContainer.Items)
			{
				ContentPresenter c = (ContentPresenter)PropertiesContainer.ItemContainerGenerator.ContainerFromItem(item);
				CheckBox chbox = c.ContentTemplate.FindName("PropCheckBox", c) as CheckBox;
				if (chbox.IsChecked == true)
				{
					var propModelCode = (ModelCode)chbox.DataContext;

					//convert an enum to another type of enum
					//After convert add to list
					retList.Add(propModelCode);

				}
			}

			return retList;
		}

		private List<ModelCode> getModelCodes()
		{
			List<ModelCode> retList = new List<ModelCode>();

			foreach (var item in TypeContainer.Items)
			{
				ContentPresenter c = (ContentPresenter)TypeContainer.ItemContainerGenerator.ContainerFromItem(item);
				CheckBox chbox = c.ContentTemplate.FindName("TypeCheckBox", c) as CheckBox;
				if (chbox.IsChecked == true)
				{
					var type = (EMSType)chbox.DataContext;

					//convert an enum to another type of enum
					//After convert add to list
					retList.Add((ModelCode)Enum.Parse(typeof(ModelCode), type.ToString()));
				}
			}

			return retList;
		}

		

		private void TypeCheckBox_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			

		}

		//private void ExportBtn_Click(object sender, RoutedEventArgs e)
		//{
		//	var b = borderHelper;
		//	Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
		//	dlg.FileName = "Results"; // Default file name
		//	dlg.DefaultExt = ".xml"; // Default file extension
		//	dlg.Filter = "Extensible Markup Language (.xml)|*.xml"; // Filter files by extension

		//	// Show save file dialog box
		//	Nullable<bool> result = dlg.ShowDialog();

		//	// Process save file dialog box results
		//	if (result == true)
		//	{
		//		// Save document
		//		string filename = dlg.FileName;
		//		using (XmlTextWriter xmlWriter = new XmlTextWriter(filename, Encoding.Unicode))
		//		{
		//			xmlWriter.Formatting = Formatting.Indented;
		//			foreach (var rd in ResList)
		//			{
		//				rd.ExportToXml(xmlWriter);
		//			}
		//		}
		//	}
		//}

		private void ResetBtn_Click(object sender, RoutedEventArgs e)
		{
			GlobalID_GV.Text = "";
		}

	}
}
