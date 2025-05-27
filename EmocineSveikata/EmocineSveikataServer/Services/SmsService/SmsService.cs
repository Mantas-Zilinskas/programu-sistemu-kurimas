using EmocineSveikataServer.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EmocineSveikataServer.Services.SmsService
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, List<string>> _topicMessages;
        
        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            // Initialize the dictionary of messages for each topic
            _topicMessages = new Dictionary<string, List<string>>
            {
                { "Depression", new List<string> {
                    "Prisimink, kad nesi vienas(-a). Ieškant pagalbos, rodomas stiprumas, ne silpnumas.",
                    "Kiekviena diena yra nauja pradžia. Mažais žingsniais pirmyn.",
                    "Tavo jausmai yra tikri ir svarbūs, bet jie nėra visas tu. Tu esi daugiau nei tavo depresija."
                }},
                { "MentalHealth", new List<string> {
                    "Psichinės sveikatos priežiūra yra sveikatos priežiūros dalis. Skirk laiko sau šiandien.",
                    "Kvėpuok giliai, būk čia ir dabar. Tu darai puikų darbą.",
                    "Savęs priežiūra nėra savanaudiškumas. Tai būtina tavo gerovei."
                }},
                { "ADHD", new List<string> {
                    "Suskirstyk didelius projektus į mažesnes užduotis. Progresas yra progresas, nesvarbu, koks mažas.",
                    "Tavo ADHD yra iššūkis, bet taip pat ir tavo stiprybės šaltinis. Brangink savo unikalų požiūrį.",
                    "Aplinka turi įtakos. Sukurk darbo erdvę, kuri padėtų tau susikoncentruoti."
                }},
                { "Therapy", new List<string> {
                    "Terapija yra saugi erdvė augti ir mokytis apie save. Didžiuokis šiuo žingsniu.",
                    "Pokyčiai vyksta palaipsniui. Kiekviena terapijos sesija yra žingsnis pirmyn.",
                    "Būk atviras(-a) terapijoje. Tavo jausmai ir mintys nusipelno būti išgirsti."
                }},
                { "Relationships", new List<string> {
                    "Sveiki santykiai reikalauja bendravimo, pagarbos ir supratimo. Tu nusipelnai viso to.",
                    "Ribų nustatymas yra sveikų santykių dalis. Tavimi rūpintis nėra savanaudiška.",
                    "Santykiai yra abipusiai. Tiek davimas, tiek gavimas yra svarbūs."
                }},
                { "PhysicalHealth", new List<string> {
                    "Kūnas ir protas yra susiję. Fizinis aktyvumas gali pagerinti nuotaiką.",
                    "Miegas yra gydymas. Stenkis išlaikyti pastovų miego grafiką.",
                    "Subalansuota mityba padeda ne tik kūnui, bet ir protui. Tai savęs priežiūros dalis."
                }}
            };
            
            // Initialize Twilio with account credentials
            string accountSid = _configuration["Twilio:AccountSid"] ?? "";
            string authToken = _configuration["Twilio:AuthToken"] ?? "";
            
            if (!string.IsNullOrEmpty(accountSid) && !string.IsNullOrEmpty(authToken))
            {
                TwilioClient.Init(accountSid, authToken);
            }
        }
        
        public async Task<bool> SendSms(string phoneNumber, string message)
        {
            try
            {
                string fromNumber = _configuration["Twilio:PhoneNumber"] ?? "";
                
                if (string.IsNullOrEmpty(fromNumber))
                {
                    return false;
                }
                
                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(phoneNumber)
                );
                
                return messageResource.Status != MessageResource.StatusEnum.Failed;
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> SendScheduledMessage(UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.PhoneNumber) || !userProfile.ReceiveSmsMessages)
            {
                return false;
            }
            
            string message = GenerateMessageForTopics(
                userProfile.SelectedTopics != null 
                ? JsonSerializer.Deserialize<List<string>>(userProfile.SelectedTopics) ?? new List<string>() 
                : new List<string>()
            );
            
            if (string.IsNullOrEmpty(message))
            {
                return false;
            }
            
            return await SendSms(userProfile.PhoneNumber, message);
        }
        
        public async Task<bool> SendTestMessage(UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.PhoneNumber))
            {
                return false;
            }
            
            var topics = userProfile.SelectedTopics != null 
                ? JsonSerializer.Deserialize<List<string>>(userProfile.SelectedTopics) ?? new List<string>() 
                : new List<string>();
            
            string message = GenerateMessageForTopics(topics);
            
            if (string.IsNullOrEmpty(message))
            {
                message = "Tai yra bandomoji žinutė iš EmocinėSveikata programos. Ačiū, kad naudojatės mūsų paslauga!";
            }
            
            return await SendSms(userProfile.PhoneNumber, message);
        }
        
        public string GenerateMessageForTopics(List<string> topics)
        {
            if (topics == null || topics.Count == 0)
            {
                return "Ačiū, kad rūpinatės savo emocine sveikata. Linkime jums geros dienos!";
            }
            
            // Get a random topic from the user's selected topics
            var random = new Random();
            var topic = topics[random.Next(topics.Count)];
            
            // If the topic has messages, get a random one
            if (_topicMessages.TryGetValue(topic, out var messages) && messages.Count > 0)
            {
                return messages[random.Next(messages.Count)];
            }
            
            return "Ačiū, kad rūpinatės savo emocine sveikata. Linkime jums geros dienos!";
        }
    }
}
