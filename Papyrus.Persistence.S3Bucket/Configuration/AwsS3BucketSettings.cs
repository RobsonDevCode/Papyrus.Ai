namespace Papyrus.Persistence.S3Bucket.Configuration;

public record AwsS3BucketSettings
{
    public string AccessKey { get; init; }
    public string PdfBucketName { get; init; }
    public string AudioBucketName { get; init; }
    public string Region { get; init; }
}