using CSTS.Client.Models;
using CSTS.Client.Services;
using System.Windows;
using System.Windows.Controls;

namespace CSTS.Client.Pages
{
    public partial class CreateTicketPage : Page
    {
        private readonly ApiClient _apiClient;
        private readonly LoginResponseDto _loginResponse;

        public CreateTicketPage(LoginResponseDto loginResponse, ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient; // Use injected ApiClient
            _loginResponse = loginResponse; // Store login response
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var priority = selectedItem.Content.ToString();
                if (priority != null) // Additional null check for content
                {
                    var createTicketRequest = new Models.CreateTicketRequest
                    {
                        Subject = SubjectTextBox.Text,
                        Description = DescriptionTextBox.Text,
                        Priority = priority,
                        UserId = _loginResponse.UserId
                    };

                    var ticket = await _apiClient.CreateTicketAsync(createTicketRequest);

                    if (ticket != null)
                    {
                        this.NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create ticket.");
                    }
                }
                else
                {
                    MessageBox.Show("Priority cannot be null.");
                }
            }
            else
            {
                MessageBox.Show("Please select a priority.");
            }
        }
    }
}

