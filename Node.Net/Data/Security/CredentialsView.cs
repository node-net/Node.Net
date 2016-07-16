using System;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Data.Security
{
    public class CredentialsView : Grid
    {
        public CredentialsView()
        {
            DataContext = CurrentUserCredentials.Default;
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();

        public CredentialsList CredentialsList = new CredentialsList();
        public CredentialView CredentialView = new CredentialView();
        public Button AddButton = new Button { Content = "Add" };
        public Button DoneButton = new Button { Content = "Done" };
        public Button CancelButton = new Button { Content = "Cancel" };

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());

            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(AddButton);
            stackPanel.Children.Add(DoneButton);
            stackPanel.Children.Add(CancelButton);

            Children.Add(stackPanel);

            Children.Add(CredentialsList);
            Grid.SetRow(CredentialsList, 1);
            Children.Add(CredentialView);
            Grid.SetRow(CredentialView, 1);
            Grid.SetColumn(CredentialView, 1);

            CredentialsList.SelectionChanged += CredentialsList_SelectionChanged;
            AddButton.Click += AddButton_Click;
            CancelButton.Click += CancelButton_Click;
            DoneButton.Click += DoneButton_Click;
            Update();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var credentials = DataContext as ICredentials;
            var credential = CredentialView.DataContext as Credential;
            if (credential != null && credentials != null)
            {
                if (credential.Domain.Length > 0 && credential.UserName.Length > 0)
                {
                    credentials.Set(credential.Domain, credential.UserName, credential.Password);
                }
            }
            AddButton.Visibility = Visibility.Visible;
            Update();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            Update();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Hidden;
            Update();
        }

        private void CredentialsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            CredentialsList.DataContext = null;
            CredentialsList.DataContext = DataContext;
            if (AddButton.Visibility == Visibility.Hidden)
            {
                // Add a new Credential
                CredentialView.DataContext = new Credential();
                CredentialsList.Visibility = Visibility.Hidden;
                CancelButton.Visibility = Visibility.Visible;
                DoneButton.Visibility = Visibility.Visible;
            }
            else
            {
                if (CredentialsList.SelectedIndex == -1)
                {
                    // Show just the List
                    CredentialsList.Visibility = Visibility.Visible;
                    CredentialView.DataContext = null;
                    AddButton.Visibility = Visibility.Visible;
                    DoneButton.Visibility = Visibility.Hidden;
                    CancelButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    CredentialView.DataContext = (CredentialsList.SelectedItem as ListBoxItem).DataContext;
                    DoneButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    AddButton.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
