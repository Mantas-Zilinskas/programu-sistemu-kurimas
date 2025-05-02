using EmocineSveikataServer.Dto.PositiveMessageDtos;

namespace EmocineSveikataServer.Services.PositiveMessageService
{
    public class PositiveMessageService : IPositiveMessageService
    {
        public PositiveMessageService()
        {

        }

        public PositiveMessageDto GetRandomMessage()
        {
            List<string> positiveMessages =
            [
                "Tau viskas pavyks!",
                "Tu stipresnis, nei manai!",
                "Niekada nepasiduok!",
                "Tu tikrai patirsi laim�!",
                "Tavo pastangos vertingos!",
                "Tavo ateitis bus nuostabi!",
                "Tu gali nugal�ti visas kli�tis!",
                "Tu gali pasiekti bet k�!",
                "Tu gali pasiekti savo svajones!",
                "I���kiai tik sustiprins tave!",
                "Nebijok prad�ti i� naujo!"
            ];

            Random random = new();
            string randomPositiveMessage = positiveMessages[random.Next(positiveMessages.Count)];

            return new PositiveMessageDto
            {
                Message = randomPositiveMessage
            };
        }
    }
}
