namespace ReBook.ViewModels.AccountVM
{
    public class LoginRegisterVM
    {
        public ReBook.ViewModels.AccountVM.LoginVM LoginTab;
        public ReBook.ViewModels.AccountVM.RegisterVM RegisterTab;
        public string CurrentTab { get; set; }

        public LoginRegisterVM()
        {
            LoginTab = new ReBook.ViewModels.AccountVM.LoginVM();
            RegisterTab = new ReBook.ViewModels.AccountVM.RegisterVM();
            CurrentTab = "Login";
        }
    }
}
