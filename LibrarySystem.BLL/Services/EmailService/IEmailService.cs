using LibrarySystem.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.EmailService;
public interface IEmailService
{
	Task SendEmail(EmailViewModel request);
}
