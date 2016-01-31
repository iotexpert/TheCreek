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
//using System.Windows.Forms;
using System.Diagnostics;

namespace CRC_v1_10
{
    public partial class CyCustomizer
    {
        public void GenerateHFile(ref Dictionary<string, string> paramDict)
        {
            StringWriter writer = new StringWriter();

            string m_CRCSize;
            string PolyName;
            string PolyValueLower;
            string PolyValueUpper;
            string SeedValueUpper;
            string SeedValueLower;
            string instanceName;
            int CRCMask;
            int j;

            paramDict.TryGetValue("PolyName", out PolyName);
            paramDict.TryGetValue("Resolution", out m_CRCSize);
            paramDict.TryGetValue("PolyValueLower", out PolyValueLower);
            paramDict.TryGetValue("PolyValueUpper", out PolyValueUpper);
            paramDict.TryGetValue("SeedValueUpper", out SeedValueUpper);
            paramDict.TryGetValue("SeedValueLower", out SeedValueLower);
            //paramDict.TryGetValue("", out m_instanceName);
            paramDict.TryGetValue(INSTANCE_NAME_PARAM, out instanceName);
            int CRCSize = int.Parse(m_CRCSize);

            PolyValueLower = "0x" + Convert.ToUInt32(PolyValueLower).ToString("X");
            PolyValueUpper = "0x" + Convert.ToUInt32(PolyValueUpper).ToString("X");
            SeedValueLower = "0x" + Convert.ToUInt32(SeedValueLower).ToString("X");
            SeedValueUpper = "0x" + Convert.ToUInt32(SeedValueUpper).ToString("X");

            #region  CRC_Mask

            CRCMask = 0;

            for (j = 0; j < CRCSize; j++)
            {
                CRCMask = CRCMask | (1 << j);
            }

            #endregion

            #region File .h
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine(" *  Paramenters definition");
            writer.WriteLine(" ***************************************/");
            writer.WriteLine("#define " + instanceName + "_CRCSize        " + m_CRCSize + "");
            writer.WriteLine("");

            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine("     *  Function Prototypes");
            writer.WriteLine(" ***************************************/");
            writer.WriteLine("void " + instanceName + "_Start(void);");
            writer.WriteLine("void " + instanceName + "_Stop(void);");
            writer.WriteLine("void " + instanceName + "_Reset(void);");

            if (CRCSize <= 8)    /* 8bit - CRC */
            {
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint8 seed);");
                writer.WriteLine("uint8 " + instanceName + "_ReadPolynomial(void);");
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint8 polynomial);");
                writer.WriteLine("uint8 " + instanceName + "_ReadCRC(void);");
            }
            else if (CRCSize <= 16)    /* 16bit - CRC */
            {
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint16 seed);");
                writer.WriteLine("uint16 " + instanceName + "_ReadPolynomial(void);");
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint16 polynomial);");
                writer.WriteLine("uint16 " + instanceName + "_ReadCRC(void);");
            }
            else if (CRCSize <= 24)    /* 24bit - CRC */
            {
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed);");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void);");
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial);");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRC(void);");
            }
            else if (CRCSize <= 32)    /* 32bit - CRC */
            {
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed);");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void);");
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial);");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRC(void);");
            }
            else    /* 64bit - CRC */
            {
                writer.WriteLine("void " + instanceName + "_WriteSeedUpper(uint32 seed);");
                writer.WriteLine("void " + instanceName + "_WriteSeedLower(uint32 seed);");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialUpper(void);");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialLower(void);");
                writer.WriteLine("void " + instanceName + "_WritePolynomialUpper(uint32 polynomial);");
                writer.WriteLine("void " + instanceName + "_WritePolynomialLower(uint32 polynomial);");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRCUpper(void);");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRCLower(void);");
                writer.WriteLine("");
            }
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine(" *  Initialization Values");
            writer.WriteLine(" ***************************************/");
            writer.WriteLine("#define " + instanceName + "_MASK 		                " + CRCMask + "");
            if (CRCSize <= 32)
            {
                writer.WriteLine("#define " + instanceName + "_DEFAULT_POLYNOM 		    " + PolyValueLower + "");
                writer.WriteLine("#define " + instanceName + "_DEFAULT_SEED		  		" + SeedValueLower + "");
            }
            else
            {
                writer.WriteLine("#define " + instanceName + "_DEFAULT_POLYNOM_LOWER     	" + PolyValueLower + "");
                writer.WriteLine("#define " + instanceName + "_DEFAULT_POLYNOM_UPPER     	" + PolyValueUpper + "");
                writer.WriteLine("#define " + instanceName + "_DEFAULT_SEED_LOWER			" + SeedValueLower + "");
                writer.WriteLine("#define " + instanceName + "_DEFAULT_SEED_UPPER		  	" + SeedValueUpper + "");
            }
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine(" *  Registers");
            writer.WriteLine(" ***************************************/");
            if (CRCSize <= 8)    /* 8bit - CRC */
            {
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__D0_REG )");

                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__A0_REG )");
            }
            else if (CRCSize <= 16)    /* 16bit - CRC */
            {
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D1_REG         (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D1_REG_PTR     ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG_PTR    ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__D0_REG )");

                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("#define " + instanceName + "_SEED_A__A1_REG            (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A1_REG_PTR        ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + instanceName + "_c1DP_CRCdp_a__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c1DP_CRCdp_a__A0_REG )");
            }
            else if (CRCSize <= 24)    /* 24bit - CRC */
            {
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D1_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D0_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__D0_REG )");

                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("#define " + instanceName + "_SEED_B__A1_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A0_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__A0_REG )");
            }
            else if (CRCSize <= 32)    /* 32bit - CRC */
            {
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D1_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D1_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D1_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__D1_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D0_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG         (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__D0_REG )");
                writer.WriteLine("#define " + instanceName + "_POLYNOM_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__D0_REG )");

                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("#define " + instanceName + "_SEED_B__A1_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A1_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A1_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__A1_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A0_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_b__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_b__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG            (*(reg8 *) " + instanceName + "_c2DP_CRCdp_a__A0_REG )");
                writer.WriteLine("#define " + instanceName + "_SEED_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c2DP_CRCdp_a__A0_REG )");
            }
            else/* 64bit - CRC */
            {
                if (CRCSize <= 40)
                {

                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__D1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D1_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__D0_REG )");

                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__A1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A1_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__A0_REG )");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__D1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D1_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D1_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__D0_REG )");

                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__A1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A1_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A1_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + instanceName + "_c3DP_CRCdp_a__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c3DP_CRCdp_a__A0_REG )");
                }
                else if (CRCSize <= 56)
                {
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_D__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_D__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__D1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_D__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_D__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__D0_REG )");

                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_D__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_D__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__A1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_D__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_D__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__A0_REG )");

                }
                else
                {
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_D__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_D__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_C__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_B__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_A__D1_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__D1_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_A__D1_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__D1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_D__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_D__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_C__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_B__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG         (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__D0_REG )");
                    writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_A__D0_REG_PTR     ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__D0_REG )");

                    writer.WriteLine("");
                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_D__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_D__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_C__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_B__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_A__A1_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__A1_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_UPPER_A__A1_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__A1_REG )");

                    writer.WriteLine("");

                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_D__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_d__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_D__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_d__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_C__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_c__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_B__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_b__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG            (*(reg8 *) " + instanceName + "_c4DP_CRCdp_a__A0_REG )");
                    writer.WriteLine("#define " + instanceName + "_SEED_LOWER_A__A0_REG_PTR        ((reg8 *) " + instanceName + "_c4DP_CRCdp_a__A0_REG )");
                }

            }
            writer.WriteLine("#define " + instanceName + "_CONTROL            		    (*(reg8 *) " + instanceName + "_ctrlreg__CONTROL_REG)");
            writer.WriteLine("#define " + instanceName + "_CONTROL_PTR        			((reg8 *) " + instanceName + "_ctrlreg__CONTROL_REG)");
            writer.WriteLine("");
            writer.WriteLine("/***************************************");
            writer.WriteLine(" *  Constants");
            writer.WriteLine(" ***************************************/");
            writer.WriteLine("#define " + instanceName + "_CTRL_ENABLE						0x01");
            writer.WriteLine("#define " + instanceName + "_CTRL_RISING_EDGE				0x02");
            writer.WriteLine("#define " + instanceName + "_CTRL_RESET_DFF					0x04");
            writer.WriteLine("");
            #endregion
            paramDict.Add("DefineH", writer.ToString());

            writer = new StringWriter();

            #region File .c
            #region Start
            writer.WriteLine("/*------------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Start(void)");
            writer.WriteLine(" *------------------------------------------------------------------------------");
            writer.WriteLine(" * Summary:");
            writer.WriteLine(" *  Initializes seed and polynomial registers. Computation of CRC");
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
            writer.WriteLine("void " + instanceName + "_Start(void)");
            writer.WriteLine("{");
            writer.WriteLine(" ");
            writer.WriteLine("  /* Writes seed value and ponynom value provided for customizer */");
            writer.WriteLine("  if ( " + instanceName + "_firsttime == 0 )");
            writer.WriteLine("  {");
            if (CRCSize <= 32)
            {
                writer.WriteLine("      " + instanceName + "_WritePolynomial(" + instanceName + "_DEFAULT_POLYNOM);");
                writer.WriteLine("      " + instanceName + "_WriteSeed(" + instanceName + "_DEFAULT_SEED);");
            }
            else
            {
                writer.WriteLine("      " + instanceName + "_WritePolynomialUpper(" + instanceName + "_DEFAULT_POLYNOM_UPPER);");
                writer.WriteLine("      " + instanceName + "_WritePolynomialLower(" + instanceName + "_DEFAULT_POLYNOM_LOWER);");
                writer.WriteLine("      " + instanceName + "_WriteSeedUpper(" + instanceName + "_DEFAULT_SEED_UPPER);");
                writer.WriteLine("      " + instanceName + "_WriteSeedLower(" + instanceName + "_DEFAULT_SEED_LOWER);");
            }
            writer.WriteLine("      ");
            writer.WriteLine("      " + instanceName + "_firsttime = 1;");
            writer.WriteLine("      }");
            writer.WriteLine("      ");
            writer.WriteLine("    " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_ENABLE;");
            writer.WriteLine("	");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region Stop
            writer.WriteLine("/*------------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Stop(void)");
            writer.WriteLine(" *------------------------------------------------------------------------------");
            writer.WriteLine(" * Summary:");
            writer.WriteLine(" *  Stops CRC computation, CRC store in CRC register. ");
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
            writer.WriteLine("void " + instanceName + "_Stop(void)");
            writer.WriteLine("{");
            writer.WriteLine("   " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_ENABLE;");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion

            #region Reset
            writer.WriteLine("/*------------------------------------------------------------------------------");
            writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Rest(void)");
            writer.WriteLine(" *------------------------------------------------------------------------------");
            writer.WriteLine(" * Summary:");
            writer.WriteLine(" *  Resets the CRC register to default seed value. Computation of CRC");
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
            writer.WriteLine("void " + instanceName + "_Reset(void)");
            writer.WriteLine("{");
            writer.WriteLine(" ");
            writer.WriteLine("  /* Writes seed value provided for customizer */");
            if (CRCSize <= 32)
            {
                writer.WriteLine("      " + instanceName + "_WriteSeed(" + instanceName + "_DEFAULT_SEED);");
            }
            else
            {
                writer.WriteLine("      " + instanceName + "_WriteSeedUpper(" + instanceName + "_DEFAULT_SEED_UPPER);");
                writer.WriteLine("      " + instanceName + "_WriteSeedLower(" + instanceName + "_DEFAULT_SEED_LOWER);");
            }
            writer.WriteLine("	");
            writer.WriteLine("}");
            writer.WriteLine("");
            #endregion
            writer.WriteLine("");
            if (CRCSize <= 8)    /* 8bit - CRC */
            {
                #region ReadCRC
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint8) Current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine("{");
                writer.WriteLine("	return(" + instanceName + "_SEED_A__A0_REG & " + instanceName + "_MASK);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint8 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed register with the start value. ");
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
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint8 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_SEED_A__A0_REG = seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint8) CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint8 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("	return " + instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint8 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint8) polynomial: CRC polynomial.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint8 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D0_REG = polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (CRCSize <= 16)    /* 16bit - CRC */
            {
                #region ReadCRC
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint16) Current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint16 seed;");
                writer.WriteLine("	seed = ((uint16) " + instanceName + "_SEED_A__A1_REG) << 8;");
                writer.WriteLine("	seed |= " + instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return (seed & " + instanceName + "_MASK);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint16 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed register with the start value. ");
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
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint16 seed)");
                writer.WriteLine("{");
                writer.WriteLine("  " + instanceName + "_SEED_A__A1_REG = HI8(seed);");
                writer.WriteLine("  " + instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint16) CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint16 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint16 polynomial;");
                writer.WriteLine("	polynomial = ((uint16) " + instanceName + "_POLYNOM_A__D1_REG) << 8;");
                writer.WriteLine("	polynomial |= (" + instanceName + "_POLYNOM_A__D0_REG);");
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint16 polynom)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint16) polynomial: CRC polynomial.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint16 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D1_REG = HI8(polynomial);");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (CRCSize <= 24)    /* 24bit - CRC */
            {
                #region ReadCRC
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("	seed = ((uint32) (" + instanceName + "_SEED_B__A1_REG)) << 16;");
                writer.WriteLine("	seed |= ((uint32) (" + instanceName + "_SEED_B__A0_REG)) << 8;");
                writer.WriteLine("	seed |= " + instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("  ");
                writer.WriteLine("	return (seed & " + instanceName + "_MASK);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed register with the start value. ");
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
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_SEED_B__A1_REG = LO8(HI16(seed));");
                writer.WriteLine("	" + instanceName + "_SEED_B__A0_REG = HI8(seed);");
                writer.WriteLine("	" + instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("	polynomial = ((uint32) " + instanceName + "_POLYNOM_B__D1_REG) << 16;");
                writer.WriteLine("	polynomial |= ((uint32) " + instanceName + "_POLYNOM_B__D0_REG) << 8;");
                writer.WriteLine("	polynomial |= " + instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: CRC polynomial.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_POLYNOM_B__D1_REG = LO8(HI16(polynomial));");
                writer.WriteLine("	" + instanceName + "_POLYNOM_B__D0_REG = HI8(polynomial);");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else if (CRCSize <= 32)    /* 32bit - CRC */
            {
                #region ReadCRC
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current CRC value.");
                writer.WriteLine(" * ");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current CRC value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRC(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("	seed = ((uint32) " + instanceName + "_SEED_B__A1_REG) << 24;");
                writer.WriteLine("	seed |= ((uint32) " + instanceName + "_SEED_A__A1_REG) << 16;");
                writer.WriteLine("	seed |= ((uint32) " + instanceName + "_SEED_B__A0_REG) << 8;");
                writer.WriteLine("	seed |= " + instanceName + "_SEED_A__A0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return(seed & " + instanceName + "_MASK);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeed
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed register with the start value. ");
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
                writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_SEED_B__A1_REG = HI8(HI16(seed));");
                writer.WriteLine("	" + instanceName + "_SEED_A__A1_REG = LO8(HI16(seed));");
                writer.WriteLine("	" + instanceName + "_SEED_B__A0_REG = HI8(seed);");
                writer.WriteLine("	" + instanceName + "_SEED_A__A0_REG = LO8(seed);");
                writer.WriteLine("  ");
                writer.WriteLine("  /* Reset triger */");
                writer.WriteLine("  " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("  " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:     ");
                writer.WriteLine(" *  (uint32) CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("	polynomial = ((uint32) " + instanceName + "_POLYNOM_B__D1_REG) << 24;");
                writer.WriteLine("	polynomial |= ((uint32) " + instanceName + "_POLYNOM_A__D1_REG) << 16;");
                writer.WriteLine("	polynomial |= ((uint32) " + instanceName + "_POLYNOM_B__D0_REG) << 8;");
                writer.WriteLine("	polynomial |= " + instanceName + "_POLYNOM_A__D0_REG;");
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomial
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: CRC polynomial.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial)");
                writer.WriteLine("{");
                writer.WriteLine("	" + instanceName + "_POLYNOM_B__D1_REG = HI8(HI16(polynomial));");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D1_REG = LO8(HI16(polynomial));");
                writer.WriteLine("	" + instanceName + "_POLYNOM_B__D0_REG = HI8(polynomial);");
                writer.WriteLine("	" + instanceName + "_POLYNOM_A__D0_REG = LO8(polynomial);");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion
            }
            else  /* 64bit - CRC */
            {
                #region ReadCRCUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadCRCUpper(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the current CRC Upper value. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current CRC Upper value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRCUpper(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("  ");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  seed = " + instanceName + "_SEED_UPPER_C__A1_REG;");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_UPPER_C__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_UPPER_B__A1_REG;");
                }
                else if (CRCSize <= 56)
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_UPPER_D__A1_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_UPPER_C__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_UPPER_B__A1_REG;");
                }
                else
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_UPPER_D__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_UPPER_C__A1_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_UPPER_B__A1_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_UPPER_A__A1_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadCRCLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadCRCLower(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" * 	Reads the current CRC Lower value. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) Current CRC Lower value.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadCRCLower(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 seed;");
                writer.WriteLine("  ");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_LOWER_B__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_LOWER_A__A0_REG;");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_LOWER_A__A1_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_LOWER_A__A0_REG;");
                }
                else
                {
                    writer.WriteLine("  seed = ((uint32) " + instanceName + "_SEED_LOWER_D__A0_REG) << 24;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_C__A0_REG) << 16;");
                    writer.WriteLine("  seed |= ((uint32) " + instanceName + "_SEED_LOWER_B__A0_REG) << 8;");
                    writer.WriteLine("  seed |= " + instanceName + "_SEED_LOWER_A__A0_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("	return seed;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeedUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeedUpper(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed Upper register with the start value. ");
                writer.WriteLine(" *  Only generated for 33-64-bit CRC.");
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
                writer.WriteLine("void " + instanceName + "_WriteSeedUpper(uint32 seed)");
                writer.WriteLine("{");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_C__A1_REG = LO8(seed);");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_C__A1_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_B__A1_REG = LO8(seed);");
                }
                else if (CRCSize <= 56)
                {
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_D__A1_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_C__A1_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_B__A1_REG = HI8(seed);");
                }
                else
                {
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_D__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_C__A1_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_B__A1_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_UPPER_A__A1_REG = LO8(seed);");
                }
                writer.WriteLine("   ");
                writer.WriteLine("   /* Reset triger */");
                writer.WriteLine("    " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("    " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WriteSeedLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeedLower(uint32 seed)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC Seed Lower register with the start value. ");
                writer.WriteLine(" *  Only generated for 33-64-bit CRC.");
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
                writer.WriteLine("void " + instanceName + "_WriteSeedLower(uint32 seed)");
                writer.WriteLine("{");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_B__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_B__A0_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_A__A1_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_B__A0_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                else
                {
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_D__A0_REG = HI8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_C__A0_REG = LO8(HI16(seed));");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_B__A0_REG = HI8(seed);");
                    writer.WriteLine("  " + instanceName + "_SEED_LOWER_A__A0_REG = LO8(seed);");
                }
                writer.WriteLine("   ");
                writer.WriteLine("   /* Reset triger */");
                writer.WriteLine("    " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("    " + instanceName + "_CONTROL &= ~" + instanceName + "_CTRL_RESET_DFF;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomialUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomialUpper(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial Upper. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return: ");
                writer.WriteLine(" *  (uint32) CRC polynomial Upper.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialUpper(void)");
                writer.WriteLine("{");
                writer.WriteLine("  uint32 polynomial;");
                writer.WriteLine("  ");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  polynomial = " + instanceName + "_POLYNOM_UPPER_C__D1_REG;");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + instanceName + "_POLYNOM_UPPER_C__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_UPPER_B__D1_REG;");
                }
                else if (CRCSize <= 56)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + instanceName + "_POLYNOM_UPPER_D__D1_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_UPPER_C__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_UPPER_B__D1_REG;");
                }
                else
                {
                    writer.WriteLine("  polynomial = ((uint32) " + instanceName + "_POLYNOM_UPPER_D__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_UPPER_C__D1_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_UPPER_B__D1_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_UPPER_A__D1_REG;");
                }
                writer.WriteLine("  ");
                writer.WriteLine("  ");
                writer.WriteLine("  return polynomial;");
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region ReadPolynomialLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomialLower(void)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Reads the CRC polynomial Lower. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" * Return:");
                writer.WriteLine(" *  (uint32) CRC polynomial Lower.");
                writer.WriteLine(" *");
                writer.WriteLine(" * Theory:");
                writer.WriteLine(" *  See summary");
                writer.WriteLine(" *");
                writer.WriteLine(" * Side Effects:");
                writer.WriteLine(" *  None");
                writer.WriteLine(" *");
                writer.WriteLine(" *----------------------------------------------------------------------------*/");
                writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialLower(void)");
                writer.WriteLine("{");
                writer.WriteLine("   uint32 polynomial;");
                writer.WriteLine("   ");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  polynomial = ( (uint32) " + instanceName + "_POLYNOM_LOWER_B__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ( (uint32) " + instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ( (uint32) " + instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  polynomial = ((uint32) " + instanceName + "_POLYNOM_LOWER_A__D1_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                else
                {
                    writer.WriteLine("  polynomial = ((uint32) " + instanceName + "_POLYNOM_LOWER_D__D0_REG) << 24;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_LOWER_C__D0_REG) << 16;");
                    writer.WriteLine("  polynomial |= ((uint32) " + instanceName + "_POLYNOM_LOWER_B__D0_REG) << 8;");
                    writer.WriteLine("  polynomial |= " + instanceName + "_POLYNOM_LOWER_A__D0_REG;");
                }
                writer.WriteLine("   ");
                writer.WriteLine("	return polynomial;");
                writer.WriteLine("}");
                #endregion

                #region WritePolynomialUpper
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomialUpper(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial Upper. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: CRC polynomial Upper.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomialUpper(uint32 polynomial)");
                writer.WriteLine("{");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_C__D1_REG = LO8(polynomial);");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);");
                }
                else if (CRCSize <= 56)
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_D__D1_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);");
                }
                else
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_D__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_C__D1_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_B__D1_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_UPPER_A__D1_REG = LO8(polynomial);");
                }
                writer.WriteLine("}");
                writer.WriteLine("");
                #endregion

                #region WritePolynomialLower
                writer.WriteLine("/*------------------------------------------------------------------------------");
                writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomialLower(uint32 polynomial)");
                writer.WriteLine(" *------------------------------------------------------------------------------");
                writer.WriteLine(" * Summary:");
                writer.WriteLine(" *  Writes the CRC polynomial Lower. Only generated for 33-64-bit CRC.");
                writer.WriteLine(" *");
                writer.WriteLine(" * m_Parameters:");
                writer.WriteLine(" *  (uint32) polynomial: CRC polynomial Lower.");
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
                writer.WriteLine("void " + instanceName + "_WritePolynomialLower(uint32 polynomial)");
                writer.WriteLine("{");
                if (CRCSize <= 40)
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_B__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                else if (CRCSize <= 48)
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_A__D1_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                else
                {
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_D__D0_REG = HI8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);");
                    writer.WriteLine("  " + instanceName + "_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);");
                }
                writer.WriteLine("}");
            }
                #endregion

            #endregion
            paramDict.Add("DefineC", writer.ToString());

        }

    }
}
