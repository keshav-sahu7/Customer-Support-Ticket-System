using CSTS.Client.Models;
using CSTS.Client.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CSTS.Client.Pages
{
    public partial class TicketDetailsPage : Page
    {
        private readonly Ticket _ticket;
        private readonly ApiClient _apiClient;
        private readonly LoginResponseDto _loginResponse; // Changed from User to LoginResponseDto


        public TicketDetailsPage(Ticket ticket, LoginResponseDto loginResponse, ApiClient apiClient) // Changed constructor parameters
        {
            InitializeComponent();
            _ticket = ticket;
            _apiClient = apiClient; // Use injected ApiClient
            _loginResponse = loginResponse; // Store login response
            this.DataContext = _ticket;
            LoadData();
        }

        private async void LoadData()
        {
            var ticket = await _apiClient.GetTicketAsync(_ticket.Id);
            if (ticket != null)
            {
                this.DataContext = ticket;
            }

            var comments = await _apiClient.GetCommentsAsync(_ticket.Id);
            CommentsListView.ItemsSource = comments;

            if (_loginResponse.Role == UserRole.Admin.ToString()) // Use role from login response
            {
                var users = await _apiClient.GetUsersAsync();
                AssignToComboBox.ItemsSource = users;
                AssignToComboBox.DisplayMemberPath = "Username";
                AssignToComboBox.SelectedValuePath = "Id";
            }
            else
            {
                AssignToComboBox.IsEnabled = false;
                StatusComboBox.IsEnabled = false;
                // AssignButton.IsEnabled = false; // These buttons might be in XAML, so keep commented for now
                // ChangeStatusButton.IsEnabled = false;
            }

        }

        private async void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssignToComboBox.SelectedValue != null)
            {
                var assignToId = (Guid)AssignToComboBox.SelectedValue;
                var updatedTicket = await _apiClient.AssignTicketAsync(_ticket.Id, assignToId);
                if (updatedTicket != null)
                {
                    this.DataContext = updatedTicket;
                }
            }
        }

        private async void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem) // Use pattern matching for null check
            {
                var status = selectedItem.Content.ToString();
                if (status != null) // Additional null check
                {
                    var updatedTicket = await _apiClient.UpdateTicketStatusAsync(_ticket.Id, status);
                    if (updatedTicket != null)
                    {
                        this.DataContext = updatedTicket;
                    }
                }
            }
        }

        private async void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            var comment = CommentTextBox.Text;
            if (!string.IsNullOrEmpty(comment))
            {
                var newComment = await _apiClient.AddCommentAsync(_ticket.Id, comment);
                if (newComment != null)
                {
                    CommentTextBox.Text = string.Empty;
                    var comments = await _apiClient.GetCommentsAsync(_ticket.Id);
                    CommentsListView.ItemsSource = comments;
                }
            }
        }
    }
}