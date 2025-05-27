using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmocineSveikataServer.Services.SmsService
{
    public class DemoSmsService : ISmsService
    {
        private readonly ILogger<DemoSmsService> _logger;
        private readonly Random _random = new Random();
        
        private readonly Dictionary<string, List<string>> _wellnessMessages = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Depresija", new List<string>
                {
                    "Kiekviena diena yra nauja galimybė. Jūs esate stipresnis nei manote.",
                    "Prisiminkite, kad tamsa nėra amžina. Patikėkite savimi ir šviesa, kuri jumyse.",
                    "Žingsnelis po žingsnelio, diena po dienos. Jūs darote pažangą.",
                    "Rūpinimasis savimi nėra savanaudiškumas. Tai būtina jūsų emocinei sveikatai."
                }
            },
            { "Psichinė sveikata", new List<string>
                {
                    "Šiandien skirkite sau bent 5 minutes vidinei ramybei ir tylai.",
                    "Jūsų jausmai yra svarbūs ir teisėti. Leiskite sau juos jausti.",
                    "Protinė sveikata yra tiek pat svarbi kaip ir fizinė. Skirkite jai dėmesio kasdien.",
                    "Jūs nesate savo mintys. Jūs esate tas, kuris jas stebi."
                }
            },
            { "ADHD", new List<string>
                {
                    "Jūsų smegenys veikia unikaliai, ir tai yra jūsų stiprybė!",
                    "Šiandien susitelkite į vieną svarbų dalyką, o ne į daugybę.",
                    "Jūsų kūrybiškumas ir energija yra nuostabios dovanos pasauliui.",
                    "Struktūra padeda, bet nesijaudinkite dėl tobulo plano. Tiesiog pradėkite."
                }
            },
            { "Terapija", new List<string>
                {
                    "Pagalbos prašymas yra drąsos, o ne silpnumo ženklas.",
                    "Terapija yra kelionė į savęs pažinimą ir augimą.",
                    "Kiekvienas žmogus kartais gali pasinaudoti objektyviu požiūriu ir palaikymu.",
                    "Pokyčiai prasideda nuo pripažinimo ir tęsiasi per praktiką."
                }
            },
            { "Santykiai", new List<string>
                {
                    "Sveiki santykiai prasideda nuo savęs meilės ir rūpinimosi savimi.",
                    "Šiandien parodykite dėkingumą kažkam, kas jums svarbus.",
                    "Ribų nustatymas yra sveikų santykių dalis.",
                    "Bendraukite atvirai ir su empatija. Tai gilina ryšius."
                }
            },
            { "Fizinė sveikata", new List<string>
                {
                    "Judėjimas gerina ne tik fizinę, bet ir emocinę sveikatą.",
                    "Pakankamai miegokite. Miegas yra būtinas jūsų emocinei sveikatai.",
                    "Subalansuota mityba padeda palaikyti emocinį stabilumą.",
                    "Kūnas ir protas yra susiję. Rūpinkitės abiem."
                }
            },
            { "Bendras", new List<string>
                {
                    "Prisiminkite, kad nesate vieni. Mes visi susiduriam su iššūkiais.",
                    "Šiandien atraskite bent vieną dalyką, už kurį esate dėkingi.",
                    "Jūs esate mylimas ir vertinamas labiau, nei galite įsivaizduoti.",
                    "Būkite malonus sau šiandien. Jūs darote viską, ką galite.",
                    "Prisiminkite kvėpuoti. Kelios gilios įkvėpimai gali pakeisti jūsų nuotaiką.",
                    "Kiekviena diena yra nauja galimybė. Pasinaudokite ja.",
                    "Jūsų emocijos yra kaip bangos - jos ateina ir praeina.",
                    "Pasitikėkite savimi. Jūs turite viską, ko reikia susidoroti su šia diena.",
                    "Jūs esate daug daugiau nei jūsų sunkumai ar iššūkiai.",
                    "Mažais žingsneliais galima nueiti labai toli."
                }
            }
        };

        public DemoSmsService(ILogger<DemoSmsService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendDailyWellnessMessageAsync(string phoneNumber, string[]? topics = null)
        {
            try
            {
                string message = GetRandomWellnessMessage(topics);
                
                message += "\n\nEmocinė Sveikata (DEMO)"; 
                
                await Task.Delay(500);
                
                _logger.LogInformation($"DEMO WELLNESS MESSAGE: To: {phoneNumber}, Message: {message}");
                Console.WriteLine($"[DEMO SMS] Daily wellness message sent to {phoneNumber}: {message}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending wellness message to {phoneNumber}");
                return false;
            }
        }
        
        private string GetRandomWellnessMessage(string[] topics)
        {
            if (topics == null || topics.Length == 0)
            {
                return GetRandomMessageFromTopic("Bendras");
            }
            
            string selectedTopic = topics[_random.Next(topics.Length)];
            
            if (_wellnessMessages.ContainsKey(selectedTopic))
            {
                return GetRandomMessageFromTopic(selectedTopic);
            }
            
            return GetRandomMessageFromTopic("Bendras");
        }
        
        private string GetRandomMessageFromTopic(string topic)
        {
            var messages = _wellnessMessages[topic];
            return messages[_random.Next(messages.Count)];
        }
    }
}
