using EMS.Common;
using System.Windows;
using System.Windows.Controls;

namespace UIClient.View
{
	/// <summary>
	/// Interaction logic for ResourceView.xaml
	/// </summary>
	public partial class ResourceView : UserControl
	{
		public ResourceView()
		{
			DataContext = this;
			InitializeComponent();
		}

		public static readonly DependencyProperty ResourceDescProperty =
	DependencyProperty.Register("ResourceDesc", typeof(ResourceDescription), typeof(ResourceView), new PropertyMetadata(null));

		public ResourceDescription ResourceDesc
		{
			get
			{
				return (ResourceDescription)this.GetValue(ResourceDescProperty);
			}
			set
			{
				this.SetValue(ResourceDescProperty, value);
			}
		}

	}
}
