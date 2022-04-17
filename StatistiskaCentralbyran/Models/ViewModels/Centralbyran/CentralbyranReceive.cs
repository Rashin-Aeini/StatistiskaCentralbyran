namespace StatistiskaCentralbyran.Models.ViewModels.Centralbyran
{
    public partial class CentralbyranReceive
    {
        public CentralbyranColumn[] Columns { get; set; }
        public object[] Comments { get; set; }
        public CentralbyranData[] Data { get; set; }
        public CentralbyranMetadata[] Metadata { get; set; }
    }
}
