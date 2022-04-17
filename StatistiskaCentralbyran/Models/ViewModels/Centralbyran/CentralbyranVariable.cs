namespace StatistiskaCentralbyran.Models.ViewModels.Centralbyran
{
    public class CentralbyranVariable
    {
        public string Code { get; set; }
        public string Text { get; set; }
        public string[] Values { get; set; }
        public string[] ValueTexts { get; set; }
        public bool Elimination { get; set; }
        public bool Time { get; set; }
    }
}
