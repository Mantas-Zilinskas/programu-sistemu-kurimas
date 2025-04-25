using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace EmocineSveikataServer.Services.Meets
{
    public class GoogleMeetService
    {
        private readonly IConfiguration _config;

        public GoogleMeetService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> CreateMeetAsync()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = _config["Google:ClientId"],
                    ClientSecret = _config["Google:ClientSecret"]
                },
                new[] { CalendarService.Scope.Calendar },
                "user",
                CancellationToken.None,
                new FileDataStore("token-store", true),
                new FixedPortCodeReceiver()
            );

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Emocine Sveikata"
            });

            var calendarEvent = new Event
            {
                Summary = "Privatus Pokalbis",
                Start = new EventDateTime
                {
                    DateTime = DateTime.Now.AddMinutes(5),
                    TimeZone = "Europe/Vilnius"
                },
                End = new EventDateTime
                {
                    DateTime = DateTime.Now.AddMinutes(30),
                    TimeZone = "Europe/Vilnius"
                },
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                }
            };

            var request = service.Events.Insert(calendarEvent, "primary");
            request.ConferenceDataVersion = 1;

            var createdEvent = await request.ExecuteAsync();

            return createdEvent.ConferenceData.EntryPoints
                .FirstOrDefault(ep => ep.EntryPointType == "video")?.Uri ?? "No Meet link generated";
        }
    }
}
