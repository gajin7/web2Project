using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using WebApp.Providers;
using WebApp.Results;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IUnitOfWork _unitOfWork;
        private DbContext _context;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
          
        }

        public AccountController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = User.Identity.GetUserId();
            User usr = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == userId).FirstOrDefault();

            string date = usr.DateOfBirth.ToUniversalTime().Date.ToString().Split(' ')[0];

            var pic = _unitOfWork.Pictures.GetAll().Where((u) => u.AppUserId == userId).FirstOrDefault();
            string imgSource = "";
            if (pic != null)
                imgSource = System.Convert.ToBase64String(pic.ImageSource);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                FirstName = usr.FirstName,
                LastName = usr.LastName,
                Date = date,
                City = usr.Address.Split(',')[0],
                Street = usr.Address.Split(',')[1],
                Number = usr.Address.Split(',')[2],
                Type = usr.UserType.ToString(),
                Status = GetProfileStatus(User.Identity.GetUserId()),
                Image = imgSource


                // HasRegistered = externalLogin == null,
                // LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        [Authorize(Roles = "AppUser")]
        [HttpPost]
        [System.Web.Http.Route("api/Profile/GetInfo")]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("EditProfile")]
        public IHttpActionResult EditProfile(EditProfileBindingModel model)
        {


            if (!ModelState.IsValid)
            {
                var mssg = ModelState.Values.Select((u) => u.Errors.Select((i) => i.ErrorMessage).FirstOrDefault()).FirstOrDefault();
                return BadRequest(ModelState);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = User.Identity.GetUserId();
            ApplicationUser appUsr = UserManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            appUsr.Email = model.Email;
            appUsr.UserName = model.Email;
          
            UserManager.Update(appUsr);


            User usr = _unitOfWork.Users.GetAll().Where(u => u.AppUserId == userId).FirstOrDefault();
            usr.FirstName = model.FirstName;
            usr.LastName = model.LastName;
            usr.Address = model.City + "," + model.Street + "," + model.Number;

            switch (model.TypeOfPerson)
            {
                case "regular":
                    usr.UserType = Enums.UserType.regular;
                    usr.Approved = true;
                    usr.Checked = true;
                    break;
                case "student":
                    usr.UserType = Enums.UserType.student;
                    usr.Approved = false;
                    usr.Checked = false;
                    break;
                case "retiree":
                    usr.UserType = Enums.UserType.retiree;
                    usr.Approved = false;
                    usr.Checked = false;
                    break;
                default:
                    break;
            }

            _unitOfWork.Users.Update(usr);
            _unitOfWork.Complete();


            return Ok("Changes saved");

        }

       
        public string GetProfileStatus(string appUserId)
        {
           
            var usr = _unitOfWork.Users.GetAll().Where((u) => u.AppUserId == appUserId).FirstOrDefault();

            string retVal = "";

            if (usr.Checked == false && usr.postedImage == false)
                retVal = "Picture missing";
            else if (usr.Checked == false)
                retVal = "Proccesing";
            else if (usr.Checked == true && usr.Approved == true)
                retVal = "Approved";
            else if (usr.Checked == true && usr.Approved == false)
                retVal = "Decline";

            return retVal;

        }


        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        [Authorize(Roles = "AppUser")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        [Authorize(Roles = "AppUser")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var mssg = ModelState.Values.Select((u) => u.Errors.Select((i) => i.ErrorMessage).FirstOrDefault()).FirstOrDefault();
                return BadRequest(mssg);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.LastOrDefault());
            }

            return Ok("Password succesfully changed");
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
          
            if (!ModelState.IsValid)
            {
                var mssg = ModelState.Values.Select((u) => u.Errors.Select((i) => i.ErrorMessage).FirstOrDefault()).FirstOrDefault();
                return BadRequest(mssg);
            }

            DateTime tempDate;
            DateTime.TryParse(model.Date, out tempDate);
            Enums.UserType type = new Enums.UserType();
            bool approved = false;
            bool check = false;
            switch (model.TypeOfPerson)
            {
                case "Student":
                    type = Enums.UserType.student;
                    break;
                case "Regular":
                    type = Enums.UserType.regular;
                    approved = true;
                    check = true;
                    break;
                case "Retiree":
                    type = Enums.UserType.retiree;
                    break;
                default:
                    break;
            }

            var Appuser = new ApplicationUser() { Id = Guid.NewGuid().ToString(), UserName = model.Email, Email = model.Email, PasswordHash = ApplicationUser.HashPassword(model.Password) };

          
            IdentityResult result = await UserManager.CreateAsync(Appuser, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.LastOrDefault());
            }

            UserManager.AddToRole(Appuser.Id, "AppUser");
            
           
          

            var user = new User() { AppUserId = Appuser.Id, FirstName = model.FirstName, LastName = model.LastName, DateOfBirth = tempDate, Address = model.City + "," + model.Street + "," + model.Number, UserType = type, Approved = approved, postedImage = false, Checked = check };
            _unitOfWork.Users.Add(user);
            _unitOfWork.Complete();

         

            return Ok("Succesfully registrated! Please login to continue");
        }

        [AllowAnonymous]
        [Route("PostImage")]
        public async Task<IHttpActionResult> PostImage()
        {
            var req = HttpContext.Current.Request;

            string email = req.Url.ToString().Split('=')[1];
            foreach (string file in req.Files)
            {

                var postedFile = req.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".img", ".jpeg" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var temp = postedFile.FileName;
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {

                        var message = string.Format("Please Upload image of type .jpg,.gif,.png,.img,.jpeg.");

                        return BadRequest(message);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {
                        var message = string.Format("Please Upload a file upto 1 mb.");

                        return BadRequest(message);
                    }
                    else
                    {
                        var userId = UserManager.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();

                        var tempUser = _unitOfWork.Users.GetAll().Where((u) => u.AppUserId == userId).FirstOrDefault();
                        if(tempUser.UserType != Enums.UserType.retiree && tempUser.UserType != Enums.UserType.student)
                            return BadRequest("You can't add image for regular user!");
                        tempUser.postedImage = true;
                        tempUser.Approved = false;
                        tempUser.Checked = false;
                        _unitOfWork.Users.Update(tempUser);
                        _unitOfWork.Complete();

                        Bitmap bmp = new Bitmap(postedFile.InputStream);
                        System.Drawing.Image img = (System.Drawing.Image)bmp;
                        byte[] imagebytes = ImageToByteArray(img);


                        _unitOfWork.Pictures.Add(new Picture() { ImageSource = imagebytes, AppUserId = userId });
                        _unitOfWork.Complete();

 
                    }
                }
            }
            return Ok();

        }

        [AllowAnonymous]
        [Route("EditImage")]
        public async Task<IHttpActionResult> EditImage()
        {
            var req = HttpContext.Current.Request;

            string email = req.Url.ToString().Split('=')[1];
            foreach (string file in req.Files)
            {

                var postedFile = req.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".img", ".jpeg" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var temp = postedFile.FileName;
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {

                        var message = string.Format("Please Upload image of type .jpg,.gif,.png,.img,.jpeg.");

                        return BadRequest(message);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {
                        var message = string.Format("Please Upload a file upto 1 mb.");

                        return BadRequest(message);
                    }
                    else
                    {
                        var userId = UserManager.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();

                        var tempUser = _unitOfWork.Users.GetAll().Where((u) => u.AppUserId == userId).FirstOrDefault();
                        if (tempUser.UserType != Enums.UserType.retiree && tempUser.UserType != Enums.UserType.student)
                            return BadRequest("You can't add image for regular user!");
                        if (tempUser.postedImage == true)
                        {
                           
                            var picToDelete = _unitOfWork.Pictures.GetAll().Where((u) => u.AppUserId == tempUser.AppUserId).FirstOrDefault();
                            _unitOfWork.Pictures.Remove(picToDelete);
                            _unitOfWork.Complete();
                        }
                       
                            tempUser.postedImage = true;
                            tempUser.Approved = false;
                            tempUser.Checked = false;
                            _unitOfWork.Users.Update(tempUser);
                            _unitOfWork.Complete();

                            Bitmap bmp = new Bitmap(postedFile.InputStream);
                            System.Drawing.Image img = (System.Drawing.Image)bmp;
                            byte[] imagebytes = ImageToByteArray(img);


                            _unitOfWork.Pictures.Add(new Picture() { ImageSource = imagebytes, AppUserId = userId });
                            _unitOfWork.Complete();

                        
                    }
                }
            }
            return Ok();

        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }




        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        public class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
