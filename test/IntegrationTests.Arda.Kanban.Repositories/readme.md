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

