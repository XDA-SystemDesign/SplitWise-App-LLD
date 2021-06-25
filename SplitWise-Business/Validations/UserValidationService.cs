using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.Validations
{
    public class UserValidationService: IUserValidationService
    {
        private IUserService userService;
        public UserValidationService(IUserService userService)
        {
            this.userService = userService;
        }

        public UserResponseDTO GetUser(int userId)
        {
            if (userId <= 0) throw new ValidationException("Invalid user id sent");
            var user = this.userService.GetUserByUserId(userId);
            if (user is null) throw new ValidationException("Invalid user id sent");
            return new UserResponseDTO { UserId = user.Userid, name = user.name };
        }
        public UserResponseDTO createUser(UserRequestDTO user)
        {
            if (!this.validate(user))
            {
                throw new ValidationException("Invalid user");
            }
            var output = this.userService.createUser(user.name);

            return new UserResponseDTO{UserId = output.Userid, name = output.name};
        }

        private bool validate(UserRequestDTO user_req)
        {
            if (string.IsNullOrEmpty(user_req.name)) return false;
            return true;
        }
    }
}
