using System.Threading.Tasks;
using Com.Qsw.Framework.Session.Interface;
using Com.Qsw.Module.UserState.Interface;

namespace Com.Qsw.Module.UserState.Impl
{
    public interface IUserStateInfoRepository : IEntityMemoryRepository<UserStateInfo>
    {
        Task<UserStateInfo> GetByUserId(string userId);
    }
}