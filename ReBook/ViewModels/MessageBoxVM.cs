namespace ReBook.ViewModels
{
    public class MessageBoxVM
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        public string Button { get; set; }
        public string OnClick { get; set; }

        public MessageBoxVM()
        {
            Error = false;
            Title = "Message Box";
            Message = "Nothing to say";
            Button = "OK";
            OnClick = null;
        }
    }
}
