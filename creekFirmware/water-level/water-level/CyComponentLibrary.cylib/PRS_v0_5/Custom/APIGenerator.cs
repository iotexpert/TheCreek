/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CyCustomizer.PRS_v0_5
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
           //paramDict.TryGetValue("", out instanceName);
           paramDict.TryGetValue(p_instanceNameParam, out instanceName);
           int PRSSize = int.Parse(sPRSSize);
           int RunMode = int.Parse(sRunMode);

           #region File .h
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Paramenters definition");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("#define " + instanceName + "_PRSSize        " + sPRSSize + "");
           writer.WriteLine("#define " + instanceName + "_RUN_MODE       " + RunMode + "");
           writer.WriteLine("");

           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine("     *  Function Prototypes");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("void " + instanceName + "_Start(void);");
           writer.WriteLine("void " + instanceName + "_Stop(void);");

           if (RunMode > 0)
           {
               writer.WriteLine("void " + instanceName + "_Step(void);");
           }
           if (PRSSize <= 8)    /* 8bit - PRS */
           {
               writer.WriteLine("uint8 " + instanceName + "_Read(void);");
               writer.WriteLine("void " + instanceName + "_WriteSeed(uint8 seed);");
               writer.WriteLine("uint8 " + instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint8 polynomial);");
           }
           else if (PRSSize <= 16)    /* 16bit - PRS */
           {
               writer.WriteLine("uint16 " + instanceName + "_Read(void);");
               writer.WriteLine("void " + instanceName + "_WriteSeed(uint16 seed);");
               writer.WriteLine("uint16 " + instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint16 polynomial);");
           }
           else if (PRSSize <= 24)    /* 24bit - PRS */
           {
               writer.WriteLine("uint32 " + instanceName + "_Read(void);");
               writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed);");
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial);");
           }
           else if (PRSSize <= 32)    /* 32bit - PRS */
           {
               writer.WriteLine("uint32 " + instanceName + "_Read(void);");
               writer.WriteLine("void " + instanceName + "_WriteSeed(uint32 seed);");
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void);");
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial);");
           }
           else    /* 64bit - PRS */
           {
               writer.WriteLine("uint32 " + instanceName + "_ReadUpper(void);");
               writer.WriteLine("uint32 " + instanceName + "_ReadLower(void);");
               writer.WriteLine("void " + instanceName + "_WriteSeedUpper(uint32 seed);");
               writer.WriteLine("void " + instanceName + "_WriteSeedLower(uint32 seed);");
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialUpper(void);");
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialLower(void);");
               writer.WriteLine("void " + instanceName + "_WritePolynomialUpper(uint32 polynomial);");
               writer.WriteLine("void " + instanceName + "_WritePolynomialLower(uint32 polynomial);");
               writer.WriteLine("");
           }
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Initialization Values");
           writer.WriteLine(" ***************************************/");
           if (PRSSize <= 32)
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM		    " + PolyValueLower + "");
               writer.WriteLine("#define " + instanceName + "_SEED		  		" + SeedValueLower + "");
           }
           else  
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER     	" + PolyValueLower + "");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER     	" + PolyValueUpper + "");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER			" + SeedValueLower + "");
               writer.WriteLine("#define " + instanceName + "_SEED_UPPER		  	" + SeedValueUpper + "");
           }
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Registers");
           writer.WriteLine(" ***************************************/");
           if (PRSSize <= 8)    /* 8bit - PRS */
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER          (*(reg8 *) " + instanceName + "_sC8_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_PTR		((reg8 *) " + instanceName + "_sC8_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER		        (*(reg8 *) " + instanceName + "_sC8_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER_PTR		    ((reg8 *) " + instanceName + "_sC8_PRSdp_u0__A0_REG )");
           }
           else if (PRSSize <= 16)    /* 16bit - PRS */
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER          (*(reg16 *) " + instanceName + "_sC16_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_PTR		((reg16 *) " + instanceName + "_sC16_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER		        (*(reg16 *) " + instanceName + "_sC16_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER_PTR		    ((reg16 *) " + instanceName + "_sC16_PRSdp_u0__A0_REG )");
           }
           else if (PRSSize <= 24)    /* 24bit - PRS */
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER          (*(reg32 *) " + instanceName + "_sC24_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_PTR		((reg32 *) " + instanceName + "_sC24_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER		        (*(reg32 *) " + instanceName + "_sC24_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER_PTR		    ((reg32 *) " + instanceName + "_sC24_PRSdp_u0__A0_REG )");
           }
           else if (PRSSize <= 32)    /* 32bit - PRS */
           {
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER          (*(reg32 *) " + instanceName + "_sC32_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_PTR		((reg32 *) " + instanceName + "_sC32_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER		        (*(reg32 *) " + instanceName + "_sC32_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER_PTR		    ((reg32 *) " + instanceName + "_sC32_PRSdp_u0__A0_REG )");
           }
           else/* 64bit - PRS */
           {
               writer.WriteLine("	/* TODO */");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER          (*(reg32 *) " + instanceName + "_sC64_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_LOWER_PTR		((reg32 *) " + instanceName + "_sC64_PRSdp_u0__D0_REG )");
               writer.WriteLine("#define " + instanceName + "_POLYNOM_UPPER_PTR		" + instanceName + "_POLYNOM_LOWER_PTR + 4");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER		        (*(reg32 *) " + instanceName + "_sC64_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_LOWER_PTR		    ((reg32 *) " + instanceName + "_sC64_PRSdp_u0__A0_REG )");
               writer.WriteLine("#define " + instanceName + "_SEED_UPPER_PTR			" + instanceName + "_SEED_LOWER_PTR + 4");
           }
           writer.WriteLine("#define " + instanceName + "_CONTROL            (*(reg8 *) " + instanceName + "_ctrlreg__CONTROL_REG)");
           writer.WriteLine("#define " + instanceName + "_CONTROL_PTR        ((reg8 *) " + instanceName + "_ctrlreg__CONTROL_REG)");
           writer.WriteLine("");
           writer.WriteLine("/***************************************");
           writer.WriteLine(" *  Constants");
           writer.WriteLine(" ***************************************/");
           writer.WriteLine("#define " + instanceName + "_CTRL_ENABLE						0x01");
           writer.WriteLine("#define " + instanceName + "_CTRL_RISING_EDGE					0x02");
           writer.WriteLine("");
           #endregion
           paramDict.Add("DefineH", writer.ToString());

           #region File .c
           #region Start
           writer.WriteLine("/*------------------------------------------------------------------------------");
           writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Start(void)");
           writer.WriteLine(" *------------------------------------------------------------------------------");
           writer.WriteLine(" * Summary:");
           writer.WriteLine(" *  Initializes seed and polynomial registers. Computation of PRS");
           writer.WriteLine(" *  starts on riseing edge of input clock.");
           writer.WriteLine(" *");
           writer.WriteLine(" * Parameters:");
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
           writer.WriteLine("");
           writer.WriteLine("    /* Writes seed value and ponynom value provided for customizer */");
           writer.WriteLine("	if ( " + instanceName + "_firsttime == 0 )");
           writer.WriteLine("    {");
           if (PRSSize <= 8)
           {
               writer.WriteLine("            CY_SET_REG8(" + instanceName + "_SEED_LOWER_PTR, " + instanceName + "_SEED);");
               writer.WriteLine("            CY_SET_REG8(" + instanceName + "_POLYNOM_LOWER_PTR, " + instanceName + "_POLYNOM);");
           }
           else if (PRSSize <= 16)
           {
               writer.WriteLine("            CY_SET_REG16(" + instanceName + "_SEED_LOWER_PTR, " + instanceName + "_SEED);");
               writer.WriteLine("            CY_SET_REG16(" + instanceName + "_POLYNOM_LOWER_PTR, " + instanceName + "_POLYNOM);");
           }
           else if (PRSSize <= 24)
           {
               writer.WriteLine("            CY_SET_REG24(" + instanceName + "_SEED_LOWER_PTR, " + instanceName + "_SEED);");
               writer.WriteLine("            CY_SET_REG24(" + instanceName + "_POLYNOM_LOWER_PTR, " + instanceName + "_POLYNOM);");
           }
           else if (PRSSize <= 32)
           {
               writer.WriteLine("            CY_SET_REG32(" + instanceName + "_SEED_LOWER_PTR, " + instanceName + "_SEED);	");
               writer.WriteLine("            CY_SET_REG32(" + instanceName + "_POLYNOM_LOWER_PTR, " + instanceName + "_POLYNOM);");
           }
           else
           {
               writer.WriteLine("    //		CY_SET_REG32(" + instanceName + "_SEED_UPPER_PTR, " + instanceName + "_SEED_UPPER);	");
               writer.WriteLine("    //		CY_SET_REG32(" + instanceName + "_SEED_LOWER_PTR, " + instanceName + "_SEED_LOWER);	");
               writer.WriteLine("    //		CY_SET_REG32(" + instanceName + "_POLYNOM_UPPER_PTR, " + instanceName + "_POLYNOM_UPPER);");
               writer.WriteLine("    //		CY_SET_REG32(" + instanceName + "_POLYNOM_LOWER_PTR, " + instanceName + "_POLYNOM_LOWER);");
           }
           writer.WriteLine("        ");
           writer.WriteLine("       " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_ENABLE;");
           writer.WriteLine("       " + instanceName + "_firsttime = 1;");
           writer.WriteLine("    }");
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
           writer.WriteLine(" *  Stops PRS computation, PRS store in PRS register. ");
           writer.WriteLine(" *");
           writer.WriteLine(" * Parameters:  ");
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

           if (RunMode > 0)
           {
               #region Step
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_Step(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Increments the PRS by one when in API single step mode. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:  ");
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
               writer.WriteLine("void " + instanceName + "_Step(void)");
               writer.WriteLine("{");
               writer.WriteLine("    " + instanceName + "_CONTROL |= " + instanceName + "_CTRL_RISING_EDGE;");
               writer.WriteLine("    ");
               writer.WriteLine("    /* TODO");
               writer.WriteLine("		need to immplement delay ???");
               writer.WriteLine("	*/");
               writer.WriteLine("    ");
               writer.WriteLine("    " + instanceName + "_CONTROL &= ~ " + instanceName + "_CTRL_RISING_EDGE;      ");
               writer.WriteLine("}");
               #endregion
           }
           writer.WriteLine("");
           if (PRSSize <= 8)    /* 8bit - PRS */
           {
               #region Read
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_Read(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the current PRS value.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint8 " + instanceName + "_Read(void)");
               writer.WriteLine("{");
               writer.WriteLine("	return ( CY_GET_REG8(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WriteSeed
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint8 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("	CY_SET_REG8(" + instanceName + "_SEED_LOWER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint8 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint8 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine("{");
               writer.WriteLine("	return ( CY_GET_REG8(" + instanceName + "_POLYNOM_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WritePolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint8 polynomial)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint8 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine("	CY_SET_REG8(" + instanceName + "_POLYNOM_LOWER_PTR, polynomial);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion
           }
           else if (PRSSize <= 16)    /* 16bit - PRS */
           {
               #region Read
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_Read(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the current PRS value.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint16 " + instanceName + "_Read(void)");
               writer.WriteLine("{");
               writer.WriteLine("	return ( CY_GET_REG16(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               writer.WriteLine("");
               #endregion

               #region WriteSeed
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint16 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("	CY_SET_REG16(" + instanceName + "_SEED_LOWER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint16 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint16 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine("{");
               writer.WriteLine(" 	return ( CY_GET_REG16(" + instanceName + "_POLYNOM_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WritePolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint16 polynom)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint16 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine("	CY_SET_REG16(" + instanceName + "_POLYNOM_LOWER_PTR, polynomial);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion
           }
           else if (PRSSize <= 24)    /* 24bit - PRS */
           {
               #region Read
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_Read(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the current PRS value.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_Read(void)");
               writer.WriteLine("{");
               writer.WriteLine("	return ( CY_GET_REG24(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WriteSeed
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint32 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("	CY_SET_REG24(" + instanceName + "_SEED_LOWER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine("{");
               writer.WriteLine(" 	return ( CY_GET_REG24(" + instanceName + "_POLYNOM_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WritePolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint32 Polynomial)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine("	CY_SET_REG24(" + instanceName + "_POLYNOM_LOWER_PTR, polynomial);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion
           }
           else if (PRSSize <= 32)    /* 32bit - PRS */
           {
               #region Read
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_Read(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the current PRS value.");
               writer.WriteLine(" * ");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_Read(void)");
               writer.WriteLine("{");
               writer.WriteLine("	return ( CY_GET_REG32(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WriteSeed
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeed(uint32 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed register with the start value. ");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("	CY_SET_REG32(" + instanceName + "_SEED_LOWER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomial(void)");
               writer.WriteLine("{");
               writer.WriteLine("   	return ( CY_GET_REG32(" + instanceName + "_POLYNOM_LOWER_PTR) );");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WritePolynomial
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomial(uint32 polynomial)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomial(uint32 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine("  	CY_SET_REG32(" + instanceName + "_POLYNOM_LOWER_PTR, polynomial);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion
           }
           else  /* 64bit - PRS */
           {
               #region ReadUpper
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadUpper(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the current PRS Upper value. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadUpper(void)");
               writer.WriteLine("{");
               writer.WriteLine("//	return ( CY_GET_REG32(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("	return 0;");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadLower
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadLower(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" * 	Reads the current PRS Lower value. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadLower(void)");
               writer.WriteLine("{");
               writer.WriteLine("//	return ( CY_GET_REG32(" + instanceName + "_SEED_LOWER_PTR) );");
               writer.WriteLine("	return 0;");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WriteSeedUpper
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeedUpper(uint32 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed Upper register with the start value. ");
               writer.WriteLine(" *  Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("//	CY_SET_REG32(" + instanceName + "_SEED_UPPER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WriteSeedLower
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WriteSeedLower(uint32 seed)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS Seed Lower register with the start value. ");
               writer.WriteLine(" *  Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("//  CY_SET_REG32(" + instanceName + "_SEED_LOWER_PTR, seed);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomialUpper
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomialUpper(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial Upper. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialUpper(void)");
               writer.WriteLine("{");
               writer.WriteLine("//	return ( CY_GET_REG32(" + instanceName + "_POLYNOM_UPPER_PTR) );");
               writer.WriteLine("	return 0;	");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region ReadPolynomialLower
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: uint32 " + instanceName + "_ReadPolynomialLower(void)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Reads the PRS polynomial Lower. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("uint32 " + instanceName + "_ReadPolynomialLower(void)");
               writer.WriteLine("{");
               writer.WriteLine("//	return ( CY_GET_REG32(" + instanceName + "_POLYNOM_LOWER_PTR) );");
               writer.WriteLine("	return 0;");
               writer.WriteLine("}");
               #endregion

               #region WritePolynomialUpper
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomialUpper(uint32 polynomial)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial Upper. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomialUpper(uint32 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine("  	CY_SET_REG32(" + instanceName + "_POLYNOM_UPPER_PTR, polynomial);");
               writer.WriteLine("}");
               writer.WriteLine("");
               #endregion

               #region WritePolynomialLower
               writer.WriteLine("/*------------------------------------------------------------------------------");
               writer.WriteLine(" * FUNCTION NAME: void " + instanceName + "_WritePolynomialLower(uint32 polynomial)");
               writer.WriteLine(" *------------------------------------------------------------------------------");
               writer.WriteLine(" * Summary:");
               writer.WriteLine(" *  Writes the PRS polynomial Lower. Only generated for 33-64-bit PRS.");
               writer.WriteLine(" *");
               writer.WriteLine(" * Parameters:");
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
               writer.WriteLine("void " + instanceName + "_WritePolynomialLower(uint32 polynomial)");
               writer.WriteLine("{");
               writer.WriteLine(" 	//CY_SET_REG32(" + instanceName + "_POLYNOM_UPPER_PTR, polynomial);");
               writer.WriteLine("}");
           }
               #endregion
           #endregion
           paramDict.Add("DefineC", writer.ToString());

       }
    }
}
