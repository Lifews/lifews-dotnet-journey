using System.Threading.Tasks;

namespace About.Ioc.Services;

internal interface IWebClient
{
    Task<string> GetStringAsync(string url);
}
