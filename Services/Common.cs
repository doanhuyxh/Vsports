using System.Text;
using vsports.Data;

namespace vsports.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(CharSet.Length);
                result.Append(CharSet[index]);
            }
            return result.ToString();
        }

        public async Task<string> UploadImgAvatarAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/img_avatar");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path =  DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/img_avatar/{path}";
        }

        public async Task<string> UploadImgBackgroudAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/img_backgroud");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/img_backgroud/{path}";
        }

        public async Task<string> UploadPdfAsync(IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/pdf");

                if (file.FileName == null)
                    path = "file.pdf";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/pdf/{path}";
        }
    }
}
