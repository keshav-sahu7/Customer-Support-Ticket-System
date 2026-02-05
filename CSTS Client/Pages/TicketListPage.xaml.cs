using CSTS.Client.Models;
using CSTS.Client.Services;
using System.Windows;
using System.Windows.Controls;
using System; // Added for Enum.Parse

namespace CSTS.Client.Pages
{
    public partial class TicketListPage : Page
    {
        private readonly LoginResponseDto _loginResponse; // Changed type
        private readonly ApiClient _apiClient;

        public TicketListPage(LoginResponseDto loginResponse) // Changed parameter type
        {
            InitializeComponent();
            _loginResponse = loginResponse;
            _apiClient = new ApiClient(_loginResponse.Token); // ApiClient now takes token
            LoadTickets();
        }

        private async void LoadTickets()
        {
            // Assuming UserRole enum is accessible, either in Client.Models or referenced
            UserRole userRole = Enum.Parse<UserRole>(_loginResponse.Role); // Get role from login response
            var tickets = await _apiClient.GetTicketsAsync(_loginResponse.UserId, userRole);
            TicketsListView.ItemsSource = tickets;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTickets();
        }

        private void CreateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateTicketPage(_loginResponse, _apiClient));
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTicket = (sender as Button)?.DataContext as Ticket;
            if (selectedTicket != null)
            {
                this.NavigationService.Navigate(new TicketDetailsPage(selectedTicket, _loginResponse, _apiClient));
            }
        }
    }
}
