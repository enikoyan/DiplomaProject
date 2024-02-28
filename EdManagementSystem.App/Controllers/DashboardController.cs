using EdManagementSystem.App.Models;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    [Authorize(Roles = "teacher")]
    [Route("dashboard/[action]")]
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseAddress = new Uri("https://localhost:44370/api");
        private readonly IMemoryCache _memoryCache;
        private readonly string cacheKey = "profileData";

        public DashboardController(IMemoryCache memoryCache)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseAddress;
            _memoryCache = memoryCache;
        }

        #region ProfileInfo
        [ResponseCache(Duration = 48000, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ActionName("profile")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ProfileViewModel profileData;

            // Try to get data from cache
            if (!_memoryCache.TryGetValue(cacheKey, out profileData!))
            {
                /* GET DATA FROM API if there is no cache */

                // Get userID (email)
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.Name);

                // Get mainInfo from API
                var mainInfo = await GetMainProfileInfo(userId!);
                var additionalInfo = await GetAdditionalInfo(userId!);

                // Formate data
                DateTime registrationDate = mainInfo!.RegDate;
                string formattedDate = registrationDate.ToShortDateString();

                profileData = new ProfileViewModel
                {
                    Fio = mainInfo.Fio,
                    Post = mainInfo.Post,
                    Rate = mainInfo.Rate,
                    Place = mainInfo.Address,
                    PhoneNumber = mainInfo.PhoneNumber,
                    Email = userId,
                    Experience = mainInfo.Experience,
                    RegDate = formattedDate,
                    SquadsCount = additionalInfo[0],
                    StudentsCount = additionalInfo[1],
                    Courses = await GetCourses(userId),
                    Squads = await GetSquads(userId),
                    SocialMediaList = await GetSocialMedia(userId)
                };

                // Saving data in cache
                _memoryCache.Set(cacheKey, profileData);
            }

            return PartialView(profileData);
        }

        private async Task<List<List<string>>> GetSocialMedia(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/SocialMedia/GetSocialMedia/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var socialMediaList = JsonConvert.DeserializeObject<List<SocialMedium>>(content);

                List<List<string>> result = new List<List<string>>();

                foreach (var sm in socialMediaList)
                {
                    result.Add(new List<string> { sm.SocialMediaName, sm.SocialMediaUrl });
                }

                return result;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<Teacher?> GetMainProfileInfo(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/Teachers/GetTeacherByEmail/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var teacher = JsonConvert.DeserializeObject<Teacher>(content);

                return teacher;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<List<int>> GetAdditionalInfo(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetSquadsCount/{userId}");
            HttpResponseMessage response2 = await _httpClient.GetAsync(_baseAddress + $"/profile/GetStudentsCount/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var content2 = response2.Content.ReadAsStringAsync().Result;

                List<int> result = new List<int>();
                result.Add(Convert.ToInt32(content));
                result.Add(Convert.ToInt32(content2));

                return result;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<List<string>> GetCourses(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetCoursesOfTeacher/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                List<string> courses = JsonConvert.DeserializeObject<List<string>>(content);

                return courses;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<List<string>> GetSquads(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetSquadsOfTeacher/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                List<string> squads = JsonConvert.DeserializeObject<List<string>>(content);

                return squads;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        #endregion

        #region GetPages
        [ActionName("students")]
        public IActionResult Students()
        {
            return PartialView();
        }

        [ActionName("schedule")]
        public IActionResult Schedule()
        {
            return PartialView();
        }

        [ResponseCache(Duration = 48000, Location = ResponseCacheLocation.Any)]
        [ActionName("techSupport")]
        public IActionResult TechSupport()
        {
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "answers-list.json");
            string json = System.IO.File.ReadAllText(jsonFilePath);

            List<TechSupportViewModel> questions = JsonConvert.DeserializeObject<List<TechSupportViewModel>>(json);

            return PartialView(questions);
        }

        [ActionName("attendance")]
        public IActionResult Attendance()
        {
            return PartialView();
        }

        [ActionName("analytics")]
        public IActionResult Analytics()
        {
            return PartialView();
        }

        [ActionName("homeworks")]
        public IActionResult Homeworks()
        {
            return PartialView();
        }

        [ActionName("materials")]
        public IActionResult Materials()
        {
            return PartialView();
        }
        #endregion
    }
}
