using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls.Test.Prototype
{
    public class StateGrid : System.Windows.Controls.Grid
    {
        public StateGrid()
        {
            DataContextChanged += _DataContextChanged;
        }

        private ChildrenGrid _childrenGridTop = new ChildrenGrid();// { Orientation = Orientation.Horizontal};
        private ChildrenGrid _childrenGrid = new ChildrenGrid();

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Children.Add(_childrenGridTop);
            Children.Add(_childrenGrid);
            Grid.SetRow(_childrenGrid, 1);
        }
        private void Update()
        {
            _childrenGridTop.DataContext = DataContext;
            _childrenGrid.DataContext = DataContext;

            if(DataContext != null)
            {
                var value = ObjectExtension.GetValue(DataContext) as IDictionary;
                if(value != null && value.Contains("Type"))
                {
                    string type = value["Type"].ToString();
                    var wrapper = new Dictionary<string, dynamic>();
                    wrapper.Add("tmp", ObjectExtension.GetValue(DataContext));
                    _childrenGridTop.DataContext = new KeyValuePair<string, dynamic>("tmp", wrapper);
                }
               // _childrenGridTop.DataContext = new KeyValuePair<string, dynamic>("tmp", DataContext.GetValue());
            }
        }
    }
}
