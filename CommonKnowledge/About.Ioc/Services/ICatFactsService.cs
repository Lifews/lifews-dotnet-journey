using System.Collections.Generic;
using System.Threading.Tasks;

namespace About.Ioc.Services;

interface ICatFactsService
{
    Task<IEnumerable<string>> GetCatFactsAsync(int limit);
}
