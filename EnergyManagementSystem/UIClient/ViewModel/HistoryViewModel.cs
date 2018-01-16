using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UIClient.View;

namespace UIClient.ViewModel
{
	public class HistoryViewModel : ViewModelBase
	{
		private ICommand showDataCommand;
		private string generatorGid;

		public HistoryViewModel(HistoryView mainWindow)
		{
		}

		#region Commands

		public ICommand ShowDataCommand => showDataCommand ?? (showDataCommand = new RelayCommand(ShowDataCommandExecute));

		#endregion

		#region Properties

		public string GeneratorGid
		{
			get { return generatorGid; }
			set { this.generatorGid = value; }
		}

		#endregion

		#region Command Executions

		private void ShowDataCommandExecute(object obj)
		{
			if (generatorGid != string.Empty)
			{
				if (generatorGid.Trim() != string.Empty)
				{
					try
					{
						long gid = Convert.ToInt64(generatorGid);
						try
						{
							List<Tuple<double, DateTime>> measurementsList = CalculationEngineUIProxy.Instance.GetHistoryMeasurements(gid);
						}
						catch (Exception ex)
						{
							CommonTrace.WriteTrace(CommonTrace.TraceError, "[HistoryViewModel] Error ShowDataCommandExecute {0}", ex.Message);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
		}

		#endregion

	}
}
