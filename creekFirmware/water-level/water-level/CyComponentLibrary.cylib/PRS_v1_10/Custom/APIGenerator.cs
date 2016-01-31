/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CyCustomizer.PRS_v1_10
{
   public partial class CyCustomizer
    {
       public void GenerateHFile(ref Dictionary<string, string> paramDict)
       {
           StringWriter writer = new StringWriter();
           string sRunMode;
           string sPRSSize;
           string PolyValueLower;
           string PolyValueUpper;
           string SeedValueUpper;
           string SeedValueLower;

           paramDict.TryGetValue("RunMode", out sRunMode);
           paramDict.TryGetValue("Resolution", out sPRSSize);
           paramDict.TryGetValue("PolyValueLower", out PolyValueLower);
           paramDict.TryGetValue("PolyValueUpper", out PolyValueUpper);
           paramDict.TryGetValue("SeedValueUpper", out SeedValueUpper);
           paramDict.TryGetValue("SeedValueLower", out SeedValueLower);
           //paramDict.TryGetValue("", out m_instanceName);
           paramDict.TryGetValue(INSTANCE_NAME_PARAM, out m_instanceName);
           int PRSSize = int.Parse(sPRSSize);
           int RunMode = int.Parse(sRunMode);

           PolyValueLower = "0x" + Convert.ToUInt32(PolyValueLower).ToString("X");
           PolyValueUpper = "0x" + Convert.ToUInt32(PolyValueUpper).ToString("X");
           SeedValueLower = "0x" + Convert.ToUInt32(SeedValueLower).ToString("X");
           SeedValueUpper = "0x" + Convert.ToUInt32(SeedValueUpper).ToString("X");

           #region File .h
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Paramenters definition");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("#define " + m_instanceName + "_PRSSize        " + sPRSSize + "");
           writer.WriteLine("#define " + m_instanceName + "_RUN_MODE       " + RunMode + "");
           writer.WriteLine("");

           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine("     *  Function Prototypes");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("void " + m_instanceName + "_Start(void);");
           writer.WriteLine("void " + m_instanceName + "_Stop(void);");

           if (RunMode > 0)
           {
               writer.WriteLine("void " + m_instanceName + "_Step(void);");
           }
           if (PRSSize <= 8)    /* 8bit - PRS */
           {
               writer.WriteLine("uint8 " + m_instanceName + "_Read(void);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint8 seed);");
               writer.WriteLine("uint8 " + m_instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint8 polynomial);");
           }
           else if (PRSSize <= 16)    /* 16bit - PRS */
           {
               writer.WriteLine("uint16 " + m_instanceName + "_Read(void);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint16 seed);");
               writer.WriteLine("uint16 " + m_instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint16 polynomial);");
           }
           else if (PRSSize <= 24)    /* 24bit - PRS */
           {
               writer.WriteLine("uint32 " + m_instanceName + "_Read(void);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint32 seed);");
               writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial);");
           }
           else if (PRSSize <= 32)    /* 32bit - PRS */
           {
               writer.WriteLine("uint32 " + m_instanceName + "_Read(void);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint32 seed);");
               writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial);");
           }
           else    /* 64bit - PRS */
           {
               writer.WriteLine("uint32 " + m_instanceName + "_ReadUpper(void);");
               writer.WriteLine("uint32 " + m_instanceName + "_ReadLower(void);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeedUpper(uint32 seed);");
               writer.WriteLine("void " + m_instanceName + "_WriteSeedLower(uint32 seed);");
               writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomialUpper(void);");
               writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomialLower(void);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomialUpper(uint32 polynomial);");
               writer.WriteLine("void " + m_instanceName + "_WritePolynomialLower(uint32 polynomial);");
               writer.WriteLine("");
           }
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Initialization Values");
           writer.WriteLine(" ***************************************/");
           if (PRSSize <= 32)
           {
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_POLYNOM 		    " + PolyValueLower + "");
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_SEED		  		" + SeedValueLower + "");
           }
           else  
           {
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_POLYNOM_LOWER     	" + PolyValueLower + "");
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_POLYNOM_UPPER     	" + PolyValueUpper + "");
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_SEED_LOWER			" + SeedValueLower + "");
               writer.WriteLine("#define " + m_instanceName + "_DEFAULT_SEED_UPPER		  	" + SeedValueUpper + "");
           }
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Registers");
           writer.WriteLine(" ***************************************/");
           if (PRSSize <= 8)    /* 8bit - PRS */
           {
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D0_REG )");

               writer.WriteLine("");
               writer.WriteLine("");

               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A0_REG )"); 
           }
           else if (PRSSize <= 16)    /* 16bit - PRS */
           {
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D1_REG         (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG_PTR    ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__D0_REG )");

               writer.WriteLine("");
               writer.WriteLine("");
               
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A1_REG            (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c1DP_PRSdp_a__A0_REG )"); 
           }
           else if (PRSSize <= 24)    /* 24bit - PRS */
           {
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D0_REG )");

               writer.WriteLine("");
               writer.WriteLine("");

               writer.WriteLine("#define " + m_instanceName + "_SEED_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A1_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A0_REG )");
               writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A0_REG )");
           }
           else if (PRSSize <= 32)    /* 32bit - PRS */
           {
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D1_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__D0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__D0_REG )");
                
                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("#define " + m_instanceName + "_SEED_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_A__A1_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_A__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A1_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_b__A0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A0_REG )");
                writer.WriteLine("#define " + m_instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c2DP_PRSdp_a__A0_REG )");
           }
           else/* 64bit - PRS */
           {
               if (PRSSize <= 40)
               {
                   
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D1_REG )");
                    
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D0_REG )");
                   
                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A1_REG )");
                                        
                    writer.WriteLine(""); 
    
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A0_REG )");
               }
               else if (PRSSize <= 48)
               {
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D1_REG )");
                    
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D1_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__D0_REG )");               
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__D0_REG )");

                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A1_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_c__A0_REG )");                   
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c3DP_PRSdp_a__A0_REG )");
               }
               else if (PRSSize <= 56)
               {
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D1_REG )");                   
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D1_REG )");
                   
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D0_REG )");                   
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D0_REG )");
                   
                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_D__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_D__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A1_REG )");
                     
                    writer.WriteLine("");
    
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_D__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_D__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A0_REG )");
    
               }
               else
               {
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_A__D1_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_UPPER_A__D1_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D1_REG )");
    
                    writer.WriteLine("");
    
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__D0_REG )");
                    
                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_D__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_D__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_A__A1_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A1_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_UPPER_A__A1_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A1_REG )");
    
                    writer.WriteLine("");
                   
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_D__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_D__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_d__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_c__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_b__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A0_REG )");
                    writer.WriteLine("#define " + m_instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + m_instanceName + "_c4DP_PRSdp_a__A0_REG )");
               }
                
           }
           writer.WriteLine("#define " + m_instanceName + "_CONTROL            		    (*(reg8 *) " + m_instanceName + "_ctrlreg__CONTROL_REG)");
           writer.WriteLine("#define " + m_instanceName + "_CONTROL_PTR        			((reg8 *) " + m_instanceName + "_ctrlreg__CONTROL_REG)");
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Constants");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("#define " + m_instanceName + "_CTRL_ENABLE						0x01");
           writer.WriteLine("#define " + m_instanceName + "_CTRL_RISING_EDGE				0x02");
           writer.WriteLine("#define " + m_instanceName + "_CTRL_RESET_DFF					0x04");
           writer.WriteLine("");
           #endregion
           paramDict.Add("DefineH", writer.ToString());

 		   writer = new StringWriter();

           	#region File .c
           	#region Start
           	writer.WriteLine("/*------------------------------------------------------------------------------");
           	writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_Start(void)");
           	writer.WriteLine(" *------------------------------------------------------------------------------");
           	writer.WriteLine(" * Summary:");
           	writer.WriteLine(" *  Initializes seed and polynomial registers. Computation of PRS");
           	writer.WriteLine(" *  starts on riseing edge of input clock.");
           	writer.WriteLine(" *");
           	writer.WriteLine(" * m_Parameters:");
           	writer.WriteLine(" *  None");
           	writer.WriteLine(" *");
           	writer.WriteLine(" * Return:");
           	writer.WriteLine(" *  None");
           	writer.WriteLine(" *");
           	writer.WriteLine(" * Theory:");
           	writer.WriteLine(" *  See summary");
           	writer.WriteLine(" *");
           	writer.WriteLine(" * Side Effects:");
           	writer.WriteLine(" *  None");
           	writer.WriteLine(" *");
           	writer.WriteLine(" *----------------------------------------------------------------------------*/");
           	writer.WriteLine("void " + m_instanceName + "_Start(void)");
           	writer.WriteLine("{");
           	writer.WriteLine(" ");
           	writer.WriteLine("  /* Writes seed value and ponynom value provided for customizer */");
           	writer.WriteLine("  if ( " + m_instanceName + "_firsttime == 0 )");
           	writer.WriteLine("  {");
            if (PRSSize <= 32)
           	{
                writer.WriteLine("      " + m_instanceName + "_WritePolynomial(" + m_instanceName + "_DEFAULT_POLYNOM);");
                writer.WriteLine("      " + m_instanceName + "_WriteSeed(" + m_instanceName + "_DEFAULT_SEED);");
           	}
           	else
           	{
                writer.WriteLine("      " + m_instanceName + "_WritePolynomialUpper(" + m_instanceName + "_DEFAULT_POLYNOM_UPPER);");
                writer.WriteLine("      " + m_instanceName + "_WritePolynomialLower(" + m_instanceName + "_DEFAULT_POLYNOM_LOWER);");
                writer.WriteLine("      " + m_instanceName + "_WriteSeedUpper(" + m_instanceName + "_DEFAULT_SEED_UPPER);");
                writer.WriteLine("      " + m_instanceName + "_WriteSeedLower(" + m_instanceName + "_DEFAULT_SEED_LOWER);");
           	}
            writer.WriteLine("      ");
           	writer.WriteLine("      " + m_instanceName + "_firsttime = 1;");
           	writer.WriteLine("      }");
            writer.WriteLine("      ");
           	writer.WriteLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_ENABLE;");
           	writer.WriteLine("	");
           	writer.WriteLine("}");
           	writer.WriteLine("");
           	#endregion

            #region Stop
           writer.WriteLine("/*------------------------------------------------------------------------------");
           writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_Stop(void)");
           writer.WriteLine(" *------------------------------------------------------------------------------");
           writer.WriteLine(" * Summary:");
           writer.WriteLine(" *  Stops PRS computation, PRS store in PRS register. ");
           writer.WriteLine(" *");
           writer.WriteLine(" * m_Parameters:  ");
           writer.WriteLine(" *  None");
           writer.WriteLine(" *");
           writer.WriteLine(" * Return:");
           writer.WriteLine(" *  None");
           writer.WriteLine(" *");
           writer.WriteLine(" * Theory:");
           writer.WriteLine(" *  See summary");
           writer.WriteLine(" *");
           writer.WriteLine(" * Side Effects:");
           writer.WriteLine(" *  None");
           writer.WriteLine(" *");
           writer.WriteLine(" *----------------------------------------------------------------------------*/");
           writer.WriteLine("void " + m_instanceName + "_Stop(void)");
           writer.WriteLine("{");
           writer.WriteLine("   " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_ENABLE;");
           writer.WriteLine("}");
           writer.WriteLine("");
           #endregion

           if (RunMode > 0)
           {
                #region Step
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_Step(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Increments the PRS by one when in API single step mode. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * m_Parameters:  ");
               writer.WriteLine(" *  None");
               writer.WriteLine(" *");
               writer.WriteLine(" * Return:");
               writer.WriteLine(" *  None");
               writer.WriteLine(" *");
               writer.WriteLine(" * Theory:");
               writer.WriteLine(" *  See summary");
               writer.WriteLine(" *");
               writer.WriteLine(" * Side Effects:");
               writer.WriteLine(" *  None");
               writer.WriteLine(" *");
               writer.WriteLine(" *----------------------------------------------------------------------------*/");
               writer.WriteLine("void " + m_instanceName + "_Step(void)");
               writer.WriteLine("{");
               writer.WriteLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RISING_EDGE;");
               writer.WriteLine("    ");
               writer.WriteLine("    /* TODO");
               writer.WriteLine("		need to immplement delay");
               writer.WriteLine("	*/");
               writer.WriteLine("    ");
               writer.WriteLine("    " + m_instanceName + "_CONTROL &= ~ " + m_instanceName + "_CTRL_RISING_EDGE;      ");
               if (PRSSize > 8)
               {
                    writer.WriteLine("    "); 
                    writer.WriteLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RISING_EDGE;");
                    writer.WriteLine("    ");
                    writer.WriteLine("    /* TODO");
                    writer.WriteLine("		need to immplement delay");
                    writer.WriteLine("	*/");
                    writer.WriteLine("    ");
                    writer.WriteLine("    " + m_instanceName + "_CONTROL &= ~ " + m_instanceName + "_CTRL_RISING_EDGE;"); 
               }
               writer.WriteLine("}");
               #endregion
           }
           writer.WriteLine("");
            if (PRSSize <= 8)    /* 8bit - PRS */
            {
                #region Read
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint8 " + m_instanceName + "_Read(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint8) Current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + m_instanceName + "_Read(void)");
                writer.WriteLine("{");
                writer.WriteLine("	return " + m_instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeed(uint8 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint8) seed: Seed register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *   None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint8 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_SEED_A__A0_REG = seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint8 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint8) PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("	return " + m_instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomial(uint8 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint8) polynomial: PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint8 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D0_REG = polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (PRSSize <= 16)    /* 16bit - PRS */
            {
                #region Read
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + m_instanceName + "_Read(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint16) Current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + m_instanceName + "_Read(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint16 seed;");
                writer.WriteLine("	seed = ((uint16) " + m_instanceName + "_SEED_A__A1_REG) << 8;");
                writer.WriteLine("	seed |= " + m_instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeed(uint16 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint16) seed: Seed register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint16 seed)");
                writer.WriteLine("{");
                writer.WriteLine("  " + m_instanceName + "_SEED_A__A1_REG = HI8(seed);");
                writer.WriteLine("  " + m_instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
    
                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint16) PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint16 polynomial;");
                writer.WriteLine("	polynomial = ((uint16) " + m_instanceName + "_POLYNOM_A__D1_REG) << 8;");
                writer.WriteLine("	polynomial |= (" + m_instanceName + "_POLYNOM_A__D0_REG);");
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomial(uint16 polynom)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint16) polynomial: PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *   None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint16 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D1_REG = HI8(polynomial);");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (PRSSize <= 24)    /* 24bit - PRS */
            {
                #region Read
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_Read(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_Read(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("	seed = ((uint32) (" + m_instanceName + "_SEED_B__A1_REG)) << 16;");
                writer.WriteLine("	seed |= ((uint32) (" + m_instanceName + "_SEED_B__A0_REG)) << 8;");
                writer.WriteLine("	seed |= " + m_instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("  ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) seed: Seed register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:     ");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_SEED_B__A1_REG = LO8(HI16(seed));");
                writer.WriteLine("	" + m_instanceName + "_SEED_B__A0_REG = HI8(seed);");
                writer.WriteLine("	" + m_instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("	polynomial = ((uint32) " + m_instanceName + "_POLYNOM_B__D1_REG) << 16;");
                writer.WriteLine("	polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_B__D0_REG) << 8;");
                writer.WriteLine("	polynomial |= " + m_instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_B__D1_REG = LO8(HI16(polynomial));");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_B__D0_REG = HI8(polynomial);");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (PRSSize <= 32)    /* 32bit - PRS */
            {
                #region Read
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_Read(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current PRS value.");
                writer.WriteLine(" * ");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current PRS value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_Read(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("	seed = ((uint32) " + m_instanceName + "_SEED_B__A1_REG) << 24;");
                writer.WriteLine("	seed |= ((uint32) " + m_instanceName + "_SEED_A__A1_REG) << 16;");
                writer.WriteLine("	seed |= ((uint32) " + m_instanceName + "_SEED_B__A0_REG) << 8;");
                writer.WriteLine("	seed |= " + m_instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return seed;"); 
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) seed: Seed register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_SEED_B__A1_REG = HI8(HI16(seed));");
                writer.WriteLine("	" + m_instanceName + "_SEED_A__A1_REG = LO8(HI16(seed));");
                writer.WriteLine("	" + m_instanceName + "_SEED_B__A0_REG = HI8(seed);");
                writer.WriteLine("	" + m_instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:     ");
                writer.WriteLine(" *  (uint32) PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("	polynomial = ((uint32) " + m_instanceName + "_POLYNOM_B__D1_REG) << 24;");
                writer.WriteLine("	polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_A__D1_REG) << 16;");
                writer.WriteLine("	polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_B__D0_REG) << 8;");
                writer.WriteLine("	polynomial |= " + m_instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: PRS polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_B__D1_REG = HI8(HI16(polynomial));");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D1_REG = LO8(HI16(polynomial));");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_B__D0_REG = HI8(polynomial);");
                writer.WriteLine("	" + m_instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else  /* 64bit - PRS */
            {
                #region ReadUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadUpper(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current PRS Upper value. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current PRS Upper value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadUpper(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("  ");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  seed = " + m_instanceName + "_SEED_UPPER_C__A1_REG;");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_UPPER_C__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_UPPER_B__A1_REG;");
                }
                else if (PRSSize <= 56)
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_UPPER_D__A1_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_UPPER_C__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_UPPER_B__A1_REG;");
                }
                else
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_UPPER_D__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_UPPER_C__A1_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_UPPER_B__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_UPPER_A__A1_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadLower(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" * 	Reads the current PRS Lower value. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current PRS Lower value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadLower(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("  ");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_LOWER_B__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_LOWER_A__A0_REG;");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_LOWER_A__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_LOWER_A__A0_REG;"); 
                }
                else
                {
                    writer.WriteLine("  seed = ((uint32) " + m_instanceName + "_SEED_LOWER_D__A0_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + m_instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + m_instanceName + "_SEED_LOWER_A__A0_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeedUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeedUpper(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed Upper register with the start value. ");
                writer.WriteLine(" *  Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) seed: Seed Upper register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeedUpper(uint32 seed)");
                writer.WriteLine("{");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_C__A1_REG = LO8(seed);");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_C__A1_REG = HI8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_B__A1_REG = LO8(seed);");
                }
                else if (PRSSize <= 56)
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_D__A1_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_C__A1_REG = HI8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_B__A1_REG = LO8(seed);");
                }
                else
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_D__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_C__A1_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_B__A1_REG = LO8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_UPPER_A__A1_REG = LO8(seed);");
                }
                writer.WriteLine("   ");
                writer.WriteLine("   /* Reset triger */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("    " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeedLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WriteSeedLower(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS Seed Lower register with the start value. ");
                writer.WriteLine(" *  Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) seed: Seed Lower register start value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WriteSeedLower(uint32 seed)");
                writer.WriteLine("{");    
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_B__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_B__A0_REG = LO8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_A__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_B__A0_REG = LO8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                else
                {
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_D__A0_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_B__A0_REG = LO8(seed);");
                    writer.WriteLine("  " + m_instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                writer.WriteLine("   ");
                writer.WriteLine("   /* Reset triger */");
                writer.WriteLine("    " + m_instanceName + "_CONTROL |= " + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("    " + m_instanceName + "_CONTROL &= ~" + m_instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomialUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadPolynomialUpper(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial Upper. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return: ");
                writer.WriteLine(" *  (uint32) PRS polynomial Upper.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomialUpper(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("  ");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  polynomial = " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG;");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG;");
                }
                else if (PRSSize <= 56)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG;");
                }
                else
                {
                    writer.WriteLine("  polynomial = ((uint32) " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_UPPER_A__D1_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("  ");
                writer.WriteLine("  return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomialLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + m_instanceName + "_ReadPolynomialLower(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the PRS polynomial Lower. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) PRS polynomial Lower.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + m_instanceName + "_ReadPolynomialLower(void)");
                writer.WriteLine("{");
                writer.WriteLine("   uint32 polynomial;");
                writer.WriteLine("   ");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  polynomial = ( (uint32) " + m_instanceName + "_POLYNOM_LOWER_B__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ( (uint32) " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ( (uint32) " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + m_instanceName + "_POLYNOM_LOWER_A__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                else
                {
                    writer.WriteLine("  polynomial = ((uint32) " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                #endregion

                #region WritePolynomialUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomialUpper(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial Upper. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: PRS polynomial Upper.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomialUpper(uint32 polynomial)");
                writer.WriteLine("{");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG = LO8(polynomial);");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);");
                }
                else if (PRSSize <= 56)
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);");
                }
                else
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_D__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_C__D1_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_UPPER_A__D1_REG = LO8(polynomial);");
                }
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomialLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + m_instanceName + "_WritePolynomialLower(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the PRS polynomial Lower. Only generated for 33-64-bit PRS.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: PRS polynomial Lower.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *   None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("void " + m_instanceName + "_WritePolynomialLower(uint32 polynomial)");
                writer.WriteLine("{");
                if (PRSSize <= 40)
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_B__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG = LO8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                else if (PRSSize <= 48)
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_A__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG = LO8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                else
                {
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_D__D0_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_B__D0_REG = LO8(polynomial);");
                    writer.WriteLine("  " + m_instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                writer.WriteLine("}");
                }
                #endregion

           #endregion
           paramDict.Add("DefineC", writer.ToString());

       }
    }
}
