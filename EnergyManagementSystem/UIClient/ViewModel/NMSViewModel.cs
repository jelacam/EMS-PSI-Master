using EMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UIClient.View;

namespace UIClient.ViewModel
{
    public class NMSViewModel : ViewModelBase
    {
        private NMSView NMSview;
        private TestGda tgda;
        private ObservableCollection<ResourceDescription> resList;
        private ObservableCollection<ModelCode> avaliableProperties;

        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
        private Dictionary<ModelCode, List<ModelCode>> propertyMap = new Dictionary<ModelCode, List<ModelCode>>();
        private ICommand findCommand;
        private ICommand typeCheckBoxChangedCommand;
        private RelayCommand goToReferenceCommand;

        public NMSViewModel(NMSView mainWindow)
        {
            Title = "NMS";
            this.NMSview = mainWindow;
            this.NMSview.Loaded += View_Loaded;
            ResList = new ObservableCollection<ResourceDescription>();
            AvaliableProperties = new ObservableCollection<ModelCode>();
        }



        #region Properties
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
        #endregion


        #region Commands
        public ICommand FindCommand => findCommand ?? (findCommand = new RelayCommand<string>(FindCommandExecute));

        public ICommand TypeCheckBoxChangedCommand => typeCheckBoxChangedCommand ?? (typeCheckBoxChangedCommand = new RelayCommand(TypeCheckBoxChangedCommandExecute));

        public ICommand GoToReferenceCommand => goToReferenceCommand ?? (goToReferenceCommand = new RelayCommand(GoToReferenceCommandExecute));


        #endregion


        #region CommandExecutions
        private void FindCommandExecute(string textForFind)
        {
            var hex_val = textForFind;
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
                    {
                        ResList.Add(rd);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void TypeCheckBoxChangedCommandExecute(object obj)
        {
            UpdatePropertyFilter();
        }
        private void GoToReferenceCommandExecute(object obj)
        {
            var grid = obj as Grid;
            var property = grid.DataContext as Property;
            var resDesc = grid.Tag as ResourceDescription;

            List<ResourceDescription> refResList = new List<ResourceDescription>();

            ReferenceView RefView = new ReferenceView(tgda, resDesc.Id, property);
            RefView.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

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

        private List<ModelCode> getSelectedProp()
        {
            List<ModelCode> retList = new List<ModelCode>();
            foreach (var item in NMSview.PropertiesContainer.Items)
            {
                ContentPresenter c = (ContentPresenter)NMSview.PropertiesContainer.ItemContainerGenerator.ContainerFromItem(item);
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

            foreach (var item in NMSview.TypeContainer.Items)
            {
                ContentPresenter c = (ContentPresenter)NMSview.TypeContainer.ItemContainerGenerator.ContainerFromItem(item);
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

        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

        public override void Dispose()
        {
            NMSview.Loaded -= View_Loaded;
            base.Dispose();
        }
    }
}
