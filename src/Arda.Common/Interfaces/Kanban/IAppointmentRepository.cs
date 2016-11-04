using Arda.Common.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arda.Common.Interfaces.Kanban
{
    public interface IAppointmentRepository
    {
        bool AddNewAppointment(AppointmentViewModel appointment);

        List<AppointmentViewModel> GetAllAppointments();

        List<AppointmentViewModel> GetAllAppointments(string user);

        AppointmentViewModel GetAppointmentByID(Guid id);

        bool DeleteAppointmentByID(Guid id);

        bool EditAppointment(AppointmentViewModel appointment);
    }
}
