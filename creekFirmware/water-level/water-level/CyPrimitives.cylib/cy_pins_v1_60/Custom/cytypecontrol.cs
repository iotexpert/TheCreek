/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using CyDesigner.Common.Base;

using Cypress.Comps.PinsAndPorts.Common_v1_60;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_60
{
    public partial class CyTypeControl : UserControl
    {
        CyPerPinDataControl m_perPinDataControl;
        bool m_isGen4;

        public CyPerPinDataControl PerPinDataControl
        {
            get { return m_perPinDataControl; }
            set { m_perPinDataControl = value; }
        }

        public CyTypeControl(bool isGen4)
        {
            m_isGen4 = isGen4;

            InitializeComponent();

            m_displayDigitalInputCheckBox.DataBindings.Add(new Binding("Enabled", m_digInputCheckBox, "Checked"));
            m_displayDigitalOutputCheckBox.DataBindings.Add(new Binding("Enabled", m_digOutputCheckBox, "Checked"));

            m_bidirCheckBox.CheckedChanged += new EventHandler(m_bidirCheckBox_CheckedChanged);
            m_analogCheckBox.CheckedChanged += new EventHandler(m_analogCheckBox_CheckedChanged);
            m_digInputCheckBox.CheckedChanged += new EventHandler(m_digInputCheckBox_CheckedChanged);
            m_digOutputCheckBox.CheckedChanged += new EventHandler(m_digOutputCheckBox_CheckedChanged);
            m_oeCheckBox.CheckedChanged += new EventHandler(m_oeCheckBox_CheckedChanged);
            m_displayDigitalInputCheckBox.CheckedChanged +=
                new EventHandler(m_displayDigitalInputCheckBox_CheckedChanged);
            m_displayDigitalOutputCheckBox.CheckedChanged +=
                new EventHandler(m_displayDigitalOutputCheckBox_CheckedChanged);

            m_errorProvider.SetIconAlignment(m_previewLabel, ErrorIconAlignment.MiddleRight);
            m_errorProvider.SetIconPadding(m_previewLabel, 3);

            m_analogCheckBox.Enabled =
                    !m_isGen4 ||
                    m_analogCheckBox.CheckState == CheckState.Checked ||
                    m_analogCheckBox.CheckState == CheckState.Indeterminate;
        }

        void m_displayDigitalOutputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_DisplayOutputHWConnections, DisplayOutputHWConnections));
        }

        void m_displayDigitalInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_DisplayInputHWConnections, DisplayInputHWConnections));
        }

        void m_oeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnPinTypeChanged(AnalogCheckState, InputCheckState, OutputCheckState, OECheckState,
                BidirCheckState);
        }

        void m_digOutputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_digOutputCheckBox.Checked == false && m_digInputCheckBox.Checked == false &&
                m_analogCheckBox.Checked == false && m_bidirCheckBox.Checked == false)
            {
                MessageBox.Show("Cannot uncheck all Pin Type options.", "Operation Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                m_digOutputCheckBox.Checked = true;
            }
            else
            {
                if (m_digOutputCheckBox.Checked)
                {
                    m_bidirCheckBox.Checked = false;
                }
                PerPinDataControl.OnPinTypeChanged(AnalogCheckState, InputCheckState, OutputCheckState, OECheckState,
                    BidirCheckState);

                UpdateOECheckBoxEnabledState();
            }
        }

        void m_digInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_digOutputCheckBox.Checked == false && m_digInputCheckBox.Checked == false &&
                m_analogCheckBox.Checked == false && m_bidirCheckBox.Checked == false)
            {
                MessageBox.Show("Cannot uncheck all Pin Type options.", "Operation Not Allowed",
                     MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                m_digInputCheckBox.Checked = true;
            }
            else
            {
                if (m_digInputCheckBox.Checked)
                {
                    m_bidirCheckBox.Checked = false;
                }
                PerPinDataControl.OnPinTypeChanged(AnalogCheckState, InputCheckState, OutputCheckState, OECheckState,
                    BidirCheckState);
            }
        }

        void m_analogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_digOutputCheckBox.Checked == false && m_digInputCheckBox.Checked == false &&
                m_analogCheckBox.Checked == false && m_bidirCheckBox.Checked == false)
            {
                MessageBox.Show("Cannot uncheck all Pin Type options.", "Operation Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                m_analogCheckBox.Checked = true;
            }
            else
            {
                PerPinDataControl.OnPinTypeChanged(AnalogCheckState, InputCheckState, OutputCheckState, OECheckState,
                    BidirCheckState);
            }
        }

        void m_bidirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_digOutputCheckBox.Checked == false && m_digInputCheckBox.Checked == false &&
                 m_analogCheckBox.Checked == false && m_bidirCheckBox.Checked == false)
            {
                MessageBox.Show("Cannot uncheck all Pin Type options.", "Operation Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                m_bidirCheckBox.Checked = true;
            }
            else
            {
                if (m_bidirCheckBox.Checked)
                {
                    m_digInputCheckBox.Checked = false;
                    m_digOutputCheckBox.Checked = false;
                }

                PerPinDataControl.OnPinTypeChanged(AnalogCheckState, InputCheckState, OutputCheckState, OECheckState,
                    BidirCheckState);
            }
        }

        void UpdateOECheckBoxEnabledState()
        {
            m_oeCheckBox.Enabled = (m_digOutputCheckBox.CheckState == CheckState.Indeterminate) ?
                    false : m_digOutputCheckBox.Checked;
        }

        void UpdatePreviewPic()
        {
            m_pinPicture.ShowAnalog = AnalogCheckState == CheckState.Checked;
            m_pinPicture.ShowDigitalInput = InputCheckState == CheckState.Checked;
            m_pinPicture.ShowDigitalOutput = OutputCheckState == CheckState.Checked;
            m_pinPicture.ShowBidirectional = BidirCheckState == CheckState.Checked;
            m_pinPicture.ShowOutputEnable = OECheckState == CheckState.Checked;
            m_pinPicture.ShowDigitalInputConnection = m_displayDigitalInputCheckBox.CheckState == CheckState.Checked;
            m_pinPicture.ShowDigitalOutputConnection = m_displayDigitalOutputCheckBox.CheckState == CheckState.Checked;
            m_pinPicture.ShowAnnotation = AnnotCheckState == CheckState.Checked;
            m_pinPicture.Invalidate();
        }

        public CheckState AnalogCheckState
        {
            get { return m_analogCheckBox.CheckState; }
            set
            {
                m_analogCheckBox.CheckedChanged -= m_analogCheckBox_CheckedChanged;
                m_analogCheckBox.CheckState = value;
                UpdatePreviewPic();
                m_analogCheckBox.CheckedChanged += new EventHandler(m_analogCheckBox_CheckedChanged);
                m_analogCheckBox.Enabled =
                    !m_isGen4 ||
                    m_analogCheckBox.CheckState == CheckState.Checked ||
                    m_analogCheckBox.CheckState == CheckState.Indeterminate;
            }
        }

        public CheckState InputCheckState
        {
            get { return m_digInputCheckBox.CheckState; }
            set
            {
                m_digInputCheckBox.CheckedChanged -= m_digInputCheckBox_CheckedChanged;
                m_digInputCheckBox.CheckState = value;
                UpdatePreviewPic();
                m_digInputCheckBox.CheckedChanged += new EventHandler(m_digInputCheckBox_CheckedChanged);
            }
        }

        public CheckState OutputCheckState
        {
            get { return m_digOutputCheckBox.CheckState; }
            set
            {
                m_digOutputCheckBox.CheckedChanged -= m_digOutputCheckBox_CheckedChanged;
                m_digOutputCheckBox.CheckState = value;
                UpdateOECheckBoxEnabledState();
                UpdatePreviewPic();
                m_digOutputCheckBox.CheckedChanged += new EventHandler(m_digOutputCheckBox_CheckedChanged);
            }
        }

        public CheckState OECheckState
        {
            get { return m_oeCheckBox.CheckState; }
            set
            {
                m_oeCheckBox.CheckedChanged -= m_oeCheckBox_CheckedChanged;
                m_oeCheckBox.CheckState = value;
                UpdatePreviewPic();
                m_oeCheckBox.CheckedChanged += new EventHandler(m_oeCheckBox_CheckedChanged);
            }
        }

        public CheckState BidirCheckState
        {
            get { return m_bidirCheckBox.CheckState; }
            set
            {
                m_bidirCheckBox.CheckedChanged -= m_bidirCheckBox_CheckedChanged;
                m_bidirCheckBox.CheckState = value;
                UpdatePreviewPic();
                m_bidirCheckBox.CheckedChanged += new EventHandler(m_bidirCheckBox_CheckedChanged);
            }
        }

        public CheckState AnnotCheckState
        {
            get { return m_annotCheckBox.CheckState; }
            set
            {
                m_annotCheckBox.CheckedChanged -= m_annotCheckBox_CheckedChanged;
                m_annotCheckBox.CheckState = value;
                UpdatePreviewPic();
                m_annotCheckBox.CheckedChanged += new EventHandler(m_annotCheckBox_CheckedChanged);
            }
        }

        public string DisplayInputHWConnections
        {
            get
            {
                if (m_displayDigitalInputCheckBox.CheckState == CheckState.Indeterminate)
                {
                    return null;
                }
                if (m_displayDigitalInputCheckBox.Checked)
                {
                    return CyPortConstants.Display_TRUE;
                }
                return CyPortConstants.Display_FALSE;
            }
            set
            {
                m_displayDigitalInputCheckBox.CheckedChanged -= m_displayDigitalInputCheckBox_CheckedChanged;
                if (value == null)
                {
                    m_displayDigitalInputCheckBox.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.Display_FALSE:
                            m_displayDigitalInputCheckBox.CheckState = CheckState.Unchecked;
                            m_displayDigitalInputCheckBox.Checked = false;
                            break;

                        case CyPortConstants.Display_TRUE:
                            m_displayDigitalInputCheckBox.CheckState = CheckState.Checked;
                            m_displayDigitalInputCheckBox.Checked = true;
                            break;

                        default:
                            m_displayDigitalInputCheckBox.CheckState = CheckState.Indeterminate;
                            break;
                    }
                }
                UpdatePreviewPic();
                m_displayDigitalInputCheckBox.CheckedChanged +=
                    new EventHandler(m_displayDigitalInputCheckBox_CheckedChanged);
            }
        }

        public string DisplayOutputHWConnections
        {
            get
            {
                if (m_displayDigitalOutputCheckBox.CheckState == CheckState.Indeterminate)
                {
                    return null;
                }
                if (m_displayDigitalOutputCheckBox.Checked)
                {
                    return CyPortConstants.Display_TRUE;
                }
                return CyPortConstants.Display_FALSE;
            }
            set
            {
                m_displayDigitalOutputCheckBox.CheckedChanged -= m_displayDigitalOutputCheckBox_CheckedChanged;
                if (value == null)
                {
                    m_displayDigitalOutputCheckBox.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.Display_FALSE:
                            m_displayDigitalOutputCheckBox.CheckState = CheckState.Unchecked;
                            m_displayDigitalOutputCheckBox.Checked = false;
                            break;

                        case CyPortConstants.Display_TRUE:
                            m_displayDigitalOutputCheckBox.CheckState = CheckState.Checked;
                            m_displayDigitalOutputCheckBox.Checked = true;
                            break;

                        default:
                            m_displayDigitalOutputCheckBox.CheckState = CheckState.Indeterminate;
                            break;
                    }
                }
                UpdatePreviewPic();
                m_displayDigitalOutputCheckBox.CheckedChanged +=
                    new EventHandler(m_displayDigitalOutputCheckBox_CheckedChanged);
            }
        }

        public string DisplayInputHWConnectionsErrorText
        {
            get { return m_errorProvider.GetError(m_displayDigitalInputCheckBox); }
            set { m_errorProvider.SetError(m_displayDigitalInputCheckBox, value); }
        }

        public string DisplayOutputConnectionsErrorText
        {
            get { return m_errorProvider.GetError(m_displayDigitalOutputCheckBox); }
            set { m_errorProvider.SetError(m_displayDigitalOutputCheckBox, value); }
        }

        public string ShowAnnotationTermErrorText
        {
            get { return m_errorProvider.GetError(m_annotCheckBox); }
            set { m_errorProvider.SetError(m_annotCheckBox, value); }
        }

        public string PinTypeErrorText
        {
            get { return m_errorProvider.GetError(m_previewLabel); }
            set { m_errorProvider.SetError(m_previewLabel, value); }
        }

        public string ShowAnnotationTerm
        {
            get
            {
                if (m_annotCheckBox.CheckState == CheckState.Indeterminate)
                {
                    return null;
                }
                if (m_annotCheckBox.Checked)
                {
                    return CyPortConstants.ShowAnnotValue_TRUE;
                }
                return CyPortConstants.ShowAnnotValue_FALSE;
            }

            set
            {
                m_annotCheckBox.CheckedChanged -= m_annotCheckBox_CheckedChanged;
                if (value == null)
                {
                    m_annotCheckBox.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.ShowAnnotValue_FALSE:
                            m_annotCheckBox.CheckState = CheckState.Unchecked;
                            m_annotCheckBox.Checked = false;
                            break;

                        case CyPortConstants.HotSwapValue_TRUE:
                            m_annotCheckBox.CheckState = CheckState.Checked;
                            m_annotCheckBox.Checked = true;
                            break;

                        default:
                            m_annotCheckBox.CheckState = CheckState.Indeterminate;
                            break;
                    }
                }
                UpdatePreviewPic();
                m_annotCheckBox.CheckedChanged += new EventHandler(m_annotCheckBox_CheckedChanged);
            }
        }

        private void m_annotCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_perPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
            CyParamInfo.Formal_ParamName_ShowAnnotationTerm,ShowAnnotationTerm));
        }
    }

    public class CyPinPicture : PictureBox
    {
        bool m_showAnalog;
        bool m_showDigitalInput;
        bool m_showDigitalOutput;
        bool m_showOutputEnable;
        bool m_showBidir;
        bool m_showDigInConn;
        bool m_showDigOutConn;
        bool m_showAnnotation;
        Dictionary<string, Metafile> m_pics;

        const string BidirImageKey = "Bidir";
        const string AnalogImageKey = "Analog";
        const string DigInImageKey = "DigIn";
        const string DigOutImageKey = "DigOut";
        const string DigInNCImageKey = "DigInNC";
        const string DigOutNCImageKey = "DigOutNC";
        const string DigInAnalogImageKey = "DigInAnalog";
        const string DigOutOEImageKey = "DigOutOE";
        const string DigInNCAnalogImageKey = "DigInNCAnalog";
        const string DigOutNCOEImageKey = "DigOutNCOE";
        const string AnnLeftImageKey = "AnnLeft";
        const string AnnRightImageKey = "AnnRight";
        const string Ann1DownLeftImageKey = "Ann1DownLeft";
        const string Ann1DownRightImageKey = "Ann1DownRight";
        const string Ann2DownLeftImageKey = "Ann2DownLeft";

        public bool ShowAnalog
        {
            get { return m_showAnalog; }
            set
            {
                m_showAnalog = value;
            }
        }

        public bool ShowDigitalInput
        {
            get { return m_showDigitalInput; }
            set
            {
                m_showDigitalInput = value;
            }
        }

        public bool ShowDigitalOutput
        {
            get { return m_showDigitalOutput; }
            set
            {
                m_showDigitalOutput = value;
            }
        }

        public bool ShowOutputEnable
        {
            get { return m_showOutputEnable; }
            set
            {
                m_showOutputEnable = value;
            }
        }

        public bool ShowBidirectional
        {
            get { return m_showBidir; }
            set
            {
                m_showBidir = value;
            }
        }

        public bool ShowDigitalInputConnection
        {
            get { return m_showDigInConn; }
            set
            {
                m_showDigInConn = value;
            }
        }

        public bool ShowDigitalOutputConnection
        {
            get { return m_showDigOutConn; }
            set
            {
                m_showDigOutConn = value;
            }
        }

        public bool ShowAnnotation
        {
            get { return m_showAnnotation; }
            set
            {
                m_showAnnotation = value;
            }
        }

        public CyPinPicture()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            m_pics = new Dictionary<string, Metafile>();

            using (MemoryStream ms = new MemoryStream(Resource1.BidirEMF))
            {
                m_pics.Add(BidirImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.AnalogEMF))
            {
                m_pics.Add(AnalogImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigInEMF))
            {
                m_pics.Add(DigInImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigOutEMF))
            {
                m_pics.Add(DigOutImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigInNCEMF))
            {
                m_pics.Add(DigInNCImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigOutNCEMF))
            {
                m_pics.Add(DigOutNCImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigInAnalogEMF))
            {
                m_pics.Add(DigInAnalogImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigOutOEEMF))
            {
                m_pics.Add(DigOutOEImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigInNCAnalogEMF))
            {
                m_pics.Add(DigInNCAnalogImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.DigOutNCOEEMF))
            {
                m_pics.Add(DigOutNCOEImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.AnnLeftEMF))
            {
                m_pics.Add(AnnLeftImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.AnnRightEMF))
            {
                m_pics.Add(AnnRightImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.Ann1DownLeftEMF))
            {
                m_pics.Add(Ann1DownLeftImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.Ann1DownRightEMF))
            {
                m_pics.Add(Ann1DownRightImageKey, new Metafile(ms));
            }
            using (MemoryStream ms = new MemoryStream(Resource1.Ann2DownLeftEMF))
            {
                m_pics.Add(Ann2DownLeftImageKey, new Metafile(ms));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle border = new Rectangle(0, 0, this.Width, this.Height);
            e.Graphics.Clear(BackColor);

            //Deal with left side (dig out, oe, bidir).
            if (ShowBidirectional)
            {
                if (ShowAnnotation)
                {
                    if (ShowAnalog)
                    {
                        e.Graphics.DrawImage(m_pics[Ann1DownLeftImageKey], border);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_pics[AnnRightImageKey], border);
                    }
                }
                e.Graphics.DrawImage(m_pics[BidirImageKey], border);
            }
            else if (ShowDigitalOutput)
            {
                if (ShowOutputEnable)
                {
                    if (ShowAnnotation)
                    {
                        if (ShowAnalog || ShowDigitalInput)
                        {
                            if (ShowAnalog && ShowDigitalInput)
                            {
                                e.Graphics.DrawImage(m_pics[Ann2DownLeftImageKey], border);
                            }
                            else
                            {
                                e.Graphics.DrawImage(m_pics[Ann1DownRightImageKey], border);
                            }
                        }
                        else
                        {
                            e.Graphics.DrawImage(m_pics[AnnRightImageKey], border);
                        }
                    }
                    if (ShowDigitalOutputConnection)
                    {
                        e.Graphics.DrawImage(m_pics[DigOutOEImageKey], border);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_pics[DigOutNCOEImageKey], border);
                    }
                }
                else
                {
                    if (ShowAnnotation)
                    {
                        if (ShowAnalog || ShowDigitalInput)
                        {
                            e.Graphics.DrawImage(m_pics[Ann1DownLeftImageKey], border);
                        }
                        else
                        {
                            e.Graphics.DrawImage(m_pics[AnnRightImageKey], border);
                        }
                    }
                    if (ShowDigitalOutputConnection)
                    {
                        e.Graphics.DrawImage(m_pics[DigOutImageKey], border);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_pics[DigOutNCImageKey], border);
                    }
                }
            }

            //Deal with right side (dig in, analog).
            if (ShowDigitalInput)
            {
                if (ShowAnnotation && !ShowBidirectional && !ShowDigitalOutput)
                {
                    e.Graphics.DrawImage(m_pics[AnnLeftImageKey], border);
                }
                if (ShowAnalog)
                {
                    if (ShowDigitalInputConnection)
                    {
                        e.Graphics.DrawImage(m_pics[DigInAnalogImageKey], border);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_pics[DigInNCAnalogImageKey], border);
                    }
                }
                else
                {
                    if (ShowDigitalInputConnection)
                    {
                        e.Graphics.DrawImage(m_pics[DigInImageKey], border);
                    }
                    else
                    {
                        e.Graphics.DrawImage(m_pics[DigInNCImageKey], border);
                    }
                }
            }
            else if (ShowAnalog)
            {
                if (ShowAnnotation && !ShowBidirectional && !ShowDigitalOutput)
                {
                    e.Graphics.DrawImage(m_pics[AnnLeftImageKey], border);
                }
                e.Graphics.DrawImage(m_pics[AnalogImageKey], border);
            }

            base.OnPaint(e);
        }
    }
}
