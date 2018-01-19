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
		private List<Tuple<double, DateTime>> measurements;
		private DateTime startTime;
		private DateTime endTime;

		public HistoryViewModel(HistoryView mainWindow)
		{
			startTime = DateTime.Now;
			endTime = DateTime.Now;
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

		public List<Tuple<double, DateTime>> Measurements
		{
			get { return measurements; }
			set { measurements = value; }
		}

		public DateTime StartTime
		{
			get { return startTime; }
			set { startTime = value; }
		}

		public DateTime EndTime
		{
			get { return endTime; }
			set { endTime = value; }
		}

		#endregion

		#region Command Executions

		private void ShowDataCommandExecute(object obj)
		{
			if (generatorGid != null && generatorGid != string.Empty)
			{
				if (generatorGid.Trim() != string.Empty)
				{
					try
					{
						long gid = Convert.ToInt64(generatorGid);
						try
						{
							Measurements = CalculationEngineUIProxy.Instance.GetHistoryMeasurements(gid, startTime, endTime);
							OnPropertyChanged(nameof(Measurements));

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
