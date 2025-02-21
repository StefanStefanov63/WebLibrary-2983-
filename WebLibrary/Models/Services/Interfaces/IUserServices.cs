using WebLibrary.Models.Services.ViewModelsClasses;

namespace WebLibrary.Models.Services.Interfaces
{
    public interface IUserServices
    {
        Task<int> ChangeUserRole(string aUserName, string anOldRole, string aNewRole, IServiceScopeFactory aScopeFactory);
        ICollection<UserViewModel> GetAllUsers();
        ICollection<UserViewModel> GetAllUsersByRole(string aRole);
    }
}
