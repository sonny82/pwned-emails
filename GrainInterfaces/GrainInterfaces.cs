using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IDomainGrain : IGrainWithStringKey
    {
        Task<bool> AddEmailAddress(string email);
        Task<bool> CheckEmailAddress(string email);
    }
}