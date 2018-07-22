using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tarsier.Labels {
    public class OutlinedLabel : Label {
        private float _outlineWidth;
        private Color _outlineColor;
        
        #region Constructor
        /// <summary>
        ///   Constructs a new OutlinedLabel object.
        /// </summary>
        public OutlinedLabel() {
            this._outlineWidth = 1f;
            this._outlineColor = Color.Green;
            this.Invalidate();
        }
        #endregion

        #region Public Properties

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The thickness of the label outline")]
        [DefaultValue(1f)]
        public float OutlineWidth {
            get { return this._outlineWidth; }
            set {
                this._outlineWidth = value;
                if(value == 0) {
                    //If border size equals zero, disable the
                    // border by setting it as transparent
                    this.OutlineColor = Color.Transparent;
                } 

                this.OnTextChanged(EventArgs.Empty);
            }
        }

        
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The outline color of this label")]
        public Color OutlineColor {
            get { return this._outlineColor; }
            set {
                this._outlineColor = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Event Handling
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);
            this.Invalidate();
        }
        #endregion

        #region Drawing
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            using(GraphicsPath gp = new GraphicsPath()) {
                using(Pen outline = new Pen(this.OutlineColor, this.OutlineWidth) { LineJoin = LineJoin.Round }) {
                    using(StringFormat sf = new StringFormat()) {
                        using(Brush foreBrush = new SolidBrush(ForeColor)) {
                            gp.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, ClientRectangle, sf);
                            e.Graphics.ScaleTransform(1.3f, 1.35f);
                            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                            e.Graphics.DrawPath(outline, gp);
                            e.Graphics.FillPath(foreBrush, gp);
                        }
                    }
                }
            }
        }
        #endregion
    }
}