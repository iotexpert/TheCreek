/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;

namespace QuadDec_v1_50
{
    public partial class CyDiagram : UserControl
    {
        private Bitmap m_backBuffer;
        private Graphics m_graphics;
        private Brush m_foreBrush;
        private Pen m_pen;
        private int m_clockWidth;
        private int m_clockHeight;
        private int m_abSignalLen;
        private int m_noiseWidth;
        private int m_baseX;
        private byte m_counterRes;
        private bool m_useIndex;
        private bool m_useFiltering;

        public CyDiagram()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Font = new Font(FontFamily.GenericSansSerif, 13, FontStyle.Regular);

            m_backBuffer = new Bitmap(this.Width, this.Height);
            m_graphics = Graphics.FromImage(m_backBuffer);
            m_foreBrush = Brushes.Black;
            m_pen = new Pen(m_foreBrush, 2);
            m_clockWidth = 5;
            m_clockHeight = 13;
            m_abSignalLen = 12;
            m_noiseWidth = 5;
            m_baseX = 60;
            m_counterRes = 4;
        }

        #region Properties
        public byte CounterResolution
        {
            get { return m_counterRes; }
            set 
            {
                if (value == 1 || value == 2 || value == 4)
                {
                    m_counterRes = value;
                }
                else
                {
                    m_counterRes = 0;
                }
                this.Refresh();
            }
        }

        public bool UseIndex
        {
            get { return m_useIndex; }
            set 
            {
                m_useIndex = value;
                this.Refresh();
            }
        }

        public bool UseFiltering
        {
            get { return m_useFiltering; }
            set 
            {
                m_useFiltering = value;
                m_baseX = m_useFiltering ? 85 : 60;
                this.Refresh();
            }
        }
        #endregion

        private void DrawStep(Point basePoint,int ticks, bool inverse, bool noisy)
        {
            GraphicsPath step = new GraphicsPath();
            Point topLeftPoint = new Point(
                noisy?basePoint.X + m_noiseWidth:basePoint.X,
                inverse ? basePoint.Y + m_clockHeight : basePoint.Y - m_clockHeight);
            Point topRightPoint = new Point(
                topLeftPoint.X + m_clockWidth * ticks,
                topLeftPoint.Y);
            Point bottomPoint = new Point(
                noisy?basePoint.X + m_clockWidth * ticks + m_noiseWidth*2:basePoint.X + m_clockWidth * ticks,
                basePoint.Y);
            Point endPoint = new Point(
                basePoint.X + m_clockWidth * 2 * ticks,
                basePoint.Y);

            step.AddLine(basePoint,topLeftPoint);
            step.AddLine(topLeftPoint, topRightPoint);
            step.AddLine(topRightPoint, bottomPoint);
            step.AddLine(bottomPoint, endPoint);

            m_graphics.DrawPath(m_pen, step);
        }

        private void DrawSignal(Point startPoint, int startTick, int stepLen, bool noisy)
        {
            Point basePoint = new Point(m_clockWidth * 2 * startTick, startPoint.Y);
            int stepCount = this.Width / (stepLen * m_clockWidth * 2);

            if (noisy)
            {
                int noiseTicks = 2;
                Point noisePoint = new Point(
                    basePoint.X - m_clockWidth * 8,
                    basePoint.Y);
                Point endNoisePoint = new Point(
                    noisePoint.X + noiseTicks * m_clockWidth + m_noiseWidth*2,
                    noisePoint.Y);
                m_graphics.DrawLine(m_pen, startPoint, noisePoint);
                DrawStep(noisePoint, noiseTicks, false, true);
                m_graphics.DrawLine(m_pen, endNoisePoint, basePoint);
            }
            else
            {
                m_graphics.DrawLine(m_pen, startPoint, basePoint);
            }

            for (int i = 0; i < stepCount; i++)
            {
                DrawStep(basePoint, stepLen, false, false);
                basePoint.Offset(m_clockWidth * 2 * stepLen, 0);
            }
        }

        private void DrawCounterSignal(Point startPoint, int startTick)
        {
            Point basePoint = new Point(m_clockWidth * 2 * startTick, startPoint.Y);
            int stepCount = this.Width / (2 * m_clockWidth * 2);

            m_graphics.DrawLine(m_pen, startPoint, basePoint);

            Point endStepPoint;
            if (m_counterRes != 2)
            {
                for (int i = 0; i < stepCount; i++)
                {
                    DrawStep(basePoint, 2, false, false);
                    endStepPoint = new Point(basePoint.X + 4 * m_clockWidth, basePoint.Y);
                    basePoint.Offset(m_clockWidth * 2 * m_abSignalLen / m_counterRes, 0);
                    m_graphics.DrawLine(m_pen, endStepPoint, basePoint);
                }
            }
            else
            {
                for (int i = 0; i < stepCount; i++)
                {
                    DrawStep(basePoint, 2, false, false);
                    endStepPoint = new Point(basePoint.X + 4 * m_clockWidth, basePoint.Y);
                    basePoint.Offset(m_clockWidth * 2 * 3, 0);
                    m_graphics.DrawLine(m_pen, endStepPoint, basePoint);

                    DrawStep(basePoint, 2, false, false);
                    endStepPoint = new Point(basePoint.X + 4 * m_clockWidth, basePoint.Y);
                    basePoint.Offset(m_clockWidth * 2 * 9, 0);
                    m_graphics.DrawLine(m_pen, endStepPoint, basePoint);
                }
            }
        }

        private void DrawIndex(Point startPoint, int startTick)
        {
            Point basePoint = new Point(m_clockWidth * 2 * startTick, startPoint.Y);
            Point bottomPoint = new Point(basePoint.X + m_clockWidth * 6, basePoint.Y);
            Point endPoint = new Point(this.Width, bottomPoint.Y);

            m_graphics.DrawLine(m_pen, startPoint, basePoint);
            DrawStep(basePoint, 6, true, false);
            m_graphics.DrawLine(m_pen, bottomPoint, endPoint);

            int resetLen = (m_abSignalLen / 4) * m_clockWidth + (int)m_pen.Width;

            Pen cicreledPen = new Pen(Color.Red);
            Point circeledPoint = new Point(basePoint.X - m_clockWidth*6, m_clockHeight/2);
            Rectangle circledRect = new Rectangle(circeledPoint,
                new Size(resetLen * 6, circeledPoint.Y + bottomPoint.Y + m_clockHeight / 2));
            cicreledPen.DashStyle = DashStyle.Dash;
            cicreledPen.Width = 2;
            m_graphics.DrawEllipse(cicreledPen, circledRect);
        }

        private void DrawString(String text, Point signalBasePoint)
        {
            float baseY = signalBasePoint.Y;
            SizeF textSize = m_graphics.MeasureString(text, this.Font);
            PointF textPoint = new PointF(signalBasePoint.X - textSize.Width - 5, baseY - textSize.Height);

            m_graphics.FillRectangle(Brushes.White, new RectangleF(textPoint, textSize));
            m_graphics.DrawString(text, this.Font, m_foreBrush, textPoint);
        }

        private void DrawClock()
        {
            int baseY = m_clockHeight * 2;
            Point basePoint = new Point(m_baseX, baseY);

            DrawString("Clock", basePoint);
            DrawSignal(basePoint, 10, 1, false);
        }

        private void DrawA()
        {
            int baseY = m_clockHeight * 4;
            Point basePoint = new Point(m_baseX, baseY);

            DrawString("A", basePoint);
            DrawSignal(basePoint, 15, m_abSignalLen, false);
        }

        private void DrawB()
        {
            int baseY = m_clockHeight*6;
            Point basePoint = new Point(m_baseX, baseY);

            DrawString("B", basePoint);
            DrawSignal(basePoint, 18, m_abSignalLen, m_useFiltering);
        }

        private void DrawAFiltered()
        {
            if (m_useFiltering)
            {
                int baseY = m_clockHeight * 8;
                Point basePoint = new Point(m_baseX, baseY);

                DrawString("A filtered", basePoint);
                DrawSignal(basePoint, 18, m_abSignalLen, false);
            }
        }

        private void DrawBFiltered()
        {
            if (m_useFiltering)
            {
                int baseY = m_clockHeight * 10;
                Point basePoint = new Point(m_baseX, baseY);

                DrawString("B filtered", basePoint);
                DrawSignal(basePoint, 21, m_abSignalLen, false);
            }
        }

        private void DrawIndex()
        {
            if (m_useIndex)
            {
                int baseY = m_clockHeight * 7;
                Point basePoint = new Point(m_baseX, baseY);
                Point basePointForString = new Point(basePoint.X, basePoint.Y + m_clockHeight);

                DrawString("Index", basePointForString);
                DrawIndex(basePoint, 36);
            }
        }

        private void DrawCounter()
        {
            int baseY = CalcCounterBaseY();
            Point basePoint = new Point(m_baseX, baseY);

            DrawString(m_counterRes.ToString() + "x", basePoint);

            int startTick = 0;
            if (m_useFiltering)
            {
                startTick = 19;
            }
            else
            {
                startTick = 16;
            }
            DrawCounterSignal(basePoint, startTick);
        }

        private int CalcCounterBaseY()
        {
            if (m_useFiltering)
            {
                return m_clockHeight * 12;
            }
            else if (m_useIndex)
            {
                return m_clockHeight * 10;
            }
            else
            {
                return m_clockHeight * 8;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!m_useFiltering && !m_useIndex)
            {
                m_clockHeight = (int)(this.Height / 9);
            }
            else if (m_useIndex)
            {
                m_clockHeight = (int)(this.Height / 11);
            }
            else
            {
                m_clockHeight = (int)(this.Height / 13.3);
            }
            m_clockWidth = (int)(this.Width / 92);
            m_backBuffer = new Bitmap(this.Width, this.Height);
            m_graphics = Graphics.FromImage(m_backBuffer);

            float fontSize;
            if (m_clockHeight > 0 && m_clockHeight < 12)
            {
                fontSize = m_clockHeight;
            }
            else
            {
                fontSize = 11;
            }
            this.Font = new Font(this.Font.FontFamily, fontSize * 96 / m_graphics.DpiX, this.Font.Style);

            DrawClock();
            DrawA();
            DrawB();
            DrawAFiltered();
            DrawBFiltered();            
            DrawIndex();
            if (m_counterRes != 0) { DrawCounter(); }

            e.Graphics.DrawImageUnscaled(m_backBuffer, 0, 0);

            base.OnPaint(e);
        }

        private void CyDiagram_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
