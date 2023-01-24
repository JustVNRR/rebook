namespace ReBook.ViewModels
{
    public class JsonVM
    {
        public bool success { get; set; } = true;
        public string responseText { get; set; }
        public string targetId { get; set; } = "#main-modal-content";
        public string callback { get; set; }
        public string notification { get; set; }
    }
}
