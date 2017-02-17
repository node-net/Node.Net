namespace Node.Net.View
{
    public class DynamicViewSelector : System.Windows.Controls.UserControl
    {
        private DynamicView dynamicView = new DynamicView();
        private ComboBox comboBox = null;
        public DynamicViewSelector() 
        {
            dynamicView = new DynamicView();
            DataContextChanged += DynamicViewSelector_DataContextChanged;
        }

        
        public DynamicViewSelector(System.Windows.FrameworkElement defaultElement) 
        {
            dynamicView = new DynamicView(defaultElement);
            DataContextChanged += DynamicViewSelector_DataContextChanged;
        }

        public DynamicView DynamicView => dynamicView;
        void DynamicViewSelector_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = System.Windows.GridLength.Auto });
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            comboBox = new ComboBox();
            comboBox.DataContext = dynamicView.Elements.Keys;
            comboBox.SelectionChanged += comboBox_SelectionChanged;

            System.Windows.Controls.Grid horizontalGrid = new System.Windows.Controls.Grid();
            horizontalGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition(){Width = System.Windows.GridLength.Auto});
            horizontalGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            horizontalGrid.Children.Add(new System.Windows.Controls.Label() { Content = "Name" });
            horizontalGrid.Children.Add(comboBox);
            System.Windows.Controls.Grid.SetColumn(comboBox, 1);
            grid.Children.Add(horizontalGrid);

            grid.Children.Add(dynamicView);
            System.Windows.Controls.Grid.SetRow(dynamicView, 1);
            
            Content = grid;

            Update();
        }

        void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBoxItem cbItem = comboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
            if(!object.ReferenceEquals(null,cbItem))
            {
                string name = KeyValuePair.GetValue(cbItem.DataContext).ToString();
                dynamicView.Current = name;
                Update();
            }
        }

        private void Update()
        {
            if (object.ReferenceEquals(null, comboBox)) return;
            comboBox.DataContext = dynamicView.Elements.Keys;
            dynamicView.DataContext = DataContext;
        }
    }
}
