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
		private TestGda tgda;
		private ObservableCollection<ResourceDescription> resList;
		private ObservableCollection<ModelCode> avaliableProperties;

		private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
		private Dictionary<ModelCode, List<ModelCode>> propertyMap = new Dictionary<ModelCode, List<ModelCode>>();

		public ObservableCollection<ResourceDescription> ResList
		{
			get
			{
				return resList;
			}

			set
			{
				resList = value;
			}
		}

		public ObservableCollection<ModelCode> AvaliableProperties
		{
			get
			{
				return avaliableProperties;
			}

			set
			{
				avaliableProperties = value;
			}
		}

		public MainWindow()
		{
			ResList = new ObservableCollection<ResourceDescription>();
			AvaliableProperties = new ObservableCollection<ModelCode>();
			DataContext = this;
			InitializeComponent();

			//string message = string.Format("Network Model Service Test Client is up and running...");
			//Console.WriteLine(message);
			//CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			//message = string.Format("Result directory: {0}", Config.Instance.ResultDirecotry);
			//Console.WriteLine(message);
			//CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

		}

		private void Find_Click(object sender, RoutedEventArgs e)
		{
			var hex_val = GlobalID_GV.Text;
			ResList.Clear();

			List<ModelCode> forFind = getModelCodes();

			var allSelected = getSelectedProp();
			foreach (var modCode in forFind)
			{
				var myProps = modelResourcesDesc.GetAllPropertyIds(ModelCodeHelper.GetTypeFromModelCode(modCode));
				var mySelected = myProps.Where(x => allSelected.Contains(x));
				var retExtentValues = tgda.GetExtentValues(modCode, mySelected.ToList());
				foreach (var res in retExtentValues)
				{
					//if (!ResList.ToList().Exists((x) => x.Id == res.Id))
					ResList.Add(res);
				}
			}

			if (hex_val.Trim() != string.Empty)
			{
				try
				{
					long gid = Convert.ToInt64(hex_val, 16);
					ResourceDescription rd = tgda.GetValues(gid);
					if (!ResList.ToList().Exists((x) => x.Id == rd.Id))
						ResList.Add(rd);
				}
				catch (Exception ex)
				{

				}
			}

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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ModelResourcesDesc resDesc = new ModelResourcesDesc();

			string message = string.Format("Network Model Service Test Client is up and running...");
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			message = string.Format("Result directory: {0}", Config.Instance.ResultDirecotry);
			Console.WriteLine(message);
			CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

			tgda = new TestGda();
		}

		private void UpdatePropertyFilter()
		{
			List<ModelCode> selectedModelCodes = getModelCodes();
			List<ModelCode> properties;
			AvaliableProperties.Clear();
			foreach (var modelCode in selectedModelCodes)
			{
				properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

				foreach (var prop in properties)
				{
					if (!AvaliableProperties.Contains(prop))
					{
						AvaliableProperties.Add(prop);
					}
				}

			}
		}

		private void TypeCheckBox_Click(object sender, RoutedEventArgs e)
		{
			UpdatePropertyFilter();
		}

		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var grid = sender as Grid;
			var property = grid.DataContext as Property;
			var resDesc = grid.Tag as ResourceDescription;

			List<ResourceDescription> refResList = new List<ResourceDescription>();

			ReferenceView RefView = new ReferenceView(tgda, resDesc.Id, property);
			RefView.Visibility = System.Windows.Visibility.Visible;

		}

		private void ExportBtn_Click(object sender, RoutedEventArgs e)
		{
			var b = borderHelper;
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = "Results"; // Default file name
			dlg.DefaultExt = ".xml"; // Default file extension
			dlg.Filter = "Extensible Markup Language (.xml)|*.xml"; // Filter files by extension

			// Show save file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result == true)
			{
				// Save document
				string filename = dlg.FileName;
				using (XmlTextWriter xmlWriter = new XmlTextWriter(filename, Encoding.Unicode))
				{
					xmlWriter.Formatting = Formatting.Indented;
					foreach (var rd in ResList)
					{
						rd.ExportToXml(xmlWriter);
					}
				}
			}
		}

		private void ResetBtn_Click(object sender, RoutedEventArgs e)
		{
			GlobalID_GV.Text = "";
		}

	}
}
