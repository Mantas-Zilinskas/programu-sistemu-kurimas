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
                "Tu tikrai patirsi laimæ!",
                "Tavo pastangos vertingos!",
                "Tavo ateitis bus nuostabi!",
                "Tu gali nugalëti visas kliûtis!",
                "Tu gali pasiekti bet kà!",
                "Tu gali pasiekti savo svajones!",
                "Iððûkiai tik sustiprins tave!",
                "Nebijok pradëti ið naujo!"
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
