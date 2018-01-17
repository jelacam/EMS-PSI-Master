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
        private List<Tuple<double, DateTime>> measurementsList;

        public HistoryViewModel(HistoryView mainWindow)
		{
            measurementsList = new List<Tuple<double, DateTime>>();
            measurementsList.Add(new Tuple<double, DateTime>(4.3, DateTime.Now));
            OnPropertyChanged(nameof(MeasurementsList));
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

        public List<Tuple<double, DateTime>> MeasurementsList
        {
            get
            {
                return measurementsList;
            }

            set
            {
                measurementsList = value;
            }
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
							MeasurementsList = CalculationEngineUIProxy.Instance.GetHistoryMeasurements(gid);
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
