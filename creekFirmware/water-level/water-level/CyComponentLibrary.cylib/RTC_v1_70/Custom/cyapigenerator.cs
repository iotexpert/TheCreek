/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace RTC_v1_70
{
    public partial class CyCustomizer
    {
        #region API Generation
        public void GeneratePameters(ref Dictionary<string, string> paramDict, string m_instanceName)
        {
            StringBuilder writer = new StringBuilder();
            string startOfWeek;
            string dstEnable;
            paramDict.TryGetValue(CyParamNames.START_OF_WEEK, out startOfWeek);
            paramDict.TryGetValue(CyParamNames.DST_ENABLE, out dstEnable);

            Int32 iDstEnable;
            Int32 iStartOfWeek;
            Int32.TryParse(dstEnable, out iDstEnable);
            Int32.TryParse(startOfWeek, out iStartOfWeek);

            if (iStartOfWeek == 0)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (1u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (2u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (3u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (4u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (5u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (6u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (7u)");
            }
            else if (iStartOfWeek == 1)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (7u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (1u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (2u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (3u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (4u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (5u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (6u)");
            }
            else if (iStartOfWeek == 2)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (6u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (7u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (1u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (2u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (3u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (4u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (5u)");
            }
            else if (iStartOfWeek == 3)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (5u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (6u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (7u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (1u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (2u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (3u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (4u)");
            }
            else if (iStartOfWeek == 4)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (4u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (5u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (6u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (7u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (1u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (2u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (1u)");
            }
            else if (iStartOfWeek == 5)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (3u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (4u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (5u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (6u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (7u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (1u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (2u)");
            }
            else if (iStartOfWeek == 6)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       (2u)");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       (3u)");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      (4u)");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    (5u)");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      (6u)");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       (7u)");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     (1u)");
            }
            #endregion
            paramDict.Add("DaysOfWeek", writer.ToString());
        }
    }
}


