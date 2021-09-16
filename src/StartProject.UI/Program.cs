using System;
using System.Configuration;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;

using Moryx.ClientFramework.Kernel;

namespace StartProject.UI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var sectionName = "system.identityModel";
            try
            {
                if (config.Sections.Get(sectionName) == null)
                {
                    config.Sections.Add(sectionName, new SystemIdentityModelSection());
                    config.Save();
                    ConfigurationManager.RefreshSection(sectionName);
                }

                // FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager = new MoryxAuthorizationManager();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during authorization preparation. Exception: " + e.Message);
            }

            var heartOfLead = new HeartOfLead(args);
            heartOfLead.Initialize();
            heartOfLead.Start();
        }
    }
}
