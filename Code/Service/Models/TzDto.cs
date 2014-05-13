using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilEx;

namespace Service.Models
{
    public class TzDto
    {
        public double dstOffset { get; set; }
        public double rawOffset { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
        public string status { get; set; }
        public string error_message { get; set; }
        public string language { get; set; } /*en or es*/
        public string EnName { get; set; }
        public string EsName { get; set; }

        public string ToSqlInsert()
        {
            return @"INSERT INTO TimeZone (TimeZoneId, TimeZoneNameEnglish, TimeZoneNameSpanish, UtcOffSet, UtcDstOffSet)
                     VALUES({0}, {1}, {2}, {3}, {4})".FormatSql(timeZoneId, EnName, EsName, rawOffset, dstOffset);
        }
    }
}
