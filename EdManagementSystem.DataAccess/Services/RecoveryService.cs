using EdManagementSystem.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using EdManagementSystem.DataAccess.Models;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.DTO;

namespace EdManagementSystem.DataAccess.Services
{
    public class RecoveryService : IRecoveryService
    {
        private readonly EdSystemDbContext _context;
        private readonly IUserService _userService;
        private readonly IEmailClient _emailClient;

        public RecoveryService(EdSystemDbContext context, IUserService userService, IEmailClient emailClient)
        {
            _context = context;
            _userService = userService;
            _emailClient = emailClient;
        }

        public async Task<string> RecoveryTrigger(string userEmail, EmailConfigDTO emailConfig)
        {
            try
            {
                // Check user exist
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == userEmail);

                if (user != null)
                {
                    var currentDate = DateTime.Today;
                    var recoveryList = await _context.Recoveries
                        .Where(x => x.Date.Date == currentDate).ToListAsync();

                    if (recoveryList.Count > 3)
                    {
                        throw new Exception("Пароль можно менять только 3 раза в день!");
                    }
                    else
                    {
                        // Generate user key
                        Guid userKey = GenerateKey();

                        // Generate server key
                        Guid serverKey = GenerateKey();

                        Recovery recoveryDTO = new Recovery()
                        {
                            UserId = user.UserId,
                            Date = DateTime.Now,
                            UserKey = userKey,
                            ServerKey = serverKey,
                        };

                        var message = $"<h1>Уважаемый пользователь УМДО!</h1></br>" +
                            $"<h3>Для смены пароля используйте данный ключ: <i>{userKey}<i></h3>";

                        // Send email to client
                        _emailClient.SendMessage(emailConfig, userEmail, message);

                        // Add data in database
                        await _context.Recoveries.AddAsync(recoveryDTO);
                        await _context.SaveChangesAsync();

                        return serverKey.ToString();
                    }
                }
                else
                {
                    throw new Exception("Такого пользователя не существует! Введите корректный email.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> RecoveryProcess(Guid serverKey, Guid clientKey, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new Exception("Новый пароль не может быть пустым!");
            }
            else
            {
                // Check key expiration
                var recoverySession = await _context.Recoveries.FirstOrDefaultAsync(s => s.ServerKey == serverKey);

                if (recoverySession != null && recoverySession.Confirmed != true)
                {
                    // Calc difference between current time and addedDate
                    TimeSpan difference = DateTime.Now - recoverySession.Date;

                    // Check expire
                    if (difference.TotalMinutes > recoverySession.ExpireTime.Minute)
                    {
                        throw new Exception($"Прошло более {recoverySession.ExpireTime.Minute} " +
                            $"минут с момента оформления заявки! Попробуйте снова.");
                    }
                    else
                    {
                        if (recoverySession.UserKey == clientKey && recoverySession.ServerKey == serverKey)
                        {
                            // Get current user
                            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == recoverySession.UserId);

                            // Change password
                            var result = await _userService.ChangePassword(user!, newPassword);
                            if (result)
                            {
                                recoverySession.Confirmed = true;
                                _context.Entry(recoverySession).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                return user!.UserEmail;
                            }
                            else throw new Exception("Не удалось сменить пароль!");
                        }
                        else throw new Exception("Ключ восстановления неправильный, " +
                            "попробуйте снова через какое-то время и проверьте электронную почту!");
                    }
                }
                else throw new Exception("Ключ восстановления неправильный, попробуйте снова через какое-то время!");
            }
        }

        private Guid GenerateKey() => Guid.NewGuid();
    }
}
