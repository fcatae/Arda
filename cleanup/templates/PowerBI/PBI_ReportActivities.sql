DROP VIEW PBI_ReportActivity
GO

CREATE VIEW PBI_ReportActivity AS (
		SELECT Appointments.AppointmentHoursDispensed, Appointments.AppointmentDate, Activities.ActivityName, Users.Name
		FROM Appointments
		INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
		INNER JOIN Activities ON Activities.ActivityID = WorkloadBacklogs.WBActivityActivityID
		INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName
	)
GO

SELECT Appointments.AppointmentHoursDispensed, Appointments.AppointmentDate, Activities.ActivityName, Users.Name
FROM Appointments
INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
INNER JOIN Activities ON Activities.ActivityID = WorkloadBacklogs.WBActivityActivityID
INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName
