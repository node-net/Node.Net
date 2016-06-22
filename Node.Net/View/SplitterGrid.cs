namespace Node.Net.View
{
    public class SplitterGrid : System.Windows.Controls.Grid
    {
        private System.Windows.Controls.Orientation orientation = System.Windows.Controls.Orientation.Vertical;
        private System.Collections.Generic.List<System.Windows.FrameworkElement> elements
            = new System.Collections.Generic.List<System.Windows.FrameworkElement>();
        protected System.Collections.Generic.List<System.Windows.GridLength> gridLengths
            = new System.Collections.Generic.List<System.Windows.GridLength>();
        public SplitterGrid()
        {
            DataContextChanged += SplitterGrid_DataContextChanged;
        }

        public SplitterGrid(System.Windows.Controls.Orientation value) 
        { 
            orientation = value;
            DataContextChanged += SplitterGrid_DataContextChanged;
        }
        public SplitterGrid(System.Windows.Controls.Orientation value,System.Windows.GridLength gridLength1)
        {
            orientation = value;
            DataContextChanged +=SplitterGrid_DataContextChanged;
            gridLengths.Add(gridLength1);
            //firstGridLength = gridLength1;
        }
        
        public SplitterGrid(System.Windows.FrameworkElement[] value,System.Windows.Controls.Orientation orientation_value)
        {
            DataContextChanged += SplitterGrid_DataContextChanged;
            orientation = orientation_value;
            elements.Clear();
            foreach (System.Windows.FrameworkElement e in value) { elements.Add(e); }
            Update();
        }

        public SplitterGrid(System.Windows.FrameworkElement[] value, 
            System.Windows.Controls.Orientation orientation_value,
            System.Windows.GridLength[] grid_lengths)
        {
            DataContextChanged += SplitterGrid_DataContextChanged;
            orientation = orientation_value;
            elements.Clear();
            foreach (System.Windows.FrameworkElement e in value) { elements.Add(e); }
            gridLengths = new System.Collections.Generic.List<System.Windows.GridLength>(grid_lengths);
            Update();
        }

        void SplitterGrid_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public System.Windows.Controls.Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public System.Windows.FrameworkElement[] Elements
        {
            get { return elements.ToArray(); }
            set
            {
                elements = new System.Collections.Generic.List<System.Windows.FrameworkElement>(value);
                Update();
            }
        }

        public virtual void Update()
        {
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            if(orientation == System.Windows.Controls.Orientation.Vertical)
            {
                var rowCount = elements.Count + (elements.Count - 1);
                for (int r = 0; r < rowCount; ++r)
                {
                    
                    var rowDefinition = new System.Windows.Controls.RowDefinition();
                    //if (r == 0) rowDefinition.Height = firstGridLength;
                    if (r % 2 != 0) rowDefinition.Height = System.Windows.GridLength.Auto;
                    RowDefinitions.Add(rowDefinition);
                    if(r%2 ==0)
                    {
                        if (gridLengths.Count > r / 2) rowDefinition.Height = gridLengths[r / 2];
                        Children.Add(elements[r / 2]);
                        elements[r/2].DataContext = DataContext;
                        System.Windows.Controls.Grid.SetRow(elements[r / 2], r);
                    }
                    else
                    {

                        var rowSplitter = 
                            new System.Windows.Controls.GridSplitter
                                {
                                    HorizontalAlignment=System.Windows.HorizontalAlignment.Stretch,
                                    VerticalAlignment=System.Windows.VerticalAlignment.Center,
                                    Height=5
                                };
                        Children.Add(rowSplitter);
                        System.Windows.Controls.Grid.SetRow(rowSplitter, r);
                    }
                }
            }
            else
            {
                var colCount = elements.Count + (elements.Count - 1);
                for (int c = 0; c < colCount; ++c)
                {
                    
                    var colDefinition = new System.Windows.Controls.ColumnDefinition();
                    //if (c == 0) colDefinition.Width = firstGridLength;
                    if (c % 2 != 0) colDefinition.Width = System.Windows.GridLength.Auto;
                    ColumnDefinitions.Add(colDefinition);
                    if (c % 2 == 0)
                    {
                        if (gridLengths.Count > c / 2) colDefinition.Width = gridLengths[c / 2];
                        Children.Add(elements[c / 2]);
                        elements[c/2].DataContext = DataContext;
                        System.Windows.Controls.Grid.SetColumn(elements[c / 2], c);
                    }
                    else
                    {

                        var colSplitter =
                            new System.Windows.Controls.GridSplitter
                            {
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                                Width = 5
                            };
                        Children.Add(colSplitter);
                        System.Windows.Controls.Grid.SetColumn(colSplitter, c);
                    }
                }
            }
        }
    }
}
