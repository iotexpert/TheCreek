/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace PrISM_v1_10
{
     partial class CyCustomizer : ICyAPICustomize_v1
    {    
        #region ICyAPICustomize_v1 Members
         static string CS_CFile_Name = "PrISM.c";
         static string CS_HFile_Name = "PrISM.h";
         public const string m_instanceNameParam = "INSTANCE_NAME";
         string m_instanceName = "";

         CyAPICustomizer m_CS_CFile;
         CyAPICustomizer m_CS_HFile;

         public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
         {
             //Debug.Assert(false);
             List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
             Dictionary<string, string> paramDict = new Dictionary<string, string>();
             
             // Get the parameters from the .c file customizer
             foreach (CyAPICustomizer api in customizers)
             {
                 // Get dict from main file. 
                 if (api.OriginalName.EndsWith(CS_CFile_Name))
                 {
                     m_CS_CFile = api;
                     paramDict = api.MacroDictionary;
                     paramDict.TryGetValue(m_instanceNameParam, out m_instanceName);
                 }
                 else if (api.OriginalName.EndsWith(CS_HFile_Name))
                 {
                     m_CS_HFile = api;
                 }
             }
             GenerateAPIFiles(ref paramDict);

             //If No Data in Main object than no data will be generate
             foreach (CyAPICustomizer api in customizers)
             {
                 api.MacroDictionary = paramDict;
             }
           
             return customizers;
         }
         public void GenerateAPIFiles(ref Dictionary<string, string> paramDict)
         {
            StringWriter writer = new StringWriter();

            string INSTANCE_NAME;
            string VERSION_MAJOR;
            string VERSION_MINOR;
            string Resolution;
            string PolyValue;
            string SeedValue;
            string Density0;
            string Density1;
            string CompareType0;
            string CompareType1;
            string PulseTypeHardcoded;
			
            paramDict.TryGetValue("INSTANCE_NAME", out INSTANCE_NAME);
            paramDict.TryGetValue("CY_MAJOR_VERSION", out VERSION_MAJOR);
            paramDict.TryGetValue("CY_MINOR_VERSION", out VERSION_MINOR);
            paramDict.TryGetValue("Resolution", out Resolution);
            paramDict.TryGetValue("PolyValue", out PolyValue);
            paramDict.TryGetValue("SeedValue", out SeedValue);
            paramDict.TryGetValue("Density0", out Density0);
            paramDict.TryGetValue("Density1", out Density1);
            paramDict.TryGetValue("CompareType0", out CompareType0);
            paramDict.TryGetValue("CompareType1", out CompareType1);
            paramDict.TryGetValue("PulseTypeHardcoded", out PulseTypeHardcoded);

            int PrISM_Resolution = int.Parse(Resolution);
            int PrISM_PulseTypeHardcoded = int.Parse(PulseTypeHardcoded);
			
            #region File .h
            writer.WriteLine("/***************************************");
            writer.WriteLine("*       Function Prototypes");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("void  "+INSTANCE_NAME+"_Start(void);");
            writer.WriteLine("void  "+INSTANCE_NAME+"_Stop(void);");
            writer.WriteLine("void  "+INSTANCE_NAME+"_SetPulse0Mode(uint8 pulse0Type);");
            writer.WriteLine("void  "+INSTANCE_NAME+"_SetPulse1Mode(uint8 pulse1Type);");
            if (PrISM_Resolution <= 8)    	/* 8bit - PrISM */
            {
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadSeed(void);");
                writer.WriteLine("void  "+INSTANCE_NAME+"_WriteSeed(uint8 seed);");
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadPolynomial(void);");
                writer.WriteLine("void  "+INSTANCE_NAME+"_WritePolynomial(uint8 polynomial);");
                writer.WriteLine("uint8 " + INSTANCE_NAME + "_ReadPusle0(void);");
                writer.WriteLine("void  "+INSTANCE_NAME+"_WritePulse0(uint8 pulseDesity0);");
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadPusle1(void);");
                writer.WriteLine("void  "+INSTANCE_NAME+"_WritePulse1(uint8 pulseDesity1);");
            } 
            else if (PrISM_Resolution <= 16)    /* 16bit - PrISM */
            {
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadSeed(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WriteSeed(uint16 seed);");
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadPolynomial(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePolynomial(uint16 polynomial);");
                writer.WriteLine("uint16 " + INSTANCE_NAME + "_ReadPusle0(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse0(uint16 pulseDesity0);");
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadPusle1(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse1(uint16 pulseDesity1);");
            } 
            else if (PrISM_Resolution <= 24)    /* 24bit - PrISM */
            {
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadSeed(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WriteSeed(uint32 seed);");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPolynomial(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePolynomial(uint32 polynomial);");
                writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadPusle0(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse0(uint32 pulseDesity0);");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle1(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse1(uint32 pulseDesity1);");
            } 
            else  										/* 32bit - PrISM */
            {
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadSeed(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WriteSeed(uint32 seed);");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPolynomial(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePolynomial(uint32 polynomial);");
                writer.WriteLine("uint32 " + INSTANCE_NAME + "_ReadPusle0(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse0(uint32 pulseDesity0);");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle1(void);");
                writer.WriteLine("void   "+INSTANCE_NAME+"_WritePulse1(uint32 pulseDesity1);");
            }
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*          API Constants");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("/* Constants for SetPulse0Mode(), SetPulse1Mode(), pulse type */");
            writer.WriteLine("#define "+INSTANCE_NAME+"_LESSTHAN_OR_EQUAL       0x00u");
            writer.WriteLine("#define "+INSTANCE_NAME+"_GREATERTHAN_OR_EQUAL    0x01u");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*    Initial Parameter Constants");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define "+INSTANCE_NAME+"_PrISM_Resolution        "+Resolution+"");
            writer.WriteLine("#define "+INSTANCE_NAME+"_POLYNOM                 "+PolyValue+"");
            writer.WriteLine("#define "+INSTANCE_NAME+"_SEED                    "+SeedValue+"");
            writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY0                "+Density0+"");
            writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY1                "+Density1+"");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*              Registers");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
             if (PrISM_Resolution  <= 8)    	/* 8bit - PrISM */
             {
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY0_PTR            ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__D0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY1_PTR            ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__D1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_POLYNOM_PTR             ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__A1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_PTR                ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__A0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_COPY_PTR           ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__F0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR         ((reg8 *) "+INSTANCE_NAME+"_sC8_PrISMdp_u0__DP_AUX_CTL_REG)");
             } 
             else if (PrISM_Resolution <= 16)    /* 16bit - PrISM */
             {
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY0_PTR            ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__D0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY1_PTR            ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__D1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_POLYNOM_PTR             ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__A1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_PTR                ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__A0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_COPY_PTR           ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__F0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR         ((reg16 *) "+INSTANCE_NAME+"_sC16_PrISMdp_u0__DP_AUX_CTL_REG)");
             } 
             else if (PrISM_Resolution <= 24)   		 /* 24bit - PrISM */
             {
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY0_PTR            ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__D0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY1_PTR            ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__D1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_POLYNOM_PTR             ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__A1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_PTR                ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__A0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_COPY_PTR           ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__F0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR         ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u0__DP_AUX_CTL_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR2        ((reg32 *) "+INSTANCE_NAME+"_sC24_PrISMdp_u2__DP_AUX_CTL_REG)");
             } 
             else  										/* 32bit - PrISM */
             {
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY0_PTR            ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__D0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_DENSITY1_PTR            ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__D1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_POLYNOM_PTR             ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__A1_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_PTR                ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__A0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_SEED_COPY_PTR           ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__F0_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR         ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u0__DP_AUX_CTL_REG)");
                writer.WriteLine("#define "+INSTANCE_NAME+"_AUX_CONTROL_PTR2        ((reg32 *) "+INSTANCE_NAME+"_sC32_PrISMdp_u2__DP_AUX_CTL_REG)");
             }
            writer.WriteLine("");
            writer.WriteLine("#define "+INSTANCE_NAME+"_CONTROL                 (* (reg8 *) "+INSTANCE_NAME+"_ctrl_CtrlReg__CONTROL_REG )");
            writer.WriteLine("#define "+INSTANCE_NAME+"_CONTROL_PTR             ( (reg8 *) "+INSTANCE_NAME+"_ctrl_CtrlReg__CONTROL_REG )");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("*       Register Constants");
            writer.WriteLine("***************************************/");
            writer.WriteLine("");
            writer.WriteLine("#define "+INSTANCE_NAME+"_CTRL_ENABLE                                 0x01u");
            writer.WriteLine("#define "+INSTANCE_NAME+"_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL    0x02u");
            writer.WriteLine("#define "+INSTANCE_NAME+"_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL    0x04u");
            writer.WriteLine("");
            writer.WriteLine("#define "+INSTANCE_NAME+"_FIFO0_CLR                                   0x01u");
            #endregion 
            paramDict.Add("DefineH", writer.ToString());
            writer = new StringWriter();

            #region File .h
            writer.WriteLine("/*******************************************************************************");
            writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_Start");
            writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
            writer.WriteLine("* Summary:");
            writer.WriteLine("*  The start function sets Polynomial, Seed and Pulse Density registers provided");
            writer.WriteLine("*  by customizer. PrISM computation starts on rising edge of input clock.");
            writer.WriteLine("*");
            writer.WriteLine("* Parameters:  ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("* Return:  ");
            writer.WriteLine("*  void");
            writer.WriteLine("*");
            writer.WriteLine("**********************************************************************************/");
            writer.WriteLine("void "+INSTANCE_NAME+"_Start(void) ");
            writer.WriteLine("{");
            writer.WriteLine("    #if !defined(" + INSTANCE_NAME + "_FIRSTTIME)   /* " + INSTANCE_NAME + "_FIRSTTIME */");
            writer.WriteLine("    {");
            writer.WriteLine("       #define " + INSTANCE_NAME + "_FIRSTIME");
            writer.WriteLine("       /* Writes Seed value, polynom value and density provided by customizer */");
            if (PrISM_Resolution <= 8)    	/* 8bit - PrISM */
            {
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_AUX_CONTROL_PTR," + INSTANCE_NAME + "_FIFO0_CLR);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_SEED_COPY_PTR," + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_SEED_PTR,    " + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_POLYNOM_PTR, " + INSTANCE_NAME + "_POLYNOM);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_DENSITY0_PTR," + INSTANCE_NAME + "_DENSITY0);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_DENSITY1_PTR," + INSTANCE_NAME + "_DENSITY1);");
            }
            else if (PrISM_Resolution <= 16)    /* 16bit - PrISM */
            {
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_AUX_CONTROL_PTR," + INSTANCE_NAME + "_FIFO0_CLR | ");
                writer.WriteLine("                                                  " + INSTANCE_NAME + "_FIFO0_CLR << 8);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_SEED_COPY_PTR," + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_SEED_PTR   , " + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_POLYNOM_PTR, " + INSTANCE_NAME + "_POLYNOM);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_DENSITY0_PTR," + INSTANCE_NAME + "_DENSITY0);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_DENSITY1_PTR," + INSTANCE_NAME + "_DENSITY1);");
            }
            else if (PrISM_Resolution <= 24)    /* 24bit - PrISM */
            {
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_AUX_CONTROL_PTR," + INSTANCE_NAME + "_FIFO0_CLR | ");
                writer.WriteLine("                                                  " + INSTANCE_NAME + "_FIFO0_CLR << 8 );");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_AUX_CONTROL_PTR2," + INSTANCE_NAME + "_FIFO0_CLR );");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_SEED_COPY_PTR," + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_SEED_PTR   , " + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_POLYNOM_PTR, " + INSTANCE_NAME + "_POLYNOM);");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_DENSITY0_PTR," + INSTANCE_NAME + "_DENSITY0);");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_DENSITY1_PTR," + INSTANCE_NAME + "_DENSITY1);");
            }
            else  										/* 32bit - PrISM */
            {
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_AUX_CONTROL_PTR," + INSTANCE_NAME + "_FIFO0_CLR | ");
                writer.WriteLine("                                                  " + INSTANCE_NAME + "_FIFO0_CLR << 8 );");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_AUX_CONTROL_PTR2," + INSTANCE_NAME + "_FIFO0_CLR | ");
                writer.WriteLine("                                                  " + INSTANCE_NAME + "_FIFO0_CLR << 8 );");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_SEED_COPY_PTR," + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_SEED_PTR   , " + INSTANCE_NAME + "_SEED);");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_POLYNOM_PTR, " + INSTANCE_NAME + "_POLYNOM);");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_DENSITY0_PTR," + INSTANCE_NAME + "_DENSITY0);");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_DENSITY1_PTR," + INSTANCE_NAME + "_DENSITY1);");
            }
            if(PrISM_PulseTypeHardcoded == 0)
			{
                writer.WriteLine("");
                writer.WriteLine("        /* Writes density type provided by customizer */");
				writer.WriteLine("        if(" + INSTANCE_NAME + "_GREATERTHAN_OR_EQUAL == " + CompareType0 + " )");
				writer.WriteLine("        {");
				writer.WriteLine("            " + INSTANCE_NAME + "_CONTROL |= " + INSTANCE_NAME + "_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("        }");
				writer.WriteLine("        else");
				writer.WriteLine("        {");
				writer.WriteLine("            " + INSTANCE_NAME + "_CONTROL &= ~" + INSTANCE_NAME + "_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("        }");
				writer.WriteLine("");
				writer.WriteLine("        if(" + INSTANCE_NAME + "_GREATERTHAN_OR_EQUAL == " + CompareType1 + ")");
				writer.WriteLine("        {");
				writer.WriteLine("            " + INSTANCE_NAME + "_CONTROL |= " + INSTANCE_NAME + "_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("        }");
				writer.WriteLine("        else");
				writer.WriteLine("        {");
				writer.WriteLine("            " + INSTANCE_NAME + "_CONTROL &= ~" + INSTANCE_NAME + "_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("        }");
			}
            writer.WriteLine("    }");
            writer.WriteLine("    #endif    /* End " + INSTANCE_NAME + "_FIRSTTIME */");
            if(PrISM_PulseTypeHardcoded == 0)
			{
				writer.WriteLine("");
				writer.WriteLine("    /* Enable the PrISM computation */");
				writer.WriteLine("    " + INSTANCE_NAME + "_CONTROL |= " + INSTANCE_NAME + "_CTRL_ENABLE;");
			}
            writer.WriteLine("}");
            writer.WriteLine("");
            writer.WriteLine("");
			writer.WriteLine("/*******************************************************************************");
			writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_Stop");
			writer.WriteLine("********************************************************************************");
            writer.WriteLine("*");
			writer.WriteLine("* Summary:");
			writer.WriteLine("*  Stops PrISM computation. Outputs remain constant.");
			writer.WriteLine("*");
			writer.WriteLine("* Parameters:  ");
			writer.WriteLine("*  void");
			writer.WriteLine("*");
			writer.WriteLine("* Return: ");
			writer.WriteLine("*  void");
			writer.WriteLine("*");
			writer.WriteLine("**********************************************************************************/");
			writer.WriteLine("void "+INSTANCE_NAME+"_Stop(void)");
			writer.WriteLine("{");
            if(PrISM_PulseTypeHardcoded == 0)
            {
			    writer.WriteLine("    " + INSTANCE_NAME + "_CONTROL &= ~" + INSTANCE_NAME + "_CTRL_ENABLE;");
            }
            else
            {
			    writer.WriteLine("    /* PulseTypeHardoded option enabled - to stop PrISM use enable input */");
            }
			writer.WriteLine("}");
            if(PrISM_PulseTypeHardcoded == 0)
			{
			    writer.WriteLine("");
			    writer.WriteLine("");
				writer.WriteLine("/*******************************************************************************");
				writer.WriteLine("* FUNCTION NAME:     "+INSTANCE_NAME+"_SetPulse0Mode");
				writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
				writer.WriteLine("* Summary:");
				writer.WriteLine("*  Sets the pulse density type for Density0. Less than or Equal(<=) or ");
				writer.WriteLine("*  Greater that or Equal(>=).");
				writer.WriteLine("*");
				writer.WriteLine("* Parameters:");
				writer.WriteLine("*  pulse0Type: Selected pulse density type.");
				writer.WriteLine("*");
				writer.WriteLine("* Return:");
				writer.WriteLine("*  void");
				writer.WriteLine("* ");
				writer.WriteLine("**********************************************************************************/");
				writer.WriteLine("void "+INSTANCE_NAME+"_SetPulse0Mode(uint8 pulse0Type)");
				writer.WriteLine("{");
				writer.WriteLine("    if(pulse0Type == " + INSTANCE_NAME + "_GREATERTHAN_OR_EQUAL)");
				writer.WriteLine("    {");
				writer.WriteLine("        " + INSTANCE_NAME + "_CONTROL |= " + INSTANCE_NAME + "_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("    }");
				writer.WriteLine("    else");
				writer.WriteLine("    {");
				writer.WriteLine("        " + INSTANCE_NAME + "_CONTROL &= ~" + INSTANCE_NAME + "_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("    }");
				writer.WriteLine("}");
				writer.WriteLine("");
				writer.WriteLine("");
				writer.WriteLine("/*******************************************************************************");
				writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_SetPulse1Mode");
				writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
				writer.WriteLine("* Summary:");
				writer.WriteLine("*  Sets the pulse density type for Density1. Less than or Equal(<=) or ");
				writer.WriteLine("*  Greater that or Equal(>=).");
				writer.WriteLine("*");
				writer.WriteLine("* Parameters:  ");
				writer.WriteLine("*  pulse1Type: Selected pulse density type.");
				writer.WriteLine("*");
				writer.WriteLine("* Return:");
				writer.WriteLine("*  void");
				writer.WriteLine("* ");
				writer.WriteLine("**********************************************************************************/");
				writer.WriteLine("void "+INSTANCE_NAME+"_SetPulse1Mode(uint8 pulse1Type)");
				writer.WriteLine("{");
				writer.WriteLine("    if(pulse1Type == " + INSTANCE_NAME + "_GREATERTHAN_OR_EQUAL)");
				writer.WriteLine("    {");
				writer.WriteLine("        " + INSTANCE_NAME + "_CONTROL |= " + INSTANCE_NAME + "_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("    }");
				writer.WriteLine("    else");
				writer.WriteLine("    {");
				writer.WriteLine("        " + INSTANCE_NAME + "_CONTROL &= ~" + INSTANCE_NAME + "_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL;");
				writer.WriteLine("    }");
				writer.WriteLine("}");
			}
            if (PrISM_Resolution <= 8)    /* 8bit - PrISM */
            {
				writer.WriteLine("");
				writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Seed register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:  ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Current Period register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadSeed(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG8(" + INSTANCE_NAME + "_SEED_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:  "+INSTANCE_NAME+"_WriteSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Seed register with the start value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  seed: Seed register value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:     ");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WriteSeed(uint8 seed)");
                writer.WriteLine("{");
                writer.WriteLine("    if(seed != 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_SEED_COPY_PTR, seed);");
                writer.WriteLine("        CY_SET_REG8(" + INSTANCE_NAME + "_SEED_PTR     , seed);");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_ReadPolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG8(" + m_instanceName + "_POLYNOM_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:  " + m_instanceName + "_WritePolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  polynomial: PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint8 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG8(" + m_instanceName + "_POLYNOM_PTR, polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:  "+INSTANCE_NAME+"_ReadPusle0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density0 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density0 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadPusle0(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG8(" + INSTANCE_NAME + "_DENSITY0_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density0 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity0: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse0(uint8 pulseDesity0)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG8(" + INSTANCE_NAME + "_DENSITY0_PTR, pulseDesity0);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density1 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return: ");
                writer.WriteLine("*  Pulse Density1 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint8 "+INSTANCE_NAME+"_ReadPusle1(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG8(" + INSTANCE_NAME + "_DENSITY1_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density1 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity1: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse1(uint8 pulseDesity1)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG8(" + INSTANCE_NAME + "_DENSITY1_PTR, pulseDesity1);");
                writer.WriteLine("}");
            }
            else if (PrISM_Resolution <= 16)    /* 16bit - PrISM */
            {
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:  "+INSTANCE_NAME+"_ReadSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Seed register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Current Period register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadSeed(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG16(" + INSTANCE_NAME + "_SEED_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WriteSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Seed register with the start value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  seed: Seed register value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WriteSeed(uint16 seed)");
                writer.WriteLine("{");
                writer.WriteLine("    if(seed != 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_SEED_COPY_PTR, seed);");
                writer.WriteLine("        CY_SET_REG16(" + INSTANCE_NAME + "_SEED_PTR     , seed);");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_ReadPolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG16(" + m_instanceName + "_POLYNOM_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_WritePolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  polynomial: PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint16 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG16(" + m_instanceName + "_POLYNOM_PTR, polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density0 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density0 register value.");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadPusle0(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG16(" + INSTANCE_NAME + "_DENSITY0_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density0 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity0: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("* ");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse0(uint16 pulseDesity0)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG16(" + INSTANCE_NAME + "_DENSITY0_PTR, pulseDesity0);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density1 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density1 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint16 "+INSTANCE_NAME+"_ReadPusle1(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return (CY_GET_REG16(" + INSTANCE_NAME + "_DENSITY1_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density1 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity1: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse1(uint16 pulseDesity1)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG16(" + INSTANCE_NAME + "_DENSITY1_PTR, pulseDesity1);");
                writer.WriteLine("}");
            }
            else if (PrISM_Resolution <= 24)    /* 24bit - PrISM */
            {
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Seed register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Current Period register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadSeed(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG24(" + INSTANCE_NAME + "_SEED_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WriteSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Seed register with the start value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  seed: Seed register value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("    if(seed != 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_SEED_COPY_PTR, seed);");
                writer.WriteLine("        CY_SET_REG24(" + INSTANCE_NAME + "_SEED_PTR     , seed);");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_ReadPolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG24(" + m_instanceName + "_POLYNOM_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_WritePolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  polynomial: PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("      CY_SET_REG24(" + m_instanceName + "_POLYNOM_PTR, polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density0 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density0 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle0(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG24(" + INSTANCE_NAME + "_DENSITY0_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME: void "+INSTANCE_NAME+"_WritePulse0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density0 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity0: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse0(uint32 pulseDesity0)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG24(" + INSTANCE_NAME + "_DENSITY0_PTR, pulseDesity0);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density1 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density1 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle1(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG24(" + INSTANCE_NAME + "_DENSITY1_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density1 register with the Pulse Density value. ");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity1: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse1(uint32 pulseDesity1)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG24(" + INSTANCE_NAME + "_DENSITY1_PTR, pulseDesity1);");
                writer.WriteLine("}");
            }
            else    /* 32bit - PrISM */
            {
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Seed register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Current Period register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadSeed(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG32(" + INSTANCE_NAME + "_SEED_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WriteSeed");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Seed register with the start value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  seed: Seed register value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("    if(seed != 0)");
                writer.WriteLine("    {");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_SEED_COPY_PTR, seed);");
                writer.WriteLine("        CY_SET_REG32(" + INSTANCE_NAME + "_SEED_PTR     , seed);");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_ReadPolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG32(" + m_instanceName + "_POLYNOM_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   " + m_instanceName + "_WritePolynomial");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  polynomial: PrISM polynomial.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG32(" + m_instanceName + "_POLYNOM_PTR, polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density0 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density0 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle0(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG32(" + INSTANCE_NAME + "_DENSITY0_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse0");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density0 register with the Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity0: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse0(uint32 pulseDesity0)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG32(" + INSTANCE_NAME + "_DENSITY0_PTR, pulseDesity0);");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_ReadPusle1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Reads the PrISM Pulse Density1 register.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  Pulse Density1 register value.");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("uint32 "+INSTANCE_NAME+"_ReadPusle1(void)");
                writer.WriteLine("{");
                writer.WriteLine("    return( CY_GET_REG32(" + INSTANCE_NAME + "_DENSITY1_PTR) );");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("/*******************************************************************************");
                writer.WriteLine("* FUNCTION NAME:   "+INSTANCE_NAME+"_WritePulse1");
                writer.WriteLine("********************************************************************************");
                writer.WriteLine("*");
                writer.WriteLine("* Summary:");
                writer.WriteLine("*  Writes the PrISM Pulse Density1 register with the Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Parameters:");
                writer.WriteLine("*  pulseDesity1: Pulse Density value.");
                writer.WriteLine("*");
                writer.WriteLine("* Return:");
                writer.WriteLine("*  void");
                writer.WriteLine("*");
                writer.WriteLine("**********************************************************************************/");
                writer.WriteLine("void "+INSTANCE_NAME+"_WritePulse1(uint32 pulseDesity1)");
                writer.WriteLine("{");
                writer.WriteLine("    CY_SET_REG32(" + INSTANCE_NAME + "_DENSITY1_PTR, pulseDesity1);");
                writer.WriteLine("}");
            }
            #endregion 
            writer.WriteLine("");
            paramDict.Add("DefineC", writer.ToString());
        }
        #endregion

    }
}
