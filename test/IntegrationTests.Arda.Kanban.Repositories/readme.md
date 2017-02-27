Issues
=======

MetricRepository.AddNewMetric (row: MetricViewModel)

        MetricRepository metric = new MetricRepository(ctx);

        var row = new MetricViewModel()
        {
            MetricID = Guid.Parse(GUID),
            MetricCategory = METRIC_CATEGORY,
            MetricName = METRIC_NAME,
            Description = METRIC_DESCRIPTION,
            FiscalYearID = Guid.Parse(METRIC_FISCALYEAR_GUID),

            // Ignored data
            FullNumericFiscalYear = 0,
            TextualFiscalYear = "IGNORED"
        };

        metric.AddNewMetric(row);


Users

Model está definido como:

    using Arda.Common.ViewModels.Kanban;

Diferente dos demais modelos que estão no Main:

    using Arda.Common.ViewModels.Main;



Workload

WBActivity <- activityId
WBMetrics (nullable) <- metricId[]
WBTechnologies (nullable) <- technologyID[]
WBUsers (nullable) <- uniqueName[]

WBFilesList (nullable) <- File[]

        filesList.Add(new File()
                    {
                        FileID = f.Item1,
                        FileLink = f.Item2,
                        FileName = f.Item3,
                        FileDescription = string.Empty,
                    });


                }
            //Create workload object:
            var workloadToBeSaved = new WorkloadBacklog()
            {
                WBActivity = activity,
                WBAppointments = null,
                WBComplexity = (Complexity)workload.WBComplexity,
                WBCreatedBy = workload.WBCreatedBy,
                WBCreatedDate = workload.WBCreatedDate,
                WBDescription = workload.WBDescription,
                WBEndDate = workload.WBEndDate,
                WBExpertise = (Expertise)workload.WBExpertise,
                WBFiles = filesList,
                WBID = workload.WBID,
                WBIsWorkload = workload.WBIsWorkload,
                WBMetrics = metricList,
                WBStartDate = workload.WBStartDate,
                WBStatus = (Status)workload.WBStatus,
                WBTechnologies = technologyList,
                WBTitle = workload.WBTitle,
                WBUsers = userList
            };

            _context.WorkloadBacklogs.Add(workloadToBeSaved);
            _context.SaveChanges();
            return true;

Appointment_GetAllAppointments_Should_NotReturnUserName()            