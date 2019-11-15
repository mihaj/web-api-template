using System.Threading.Tasks;

namespace Mjc.Templates.WebApi.Core.Interfaces
{
    public interface IMyHttpFactory
    {
        Task<string> Demo1Async(string keyword);
    }
}
