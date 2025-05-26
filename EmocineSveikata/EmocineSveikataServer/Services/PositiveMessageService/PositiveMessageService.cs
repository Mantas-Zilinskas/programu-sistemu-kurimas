using System.Text.Json;
using EmocineSveikataServer.Dto.PositiveMessageDtos;
using EmocineSveikataServer.Enums;

namespace EmocineSveikataServer.Services.PositiveMessageService
{
    public class PositiveMessageService : IPositiveMessageService
    {
        private readonly Dictionary<string, List<string>> positiveMessages = new()
        {
            { DiscussionTagEnum.Depresija.ToString(),
                [
                    "Tu tikrai patirsi laimæ!",
                    "Tavo ateitis bus nuostabi!",
                    "Tau viskas pavyks!",
                ]
            },
            { DiscussionTagEnum.PsichinëSveikata.ToString(),
                [
                    "Nebijok pradëti ið naujo!",
                    "Tu stipresnis, nei manai!",
                ]
            },
            { DiscussionTagEnum.ADHD.ToString(),
                [
                    "Niekada nepasiduok!",
                ]
            },
            { DiscussionTagEnum.Terapija.ToString(),
                [
                    "Tu gali pasiekti savo svajones!",
                    "Tu gali pasiekti bet kà!",
                    "Tavo pastangos vertingos!",
                ]
            },
            { DiscussionTagEnum.Santykiai.ToString(),
                [
                    "Neleisk niekam tavæs stumdyti!",
                ]
            },
            { DiscussionTagEnum.FizinëSveikata.ToString(),
                [
                    "Iððûkiai tik sustiprins tave!",
                    "Tu gali nugalëti visas kliûtis!",
                ]
            },
        };

        public PositiveMessageService()
        {

        }

        public async Task<PositiveMessageDto> GetRandomMessage()
        {
            List<string> allPositiveMessages = positiveMessages.Values.SelectMany(list => list).ToList();

            Random random = new();
            string randomPositiveMessage = allPositiveMessages[random.Next(allPositiveMessages.Count)];

            return new PositiveMessageDto
            {
                Message = randomPositiveMessage
            };
        }

        public async Task<PositiveMessageDto> GetPreferredRandomMessage(string selectedTopicsJson)
        {
            List<string> preferredPositiveMessages = [];
            List<string>? selectedTopics = JsonSerializer.Deserialize<List<string>>(selectedTopicsJson);

            if(selectedTopics == null || selectedTopics.Count <= 0)
                return new()
                {
                    Message = ""
                };

            foreach(string selectedTopic in selectedTopics)
            {
                preferredPositiveMessages.AddRange(positiveMessages[selectedTopic.ToString()]);
            }

            Random random = new();
            string randomPositiveMessage = preferredPositiveMessages[random.Next(preferredPositiveMessages.Count)];

            return new PositiveMessageDto
            {
                Message = randomPositiveMessage,
            };
        }
    }
}
