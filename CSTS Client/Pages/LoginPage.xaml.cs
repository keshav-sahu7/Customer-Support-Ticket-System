using CSTS.Client.Services;
using System.Windows;
using System.Windows.Controls;

namespace CSTS.Client.Pages
{
    public partial class LoginPage : Page
    {
        private readonly ApiClient _apiClient;

        public LoginPage()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginRequest = new Models.LoginRequest
            {
                Username = UsernameTextBox.Text,
                Password = PasswordBox.Password
            };

            var loginResponse = await _apiClient.LoginAsync(loginRequest);

            if (loginResponse != null)
            {
                this.NavigationService.Navigate(new TicketListPage(loginResponse));
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
