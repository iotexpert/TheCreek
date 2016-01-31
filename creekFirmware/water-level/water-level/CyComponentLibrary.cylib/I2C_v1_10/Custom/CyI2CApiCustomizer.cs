/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace I2C_v1_10
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        #region API Customizer File Constants and Variables
        const string I2C_CFILE_NAME = "I2C.c";
        const string I2C_HFILE_NAME = "I2C.h";
        
        const string FF_CFILE_NAME = "I2C_FF.c";
        const string MASTER_FF_CFILE_NAME = "I2CM_FF.c";
        const string SLAVE_FF_CFILE_NAME = "I2CS_FF.c";
        const string MASTER_FF_HFILE_NAME = "I2CM_FF.h";
        const string SLAVE_FF_HFILE_NAME = "I2CS_FF.h";

        const string UDB_CFILE_NAME = "I2C_UDB.c";
        const string MASTER_UDB_CFILE_NAME = "I2CM_UDB.c";
        const string SLAVE_UDB_CFILE_NAME = "I2CS_UDB.c";
        const string MASTER_UDB_HFILE_NAME = "I2CM_UDB.h";
        const string SLAVE_UDB_HFILE_NAME = "I2CS_UDB.h";

        const string INT_CFILE_NAME = "I2CINT.c";



        CyAPICustomizer UDB_CFILE;
        CyAPICustomizer SLAVE_UDB_CFILE;
        CyAPICustomizer MASTER_UDB_CFILE;
        CyAPICustomizer MASTER_UDB_HFILE;
        CyAPICustomizer SLAVE_UDB_HFILE;

        CyAPICustomizer FF_CFILE;
        CyAPICustomizer MASTER_FF_CFILE;
        CyAPICustomizer SLAVE_FF_CFILE;
        CyAPICustomizer MASTER_FF_HFILE;
        CyAPICustomizer SLAVE_FF_HFILE;

        CyAPICustomizer INT_CFILE;
        CyAPICustomizer I2C_CFILE;
        CyAPICustomizer I2C_HFILE;
        #endregion

        #region Parameter Constants and Variables

        string instanceName = null;

        #endregion
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> inputCustomizers = new List<CyAPICustomizer>(apis);
            List<CyAPICustomizer> outputCustomizers = new List<CyAPICustomizer>();
            Dictionary<string, string> paramDict = null;
            instanceName = CyI2CInfo.GetInstanceName(query);
            CyI2CInfo.I2CMode mode;
            CyI2CInfo.ImplementationMode implementationMode;
            #region Param and file initialization

            FileInfo originalFilePath = new FileInfo(inputCustomizers[0].OriginalName);
            I2C_CFILE = new CyAPICustomizer(originalFilePath.DirectoryName + "\\" + instanceName + ".c", instanceName + ".c", null, null, CyCustBuildTypeEnum.C_FILE);

            // Get the parameters from one of the component files
            paramDict = inputCustomizers[0].MacroDictionary;

            // Get all the files
            foreach (CyAPICustomizer file in inputCustomizers)
            {
                if (file.OriginalName.EndsWith(FF_CFILE_NAME))
                    FF_CFILE = file;
                if (file.OriginalName.EndsWith(MASTER_FF_CFILE_NAME))
                    MASTER_FF_CFILE = file;
                if (file.OriginalName.EndsWith(SLAVE_FF_CFILE_NAME))
                    SLAVE_FF_CFILE = file;
                if (file.OriginalName.EndsWith(MASTER_FF_HFILE_NAME))
                    MASTER_FF_HFILE = file;
                if (file.OriginalName.EndsWith(SLAVE_FF_HFILE_NAME))
                    SLAVE_FF_HFILE = file;
                if (file.OriginalName.EndsWith(UDB_CFILE_NAME))
                    UDB_CFILE = file;
                if (file.OriginalName.EndsWith(MASTER_UDB_CFILE_NAME))
                    MASTER_UDB_CFILE = file;
                if (file.OriginalName.EndsWith(SLAVE_UDB_CFILE_NAME))
                    SLAVE_UDB_CFILE = file;
                if (file.OriginalName.EndsWith(MASTER_UDB_HFILE_NAME))
                    MASTER_UDB_HFILE = file;
                if (file.OriginalName.EndsWith(SLAVE_UDB_HFILE_NAME))
                    SLAVE_UDB_HFILE = file;
                if (file.OriginalName.EndsWith(INT_CFILE_NAME))
                    INT_CFILE = file;
                if (file.OriginalName.EndsWith(I2C_HFILE_NAME))
                    I2C_HFILE = file;
            }

            // Add the file header first
            outputCustomizers.Add(I2C_CFILE);
            outputCustomizers.Add(INT_CFILE);
            outputCustomizers.Add(I2C_HFILE);
            mode = CyI2CInfo.GetI2CMode(query);
            implementationMode = CyI2CInfo.GetImplementationMode(query);

            #endregion

            if (implementationMode == CyI2CInfo.ImplementationMode.FIXEDFUNCTION)
                GetFixedFunctionFiles(mode);
            else
                GetUDBFiles(mode);

            // EnableWakeup?
            //if (CyI2CInfo.GetEnableWakeup(query))
            // add it. 
            //  ;

            // Replace macro dictionaries with paramDict
            foreach (CyAPICustomizer file in outputCustomizers)
            {
                file.MacroDictionary = paramDict;
            }

            return outputCustomizers;
        }
        #endregion

        void GetUDBFiles(CyI2CInfo.I2CMode mode)
        {
            // For each case, return the appropriate file to be added
            switch (mode)
            {
                case CyI2CInfo.I2CMode.SLAVE:
                    I2C_CFILE.OutputContent += UDB_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += SLAVE_UDB_CFILE.OutputContent;
                    I2C_HFILE.OutputContent += SLAVE_UDB_HFILE.OutputContent;
                    break;
                case CyI2CInfo.I2CMode.MASTER:
                case CyI2CInfo.I2CMode.MULTI_MASTER:
                    I2C_CFILE.OutputContent += UDB_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += MASTER_UDB_CFILE.OutputContent;
                    I2C_HFILE.OutputContent += MASTER_UDB_HFILE.OutputContent;
                    break;
                case CyI2CInfo.I2CMode.MULTI_MASTER_SLAVE:
                    I2C_CFILE.OutputContent += UDB_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += MASTER_UDB_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += SLAVE_UDB_CFILE.OutputContent;
                    I2C_HFILE.OutputContent += MASTER_UDB_HFILE.OutputContent;
                    I2C_HFILE.OutputContent += SLAVE_UDB_HFILE.OutputContent;
                    break;
                default:
                    break;
            }
            
            I2C_HFILE.OutputContent += Environment.NewLine + "#endif /* `$INSTANCE_NAME`_H */"+ Environment.NewLine;
            I2C_HFILE.OutputContent += Environment.NewLine + "/* [] END OF FILE */" + Environment.NewLine;
        }

        void GetFixedFunctionFiles(CyI2CInfo.I2CMode mode)
        {
            // For each case, return the appropriate file to be added
            switch (mode)
            {
                case CyI2CInfo.I2CMode.SLAVE:
                    I2C_CFILE.OutputContent += FF_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += SLAVE_FF_CFILE.OutputContent;
                    I2C_HFILE.OutputContent += SLAVE_FF_HFILE.OutputContent;
                    break;
                case CyI2CInfo.I2CMode.MASTER:
                case CyI2CInfo.I2CMode.MULTI_MASTER:
                    I2C_CFILE.OutputContent += FF_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += MASTER_FF_CFILE.OutputContent;
                    I2C_HFILE.OutputContent += MASTER_FF_HFILE.OutputContent;
                    break;
                case CyI2CInfo.I2CMode.MULTI_MASTER_SLAVE:
                    I2C_CFILE.OutputContent += FF_CFILE.OutputContent;
                    I2C_CFILE.OutputContent += SLAVE_FF_CFILE.OriginalContent;
                    I2C_CFILE.OutputContent += MASTER_FF_CFILE.OriginalContent;
                    I2C_HFILE.OutputContent += MASTER_FF_HFILE.OutputContent;
                    I2C_HFILE.OutputContent += SLAVE_FF_HFILE.OutputContent;
                    break;
                default:
                    break;
            }
            
            I2C_HFILE.OutputContent += Environment.NewLine + "#endif /* `$INSTANCE_NAME`_H */"+ Environment.NewLine;
            I2C_HFILE.OutputContent += Environment.NewLine + "/* [] END OF FILE */" + Environment.NewLine;
        }
    }


    internal abstract class CyI2CInfo
    {
        //------------------------------------

        const string instanceNameParamName = "INSTANCE_NAME";
        const string I2CModeParamName = "I2C_Mode";
        const string ImplementationParamName = "Implementation";
        const string EnableWakeupParamName = "EnableWakeup";
        const string UDB_MODE = "UDB";

        public static string GetInstanceName(ICyInstQuery_v1 query)
        {
            return query.GetCommittedParam(instanceNameParamName).Value;
        }

        public static CyI2CInfo.ImplementationMode GetImplementationMode(ICyInstQuery_v1 query)
        {
            return (ImplementationMode)byte.Parse(query.GetCommittedParam(ImplementationParamName).Value);
        }

        public static CyI2CInfo.I2CMode GetI2CMode(ICyInstQuery_v1 query)
        {
            return (I2CMode)byte.Parse(query.GetCommittedParam(I2CModeParamName).Value);
        }

        public static bool GetEnableWakeup(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(EnableWakeupParamName);

            bool result;

            param.TryGetValueAs<bool>(out result);

            return result;
        }

        public enum ImplementationMode
        {
            UDB, FIXEDFUNCTION
        }

        public enum I2CMode
        {
            SLAVE = 1, MASTER, MULTI_MASTER = 6, MULTI_MASTER_SLAVE
        }
    }

}