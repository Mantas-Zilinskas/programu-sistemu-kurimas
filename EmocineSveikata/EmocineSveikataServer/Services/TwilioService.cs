using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using EmocineSveikataServer.Settings;
using EmocineSveikataServer.Models;
using System.Collections.Generic;

namespace EmocineSveikataServer.Services
{
    public interface ITwilioService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
        Dictionary<string, List<string>> GetReminderMessagesByTopic();
    }

    public class TwilioService : ITwilioService
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly Dictionary<string, List<string>> _reminderMessages;

        public TwilioService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
            
            if (!string.IsNullOrEmpty(_twilioSettings.AccountSid) && 
                !string.IsNullOrEmpty(_twilioSettings.AuthToken))
            {
                TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
            }
            
            _reminderMessages = InitializeReminderMessages();
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_twilioSettings.AccountSid) || 
                    string.IsNullOrEmpty(_twilioSettings.AuthToken) ||
                    string.IsNullOrEmpty(_twilioSettings.FromPhoneNumber))
                {
                    return false;
                }

                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(_twilioSettings.FromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                return messageResource.Status != MessageResource.StatusEnum.Failed;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Dictionary<string, List<string>> GetReminderMessagesByTopic()
        {
            return _reminderMessages;
        }

        private Dictionary<string, List<string>> InitializeReminderMessages()
        {
            return new Dictionary<string, List<string>>
            {
                { "Depresija", new List<string> {
                    "Prisimink: tavo jausmai yra svarbūs ir validūs. Skirti laiko sau nėra savanaudiška.",
                    "Šiandien padaryk mažą žingsnelį pirmyn. Net mažiausi pasiekimai yra reikšmingi.",
                    "Kvėpuok giliai. Kartais reikia tik akimirkos, kad susikauptume ir pagerėtų savijauta.",
                    "Tu nesi vienas(-a) su savo jausmais. Prašyti pagalbos yra stiprybės ženklas.",
                    "Prisimink, kad depresija nėra tavo kaltė ir ji nėra tavo tapatybė."
                }},
                { "PsichinėSveikata", new List<string> {
                    "Savęs priežiūra nėra prabanga, tai būtinybė. Šiandien skirti laiko sau.",
                    "Būk malonus(-i) sau kaip būtum malonus(-i) geram draugui.",
                    "Tavo mintys nėra faktai. Prisimink, kad gali juos stebėti be vertinimo.",
                    "Mažos kasdieninės rutinos gali stipriai palaikyti psichinę sveikatą.",
                    "Prisimink, kad kiekviena diena yra nauja galimybė rūpintis savo gerove."
                }},
                { "ADHD", new List<string> {
                    "Suskirstyk užduotis į mažesnius, lengviau valdomus žingsnius.",
                    "Nustatyk laikmačius trumpiems darbo periodams, po kurių seka trumpos pertraukos.",
                    "Išoriniai priminimai gali būti labai naudingi. Naudok lipnias pastabas, žadintuvus ar programėles.",
                    "Judėjimas padeda smegenims. Pamėgink trumpą pasivaikščiojimą ar tempimo pratimus.",
                    "Tavo smegenys veikia kitaip, ir tai suteikia tau unikalių stiprybių."
                }},
                { "Terapija", new List<string> {
                    "Pasiruošk terapijos sesijai apmąstydamas(-a), ką norėtum aptarti.",
                    "Terapija - tai kelionė, kuri kartais būna sunki, bet verta pastangų.",
                    "Užrašyk mintis tarp sesijų, kad nepamirštum svarbių įžvalgų.",
                    "Prisimink pritaikyti terapijoje išmoktus įrankius kasdieniniame gyvenime.",
                    "Pokyčiai dažnai vyksta mažais žingsneliais. Būk kantrus(-i) su savimi."
                }},
                { "Santykiai", new List<string> {
                    "Geri santykiai reikalauja pastangų ir komunikacijos iš abiejų pusių.",
                    "Prisimink paklausti, kaip sekasi tavo artimiesiems. Mažos pastangos daug reiškia.",
                    "Nustatyk ir palaikyk sveikas ribas santykiuose.",
                    "Klausyk suprasti, o ne atsakyti. Aktyvus klausymasis stiprina ryšį.",
                    "Savęs priežiūra padeda tau būti geresniam partneriui ir draugui."
                }},
                { "FizinėSveikata", new List<string> {
                    "Fizinis aktyvumas gali stipriai pagerinti nuotaiką. Pamėgink šiandien pajudėti.",
                    "Vandens gėrimas yra paprastas, bet galingas įrankis tavo gerovei.",
                    "Kokybiškas miegas yra esminis psichinei sveikatai. Stenkis laikytis reguliaraus miego grafiko.",
                    "Tavo kūnas ir protas yra susiję. Rūpindamasis(-asi) vienu, padedi ir kitam.",
                    "Mažos sveikos įpročių pertraukėlės per dieną gali turėti didelį poveikį ilgalaikei sveikatai."
                }}
            };
        }
    }
}
