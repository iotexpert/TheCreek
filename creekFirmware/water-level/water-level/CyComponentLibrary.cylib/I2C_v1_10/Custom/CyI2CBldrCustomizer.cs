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
    partial class CyCustomizer : ICyBootLoaderSupport
    {
        #region ICyBootLoaderSupport Members

        /// <summary>
        /// The bootloader requires that the communication component is configured for
        /// both transfer in and out of the PSoC device. This method lets the implementing
        /// component inform PSoC Creator if it is currently configured to handle input and
        /// output.
        /// <param name="query">The ICyInstQuery for the relevant instance of the component.</param>
        /// </summary>
        public CyCustErr IsBootloaderReady(ICyInstQuery_v1 query)
        {
            Debug.Assert(query != null);
            CyI2CInfo.I2CMode mode = CyI2CInfo.GetI2CMode(query);
            CyCustErr supportBootloader = CyCustErr.OK;
            switch (mode)
            {
                case CyI2CInfo.I2CMode.SLAVE:
                    supportBootloader = CyCustErr.OK;
                    break;
                case CyI2CInfo.I2CMode.MASTER:
                case CyI2CInfo.I2CMode.MULTI_MASTER:
                case CyI2CInfo.I2CMode.MULTI_MASTER_SLAVE:
                default:
                    supportBootloader = new CyCustErr(Resource01.I2CNotSlave, query.ComponentName);
                    break;
            }

            if (CyI2CInfo.GetImplementationMode(query) == CyI2CInfo.ImplementationMode.FIXEDFUNCTION)
                return supportBootloader;
            else
                return new CyCustErr(Resource01.I2CNotFixedFunction, query.ComponentName);
        }

        #endregion
    }
}
