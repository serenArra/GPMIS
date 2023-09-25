using System;
using Abp.Notifications;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}