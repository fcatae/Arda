DROP VIEW PBI_ReportTechnology
GO

CREATE VIEW PBI_ReportTechnology AS (
	SELECT Appointments.AppointmentHoursDispensed, Appointments.AppointmentDate, Technologies.TechnologyName, Users.Name
	FROM Appointments
	INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
	INNER JOIN WorkloadBacklogTechnologies ON WorkloadBacklogTechnologies.WorkloadBacklogWBID = WorkloadBacklogs.WBID
	INNER JOIN Technologies ON Technologies.TechnologyID = WorkloadBacklogTechnologies.TechnologyTechnologyID
	INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName
	)
GO

SELECT Appointments.AppointmentHoursDispensed, Appointments.AppointmentDate, Technologies.TechnologyName, Users.Name
FROM Appointments
INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
INNER JOIN WorkloadBacklogTechnologies ON WorkloadBacklogTechnologies.WorkloadBacklogWBID = WorkloadBacklogs.WBID
INNER JOIN Technologies ON Technologies.TechnologyID = WorkloadBacklogTechnologies.TechnologyTechnologyID
INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName