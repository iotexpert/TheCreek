/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;
using System.IO;

namespace QuadDec_v1_10
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        private const string m_instanceNameParam = "INSTANCE_NAME";
        private string m_instanceName = "";
        private string m_counterSize = "";

        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query,
                                                          ICyTerminalQuery_v1 termQuery,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> custApis = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> custDict;

            for (int i = 0; i < custApis.Count; i++)
            {
                custDict = custApis[i].MacroDictionary;
                custDict.TryGetValue(m_instanceNameParam, out m_instanceName);
                m_counterSize = query.GetCommittedParam("CounterSize").Value;
                custApis[i].MacroDictionary = GenerateApi(custDict);
            }

            return custApis;
        }

        #endregion

        private Dictionary<string, string> GenerateApi(Dictionary<string, string> dict)
        {
            GenerateCFile(dict);
            GenerateHFile(dict);
            GenerateISR(dict);
            GenerateSoftwareCounterDefine(dict);
            GenerateInterruptMaskDefine(dict);

            return dict;
        }

        private void GenerateISR(Dictionary<string, string> dict)
        {
            StringWriter writer = new StringWriter();

//            if (Convert.ToDouble(m_counterSize) == 8)
//            {
////                writer.WriteLine("    if(QdStatus & (" + m_instanceName + "_COUNTER_OVERFLOW | " + m_instanceName + "_COUNTER_UNDERFLOW )  )");
////                writer.WriteLine("    {");
////                writer.WriteLine("        " + m_instanceName + "_Cnt8_Stop();");
////                writer.WriteLine("        " + m_instanceName + "_SetCounter(0x00);");
////                writer.WriteLine("        " + m_instanceName + "_Cnt8_Start();");
////                writer.WriteLine("    }");
//            }
//            else if (Convert.ToDouble(m_counterSize) == 16)
//            {
////                writer.WriteLine("    if(CntStatus & " + m_instanceName + "_RESET)");
////                writer.WriteLine("        " + m_instanceName + "_Cnt16_WriteCounter(0x8000);");
//            }
            /*else*/
            
             
            if (Convert.ToDouble(m_counterSize) == 32)
            {
                writer.WriteLine("    " + m_instanceName + "_SwStatus = " + m_instanceName + "_STATUS;");
                writer.WriteLine("    ");
                writer.WriteLine("    if(" + m_instanceName + "_SwStatus & " + m_instanceName + "_COUNTER_OVERFLOW)");
                writer.WriteLine("    {");
                writer.WriteLine("        " + m_instanceName + "_Count32SoftPart += 32767;");
                writer.WriteLine("    }");
                writer.WriteLine("    else if(" + m_instanceName + "_SwStatus & " + m_instanceName + "_COUNTER_UNDERFLOW)");
                writer.WriteLine("    {");
                writer.WriteLine("        " + m_instanceName + "_Count32SoftPart -= 32768;");
                writer.WriteLine("    }");
//                writer.WriteLine("    if(" + m_instanceName + "_SwStatus & " + m_instanceName + "_RESET)");
//                writer.WriteLine("    {");
//                writer.WriteLine("        " + m_instanceName + "_Count32SoftPart = 0;");
//                writer.WriteLine("    }");
            }

            dict.Add("writeISR", writer.ToString());
        }

        private void GenerateCFile(Dictionary<string, string> dict)
        {
            StringWriter writer = new StringWriter();

            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Start");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Initializes UDBs and other relevant hardware. ");
            writer.WriteLine("*  Resets counter to 0, enables or disables all relevant interrupts.");
            writer.WriteLine("*  Starts monitoring the inputs and counting.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_Start(void)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 a = 0x0f;");
            writer.WriteLine("");

            if (Convert.ToDouble(m_counterSize) == 8)
            {
                writer.WriteLine("    " + m_instanceName + "_Cnt8_Start();");
            }
            if ((Convert.ToDouble(m_counterSize) == 16) || (Convert.ToDouble(m_counterSize) == 32))
            {
                writer.WriteLine("    " + m_instanceName + "_Cnt16_Start();");
            }

            writer.WriteLine("    " + m_instanceName + "_SR_AUX_CONTROL |= " + m_instanceName + 
                             "_INTERRUPTS_ENABLE;       /* enable interrupts */");
            writer.WriteLine("");
            writer.WriteLine("    if(" + m_instanceName + "_initVar == 1)");
            writer.WriteLine("    {");
            writer.WriteLine("        " + m_instanceName + "_initVar = 0;");
            writer.WriteLine("    ");
            if (Convert.ToDouble(m_counterSize) == 32)
            {
                writer.WriteLine("        /* Disable Interrupt. */");
                writer.WriteLine("        CyIntDisable(" + m_instanceName + "_ISR_NUMBER);");
                writer.WriteLine("");
                writer.WriteLine("        /* Set the ISR to point to the " + m_instanceName + "_isr Interrupt. */");
                writer.WriteLine("        CyIntSetVector(" + m_instanceName + "_ISR_NUMBER, " + m_instanceName + "_ISR);");
                writer.WriteLine("");
                writer.WriteLine("        /* Set the priority. */");
                writer.WriteLine("        CyIntSetPriority(" + m_instanceName + "_ISR_NUMBER, " + m_instanceName + "_ISR_PRIORITY);");
                writer.WriteLine("");
                writer.WriteLine("        /* Enable it. */");
                writer.WriteLine("        CyIntEnable(" + m_instanceName + "_ISR_NUMBER);");
                writer.WriteLine("");
            }
            writer.WriteLine("        " + m_instanceName + "_Control_Reg_Write(" + m_instanceName + "_START_RESET);            /* QD reset */");
            writer.WriteLine("        while(a){a--;}");
            writer.WriteLine("        " + m_instanceName + "_Control_Reg_Write(" + 
                             m_instanceName + "_ENABLE);                 /* QD enable */");
            writer.WriteLine("    }");                 
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_Stop");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Turns off UDBs and other relevant hardware.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_Stop(void)");
            writer.WriteLine("{                        ");
            if (Convert.ToDouble(m_counterSize) == 8)
            {
                writer.WriteLine("    " + m_instanceName + 
                    "_Cnt8_Stop();                              /* counter disable */");
                writer.WriteLine("    " + m_instanceName + "_SR_AUX_CONTROL |= " + m_instanceName + 
                             "_INTERRUPTS_ENABLE;       /* enable interrupts */");
            }
            if ((Convert.ToDouble(m_counterSize) == 16) || (Convert.ToDouble(m_counterSize) == 32))
            {
                writer.WriteLine("    " + 
                    m_instanceName + "_Cnt16_Stop();                       /* counter disable */");
            }
//            writer.WriteLine("    " + m_instanceName + 
//                "_Control_Reg_Write(1);                         /* QD disable */");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            if (Convert.ToDouble(m_counterSize) == 8)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_GetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reports the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  The counter value. Return type is signed and per ");
                writer.WriteLine("*  the counter size setting. A positive value indicates ");
                writer.WriteLine("*  clockwise movement (B before A).");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("int8 " + m_instanceName + "_GetCounter(void)");
                writer.WriteLine("{");
                writer.WriteLine("    int8 count8;");
                writer.WriteLine("    uint8 tmpCnt8;");
                writer.WriteLine("  ");
                writer.WriteLine("    tmpCnt8 = " + m_instanceName + "_Cnt8_ReadCounter();");
                writer.WriteLine("    ");
                writer.WriteLine("    count8 = tmpCnt8 ^ 0x80;");
//                writer.WriteLine("    if(tmpCnt8 | 0x80)");
//                writer.WriteLine("    {");
//                writer.WriteLine("        count8 ++;");
//                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return(count8);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_SetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  value:  The new value. Parameter type is signed and per the counter size  ");
                writer.WriteLine("*  setting.  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_SetCounter(int8 value)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 count8;");
                writer.WriteLine("  ");
                writer.WriteLine("    count8 = (value ^ 0x80);");
                writer.WriteLine("    " + m_instanceName + "_Cnt8_WriteCounter(count8);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
            }            
            if (Convert.ToDouble(m_counterSize) == 16)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_GetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reports the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  The counter value. Return type is signed and per ");
                writer.WriteLine("*  the counter size setting. A positive value indicates ");
                writer.WriteLine("*  clockwise movement (B before A).");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");                
                writer.WriteLine("int16 " + m_instanceName + "_GetCounter(void)");
                writer.WriteLine("{");
                writer.WriteLine("    int16 count16;");
                writer.WriteLine("    uint16 tmpCnt16;");
                writer.WriteLine("  ");
                writer.WriteLine("    tmpCnt16 = " + m_instanceName + "_Cnt16_ReadCounter();");
                writer.WriteLine("    ");
                writer.WriteLine("    count16 = tmpCnt16 ^ 0x8000;");
                writer.WriteLine("    if(tmpCnt16 | 0x8000)");
//                writer.WriteLine("    {");
//                writer.WriteLine("        count16 ++;");
//                writer.WriteLine("    }");
                writer.WriteLine("    ");
                writer.WriteLine("    return(count16);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_SetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  value:  The new value. Parameter type is signed and per the counter size  ");
                writer.WriteLine("*  setting.  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_SetCounter(int16 value)");
                writer.WriteLine("{");
                writer.WriteLine("    uint16 count16;");
                writer.WriteLine("  ");
                writer.WriteLine("    count16 = (value ^ 0x8000);");
                writer.WriteLine("    " + m_instanceName + "_Cnt16_WriteCounter(count16);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
            }
            if (Convert.ToDouble(m_counterSize) == 32)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_GetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reports the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  The counter value. Return type is signed and per ");
                writer.WriteLine("*  the counter size setting. A positive value indicates ");
                writer.WriteLine("*  clockwise movement (B before A).");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("int32 " + m_instanceName + "_GetCounter(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint16 tmpCnt16;");
                writer.WriteLine("    int32 result;");
                writer.WriteLine("");
                writer.WriteLine("    tmpCnt16 = " + m_instanceName + "_Cnt16_ReadCounter();");
                writer.WriteLine("    ");
                writer.WriteLine("    if((tmpCnt16 & 0x8000) == 0x8000)");
                writer.WriteLine("    {");
                writer.WriteLine("        result = tmpCnt16 ^ 0x8000;");
                writer.WriteLine("    }");
                writer.WriteLine("    else");
                writer.WriteLine("    {");
                writer.WriteLine("        result = 0x8000 - tmpCnt16;");
                writer.WriteLine("        result = -result;");
                writer.WriteLine("    }");
                writer.WriteLine("    result += " + m_instanceName + "_Count32SoftPart;");                
                writer.WriteLine("    return (result);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + m_instanceName + "_SetCounter");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Sets the current value of the counter.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  value:  The new value. Parameter type is signed and per the counter size  ");
                writer.WriteLine("*  setting.  ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_SetCounter(int32 value)");
                writer.WriteLine("{");
                //writer.WriteLine("    uint16 UDBcount16;");
                //writer.WriteLine("    if(value < 0) value++;");
                //writer.WriteLine("    " + m_instanceName + "_Cnt16_WriteCounter(0x8000);");
                writer.WriteLine("    " + m_instanceName + "_Count32SoftPart = value;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
            }            
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + m_instanceName + "_SetInterruptMask");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Enables / disables interrupts due to the events. ");
            writer.WriteLine("*  For the 32-bit counter, the overflow and underflow interrupts cannot be disabled; ");
            writer.WriteLine("*  these bits are ignored.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  mask:  Enable / disable bits in an 8-bit value,where 1 enables the interrupt. ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + m_instanceName + "_SetInterruptMask(uint8 mask)");
            writer.WriteLine("{");
            if (Convert.ToDouble(m_counterSize) == 32)
            {
                writer.WriteLine("    /* Underflow & Overflow interrupt for 32-bit Counter  always enable */");
                writer.WriteLine("    mask |= (" + m_instanceName + "_COUNTER_OVERFLOW | " + m_instanceName + "_COUNTER_UNDERFLOW);                                            ");
            }
            writer.WriteLine("    " + m_instanceName + "_STATUS_MASK = mask;");
            writer.WriteLine("}");
            
            dict.Add("writeCFile", writer.ToString());
        }

        private void GenerateHFile(Dictionary<string, string> dict)
        {
            StringWriter writer = new StringWriter();          

            if (Convert.ToDouble(m_counterSize) == 8)
            {
                writer.WriteLine("int8    " + m_instanceName + "_GetCounter(void);");
                writer.WriteLine("void    " + m_instanceName + "_SetCounter(int8 value);");                
            }
            if (Convert.ToDouble(m_counterSize) == 16)
            {
                writer.WriteLine("int16   " + m_instanceName + "_GetCounter(void);");
                writer.WriteLine("void    " + m_instanceName + "_SetCounter(int16 value);");                
            }
            if (Convert.ToDouble(m_counterSize) == 32)
            {
                writer.WriteLine("int32   " + m_instanceName + "_GetCounter(void);");
                writer.WriteLine("void    " + m_instanceName + "_SetCounter(int32 value);");                
            }
           
            dict.Add("writeHFile", writer.ToString());

            writer = new StringWriter();
            if (Convert.ToDouble(m_counterSize) == 8)
            {
                writer.WriteLine("#include \"" + m_instanceName + "_Cnt8.h\"");
            }
            else
            {
                writer.WriteLine("#include \"" + m_instanceName + "_Cnt16.h\"");
            }            

            dict.Add("writeIncludeCntHFile", writer.ToString());
        }
        
        private void GenerateSoftwareCounterDefine(Dictionary<string, string> dict)
        {
            StringWriter writer = new StringWriter(); 
            
            if (Convert.ToDouble(m_counterSize) == 32)
            {                
                writer.WriteLine("");
//                writer.WriteLine("extern int32 " + m_instanceName + "_Count32SoftPart;");
            }
            writer.WriteLine("");
            
            dict.Add("writeSoftwareCounterDefine", writer.ToString());
        }
        
        private void GenerateInterruptMaskDefine(Dictionary<string, string> dict)
        {
            StringWriter writer = new StringWriter(); 
            
            if (Convert.ToDouble(m_counterSize) == 32)
            {                
                writer.WriteLine("");
//                writer.WriteLine("#define COUNTER_32_BIT_INTERRUPT_MASK 0x03u");
            }
            
            dict.Add("writeInterruptMaskDefine", writer.ToString());
        }
        
    }
}

