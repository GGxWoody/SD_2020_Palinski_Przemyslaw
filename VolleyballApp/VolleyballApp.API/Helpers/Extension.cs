using System;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VolleyballApp.API.Helpers
{
    public static class Extension
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Aplication-Error");
            response.Headers.Add("Access-Control-Require-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today) age--;
            return age;
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static bool IsCorrectSet(int FistTeam, int SecondTeam, int setNumber)
        {
            if (setNumber < 5)
            {
                if ((FistTeam <= 25 && SecondTeam <= 25) && (FistTeam == 25 || SecondTeam == 25) && Math.Abs(FistTeam - SecondTeam) >= 2) return true;
                if (FistTeam + SecondTeam >= 50 && Math.Abs(FistTeam - SecondTeam) == 2) return true;
            } else
            {
                if ((FistTeam <= 15 && SecondTeam <= 15) && (FistTeam == 15 || SecondTeam == 15) && Math.Abs(FistTeam - SecondTeam) >= 2) return true;
                if (FistTeam + SecondTeam >= 30 && Math.Abs(FistTeam - SecondTeam) == 2) return true;
            }
            return false;
        }
    }
}