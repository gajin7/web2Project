using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    public class AccountHelpController : ApiController
    {

        private IUnitOfWork _unitOfWork;
        private DbContext _context;
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountHelpController()
        {
        }

        public AccountHelpController(ApplicationUserManager userManager,
           ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;

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

        public AccountHelpController(IUnitOfWork unitOfWork, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }



      
        [Route("PostImage")]
        public async Task<HttpResponseMessage> PostImage()
        {
            var req = HttpContext.Current.Request;

            string email = req.Url.ToString().Split('@')[0];
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

                        return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {
                        var message = string.Format("Please Upload a file upto 1 mb.");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                    }
                    else
                    {

                        Bitmap bmp = new Bitmap(postedFile.InputStream);
                        System.Drawing.Image img = (System.Drawing.Image)bmp;
                        byte[] imagebytes = ImageToByteArray(img);

                        var userId = UserManager.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();

                        _unitOfWork.Pictures.Add(new Picture() { ImageSource = imagebytes, AppUserId = userId });
                        _unitOfWork.Complete();

                      




                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK);

        }



        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

    }
}
