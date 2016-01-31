/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace CharLCD_v1_20
{
    public partial class CustomCharacter : UserControl
    {

        public Box[,] box;
        private int boxWidth;
        private int boxHeight;
        private int highlightBorderWidth = 1;

        #region Properties: Brushes, Columns, Rows, Name
        private bool selected = false;

        SolidBrush borderBrush = new SolidBrush(Color.LightGray);
        SolidBrush activeBrush = new SolidBrush(Color.Black);
        SolidBrush inactiveBrush = new SolidBrush(Color.White);
        
        private int borderWidth = 1;
        private int columns = 5;
        private int rows = 8;

        private string displayName = "Custom Character";

        public SolidBrush BorderBrush
        {
            get { return borderBrush; }
            set { borderBrush = value; }
        }
        public SolidBrush ActiveBrush
        {
            get { return activeBrush; }
            set { activeBrush = value; }
        }
        public SolidBrush InactiveBrush
        {
            get { return inactiveBrush; }
            set { inactiveBrush = value; }
        }
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public int BorderWidth
        {
            get { return borderWidth; }
            set 
            { 
                if(value < this.Size.Height & value < this.Size.Width)
                    borderWidth = value;
            }
        }
        public int Columns
        {
            get { return columns; }
            set 
            { 
                columns = value;
                CheckBoxArray();
            }
        }
        public int Rows
        {
            get { return rows; }
            set 
            {
                rows = value;
                CheckBoxArray();
            }
        }
        public string DisplayName
        {
            get { return displayName;}
            set { displayName = value;}
        }
        #endregion

        public CustomCharacter() 
        {
            InitializeComponent();
            CheckBoxArray();
        }
        public override string ToString()
        {
            return this.Name;
        }

        #region Appearance.  Box size. Colors. OnPaint method.
        // Before runtime, if the number of columns or rows is changed, update.   
        // Causes loss of box state data.
        private void CheckBoxArray()
        {
            CheckBoxSize();
            box = new Box[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    box[row, column] = new Box(row, column);
                }
            }
        }

        // Recalculate box width and box height.    
        public void CheckBoxSize()
        {
            boxWidth = (Size.Width - borderWidth) / columns;
            boxHeight = (Size.Height - borderWidth) / rows;
        }

        // Paint the boxes based on state
        protected override void OnPaint(PaintEventArgs e)
        {

            // Update Borders: Border Widths : Selection
            Graphics graphics = e.Graphics;
            // Draw Right and Bottom Border
            
                     
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (box[row,column].IsActive)
                    {
                        graphics.FillRectangle(borderBrush, column * boxWidth, row * boxHeight, boxWidth, boxHeight);
                        graphics.FillRectangle(activeBrush, column * boxWidth + borderWidth, row * boxHeight + borderWidth, boxWidth - borderWidth, boxHeight - borderWidth);
                    }
                    else
                    {
                        // Draw box which forms top and left border
                        graphics.FillRectangle(borderBrush, column * boxWidth, row * boxHeight, boxWidth, boxHeight);
                        // Draw standard box over border box so they overlap
                        graphics.FillRectangle(inactiveBrush, column * boxWidth + borderWidth, row * boxHeight + borderWidth, boxWidth - borderWidth, boxHeight - borderWidth);
                    }

                    Pen borderPen = new Pen(activeBrush.Color, borderWidth);
                    // Alignment == Inset instead of Center
                    borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    graphics.DrawRectangle(borderPen, 0, 0, columns * boxWidth + borderWidth - 1,
                                                            rows * boxHeight + borderWidth - 1);

                    if(selected)
                    {
                        Pen pen = new Pen(Color.Blue, highlightBorderWidth);
                        // Alignment == Inset instead of Center
                        pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                        graphics.DrawRectangle(pen, 0, 0, columns * boxWidth + borderWidth-1,
                                                                rows * boxHeight + borderWidth-1);
                        // Alignment == Center Code.
                        //graphics.DrawRectangle(pen, borderPenWidth / 2, borderPenWidth / 2, (columns) * boxWidth + borderWidth - borderPenWidth, (rows) * boxHeight + borderWidth - borderPenWidth);
                        
                        pen = new Pen(Color.DodgerBlue, highlightBorderWidth);
                        pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                        graphics.DrawRectangle(pen, highlightBorderWidth, highlightBorderWidth, 
                                    columns * boxWidth + borderWidth - 2 * highlightBorderWidth - 1, 
                                    rows * boxHeight + borderWidth - 2 * highlightBorderWidth - 1);
                        
                        // Alignment == Center Code
                        //graphics.DrawRectangle(pen, 3 * borderPenWidth / 2, 3 * borderPenWidth / 2, (columns) * boxWidth + borderWidth - 3 * borderPenWidth, (rows) * boxHeight + borderWidth - 3 * borderPenWidth);
                    }
                }
            }
        }

        // Event handler for a change in control size
        private void ArraySizeChanged(object sender, EventArgs e)
        {
            CheckBoxSize();
        }
        #endregion

        #region Box Manipulation
        /// <summary>
        /// Given a mouse location on the control GetBoxByLocation calculates which 
        /// row and column the box is and returns a reference to that box.
        /// 
        /// Returns null for invalid values.
        /// </summary>
        /// <param name="x"> X coordinate of mouse click</param>
        /// <param name="y"> Y coordinate of mouse click</param>
        /// <returns> A Box object if a valid location (inside the control) is passed in.  
        /// Otherwise it returns null.</returns>
        public Box GetBoxByLocation(int x, int y)
        {
                
            int pixPerRow = (Size.Height - borderWidth) / rows;
            int row = y / pixPerRow;
            int pixPerCol = (Size.Width - borderWidth) / columns;
            int column = x / pixPerCol;
            if (row >= 0 && row < rows && column >=0 && column < columns)
                return box[row, column];
            else
                return null;
        }

        /// <summary>
        /// GetBoxArray returns the 2-Dimensional array of boxes ("cells", "pixels", etc) to 
        /// allow the user to process the meaning of the states.
        /// </summary>
        /// <returns> A 2-D array of type Box </returns>
        public Box[,] GetBoxArray()
        {
            return this.box;
        }

        /// <summary>
        /// Match accepts a CustomCharacter as an input and matches the current CustomCharacter 
        /// to the the pixel set of the input CustomCharacter
        /// </summary>
        /// <param name="character"> a CustomCharacter object for the current CustomCharacter to copy.</param>
        public void Match(CharLCD_v1_20.CustomCharacter character)
        {
            this.box = character.GetBoxArray();
        }

        /// <summary>
        /// SetRow takes an input argument between zero and the number of rows and sets every cell 
        /// on that row to be active.
        /// </summary>
        /// <param name="row"></param>
        public void SetRow(int row)
        {
            for (int i = 0; i < columns; i++)
            {
                box[row, i].IsActive = true;
            }
        }

        /// <summary>
        /// SetColumn takes an input argument between zero and the number of columns and sets every cell 
        /// on that column to be active.
        /// </summary>
        /// <param name="row"></param>
        public void SetColumn(int column)
        {
            for (int i = 0; i < rows; i++)
            {
                box[i, column].IsActive = true;
            }
        }
        #endregion
    }

    public class Box
    {
        #region Variables and Properties
        // Properties
        private readonly int row;
        private readonly int column;
        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion

        public Box(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}