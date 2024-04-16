using System;
using System.Windows;
using APPZ.Decorator.Requests;
using APPZ.Decorator.Users;
using APPZ.Enums;

namespace APPZ.Windows
{
    public partial class TaskWindow
    {
        private IUser _user;
        
        public TaskWindow(IUser user)
        {
            _user = user;
            
            InitializeComponent();

            LblUser.Content = $"{_user.GetProperties()[UserPublicProps.Surname]} " +
                              $"{_user.GetProperties()[UserPublicProps.Name]}";
            
            _user.CreateButton(GridButtons);
            
            PoolManager.GetInstance().PreloadUpdate(_user.GetId());
        }

        private void TaskWindow_OnLoaded(object sender, EventArgs e)
            => PoolManager.GetInstance().LoadUpdate(_user.GetId());

        private void BtnLogout_OnClick(object sender, RoutedEventArgs e)
        {
            WelcomeWindow window = new WelcomeWindow();
            _user.Reset();
            window.Show();

            Close();
        }
    }
}