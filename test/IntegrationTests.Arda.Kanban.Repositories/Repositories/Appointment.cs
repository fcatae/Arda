using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Appointment : ISupportSnapshot<AppointmentViewModel>
    {
        public IEnumerable<AppointmentViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            AppointmentRepository appointment = new AppointmentRepository(context);

            return appointment.GetAllAppointments().ToArray();
        }

        [Fact]
        public void Appointment_GetAllAppointments_Should_ReturnAllValues() 
        {
            ArdaTestMgr.Validate(this, $"Appointment.GetAllAppointments()",
                (list, ctx) => {
                    var rows = from r in list
                               select new { r._AppointmentDate, r._WorkloadTitle };

                    return rows;
                });
        }
        
        [Fact]
        public void Appointment_GetAllAppointments_DoesNot_ReturnUserName()
        {
            ArdaTestMgr.Validate(this, $"Appointment.GetAllAppointments()",
                (list, ctx) => {
                    var rows = from r in list
                               select new { r._AppointmentUserUniqueName, r._AppointmentUserName };

                    return rows;
                });
        }

        [Fact]
        public void Appointment_GetAllAppointments_DoesNot_ReturnComment()
        {
            ArdaTestMgr.Validate(this, $"Appointment.GetAllAppointments()",
                (list, ctx) => {
                    var rows = from r in list
                               select new { r._AppointmentComment };

                    return rows;
                });
        }

        [Fact]
        public void Appointment_GetAllAppointmentsByUser_DoesNot_ReturnUserName()
        {
            string USER_UNIQUE_NAME = "user@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.GetAllAppointments({USER_UNIQUE_NAME})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = from r in appointment.GetAllAppointments(USER_UNIQUE_NAME)
                              select new { r._AppointmentUserUniqueName, r._AppointmentUserName };

                    return row;
                });
        }

        [Fact]
        public void Appointment_GetAllAppointmentsByUser_DoesNot_ReturnComment()
        {
            string USER_UNIQUE_NAME = "user@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.GetAllAppointments({USER_UNIQUE_NAME})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = from r in appointment.GetAllAppointments(USER_UNIQUE_NAME)
                              select new { r._AppointmentComment };

                    return row;
                });
        }

        [Fact]
        public void Appointment_GetAppointmentByID_DoesNot_ReturnUserName()
        {
            string GUID = "068397FA-A41E-443F-823D-E2A6585BD322";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.GetAppointmentByID({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = appointment.GetAppointmentByID(Guid.Parse(GUID));                              

                    return new { row._AppointmentUserUniqueName, row._AppointmentUserName }; ;
                });
        }

        [Fact]
        public void Appointment_GetAllAppointments_Should_FilterByUser()
        {
            string USER_UNIQUE_NAME = "user@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.GetAllAppointments({USER_UNIQUE_NAME})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = appointment.GetAllAppointments(USER_UNIQUE_NAME);

                    return row;
                });
        }

        [Fact]
        public void Appointment_GetAppointmentByID_Should_ReturnExactlyOne()
        {
            string GUID = "068397FA-A41E-443F-823D-E2A6585BD322";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.GetAppointmentByID({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);                    

                    var row = appointment.GetAppointmentByID(Guid.Parse(GUID));

                    return row;
                });
        }

        [Fact]
        public void Appointment_DeleteAppointmentByID_Should_ReturnRemoveExactlyOne()
        {
            string GUID = "068397fa-a41e-443f-823d-e2a6585bd322";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.DeleteAppointmentByID({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = appointment.DeleteAppointmentByID(Guid.Parse(GUID));

                    return row;
                });
        }

        [Fact]
        public void Appointment_AddNewAppointment_Should_AddRow()
        {
            string GUID = "5348CCBE-7BED-4B5C-A2CE-E3E872F2CBC5";
            string WORKLOAD_GUID = "c1507019-1d97-4629-8015-c01bf02ce6ab";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.AddNewAppointment({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = new AppointmentViewModel()
                    {
                        _AppointmentComment = "My Appointment",
                        _AppointmentDate = DateTime.Parse("2020-01-20"),
                        _AppointmentHoursDispensed = 8,
                        _AppointmentID = Guid.Parse(GUID),
                        _AppointmentTE = (decimal)100.0,
                        _AppointmentUserUniqueName = "user@ardademo.onmicrosoft.com",
                        _AppointmentWorkloadWBID = Guid.Parse(WORKLOAD_GUID)
                    };

                    appointment.AddNewAppointment(row);

                    return appointment.GetAllAppointments();
                });
        }

        [Fact]
        public void Appointment_EditAppointment_Should_ChangeRow()
        {
            string GUID = "068397FA-A41E-443F-823D-E2A6585BD322";
            string WORKLOAD_GUID = "c1507019-1d97-4629-8015-c01bf02ce6ab";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.EditAppointment({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = new AppointmentViewModel()
                    {
                        _AppointmentComment = "Updated appointment",
                        _AppointmentDate = DateTime.Parse("2111-11-11"),
                        _AppointmentHoursDispensed = 100,
                        _AppointmentID = Guid.Parse(GUID),
                        _AppointmentTE = (decimal)123.45,
                        _AppointmentUserUniqueName = "user@ardademo.onmicrosoft.com",
                        _AppointmentWorkloadWBID = Guid.Parse(WORKLOAD_GUID)
                    };

                    appointment.EditAppointment(row);

                    return appointment.GetAppointmentByID(Guid.Parse(GUID));
            });
        }

        [Fact]
        public void Appointment_EditAppointment_DoesNot_ChangeUserAndWorkload()
        {
            string GUID = "068397FA-A41E-443F-823D-E2A6585BD322";
            string WORKLOAD_GUID = "90cac674-18c0-4139-8aae-f9711bd2d5f4";

            ArdaTestMgr.Validate(this, $"AppointmentRepository.EditAppointment({GUID})",
                (list, ctx) => {
                    AppointmentRepository appointment = new AppointmentRepository(ctx);

                    var row = new AppointmentViewModel()
                    {
                        _AppointmentComment = "Updated appointment",
                        _AppointmentDate = DateTime.Parse("2111-11-11"),
                        _AppointmentHoursDispensed = 100,
                        _AppointmentID = Guid.Parse(GUID),
                        _AppointmentTE = (decimal)123.45,
                        _AppointmentUserUniqueName = "user@ardademo.onmicrosoft.com",
                        _AppointmentWorkloadWBID = Guid.Parse(WORKLOAD_GUID)
                    };

                    appointment.EditAppointment(row);

                    return appointment.GetAppointmentByID(Guid.Parse(GUID));
                });
        }
    }
}
