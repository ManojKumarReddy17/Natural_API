using System;

#nullable disable
namespace Natural_Core.S3Models
{
	public class S3Config
	{
        public string BucketName { get; set; }
        public string Image { get; set; }
        public string PresignedUrl { get; set; }
    }
}

