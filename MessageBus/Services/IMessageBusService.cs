﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.Services
{
    public interface IMessageBusService
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }
}
