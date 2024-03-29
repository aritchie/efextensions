﻿using System;


namespace Acr.EfExtensions.Tests
{
    public class SqliteEntity
    {
        public int Id { get; set; }
        public long SetId { get; set; }
        public long? NullId { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset Offset { get; set; }
    }


    public class SqliteDto
    {
        public int SetId { get; set; }
        public int? NullId { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset Offset { get; set; }
    }
}
