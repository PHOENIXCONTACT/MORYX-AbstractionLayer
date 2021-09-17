using System;
using System.Configuration;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.Security.Claims;
using Moryx.ClientFramework.Kernel;

namespace StartProject.UI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {          
            var heartOfLead = new HeartOfLead(args);
            heartOfLead.AuhtorizeEverything();
            heartOfLead.Initialize();
            heartOfLead.Start();
        }
    }
}

public static class HeartOfLeadExtension
{
    public static void AuhtorizeEverything(this HeartOfLead hol)
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
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during authorization preparation. Exception: " + e.Message);
        }
    }
}