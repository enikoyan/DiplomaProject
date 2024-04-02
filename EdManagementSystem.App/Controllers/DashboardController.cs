using EdManagementSystem.App.Models;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICacheService _cacheService;
        private string userId { get; set; } = null!;

        // CacheKeys
        private string cacheKey_profile { get; set; } = null!;
        private string cacheKey_students { get; set; } = null!;
        private string cacheKey_techSupport { get; set; } = null!;
        private string cacheKey_materials { get; set; } = null!;

        public DashboardController(ICacheService cacheService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseAddress;
            _cacheService = cacheService;
        }

        #region ProfileInfo
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, VaryByHeader = "User-Agent")]
        [ActionName("profile")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            userId = HttpContext.User.FindFirstValue(ClaimTypes.Name)!;
            cacheKey_profile = $"profile_{userId}";

            var profileData = await _cacheService.GetOrSetAsync(cacheKey_profile, async () =>
            {
                var mainInfo = await GetMainProfileInfo(userId);
                var additionalInfo = await GetAdditionalInfo(userId);

                DateTime registrationDate = mainInfo!.RegDate;
                string formattedDate = registrationDate.ToShortDateString();

                var data = new ProfileViewModel
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

                return data;
            }, TimeSpan.FromDays(7));

            return PartialView(profileData);
        }

        private async Task<List<List<string>>> GetSocialMedia(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseAddress + $"/SocialMedia/GetSocialMedia/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var socialMediaList = JsonConvert.DeserializeObject<List<SocialMedium>>(content);

                if (socialMediaList!.Count == 0 || socialMediaList == null)
                {
                    return new List<List<string>>();
                }

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

                List<int> result = [Convert.ToInt32(content), Convert.ToInt32(content2)];

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
        [ResponseCache(Duration = 48000, Location = ResponseCacheLocation.Client)]
        [ActionName("techSupport")]
        public async Task<IActionResult> TechSupport()
        {
            cacheKey_techSupport = $"techSupport";

            var techSupportData = await _cacheService.GetOrSetAsync(cacheKey_techSupport, async () =>
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "answers-list.json");
                string json = System.IO.File.ReadAllText(jsonFilePath);

                List<TechSupportViewModel> questions = JsonConvert.DeserializeObject<List<TechSupportViewModel>>(json)!;
                return questions;

            }, TimeSpan.FromDays(365));

            return PartialView(techSupportData);
        }

        #endregion

        #region StudentsPage
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.None, VaryByHeader = "User-Agent")]
        [ActionName("students")]
        [HttpGet]
        public async Task<IActionResult> Students()
        {
            userId ??= HttpContext.User.FindFirstValue(ClaimTypes.Name)!;

            cacheKey_students = $"studentsOf_{userId}";

            var studentsData = await _cacheService.GetOrSetAsync(cacheKey_students, async () =>
            {
                var coursesList = await GetCourses(userId);
                var squadsList = await GetSquads(userId);

                var studentsVM = new StudentsPageViewModel
                {
                    coursesList = coursesList,
                    squadsList = squadsList
                };

                return studentsVM;
            }, TimeSpan.FromDays(7));

            return PartialView(studentsData);
        }

        #endregion

        #region MaterialsPage
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.None, VaryByHeader = "User-Agent")]
        [ActionName("materials")]
        [HttpGet]
        public async Task<IActionResult> Materials()
        {
            userId ??= HttpContext.User.FindFirstValue(ClaimTypes.Name)!;

            cacheKey_materials = $"materialsOf_{userId}";

            var materialsData = await _cacheService.GetOrSetAsync(cacheKey_materials, async () =>
            {
                var coursesList = await GetCourses(userId);
                var squadsList = await GetSquads(userId);

                var materialsVM = new MaterialsPageViewModel
                {
                    coursesList = coursesList,
                    squadsList = squadsList
                };

                return materialsVM;
            }, TimeSpan.FromDays(7));

            return PartialView(materialsData);
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
        #endregion

        #region PrivateMethods
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
    }
}
