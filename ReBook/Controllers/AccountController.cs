using Microsoft.AspNetCore.Mvc.Filters;

namespace ReBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IJsonRenderService _json;
        private readonly IEmailService _mail;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IJsonRenderService viewRenderService,
            IEmailService mail)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _json = viewRenderService;
            _mail = mail;
        }

        public async Task<IActionResult> _UserPanelGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Delete Role",
                    Message = $"User not found",
                    Error = true
                };

                // return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                return PartialView("_MessageBox", message);
            }
            else
            {
/*                return await _json.RenderAsync("Account/_UserPanel", new UserVM
                {
                    Id = user.Id,
                    Pseudo = user.UserName,
                    Email = user.Email,
                    Avatar = user.Avatar
                });*/

                return PartialView("_UserPanel", new UserVM
                {
                    Id = user.Id,
                    Pseudo = user.UserName,
                    Email = user.Email,
                    Avatar = user.Avatar
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> _Logout()
        {
            await _signInManager.SignOutAsync();
            //return Json(new { redirectToUrl = Url.Action("index", "home") });
            return Ok();
        }

        private static string OutputModelState(ModelStateDictionary modelState)
        {
            string errors = "";
            errors += string.Join(" - ", modelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return errors;
        }

        private static string OutputIdentityErrors(IdentityResult result)
        {
            string errorMessage = "";
            foreach (var err in result.Errors)
            {
                errorMessage += err.Description + "/n";
            }

            return errorMessage;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                model.Error = OutputModelState(ModelState);

                return PartialView("_LoginRegister", new LoginRegisterVM { RegisterTab = model });

                //return await _json.RenderAsync(new JsonVM { success = false }, "Account/_LoginRegister", new LoginRegisterVM { RegisterTab = model });
            }

            ApplicationUser user = new() { UserName = model.Pseudo, Email = model.Email, Avatar = model.Avatar };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("Index", "Home",
                    new { userId = user.Id, token }, Request.Scheme);

                confirmationLink = confirmationLink.Replace("%2F", "/").Replace(' ', '+'); // Parce qu'Url.Action pourrit le token... dégueulasse

                try
                {
                    //logger.Log(LogLevel.Warning, confirmationLink);
                    await _mail.SendEmailConfirmationAsync(user.Email, confirmationLink); //Try catch ta mère...

                    /*if (signInManager.IsSignedIn(User) && User.IsInRole("admin"))
                    {
                       do something special...
                    }*/

                    var message = new MessageBoxVM
                    {
                        Title = "Registration successful",
                        Message = "Please confirm your email by clicking the confirmation link we have emailed you"
                    };

                    // return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                    return PartialView("_MessageBox", message);
                }
                catch
                {
                    var message = new MessageBoxVM
                    {
                        Title = "Registration successful",
                        Message = "Something went wrong while sending email confirmation. Please contact support@rebook.org",
                        Error = true
                    };

                    // return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                    return PartialView("_MessageBox", message);
                }
            }

            model.Error = OutputIdentityErrors(result);

            //return await _json.RenderAsync(new JsonVM { success = false }, "Account/_LoginRegister", new LoginRegisterVM { RegisterTab = model });
            return PartialView("_LoginRegister", new LoginRegisterVM { RegisterTab = model });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> _ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            MessageBoxVM message = new ()
            {
                Title = "Error",
                Message = "Email cannot be confirmed"
            };

            if (user is null)
            {
                message.Title = "Not Found";
                message.Message = $"The User ID {userId} is invalid";

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return await _json.RenderAsync("Account/_ConfirmEmail");
            }

            return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email, string Id)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.Id == Id)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserPseudoInUse(string pseudo, string Id)
        {
            var user = await _userManager.FindByNameAsync(pseudo);
            if (user == null || user.Id == Id)
            {
                return Json(true);
            }
            else
            {
                return Json($"{pseudo} is already in use.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> _LoginRegisterGet(string returnUrl, bool obstrusive = false)
        {
            LoginRegisterVM model = new ();

            model.LoginTab.ReturnUrl = returnUrl;
            model.LoginTab.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!obstrusive)
            {
                return PartialView("_LoginRegister", model);
            }
            
            return await _json.RenderAsync("Account/_LoginRegister", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
             model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

             string returnUrl = model.ReturnUrl;

             if (!ModelState.IsValid)
             {
                 model.Error = OutputModelState(ModelState);

                 return PartialView("_LoginRegister", new LoginRegisterVM { LoginTab = model });
             }

             var user = await _userManager.FindByEmailAsync(model.Email_L);

             if (user is not null &&
                     !user.EmailConfirmed &&
                     (await _userManager.CheckPasswordAsync(user, model.Password_L))) //Contre les brut force :On affiche le message indiquant que l'Email est correct si le password est bon
             {
                 model.Error = "Email not confirmed yet";

                 return PartialView("_LoginRegister", new LoginRegisterVM { LoginTab = model });
             }

             if (user is not null)
             {
                 var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password_L, model.RememberMe, true);

                 if (result.Succeeded)
                 {
                     if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                     {
                         return this.Redirect(returnUrl);
                     }

                     return PartialView("_UserPanel",new UserVM
                     {
                         Id = user.Id,
                         Pseudo = user.UserName,
                         Email = user.Email,
                         Avatar = user.Avatar
                     });
                 }

                 if (result.IsLockedOut)
                 {
                     var messageB = new MessageBoxVM
                     {
                         Title = "Account Locked Out",
                         Message = "You're account is currently locked out. Either wait a few minutes or reset your password to try to log in.",
                         Button = "Reset Password",
                         OnClick = "Account/_ForgotPasswordRequest"
                     };

                    return PartialView("_MessageBox", messageB);
                 }
             }

             model.Error = "Login attempt failed !!";

             return PartialView("_LoginRegister", new LoginRegisterVM { LoginTab = model });
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult _ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("Index", "Home", new { ReturnUrl = returnUrl, ExternalLogin = true });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> _ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            LoginVM loginVM = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError is not null)
            {
                loginVM.Error = $"Error from external provider : { remoteError}";

                return await _json.RenderAsync("Account/_LoginRegister", loginVM);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                loginVM.Error = "Error loading external login information.";

                return await _json.RenderAsync("Account/_LoginRegister", loginVM);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = null;

            if (email is not null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user is not null && !user.EmailConfirmed)
                {
                    loginVM.Error = "Email not confirmed yet";

                    return await _json.RenderAsync(new JsonVM { success = false },
                                                            "Account/_LoginRegister",
                                                            new LoginRegisterVM { LoginTab = loginVM });
                }
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                return _json.Render();
            }
            else
            {
                if (email is not null)
                {
                    if (user is null)
                    {
                        var userName = info.Principal.FindFirstValue(ClaimTypes.Name);

                        userName ??= info.Principal.FindFirstValue(ClaimTypes.Email).Trim();

                        user = new ApplicationUser
                        {
                            UserName = userName,
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Avatar = info.Principal.FindFirstValue("urn:google:picture")
                        };

                        var result = await _userManager.CreateAsync(user);

                        if (result.Succeeded)
                        {
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            var confirmationLink = Url.Action("Index", "Home",
                                new { userId = user.Id, token = token }, Request.Scheme);

                            confirmationLink = confirmationLink.Replace("%2F", "/").Replace(' ', '+'); // Parce qu'Url.Action pourrit le token... dégueulasse

                            try
                            {
                                //logger.Log(LogLevel.Warning, confirmationLink);
                                await _mail.SendEmailConfirmationAsync(user.Email, confirmationLink); //Try catch ta mère...
                                var successMessage = new MessageBoxVM
                                {
                                    Title = "Registration successful",
                                    Message = "Please confirm your email by clicking the confirmation link we have emailed you"
                                };

                                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", successMessage);
                                //return await _json.RenderAsync("_MessageBox", message);
                            }
                            catch
                            {
                                MessageBoxVM errorMessage = new ()
                                {
                                    Title = "Registration successful",
                                    Message = "Something went wrong while sending email confirmation. Please contact support@rebook.org",
                                    Error = true
                                };

                                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", errorMessage);
                            }
                        }

                        MessageBoxVM messageBoxEror = new ()
                        {
                            Title = $"User creation error",
                            Error = true
                        };
                        foreach (var error in result.Errors)
                        {
                            messageBoxEror.Message += error.Description + "\n";
                        }
                        return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", messageBoxEror);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }

                    return _json.Render();
                }

                MessageBoxVM messageB = new ()
                {
                    Title = $"Email claim not received from: {info.LoginProvider}",
                    Message = "Please contact support@rebook.org"
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", messageB);
            }
        }
        public async Task<IActionResult> _UpdateNavBar()
        {
            return await _json.RenderAsync(new JsonVM { targetId = "#main-navbar-container" }, "_MainNavBar");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult _AccessDenied()
        {
            // return await _json.RenderAsync("_AccessDenied");
            return PartialView("_AccessDenied");
        }

        public IActionResult _ForgotPasswordRequest()
        {
            // return await _json.RenderAsync("Account/_ForgotPassword", new ForgotPasswordVM());
            return PartialView("_ForgotPassword", new ForgotPasswordVM());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> _ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                MessageBoxVM message = new ()
                {
                    Title = "Forgot Password Confirmation",
                    Message = $"Check your In/Spam Box"
                };

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("Index", "Home",
                        new { email = model.Email, token }, Request.Scheme);

                    passwordResetLink = passwordResetLink.Replace("%2F", "/").Replace(' ', '+'); // Parce qu'Url.Action pourrit le token... dégueulasse

                    try
                    {
                        //logger.Log(LogLevel.Warning, passwordResetLink);
                        await _mail.SendResetPasswordAsync(model.Email, passwordResetLink);

                        return PartialView("_MessageBox", message);
                    }
                    catch (Exception e)
                    {
                        message.Message = e.Message;
                        message.Error = true;

                        return PartialView("_MessageBox", message);
                    }
                }
                return PartialView("_MessageBox", message);
            }

            model.Error = OutputModelState(ModelState);

            return PartialView("_ForgotPassword", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> _ResetPassword(string token, string email)
        {
            if (token is null || email is null)
            {
                return await _json.RenderAsync("_MessageBox", "Invalid password reset token");
                // return PartialView("_MessageBox", "Invalid password reset token");
            }

            ResetPasswordVM model = new() { Email = email, Token = token };

            return await _json.RenderAsync("Account/_ResetPassword", model);
            // return PartialView("_ResetPassword", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> _ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return RedirectToAction("_LoginRegisterGet", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        model.Error += error.Description + "\n";
                    }

                    return PartialView("_ResetPassword", model);
                }
                return RedirectToAction("_LoginRegisterGet", "Account");
            }
            return PartialView("_ResetPassword", model);
        }

        [HttpGet]
        public async Task<IActionResult> _AddPasswordGet()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return RedirectToAction("_ChangePasswordGet");
            }

            return PartialView("_AddPassword");
        }

        [HttpPost]
        public async Task<IActionResult> _AddPassword(AddPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    var message = new MessageBoxVM
                    {
                        Title = "Add Password",
                        Message = "Current User not found",
                        Error = true
                    };

                    return PartialView("_MessageBox", message);
                }

                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        model.Error += error.Description;

                    }
                    return PartialView("_AddPassword", model);
                }

                await _signInManager.RefreshSignInAsync(user);

                var successMessage = new MessageBoxVM
                {
                    Title = "Add Password",
                    Message = "Password successfully added",
                    OnClick = "Account/_UserPanelGet"
                };

                return PartialView("_MessageBox", successMessage);
            }

            model.Error = OutputModelState(ModelState);

            return PartialView("_AddPassword", model);
        }

        [HttpGet]
        public async Task<IActionResult> _ChangePasswordGet()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("_AddPasswordGet");
            }

            return PartialView("_ChangePassword");
        }

        [HttpPost]
        public async Task<IActionResult> _ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user is null)
                {
                    MessageBoxVM message = new ()
                    {
                        Title = "Change Password",
                        Message = "Current User not found",
                        Error = true
                    };
                    return PartialView("_MessageBox", message);
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        model.Error += error.Description;
                    }
                    
                    return PartialView("_ChangePassword", model);
                }

                await _signInManager.RefreshSignInAsync(user);

                var successMessage = new MessageBoxVM
                {
                    Title = "Change Password",
                    Message = "Password successfully updated",
                    OnClick = "Account/_UserPanelGet"
                };

                return PartialView("_MessageBox", successMessage);
            }

            model.Error = OutputModelState(ModelState);

            return PartialView("_ChangePassword", model);
        }
    }
}
