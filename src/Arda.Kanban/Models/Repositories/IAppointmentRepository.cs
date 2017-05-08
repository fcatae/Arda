using System;
using System.Collections.Generic;
using Arda.Kanban.ViewModels;

namespace Arda.Kanban.Models.Repositories
{
    public interface IAppointmentRepository
    {
        bool AddNewAppointment(AppointmentViewModel appointment);

        List<AppointmentViewModel> GetAllAppointmentsSimple();

        List<AppointmentViewModel> GetAllAppointmentsSimple(string user);

        List<AppointmentViewModel> GetAllAppointmentsWorkload(Guid workload);

        AppointmentViewModel GetAppointmentByID(Guid id);

        bool DeleteAppointmentByID(Guid id);

        bool EditAppointment(AppointmentViewModel appointment);
    }
}
