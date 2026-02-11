using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.GetUser;

public record GetUserResponse(int Id, string Email,string Role);
