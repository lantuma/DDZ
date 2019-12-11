﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public static class ActorMessageSenderHelper
    {
        public static void Send(this ActorMessageSender self, IActorMessage message)
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.Address);
            message.ActorId = self.ActorId;
            session.Send(message);
        }

        public static async ETTask<IActorResponse> Call(this ActorMessageSender self, IActorRequest message)
        {
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(self.Address);
            message.ActorId = self.ActorId;
            return (IActorResponse)await session.Call(message);
        }
    }
}