﻿using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using WeeklyXamarin.Core.Models;

namespace WeeklyXamarin.AdminServices.Entities
{
    public class ArticleEntity : Article, ITableEntity
    {
        public string PartitionKey { get; set; } = "Article";
        public string RowKey { get => Id; set => Id = value; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public ETag ETag {get;set;}
    }
}
