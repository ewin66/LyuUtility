﻿namespace Lyu.Abp.Core.Messages
{
    public interface IMessageTemplatesSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to replace message tokens according to case invariant rules
        /// </summary>
        bool CaseInvariantReplacement { get; set; }

        /// <summary>
        /// Gets or sets a color1 in  hex format ("#hhhhhh") to use in workflow message formatting
        /// </summary>
        string Color1 { get; set; }

        /// <summary>
        /// Gets or sets a color2 in  hex format ("#hhhhhh") to use in workflow message formatting
        /// </summary>
        string Color2 { get; set; }

        /// <summary>
        /// Gets or sets a color3 in  hex format ("#hhhhhh") to use in workflow message formatting
        /// </summary>
        string Color3 { get; set; }
    }
}