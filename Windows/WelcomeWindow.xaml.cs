using System.Windows;
using APPZ.Databases;

namespace APPZ.Windows
{
    public partial class WelcomeWindow
    {
        private readonly Database _database;
        private static bool _firstAccess = true;
        
        public WelcomeWindow()
        {
            InitializeComponent();
            
            Title = "Вхід до системи \"ФОП\"";

            _database = Database.GetInstance();
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var user = _database.GetUser(TxtBxLogin.Text, PassBxPassword.Password);
            
            if (user == null)
            {
                MessageBox.Show(
                    "Вибачте, даного користувача не було " +
                    "знайдено у базі даних, спробуйте ще раз", 
                    "Не вірні данні",
                    MessageBoxButton.OK,
                    MessageBoxImage.Hand
                );
                return;
            }
            
            if (_firstAccess)
            {
                Database.GetInstance().ReloadRequests();
                _firstAccess = !_firstAccess;
            }
            
            TaskWindow window = new TaskWindow(DecoratorHandler.Decorate(user));
            window.Show();
            Close();
        }

        private void BtnExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}