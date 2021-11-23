using System;

namespace SmartTask.Infrastructure
{
    public class SmartTaskSettings
    {
        public string DefaultConnection { get; set; }

        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public string JwtExpireMinutes { get; set; }

        public string ReportingServiceUrl { get; set; }

        public string ReportingServiceApiKey { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string EmailQueueName { get; set; }

        public string MikroIntegrationTopicName { get; set; }

        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }

        public string BlobContainerName { get; set; }

        public string AssetsContainerName { get; set; }

        public string StampKeyPrefix { get; set; }

        public string SignKeyPrefix { get; set; }

        public int? FinancingInvoiceRecapId { get; set; }

        public decimal? CorrectionAmountMaxValue { get; set; }

        public decimal? CorrectionAmountMaxPercent { get; set; }

        public string Endpoint { get; set; }

        public string AccessKey { get; set; }

        public string Secret { get; set; }

        public string LoginName { get; set; }

        public string SenderName { get; set; }

        public string Password { get; set; }

        public string Url { get; set; }
        public string S3Endpoint { get; set; }
        public string S3Bucket { get; set; }
        public string S3AccessKey { get; set; }
        public string S3SecretKey { get; set; }
    }
}
