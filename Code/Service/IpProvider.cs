using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataEx;
using RestSharp;
using Service.Models;
using UtilEx;

namespace Service
{
    public class IpProvider
    {

        private readonly Database _db;

        public IpProvider()
        {
            _db = new Database();
        }

        public IpDto GetIpData(string ipAddress)
        {
            var ipData = GetIpData(IPAddress.Parse(ipAddress));
            ipData.IpAddress = ipAddress;
            if (string.IsNullOrWhiteSpace(ipData.TimeZone))
            {
                UpdateLocation(ipData);
            }
            return ipData;
        }

        private void UpdateLocation(IpDto ipData)
        {
            var tz = GetTzFromLocation(ipData.Latitude, ipData.Longitude);
            UpdateLocation(tz, ipData.LocationId);
            ipData.TimeZone = tz.timeZoneId;
        }

        private void UpdateLocation(TzDto tz, uint locationId)
        {
            var newTzId = UpsertTimeZoneInfo(tz);
            _db.ExecuteNonQuery("UPDATE Location SET TimeZoneId = {0} WHERE Id = {1}".Fi(newTzId, locationId));
        }

        private int UpsertTimeZoneInfo(TzDto tz)
        {
            var id =
                _db.ExecuteScalar<int>(
                    "SELECT COALESCE(Id, 0) FROM TimeZone WHERE TimeZoneId = {0}".FormatSql(tz.timeZoneId));
            return id <= 0 ? _db.ExecuteScalar<int>("{0};\nSELECT LAST_INSERT_ID();".Fi(tz.ToSqlInsert())) : id;
        }

        public IpDto GetIpData(IPAddress ipAddress)
        {
            return GetIpData(ipAddress.ToInt());
        }

        public IpDto GetIpData(uint ipAddress)
        {
            var result = _db.ExecuteToList<IpDto>(GetSql(ipAddress)).SingleOrDefault();
            if (result == null) throw new ArgumentNullException("result");
            result.IpNumber = ipAddress;
            return result;
        }

        private TzDto GetTzFromLocation(double latitude, double longitude)
        {
            var esResult = GetTzFromLocation(latitude, longitude, false);
            esResult.EsName = esResult.timeZoneName;
            var enResult = GetTzFromLocation(latitude, longitude, true);
            esResult.EnName = enResult.timeZoneName;
            return esResult;
        }

        private TzDto GetTzFromLocation(double latitude, double longitude, bool isEnglish)
        {
            var restClient = new RestClient("https://maps.googleapis.com/maps/api/timezone");
            var restRequest = new RestRequest("json", Method.GET);
            restRequest.AddParameter("location", "{0},{1}".Fi(latitude, longitude));
            restRequest.AddParameter("timestamp", DateTime.UtcNow.ToEpoch());
            restRequest.AddParameter("sensor", "false");
            restRequest.AddParameter("language", isEnglish ? "en" : "es");
            restRequest.AddParameter("key", GetApiKey());
            var response = restClient.Execute<TzDto>(restRequest);
            return response.Data.status == "REQUEST_DENIED" ? null : response.Data;
        }

        private string GetApiKey()
        {
            return ConfigurationManager.AppSettings["google.api.timezone.key"];
        }

        private static string GetSql(uint ipAddress)
        {
            return @"
SELECT
	IpBlock.Id, Location.Latitude, Location.Longitude,
	Location.CountryCode, Country.EnglishName As Country,
	Location.City, TimeZone.TimeZoneId As TimeZone,
	Location.LocationId
FROM
	IpBlock
	INNER JOIN Location ON IpBlock.LocationId   = Location.LocationId
	INNER JOIN Country  ON Location.CountryCode = Country.Code
	LEFT  JOIN TimeZone ON Location.TimeZoneId  = TimeZone.Id
WHERE
	{0} BETWEEN IpBlock.StartIpNumber AND IpBlock.EndIpNumber
LIMIT 1
".Fi(ipAddress);
        }
    }
}
