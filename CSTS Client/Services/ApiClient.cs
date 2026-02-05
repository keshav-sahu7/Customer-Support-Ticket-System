using CSTS.Client.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers; // Added for AuthenticationHeaderValue
using System.Text;
using System.Windows; // Added for MessageBox

namespace CSTS.Client.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _jwtToken; // To store the token

        // Constructor for unauthenticated calls (e.g., login)
        public ApiClient()
        {
            _httpClient = new HttpClient();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _baseUrl = configuration["BaseUrl"] ?? "http://localhost:5171"; // Use simpler access and provide fallback
        }

        // Constructor for authenticated calls
        public ApiClient(string jwtToken) : this() // Call the parameterless constructor to initialize _httpClient and _baseUrl
        {
            _jwtToken = jwtToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        private async Task<T?> HandleErrorResponse<T>(HttpResponseMessage response, string methodName) where T : class
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"{errorMessage}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        private async Task<List<T>> HandleErrorResponseForList<T>(HttpResponseMessage response, string methodName)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"{errorMessage}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new List<T>();
        }



        public async Task<List<TicketComment>> GetCommentsAsync(Guid ticketId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/comment/{ticketId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<TicketComment>>(responseContent);
                return comments ?? new List<TicketComment>();
            }
            return await HandleErrorResponseForList<TicketComment>(response, nameof(GetCommentsAsync));
        }

        public async Task<Ticket?> GetTicketAsync(Guid ticketId) // Changed return type to nullable
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/tickets/{ticketId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ticket = JsonConvert.DeserializeObject<Ticket>(responseContent);
                return ticket;
            }
            return await HandleErrorResponse<Ticket>(response, nameof(GetTicketAsync));
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/users/admins");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(responseContent);
                return users ?? new List<User>();
            }
            return await HandleErrorResponseForList<User>(response, nameof(GetUsersAsync));
        }

        public async Task<Ticket?> UpdateTicketDetailsAsync(UpdateTicketDetailsRequest request, UserRole role)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/tickets/{role.ToString()}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ticket = JsonConvert.DeserializeObject<Ticket>(responseContent);
                return ticket;
            }
            return await HandleErrorResponse<Ticket>(response, nameof(UpdateTicketDetailsAsync));
        }

        public async Task<TicketComment?> AddCommentAsync(Guid userId, Guid ticketId, string comment)
        {
            var addCommentRequest = new AddCommentRequest { Comment = comment };
            var json = JsonConvert.SerializeObject(addCommentRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/comment/{userId}/{ticketId}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ticketComment = JsonConvert.DeserializeObject<TicketComment>(responseContent);
                return ticketComment;
            }
            return await HandleErrorResponse<TicketComment>(response, nameof(AddCommentAsync));
        }

        public async Task<Ticket?> CreateTicketAsync(CreateTicketRequest createTicketRequest) // Changed return type to nullable
        {
            var json = JsonConvert.SerializeObject(createTicketRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/tickets", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ticket = JsonConvert.DeserializeObject<Ticket>(responseContent);
                return ticket;
            }
            return await HandleErrorResponse<Ticket>(response, nameof(CreateTicketAsync));
        }

        public async Task<List<Ticket>> GetTicketsAsync(Guid userId, UserRole userRole)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/tickets/all/{userId.ToString()}?role={userRole}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tickets = JsonConvert.DeserializeObject<List<Ticket>>(responseContent);
                return tickets ?? new List<Ticket>();
            }
            return await HandleErrorResponseForList<Ticket>(response, nameof(GetTicketsAsync));
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequest loginRequest) // Changed return type
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);
                return loginResponse;
            }
            return await HandleErrorResponse<LoginResponseDto>(response, nameof(LoginAsync));
        }
    }
}
