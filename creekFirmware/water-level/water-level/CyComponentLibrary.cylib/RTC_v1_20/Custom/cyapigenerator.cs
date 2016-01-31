/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;

namespace RTC_v1_20
{
    public partial class CyCustomizer
    {
        #region API Generation
        public void GeneratePameters(ref Dictionary<string, string> paramDict)
        {
            StringBuilder writer = new StringBuilder();
            string StartOfWeek;
            string DstEnable;
            paramDict.TryGetValue("StartOfWeek", out StartOfWeek);
            paramDict.TryGetValue("DstEnable", out DstEnable);

            int iDstEnable = Convert.ToInt32(DstEnable);
            int iStartOfWeek = Convert.ToInt32(StartOfWeek);

            writer.AppendLine("/* Days Of Week definition */");
            if (iStartOfWeek == 0)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       1");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       2");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      3");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    4");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      5");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       6");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     7");
            }
            else if (iStartOfWeek == 1)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       7");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       1");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      2");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    3");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      4");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       5");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     6");
            }
            else if (iStartOfWeek == 2)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       6");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       7");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      1");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    2");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      3");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       4");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     5");
            }
            else if (iStartOfWeek == 3)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       5");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       6");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      7");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    1");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      2");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       3");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     4");
            }
            else if (iStartOfWeek == 4)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       4");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       5");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      6");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    7");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      1");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       2");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     1");
            }
            else if (iStartOfWeek == 5)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       3");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       4");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      5");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    6");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      7");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       1");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     2");
            }
            else if (iStartOfWeek == 6)
            {
                writer.AppendLine("#define " + m_instanceName + "_SUNDAY                       2");
                writer.AppendLine("#define " + m_instanceName + "_MONDAY                       3");
                writer.AppendLine("#define " + m_instanceName + "_TUESDAY                      4");
                writer.AppendLine("#define " + m_instanceName + "_WEDNESDAY                    5");
                writer.AppendLine("#define " + m_instanceName + "_THURDAY                      6");
                writer.AppendLine("#define " + m_instanceName + "_FRIDAY                       7");
                writer.AppendLine("#define " + m_instanceName + "_SATURDAY                     1");
            }

            #endregion
            paramDict.Add("DaysOfWeek", writer.ToString());

        }
    }
}


