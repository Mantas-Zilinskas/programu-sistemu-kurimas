namespace EmocineSveikataServer.Configuration
{
    public class SmsSettings
    {
        public bool Enabled { get; set; } = true;
        public string Provider { get; set; } = "Demo";  
        public int MessageDelay { get; set; } = 500;    
        public int DailySendHour { get; set; } = 9;     
        public bool EnableDailyMessages { get; set; } = true; 
    }
}
