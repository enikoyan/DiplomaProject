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
        private readonly Uri _baseAddress = new Uri("https://localhost:7269/api");
        private readonly IMemoryCache _memoryCache;

        public DashboardController(IMemoryCache memoryCache)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseAddress;
            _memoryCache = memoryCache;
        }

        #region ProfileInfo
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, VaryByHeader = "User-Agent")]
        [ActionName("profile")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get userID (email)
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var cacheKey_profile = $"profile_{userId}";

            // Check if there is already a cache for this query
            if (!_memoryCache.TryGetValue(cacheKey_profile, out ProfileViewModel data))
            {
                // If cache does not exist, create a new one

                // Create cache entry options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(7)); // Cache lifetime

                /* GET DATA FROM API if there is no cache */
                // Get mainInfo from API
                var mainInfo = await GetMainProfileInfo(userId);
                var additionalInfo = await GetAdditionalInfo(userId);

                // Format data
                DateTime registrationDate = mainInfo!.RegDate;
                string formattedDate = registrationDate.ToShortDateString();

                // Create view model with data
                var profileData = new ProfileViewModel
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
                    Courses = await GetCoursesNames(userId),
                    Squads = await GetSquadsNames(userId),
                    SocialMediaList = await GetSocialMedia(userId)
                };

                // Store the view model in cache
                _memoryCache.Set(cacheKey_profile, profileData, cacheEntryOptions);

                // Use the data as the response
                data = profileData;
            }

            return PartialView(data);
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

        private async Task<List<string>> GetCoursesNames(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetCoursesNamesOfTeacher/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                List<string> courses = JsonConvert.DeserializeObject<List<string>>(content)!;

                return courses!;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<List<string>> GetSquadsNames(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetSquadsNamesOfTeacher/{userId}");

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

        #region TechSupport
        [ResponseCache(Duration = 48000, Location = ResponseCacheLocation.Any)]
        [ActionName("techSupport")]
        public IActionResult TechSupport()
        {
            GetCurrentUserId();

            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "answers-list.json");
            string json = System.IO.File.ReadAllText(jsonFilePath);

            List<TechSupportViewModel> questions = JsonConvert.DeserializeObject<List<TechSupportViewModel>>(json);

            return PartialView(questions);
        }

        [HttpGet]
        [ActionName("getCurrentUserId")]
        public IActionResult GetCurrentUserId()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.Name)!;
            return Ok(userId);
        }

        #endregion

        #region StudentsPage
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.None, VaryByHeader = "User-Agent")]
        [ActionName("students")]
        [HttpGet]
        public async Task<IActionResult> Students()
        {
            // Get userID (email)
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var cacheKey_students = $"studentsOf_{userId}";

            // Check if there is already a cache for this query
            if (!_memoryCache.TryGetValue(cacheKey_students, out StudentsPageViewModel data))
            {
                // If cache does not exist, create a new one

                // Create cache entry options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Cache lifetime
                    .SetAbsoluteExpiration(TimeSpan.FromDays(7));

                // Get courses and squads based on user ID
                var coursesList = await GetCourses(userId);
                var squadsList = await GetSquads(userId);

                // Create view model with data
                var studentsVM = new StudentsPageViewModel
                {
                    coursesList = coursesList,
                    squadsList = squadsList
                };

                // Store the view model in cache
                _memoryCache.Set(cacheKey_students, studentsVM, cacheEntryOptions);

                // Use the data as the response
                data = studentsVM;
            }

            return PartialView(data);
        }

        private async Task<List<Course>> GetCourses(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetCoursesOfTeacher/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                List<Course> courses = JsonConvert.DeserializeObject<List<Course>>(content)!;

                return courses!;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        private async Task<List<Squad>> GetSquads(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/profile/GetSquadsOfTeacher/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                List<Squad> squads = JsonConvert.DeserializeObject<List<Squad>>(content)!;

                return squads;
            }
            else
            {
                throw new Exception("Не удалось получить информацию!");
            }
        }

        #endregion

        #region GetPages

        [ActionName("schedule")]
        public IActionResult Schedule()
        {
            return PartialView();
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
