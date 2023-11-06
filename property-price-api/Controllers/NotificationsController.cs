﻿using Microsoft.AspNetCore.Mvc;
using property_price_api.Helpers;
using property_price_api.Models;
using property_price_api.Services;

namespace property_price_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {

        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<Notification>> GetNotifications([FromQuery] NotificationQueryParameters parameters) =>
            await _notificationService.GetNotifications(parameters.NotifierId);

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Notification>> GetPropertyById(string id)
        {
            var notification = await _notificationService.GeNotificationById(id);

            if (notification is null)
            {
                return NotFound();
            }

            return notification;
        }

        [Authorize]
        [HttpPatch("{id:length(24)}")]
        public async Task<ActionResult<Notification>> UpdateNotificationById(string id, UpdateNotificationRequest updateNotificationRequest)
        {
            var _notification = await _notificationService.GeNotificationById(id);

            if (_notification is null)
            {
                return NotFound();
            }

            var notification = await _notificationService.UpdateNotificationById(id, updateNotificationRequest);
            return Ok(notification);
        }
    }
}

