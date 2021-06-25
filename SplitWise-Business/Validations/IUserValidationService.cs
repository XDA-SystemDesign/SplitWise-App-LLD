using SplitWise_Business.DTOs;
using SplitWise_Models;

namespace SplitWise_Business.Validations
{
    public interface IUserValidationService
    {
        public UserResponseDTO createUser(UserRequestDTO user);
        public UserResponseDTO GetUser(int userId);
    }
}