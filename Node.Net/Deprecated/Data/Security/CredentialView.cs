using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Node.Net.Data.Deprecated.Security
{
    public class CredentialView : Grid
    {
        public CredentialView()
        {
            DataContextChanged += Update;
        }

        public Credential Credential
        {
            get
            {
                var credential = DataContext as Credential;
                if (credential == null)
                {
                    credential = new Credential();
                    DataContext = new Credential();
                }
                return DataContext as Credential;
            }
            set
            {
                DataContext = value;
            }
        }
        public TextBox DomainBox = new TextBox { Width = 300, Margin = new Thickness(5) };
        public TextBox UserNameBox = new TextBox { Width = 300, Margin = new Thickness(5) };
        public PasswordBox PasswordBox = new PasswordBox { Width = 300, Margin = new Thickness(5) };

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });    // Domain
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });    // UserName
            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });    // Password
            RowDefinitions.Add(new RowDefinition());

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition());

            var domain = new Label { Content = "Domain" };
            var user = new Label { Content = "UserName" };
            var password = new Label { Content = "Password" };

            Children.Add(domain);
            Children.Add(user);
            Children.Add(password);
            Grid.SetColumn(domain, 1);
            Grid.SetColumn(user, 1);
            Grid.SetColumn(password, 1);
            Grid.SetRow(domain, 1);
            Grid.SetRow(user, 2);
            Grid.SetRow(password, 3);

            Children.Add(DomainBox);
            Children.Add(UserNameBox);
            Children.Add(PasswordBox);
            Grid.SetColumn(DomainBox, 2);
            Grid.SetColumn(UserNameBox, 2);
            Grid.SetColumn(PasswordBox, 2);
            Grid.SetRow(DomainBox, 1);
            Grid.SetRow(UserNameBox, 2);
            Grid.SetRow(PasswordBox, 3);

            DomainBox.TextChanged += DomainBox_TextChanged;
            UserNameBox.TextChanged += UserNameBox_TextChanged;
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
        private void DomainBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Credential.Domain = DomainBox.Text;
        }
        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Credential.UserName = UserNameBox.Text;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Credential.Password = Security.Credential.MakeSecureString(PasswordBox.Password);
        }





        private void Update(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
