﻿using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Entities
{
    public class AuthorEntity : Author, ITableEntity
    {
        public string PartitionKey { get; set; } = "PartitionKey";
        public string RowKey { get => Id; set => Id = value; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public ETag ETag { get; set; }

        [IgnoreDataMember]
        public new string TwitterUrl => base.TwitterUrl;
        
    }
}
