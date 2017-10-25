using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace testApp
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        int index = 1;
        IClientService service = null;
        ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += async (s, e) =>
            {
            };
        }

        public async Task<UserCredential> GetCredential()
        {
            var scopes = new[] { GmailService.Scope.GmailModify, Google.Apis.Plus.v1.PlusService.Scope.PlusLogin, Google.Apis.Plus.v1.PlusService.Scope.UserinfoEmail, Google.Apis.Plus.v1.PlusService.Scope.UserinfoProfile };
            var clientSecrets = new ClientSecrets()
            {
                ClientId = "733025278625-9nj4pg86tnu95kthh1k78c9c3caj6cov.apps.googleusercontent.com",
                ClientSecret = "JBhxhgvJCIqW6gPEYRY3C-Xp"
            };

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, $"me{index}", CancellationToken.None);
            return credential;
        }

        private void CreateTopic()
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cred = await GetCredential();
                service = new Google.Apis.Plus.v1.PlusService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = cred,
                    ApplicationName = "Test"
                });

                index++;

                Google.Apis.Plus.v1.PeopleResource.GetRequest getRequest = new Google.Apis.Plus.v1.PeopleResource.GetRequest(service, $"me");
                var response = getRequest.Execute();

                list.Items.Add(new User()
                {
                    Id = response.Id,
                    Name = response.DisplayName,
                    Image = response.Image.Url,
                    Email = response.Emails.FirstOrDefault()?.Value
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
