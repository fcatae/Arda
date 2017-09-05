DROP VIEW PBI_ReportMetrics
GO

CREATE VIEW PBI_ReportMetrics AS (
	SELECT Appointments.AppointmentHoursDispensed, Appointments.AppointmentDate, Metrics.MetricName, Users.Name
	FROM Appointments
	INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
	INNER JOIN WorkloadBacklogMetrics ON WorkloadBacklogMetrics.WorkloadBacklogWBID = WorkloadBacklogs.WBID
	INNER JOIN Metrics ON Metrics.MetricID = WorkloadBacklogMetrics.MetricMetricID
	INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName
)
GO

SELECT Appointments.AppointmentHoursDispensed,Appointments.AppointmentDate, Metrics.MetricName, Users.Name
FROM Appointments
INNER JOIN WorkloadBacklogs ON WorkloadBacklogs.WBID = Appointments.AppointmentWorkloadWBID
INNER JOIN WorkloadBacklogMetrics ON WorkloadBacklogMetrics.WorkloadBacklogWBID = WorkloadBacklogs.WBID
INNER JOIN Metrics ON Metrics.MetricID = WorkloadBacklogMetrics.MetricMetricID
INNER JOIN Users ON Appointments.AppointmentUserUniqueName = Users.UniqueName