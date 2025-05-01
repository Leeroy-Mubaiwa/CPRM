using Microsoft.Extensions.Configuration;

namespace CPRM.Services
{
    public class TawkToService
    {
        private readonly IConfiguration _configuration;

        public TawkToService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetWidgetId()
        {
            return _configuration["TawkTo:WidgetId"];
        }

        public string GetPropertyId()
        {
            return _configuration["TawkTo:PropertyId"];
        }

        public void SetUserData(string userId, string name, string email)
        {
            // This method can be used to set user data in the chat widget
            // Implementation will depend on your specific requirements
        }

        public void SetCustomAttributes(Dictionary<string, string> attributes)
        {
            // This method can be used to set custom attributes in the chat widget
            // Implementation will depend on your specific requirements
        }
    }
} 