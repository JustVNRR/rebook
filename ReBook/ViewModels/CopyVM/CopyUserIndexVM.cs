namespace ReBook.ViewModels.CopyVM
{
    public class CopyUserIndexVM
    {
        public string PageTitle { get; set; }
        public List<Copy> Copies { get; set; }
        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }

        public string TargetedUserName { get; set; }
        public string TargetedUserId { get; set; }
        public string Callback { get; set; }
        public int InitialCopyId { get; set; }
        public CopyUserIndexVM()
        {
            PageTitle = "";
            Copies = new List<Copy>();
            Callback = "";
            TargetedUserName = "";
        }
    }
}
