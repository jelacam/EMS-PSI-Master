using EMS.CIMAdapter;
using EMS.CIMAdapter.Manager;
using EMS.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace UIClient.ViewModel
{
	public class ImporterViewModel:ViewModelBase
	{
		private ICommand showOpenDialogCommand;
		private ICommand convertCommand;
		private ICommand applyCommand;

		private CIMAdapter adapter = new CIMAdapter();
		private Delta nmsDelta = null;
		private string cimFileLocation;
		private string convertReport;
		private string applyReport;

		#region Commands
		public ICommand ShowOpenDialogCommand => showOpenDialogCommand ?? (showOpenDialogCommand = new RelayCommand(ShowOpenDialogCommandExecute));

		public ICommand ConvertCommand => convertCommand ?? (convertCommand = new RelayCommand(ConvertCommandExecute,(param)=> { return CimFileLocation != string.Empty; }));

		public ICommand ApplyCommand => applyCommand ?? (applyCommand = new RelayCommand(ApplyCommandExecute,(param)=> { return nmsDelta != null; }));
		#endregion

		#region Properties
		public string CimFileLocation
		{
			get
			{
				return cimFileLocation;
			}

			set
			{
				cimFileLocation = value;
				OnPropertyChanged();
			}
		}

		public string ConvertReport
		{
			get
			{
				return convertReport;
			}

			set
			{
				convertReport = value;
				OnPropertyChanged();
			}
		}

		public string ApplyReport
		{
			get
			{
				return applyReport;
			}

			set
			{
				applyReport = value;
				OnPropertyChanged();
			}
		} 
		#endregion

		#region Command Executions
		private void ShowOpenDialogCommandExecute(object obj)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Open CIM Document File..";
			openFileDialog.Filter = "CIM-XML Files|*.xml;*.txt;*.rdf|All Files|*.*";
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == true)
			{
				CimFileLocation = openFileDialog.FileName;
			}

		}

		private void ConvertCommandExecute(object obj)
		{
			try
			{
				if (CimFileLocation == string.Empty)
				{
					MessageBox.Show("Must enter CIM/XML file.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}

				string log;
				nmsDelta = null;
				using (FileStream fs = File.Open(CimFileLocation, FileMode.Open))
				{
					nmsDelta = adapter.CreateDelta(fs, SupportedProfiles.EMSData, out log);
					ConvertReport = log;
				}
				if (nmsDelta != null)
				{
					//// export delta to file
					using (XmlTextWriter xmlWriter = new XmlTextWriter(".\\deltaExport.xml", Encoding.UTF8))
					{
						xmlWriter.Formatting = Formatting.Indented;
						nmsDelta.ExportToXml(xmlWriter);
						xmlWriter.Flush();
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(string.Format("An error occurred.\n\n{0}", e.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			CimFileLocation = string.Empty;
		}

		private void ApplyCommandExecute(object obj)
		{
			//// APPLY Delta
			if (nmsDelta != null)
			{
				try
				{
					string log = adapter.ApplyUpdates(nmsDelta);
					ApplyReport = log;
					nmsDelta = null;
				}
				catch (Exception e)
				{
					MessageBox.Show(string.Format("An error occurred.\n\n{0}", e.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("No data is imported into delta object.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		#endregion

	}
}
