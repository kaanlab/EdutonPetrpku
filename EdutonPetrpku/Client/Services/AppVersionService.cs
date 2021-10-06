using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Services
{
    public interface IAppVersionService
    {
        string Version { get; }
    }

    /// <summary>
    /// Show version number from csproj
    /// </summary>
    public class AppVersionService : IAppVersionService
    {
        public string Version => SetVersion();
        private string SetVersion()
        {
            var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return (attribute.InformationalVersion != null) ? attribute.InformationalVersion : "undefine";
        }
    }
}
