/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Runtime.InteropServices;


namespace ShiftReg_v1_10
{

    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        static string CS_CFile_Name = "ShiftReg.c";
        static string CS_HFile_Name = "ShiftReg.h";
        public const string p_instanceNameParam = "INSTANCE_NAME";
        string m_instanceName = "";

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            CyAPICustomizer CS_CFile; 
            CyAPICustomizer CS_HFile;
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();


            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(CS_CFile_Name))
                {
                    CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(p_instanceNameParam, out m_instanceName);
                }
                else if (api.OriginalName.EndsWith(CS_HFile_Name))
                {
                    CS_HFile = api;
                }
            }
            GenerateParameters(ref paramDict);

            //If No Data in Main object than no data will be generate
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion

        public void GenerateParameters(ref Dictionary<string, string> paramDict)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            string m_UseInputFifo;
            string INSTANCE_NAME;
            string m_UseOutputFifo;
            string FifoSize;
            string Direction;
            string m_Length;
            string InterruptSource;
            string UseInterrupt;
            paramDict.TryGetValue("UseInterrupt", out UseInterrupt);
            paramDict.TryGetValue("UseInputFifo", out m_UseInputFifo);
            paramDict.TryGetValue("INSTANCE_NAME", out INSTANCE_NAME);
            paramDict.TryGetValue("UseOutputFifo", out m_UseOutputFifo);
            paramDict.TryGetValue("FifoSize", out FifoSize);
            paramDict.TryGetValue("Direction", out Direction);
            paramDict.TryGetValue("Length", out m_Length);
            paramDict.TryGetValue("InterruptSource", out InterruptSource);
            int SR_SIZE = Convert.ToInt16(m_Length);
            int UseInputFifo = Convert.ToInt16(m_UseInputFifo);
            int UseOutputFifo = Convert.ToInt16(m_UseOutputFifo);
            
            UInt32 SrMask;

            #region  SR_Mask

            SrMask = 0;

            for (int j = 0; j < SR_SIZE; j++)
            {
                SrMask = (UInt32)(SrMask | (UInt32)(1 << j));
            }

            paramDict.Add("SR_MASK", SrMask.ToString("X"));
            #endregion


            #region .h

            writer.WriteLine("/***************************************");
            writer.WriteLine("*   Conditional Compilation Parameters  ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_FIFO_SIZE                " + FifoSize + "");
            writer.WriteLine("");
            if((UseInputFifo == 1) || (UseOutputFifo == 1))
            {
               writer.WriteLine("#define " + INSTANCE_NAME + "_FIFO_USED                0x01U"); 
            }
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*        Function Prototypes            ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("void     " + INSTANCE_NAME + "_Start(void);");
            writer.WriteLine("void     " + INSTANCE_NAME + "_Stop(void);");
            writer.WriteLine("void     " + INSTANCE_NAME + "_EnableInt(void);");
            writer.WriteLine("void     " + INSTANCE_NAME + "_DisableInt(void);");
            writer.WriteLine("void     " + INSTANCE_NAME + "_SetIntMode(uint8 interruptSource);");
            writer.WriteLine("uint8    " + INSTANCE_NAME + "_GetIntStatus(void);");

            writer.WriteLine("#ifdef " + INSTANCE_NAME + "_FIFO_USED");
            writer.WriteLine("uint8    " + INSTANCE_NAME + "_GetFIFOStatus(uint8 fifoId);");
            writer.WriteLine("#endif /*" + INSTANCE_NAME + "_FIFO_USED*/");
            if (SR_SIZE <= 8)
            {
                writer.WriteLine("void     " + INSTANCE_NAME + "_WriteRegValue(uint8 txDataByte);");
                writer.WriteLine("uint8    " + INSTANCE_NAME + "_ReadRegValue(void);");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("uint8    " + INSTANCE_NAME + "_WriteData(uint8 txDataByte);");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("uint8    " + INSTANCE_NAME + "_ReadData(void);");
                }
            }
            else if (SR_SIZE <= 16)
            {
                writer.WriteLine("void     " + INSTANCE_NAME + "_WriteRegValue(uint16 txDataByte);");
                writer.WriteLine("uint16   " + INSTANCE_NAME + "_ReadRegValue(void);");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("uint8    " + INSTANCE_NAME + "_WriteData(uint16 txDataByte);");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("uint16   " + INSTANCE_NAME + "_ReadData(void);");
                }
            }
            else if (SR_SIZE == 24)
            {
                writer.WriteLine("void     " + INSTANCE_NAME + "_WriteRegValue(uint32 txDataByte);");
                writer.WriteLine("uint32   " + INSTANCE_NAME + "_ReadRegValue(void);");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("uint8    " + INSTANCE_NAME + "_WriteData(uint32 txDataByte);");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("uint32   " + INSTANCE_NAME + "_ReadData(void);");
                }
            }
            else if (SR_SIZE <= 32)
            {
                writer.WriteLine("void     " + INSTANCE_NAME + "_WriteRegValue(uint32 txDataByte);");
                writer.WriteLine("uint32   " + INSTANCE_NAME + "_ReadRegValue(void);");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("uint8    " + INSTANCE_NAME + "_WriteData(uint32 txDataByte);");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("uint32   " + INSTANCE_NAME + "_ReadData(void);");
                }
            }
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*           API Constants               ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_LOAD                     0x01U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_STORE                    0x02U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RESET                    0x04U");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO                  0x01U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO                 0x02U");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RET_FIFO_FULL            0x00U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RET_FIFO_NOT_EMPTY       0x01U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RET_FIFO_EMPTY           0x02U");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RET_FIFO_NOT_DEFINED     0xFEU");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RET_FIFO_BAD_PARAM       0xFFU");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*    Enumerated Types and Parameters    ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_DIRECTION                " + Direction + "");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_SIZE                  " + m_Length + "");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*    Initial Parameter Constants        ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_MASK                  0x" + SrMask + "U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_INT_SRC                  " + InterruptSource + "");

            writer = new System.IO.StringWriter();

            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*             Registers                 ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("/* Shift Register control register */");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_CONTROL               (* (reg8 *) " + INSTANCE_NAME + "_bSR_CtrlReg__CONTROL_REG) ");
            writer.WriteLine("");
            writer.WriteLine("/* Shift Regisster interupt status register */");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_STATUS                (* (reg8 *) " + INSTANCE_NAME + "_bSR_StsReg__STATUS_REG) ");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_STATUS_MASK           (* (reg8 *) " + INSTANCE_NAME + "_bSR_StsReg__MASK_REG)");
            writer.WriteLine("#define " + INSTANCE_NAME + "_SR_AUX_CONTROL           (* (reg8 *) " + INSTANCE_NAME + "_bSR_StsReg__STATUS_AUX_CTL_REG) ");

            if (UseInputFifo == 1)
            {
                writer.WriteLine("");
                if (SR_SIZE <= 8)
                {
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB          (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__F0_REG )");
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR      ((reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__F0_REG )");
                }
                else if (SR_SIZE <= 16)
                {
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB          (*(reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__F0_REG )");
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR      ((reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__F0_REG )");
                }
                else if (SR_SIZE <= 24)
                {
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB          (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__F0_REG )");
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR      ((reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__F0_REG )");
                }
                else if (SR_SIZE <= 32)
                {
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB          (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__F0_REG )");
                    writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR      ((reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__F0_REG )");
                }
            }

            if (SR_SIZE <= 8)       /* 8bit - ShiftReg */
            {
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR8_AUX_CONTROL          (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__DP_AUX_CTL_REG) ");
                writer.WriteLine("	");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB            (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR        ((reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB      (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR  ((reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_D0_REG_PTR     ((reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__D0_REG)");
                writer.WriteLine("	      ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB         (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__F1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR     ((reg8 *) " + INSTANCE_NAME + "_bSR_sC8_BShiftRegDp_u0__F1_REG )");

            }
            else if (SR_SIZE <= 16)    /* 16bit - ShiftReg */
            {
                writer.WriteLine(" ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR16_AUX_CONTROL1        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR16_AUX_CONTROL2        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u1__DP_AUX_CTL_REG) ");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB            (*(reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR        ((reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB	     (*(reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR	 ((reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_D0_REG_PTR     ((reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__D0_REG )");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB         (*(reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__F1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR     ((reg16 *) " + INSTANCE_NAME + "_bSR_sC16_BShiftRegDp_u0__F1_REG )");

            }
            else if (SR_SIZE <= 24)    /* 24bit - ShiftReg */
            {
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR24_AUX_CONTROL1        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR24_AUX_CONTROL2        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u1__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR24_AUX_CONTROL3        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u2__DP_AUX_CTL_REG) ");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB            (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR	 ((reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB	     (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR        ((reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_D0_REG_PTR     ((reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__D0_REG )");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFOVAL_LSB          (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__F1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR     ((reg32 *) " + INSTANCE_NAME + "_bSR_sC24_BShiftRegDp_u0__F1_REG )");

            }
            else if (SR_SIZE <= 32)    /* 32bit - ShiftReg */
            {
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR32_AUX_CONTROL1        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR32_AUX_CONTROL2        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u1__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR32_AUX_CONTROL3        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u2__DP_AUX_CTL_REG) ");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SR32_AUX_CONTROL4        (*(reg8 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u3__DP_AUX_CTL_REG) ");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB            (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR        ((reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__A0_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB	     (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR	 ((reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__A1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_SHIFT_REG_D0_REG_PTR     ((reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__D0_REG )");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB         (*(reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__F1_REG )");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR     ((reg32 *) " + INSTANCE_NAME + "_bSR_sC32_BShiftRegDp_u0__F1_REG )	");
            }
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*       Register Constants              ");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_INTERRUPTS_ENABLE        0x10U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_LOAD_INT_EN              0x01U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_STORE_INT_EN             0x02U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_RESET_INT_EN             0x04U");
            writer.WriteLine("#define " + INSTANCE_NAME + "_CLK_EN                   0x01U");
            writer.WriteLine("");
			writer.WriteLine("#define " + INSTANCE_NAME + "_RESET_INT_EN_MASK        0xFBU");
            writer.WriteLine("#define " + INSTANCE_NAME + "_LOAD_INT_EN_MASK         0xFEU");
            writer.WriteLine("#define " + INSTANCE_NAME + "_STORE_INT_EN_MASK        0xFDU");
            writer.WriteLine("#define " + INSTANCE_NAME + "_INTS_EN_MASK             0x07U");
            writer.WriteLine("");
            writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT         0x02U");

            if (UseInputFifo == 1)
            {
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_MASK             0x18U");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_FULL             0x00U");
                writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_EMPTY            0x01U");
                writer.WriteLine("#define " + INSTANCE_NAME + "_IN_FIFO_NOT_EMPTY        0x02U");
            }

            writer.WriteLine("");
           
            if (UseOutputFifo == 1)
            {
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_MASK            0x60U");
                writer.WriteLine("");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_EMPTY           0x00U");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_FULL            0x01U");
                writer.WriteLine("#define " + INSTANCE_NAME + "_OUT_FIFO_NOT_EMPTY       0x02U");
            }

            #endregion

            paramDict.Add("writeHfile2", writer.ToString());

            writer = new System.IO.StringWriter();


            #region .c
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_Start");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Starts the Shift Register.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/ ");
            writer.WriteLine("void " + INSTANCE_NAME + "_Start(void)");
            writer.WriteLine("{");
            writer.WriteLine("    " + INSTANCE_NAME + "_SR_CONTROL |= " + INSTANCE_NAME + "_CLK_EN ; ");
            writer.WriteLine("    " + INSTANCE_NAME + "_EnableInt();");
            writer.WriteLine("    " + INSTANCE_NAME + "_SetIntMode(" + INSTANCE_NAME + "_INT_SRC);");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_Stop");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Disables the Shift Register");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + INSTANCE_NAME + "_Stop(void)");
            writer.WriteLine("{");
            writer.WriteLine("    " + INSTANCE_NAME + "_SR_CONTROL &= ~" + INSTANCE_NAME + "_CLK_EN;");
			writer.WriteLine("    " + INSTANCE_NAME + "_DisableInt();");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_EnableInt");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Enables the Shift Register interrupt");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + INSTANCE_NAME + "_EnableInt(void)");
            writer.WriteLine("{");
            writer.WriteLine("    " + INSTANCE_NAME + "_SR_AUX_CONTROL |= " + INSTANCE_NAME + "_INTERRUPTS_ENABLE;");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_DisableInt");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Disables the Shift Register interrupt");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + INSTANCE_NAME + "_DisableInt(void)");
            writer.WriteLine("{");
            writer.WriteLine("    " + INSTANCE_NAME + "_SR_AUX_CONTROL &= ~" + INSTANCE_NAME + "_INTERRUPTS_ENABLE;");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("#ifdef " + INSTANCE_NAME + "_FIFO_USED");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_GetFIFOStatus");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Disables the Shift Register interrupt");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void:  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("uint8 " + INSTANCE_NAME + "_GetFIFOStatus(uint8 fifoId)");
            writer.WriteLine("{");
            writer.WriteLine("    uint8 temp;");
            writer.WriteLine("    uint8 result;");
			writer.WriteLine("");
			writer.WriteLine("    result = " + INSTANCE_NAME + "_RET_FIFO_NOT_DEFINED;");
			writer.WriteLine("    temp = " + INSTANCE_NAME + "_SR_STATUS;");
			writer.WriteLine("");
            if (UseInputFifo == 1)
            {
                writer.WriteLine("");
                writer.WriteLine("    if(fifoId == " + INSTANCE_NAME + "_IN_FIFO)");
                writer.WriteLine("    {");
                writer.WriteLine("        temp = (temp & " + INSTANCE_NAME + "_IN_FIFO_MASK) >> 3;");
                writer.WriteLine("");
                writer.WriteLine("#if(" + INSTANCE_NAME + "_FIFO_SIZE == 1)");
                writer.WriteLine("");
                writer.WriteLine("        switch(temp)");
                writer.WriteLine("        {");
                writer.WriteLine("            case " + INSTANCE_NAME + "_IN_FIFO_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_EMPTY;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            default: result = " + INSTANCE_NAME + "_RET_FIFO_FULL;;");
                writer.WriteLine("        }");
                writer.WriteLine("#endif /* " + INSTANCE_NAME + "_FIFO_SIZE */");				
                writer.WriteLine("");
                writer.WriteLine("#if(" + INSTANCE_NAME + "_FIFO_SIZE == 4)");				
                writer.WriteLine("        switch(temp)");
                writer.WriteLine("        {");
                writer.WriteLine("            case " + INSTANCE_NAME + "_IN_FIFO_FULL :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_FULL;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            case " + INSTANCE_NAME + "_IN_FIFO_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_EMPTY;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            case " + INSTANCE_NAME + "_IN_FIFO_NOT_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_NOT_EMPTY;");
                writer.WriteLine("            break; ");
                writer.WriteLine("");
                writer.WriteLine("            default: result = " + INSTANCE_NAME + "_RET_FIFO_EMPTY;");
                writer.WriteLine("        }");
                writer.WriteLine("");
				writer.WriteLine("#endif /* " + INSTANCE_NAME + "_FIFO_SIZE */");
                writer.WriteLine("    }");
            }
            writer.WriteLine("");
            if (UseOutputFifo == 1)
            {
                writer.WriteLine("");
                writer.WriteLine("    if(fifoId == " + INSTANCE_NAME + "_OUT_FIFO)");
                writer.WriteLine("    {");
                writer.WriteLine("        temp = (temp & " + INSTANCE_NAME + "_OUT_FIFO_MASK) >> 5;");
                writer.WriteLine("");
                writer.WriteLine("#if(" + INSTANCE_NAME + "_FIFO_SIZE == 1)");
                writer.WriteLine("");
                writer.WriteLine("        switch(temp)");
                writer.WriteLine("        {");
                writer.WriteLine("            case " + INSTANCE_NAME + "_OUT_FIFO_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_EMPTY;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            default: result = " + INSTANCE_NAME + "_RET_FIFO_FULL;;");
                writer.WriteLine("        }");
                writer.WriteLine("#endif /* " + INSTANCE_NAME + "_FIFO_SIZE */");
				writer.WriteLine("");
				writer.WriteLine("#if(" + INSTANCE_NAME + "_FIFO_SIZE == 4)");
                writer.WriteLine("        switch(temp)");
                writer.WriteLine("        {");
                writer.WriteLine("            case " + INSTANCE_NAME + "_OUT_FIFO_FULL :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_FULL;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            case " + INSTANCE_NAME + "_OUT_FIFO_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_EMPTY;");
                writer.WriteLine("            break;");
                writer.WriteLine("");
                writer.WriteLine("            case " + INSTANCE_NAME + "_OUT_FIFO_NOT_EMPTY :");
                writer.WriteLine("                result = " + INSTANCE_NAME + "_RET_FIFO_NOT_EMPTY;");
                writer.WriteLine("            break; ");
                writer.WriteLine("");
                writer.WriteLine("            default: result = " + INSTANCE_NAME + "_RET_FIFO_FULL;");
                writer.WriteLine("        }");
                writer.WriteLine("");
                writer.WriteLine("#endif /* " + INSTANCE_NAME + "_FIFO_SIZE */");
                writer.WriteLine("    }");
            }
            writer.WriteLine("	");
            writer.WriteLine("   return (result); ");
            writer.WriteLine("}");
            writer.WriteLine("#endif /*" + INSTANCE_NAME + "_FIFO_USED*/");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_SetInterruptMode");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Sets the Interrupt Source for the Shift Register interrupt. Multiple ");
            writer.WriteLine("*  sources may be ORed together");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  interruptSource: Byte containing the constant for the selected interrupt ");
            writer.WriteLine("*  source/s.  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("void " + INSTANCE_NAME + "_SetIntMode(uint8 interruptSource)");
            writer.WriteLine("{");
            writer.WriteLine("    interruptSource &= " + INSTANCE_NAME + "_INTS_EN_MASK;");
            writer.WriteLine("    " + INSTANCE_NAME + "_SR_STATUS_MASK = (" + INSTANCE_NAME + "_SR_STATUS_MASK  & ~" + INSTANCE_NAME + "_INTS_EN_MASK) | interruptSource; ");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_GetIntStatus");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  Gets the Shift Register Interrupt status.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void  ");
            writer.WriteLine("*");
            writer.WriteLine("* Return: ");
            writer.WriteLine("*  Byte containing the constant for the selected interrupt source/s.");
            writer.WriteLine("*");
            writer.WriteLine("* Theory: ");
            writer.WriteLine("*");
            writer.WriteLine("* Side Effects:");
            writer.WriteLine("*");
            writer.WriteLine("*******************************************************************************/");
            writer.WriteLine("uint8 " + INSTANCE_NAME + "_GetIntStatus(void)");
            writer.WriteLine("{");
            writer.WriteLine("    return(" + INSTANCE_NAME + "_SR_STATUS & " + INSTANCE_NAME + "_INTS_EN_MASK);");
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
            if (SR_SIZE <= 8)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Send state directly to shift register");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  txDataByte: containing shift register state. ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + INSTANCE_NAME + "_WriteRegValue(uint8 txDataByte)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG8(" + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR, txDataByte);  ");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Directly returns current state in shift register, not data in FIFO due ");
                writer.WriteLine("*  to Store input.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  Shift Register state");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  Clears output FIFO. ");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("uint8 " + INSTANCE_NAME + "_ReadRegValue(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint8 result;");
                writer.WriteLine("	");
                writer.WriteLine("    /* Clear FIFO before software capture */");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR8_AUX_CONTROL |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR8_AUX_CONTROL &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("	  ");
                writer.WriteLine("	  /* Capture A1 to output FIFO */");
                writer.WriteLine("    result = CY_GET_REG8(" + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR);");
                writer.WriteLine("    ");
                writer.WriteLine("	  /* Read output FIFO */");
                writer.WriteLine("    result = CY_GET_REG8(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR); ");
                writer.WriteLine("	");
                writer.WriteLine("    if(" + INSTANCE_NAME + "_SR_SIZE != 8)");
                writer.WriteLine("    {");
                writer.WriteLine("        result = result & " + INSTANCE_NAME + "_SR_MASK;");
                writer.WriteLine("	  }");
                writer.WriteLine("    return(result);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Send state to FIFO for later transfer to shift register based on the Load ");
                    writer.WriteLine("*  input");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  txDataByte: containing shift register state. ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Indicates: successful execution of function when FIFO is empty; and error when");
                    writer.WriteLine("*  FIFO is full.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint8 " + INSTANCE_NAME + "_WriteData(uint8 txDataByte)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 result;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    result = CYRET_INVALID_STATE;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    if((" + INSTANCE_NAME + "_GetFIFOStatus(" + INSTANCE_NAME + "_IN_FIFO)) != " + INSTANCE_NAME + "_RET_FIFO_FULL)");
                    writer.WriteLine("    {");
                    writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR, txDataByte);");
                    writer.WriteLine("        result = CYRET_SUCCESS;");
                    writer.WriteLine("    }");
                    writer.WriteLine("    return(result);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Returns state in FIFO due to Store input.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  void ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Shift Register state");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint8 " + INSTANCE_NAME + "_ReadData(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    return(CY_GET_REG8(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR));");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
            }
            else if (SR_SIZE <= 16)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Send state directly to shift register");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  txDataByte: containing shift register state. ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + INSTANCE_NAME + "_WriteRegValue(uint16 txDataByte)");
                writer.WriteLine("{");
                writer.WriteLine("   CY_SET_REG16(" + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR, txDataByte);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Directly rreturns current state in shift register, not data in FIFO due ");
                writer.WriteLine("*  to Store input.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  Shift Register state");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  Clears output FIFO. ");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("uint16 " + INSTANCE_NAME + "_ReadRegValue(void)");
                writer.WriteLine("{");
                writer.WriteLine("   uint16 result;");
                writer.WriteLine("   ");
                writer.WriteLine("    /* Clear FIFO before software capture */");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR16_AUX_CONTROL1 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR16_AUX_CONTROL1 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR16_AUX_CONTROL2 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR16_AUX_CONTROL2 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("	");
                writer.WriteLine("    /* Capture A1 to output FIFO */");
                writer.WriteLine("    result = CY_GET_REG16(" + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR);");
                writer.WriteLine("   ");
                writer.WriteLine("    /* Read output FIFO */");
                writer.WriteLine("    result = CY_GET_REG16(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR); ");
                writer.WriteLine("   ");
                writer.WriteLine("    if(" + INSTANCE_NAME + "_SR_SIZE != 16)");
                writer.WriteLine("    {");
                writer.WriteLine("        result = result & " + INSTANCE_NAME + "_SR_MASK;");
                writer.WriteLine("    }");
                writer.WriteLine("    return(result);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Send state to FIFO for later transfer to shift register based on the Load ");
                    writer.WriteLine("*  input");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  txDataByte: containing shift register state. ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Indicates: successful execution of function when FIFO is empty; and error when");
                    writer.WriteLine("*  FIFO is full.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*  Clears output FIFO. ");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint8 " + INSTANCE_NAME + "_WriteData(uint16 txDataByte)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 result;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    result = CYRET_INVALID_STATE;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    if((" + INSTANCE_NAME + "_GetFIFOStatus(" + INSTANCE_NAME + "_IN_FIFO)) != " + INSTANCE_NAME + "_RET_FIFO_FULL)");
                    writer.WriteLine("    {");
                    writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR, txDataByte);");
                    writer.WriteLine("        result = CYRET_SUCCESS;");
                    writer.WriteLine("    }");
                    writer.WriteLine("    return(result);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Returns state in FIFO due to Store input.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  void ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Shift Register state");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint16 " + INSTANCE_NAME + "_ReadData(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    return( CY_GET_REG16(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR));");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
            }
            else if (SR_SIZE <= 24)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Send state directly to shift register");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  txDataByte: containing shift register state. ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + INSTANCE_NAME + "_WriteRegValue(uint32 txDataByte)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG24(" + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR, txDataByte); ");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Directly rreturns current state in shift register, not data in FIFO due ");
                writer.WriteLine("*  to Store input.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  Shift Register state");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  Clears output FIFO. ");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadRegValue(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint32 result;");
                writer.WriteLine("	");
                writer.WriteLine("    /* Clear FIFO before software capture */");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR24_AUX_CONTROL1 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR24_AUX_CONTROL1 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR24_AUX_CONTROL2 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR24_AUX_CONTROL2 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("	  " + INSTANCE_NAME + "_SR24_AUX_CONTROL3 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR24_AUX_CONTROL3 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("	");
                writer.WriteLine("    /* Capture A1 to output FIFO */	");
                writer.WriteLine("    result = CY_GET_REG24(" + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR);");
                writer.WriteLine("          ");
                writer.WriteLine("    result = CY_GET_REG24(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR); ");
                writer.WriteLine("    ");
                writer.WriteLine("    if(" + INSTANCE_NAME + "_SR_SIZE != 24)");
                writer.WriteLine("    {");
                writer.WriteLine("        result = result & " + INSTANCE_NAME + "_SR_MASK;");
                writer.WriteLine("	  }");
                writer.WriteLine("		");
                writer.WriteLine("    return result;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Send state to FIFO for later transfer to shift register based on the Load ");
                    writer.WriteLine("*  input");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  txDataByte: containing shift register state. ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Indicates: successful execution of function when FIFO is empty; and error");
                    writer.WriteLine("*  when FIFO is full.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint8 " + INSTANCE_NAME + "_WriteData(uint32 txDataByte)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 result;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    result = CYRET_INVALID_STATE;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    if((" + INSTANCE_NAME + "_GetFIFOStatus(" + INSTANCE_NAME + "_IN_FIFO)) != " + INSTANCE_NAME + "_RET_FIFO_FULL)");
                    writer.WriteLine("    {");
                    writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR, txDataByte);");
                    writer.WriteLine("        result = CYRET_SUCCESS;");
                    writer.WriteLine("    }");
                    writer.WriteLine("    return(result);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*   Returns state in FIFO due to Store input.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  void ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Shift Register state");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadData(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    return( CY_GET_REG24(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR) );");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
            }
            else if (SR_SIZE <= 32)
            {
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Send state directly to shift register");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  txDataByte: containing shift register state. ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("void " + INSTANCE_NAME + "_WriteRegValue(uint32 txDataByte)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG32(" + INSTANCE_NAME + "_SHIFT_REG_LSB_PTR, txDataByte);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadRegValue");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Directly rreturns current state in shift register, not data in FIFO due ");
                writer.WriteLine("*  to Store input.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void ");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  Shift Register state");
                writer.WriteLine("*");
                writer.WriteLine("* Theory: ");
                writer.WriteLine("*");
                writer.WriteLine("* Side Effects:");
                writer.WriteLine("*  Clears output FIFO. ");
                writer.WriteLine("*");
                writer.WriteLine("*******************************************************************************/");
                writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadRegValue(void)");
                writer.WriteLine("{");
                writer.WriteLine("    uint32 result;");
                writer.WriteLine("	");
                writer.WriteLine("	/* Clear FIFO before software capture */");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL1 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL1 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL2 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL2 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL3 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL3 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL4 |= " + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT; ");
                writer.WriteLine("    " + INSTANCE_NAME + "_SR32_AUX_CONTROL4 &= ~" + INSTANCE_NAME + "_OUT_FIFO_CLR_BIT;");
                writer.WriteLine("	");
                writer.WriteLine("    /* Capture A1 to output FIFO */	");
                writer.WriteLine("	  result = CY_GET_REG32(" + INSTANCE_NAME + "_SHIFT_REG_VALUE_LSB_PTR);");
                writer.WriteLine("	");
                writer.WriteLine("    result = CY_GET_REG32(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR); ");
                writer.WriteLine("	");
                writer.WriteLine("    if(" + INSTANCE_NAME + "_SR_SIZE != 32)");
                writer.WriteLine("	  {");
                writer.WriteLine("	      result = result & " + INSTANCE_NAME + "_SR_MASK;");
                writer.WriteLine("    }");
                writer.WriteLine("");
                writer.WriteLine("    return result;");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                if (UseInputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_WriteData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Send state to FIFO for later transfer to shift register based on the Load ");
                    writer.WriteLine("*  input");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  txDataByte: containing shift register state. ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Indicates: successful execution of function when FIFO is empty; and error");
                    writer.WriteLine("*  when FIFO is full.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint8 " + INSTANCE_NAME + "_WriteData(uint32 txDataByte)");
                    writer.WriteLine("{");
                    writer.WriteLine("    uint8 result;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    result = CYRET_INVALID_STATE;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    if((" + INSTANCE_NAME + "_GetFIFOStatus(" + INSTANCE_NAME + "_IN_FIFO)) != " + INSTANCE_NAME + "_RET_FIFO_FULL)");
                    writer.WriteLine("    {");
                    writer.WriteLine("	      CY_SET_REG32(" + INSTANCE_NAME + "_IN_FIFO_VAL_LSB_PTR, txDataByte);");
                    writer.WriteLine("        result = CYRET_SUCCESS;");
                    writer.WriteLine("    }");
                    writer.WriteLine("    return(result);");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                }
                
                if (UseOutputFifo == 1)
                {
                    writer.WriteLine("/*******************************************************************************");
                    writer.WriteLine("* Function Name: " + INSTANCE_NAME + "_ReadData");
                    writer.WriteLine("********************************************************************************");
                    writer.WriteLine("*");
                    writer.WriteLine("* Summary:");
                    writer.WriteLine("*  Returns state in FIFO due to Store input.");
                    writer.WriteLine("*");
                    writer.WriteLine("* Parameters:  ");
                    writer.WriteLine("*  void ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Return: ");
                    writer.WriteLine("*  Shift Register state");
                    writer.WriteLine("*");
                    writer.WriteLine("* Theory: ");
                    writer.WriteLine("*");
                    writer.WriteLine("* Side Effects:");
                    writer.WriteLine("*");
                    writer.WriteLine("*******************************************************************************/");
                    writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadData(void)");
                    writer.WriteLine("{");
                    writer.WriteLine("    return(CY_GET_REG32(" + INSTANCE_NAME + "_OUT_FIFO_VAL_LSB_PTR));");
                    writer.WriteLine("}");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    }
                  }
            
            #endregion

            paramDict.Add("writeCfile", writer.ToString());
        }
    }
}
           