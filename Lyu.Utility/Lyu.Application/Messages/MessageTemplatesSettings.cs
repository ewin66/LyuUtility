﻿using Abp.Configuration;

namespace Lyu.Application.Messages
{
    public class MessageTemplatesSettings: IMessageTemplatesSettings
    {
        protected readonly ISettingManager SettingManager;

        public MessageTemplatesSettings(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }


        public bool CaseInvariantReplacement
        {
            get { return SettingManager.GetSettingValue<bool>(MessageTemplatesSettingNames.CaseInvariantReplacement); }
        }

        public string Color1 { get { return SettingManager.GetSettingValue(MessageTemplatesSettingNames.Color1); } }
        public string Color2 { get { return SettingManager.GetSettingValue(MessageTemplatesSettingNames.Color2); } }
        public string Color3 { get { return SettingManager.GetSettingValue(MessageTemplatesSettingNames.Color3); } }
    }
}