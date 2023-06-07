using KPGeoData.Shared.Responses;

namespace KPGeoData.API.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
