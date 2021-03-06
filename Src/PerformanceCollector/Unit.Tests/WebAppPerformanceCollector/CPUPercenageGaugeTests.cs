﻿namespace Unit.Tests
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerformanceCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CPUPercenageGaugeTests
    {
        [TestMethod]
        public void BasicValidation()
        {
            CPUPercenageGauge gauge = new CPUPercenageGauge(
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes * 2", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));

            double value1 = gauge.GetValueAndReset();

            Assert.IsTrue(Math.Abs(value1) < 0.000001);

            Thread.Sleep(TimeSpan.FromSeconds(10));
            double value2 = gauge.GetValueAndReset();
            Assert.IsTrue(
                Math.Abs(value2 - ((24843750 - 24062500.0) / TimeSpan.FromSeconds(10).Ticks * 100.0)) < 0.005, 
                string.Format(CultureInfo.InvariantCulture, "Actual: {0}, Expected: {1}", value2, (24843750 - 24062500.0) / TimeSpan.FromSeconds(10).Ticks));
        }
    }
}
