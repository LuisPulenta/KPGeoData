using System.IO;

namespace KPGeoData.Shared.Helpers
{
    public class FilesHelper : IFilesHelper
    {
        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
            
                //var path = Path.Combine(Directory.GetCurrentDirectory(), folder, name);
                var path = Path.Combine("D:\\Xamarin\\KPGeoData\\KPGeoData.WEB\\", folder, name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}