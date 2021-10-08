using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


using SMath = System.Math;
using SWFCursor = System.Windows.Forms.Cursor;


namespace cmdwtf.Toolkit.WinForms
{
	/// <summary>
	/// A button control that allows a context menu to be added. The menu
	/// behavior is similar to that of <see cref="ToolStripSplitButton"/>.
	/// </summary>
	/// <remarks>
	/// Original work on the split button by
	/// <seealso href="http://wyday.com/splitbutton/">wyDay</seealso>,
	/// and was licensed under BSD-2-Clause.
	/// <see cref="SplitButton"/> is licensed under the license of the package as a whole.
	/// </remarks>
	public class SplitButton : Button
	{
		private const int SplitSectionWidth = 18;
		private static readonly int BorderSize = SystemInformation.Border3DSize.Width * 2;

		private PushButtonState _state;
		private bool _skipNextOpen;
		private Rectangle _dropDownRectangle;
		private bool _showSplit;
		private bool _isSplitMenuVisible;
		private bool _isMouseEntered;
		private TextFormatFlags _textFormatFlags = TextFormatFlags.Default;

#if !NET5_0_OR_GREATER
		private ContextMenu _splitMenu;
#endif // !NET5_0_OR_GREATER
		private ContextMenuStrip _splitMenuStrip;

		/// <summary>
		/// Creates a new <see cref="SplitButton"/> instance.
		/// </summary>
		public SplitButton()
		{
			AutoSize = true;
		}

		#region Properties

		/// <inheritdoc/>
		[Browsable(false)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return SplitMenuStrip;
			}
			set
			{
				SplitMenuStrip = value;
			}
		}

#if !NET5_0_OR_GREATER
		/// <inheritdoc cref="ContextMenuStrip"/>
		[DefaultValue(null)]
		public ContextMenu SplitMenu
		{
			get { return _splitMenu; }
			set
			{
				//remove the event handlers for the old SplitMenu
				if (_splitMenu != null)
				{
					_splitMenu.Popup -= SplitMenu_Popup;
				}

				//add the event handlers for the new SplitMenu
				if (value != null)
				{
					ShowSplit = true;
					value.Popup += SplitMenu_Popup;
				}
				else
					ShowSplit = false;

				_splitMenu = value;
			}
		}
#endif // !NET5_0_OR_GREATER

		/// <inheritdoc cref="ContextMenuStrip"/>
		[DefaultValue(null)]
		public ContextMenuStrip SplitMenuStrip
		{
			get
			{
				return _splitMenuStrip;
			}
			set
			{
				//remove the event handlers for the old SplitMenuStrip
				if (_splitMenuStrip != null)
				{
					_splitMenuStrip.Closing -= SplitMenuStrip_Closing;
					_splitMenuStrip.Opening -= SplitMenuStrip_Opening;
				}

				//add the event handlers for the new SplitMenuStrip
				if (value != null)
				{
					ShowSplit = true;
					value.Closing += SplitMenuStrip_Closing;
					value.Opening += SplitMenuStrip_Opening;
				}
				else
				{
					ShowSplit = false;
				}

				_splitMenuStrip = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that will configure if
		/// the split portion of the button is shown or not.
		/// </summary>
		[DefaultValue(false)]
		public bool ShowSplit
		{
			set
			{
				if (value != _showSplit)
				{
					_showSplit = value;
					Invalidate();

					if (Parent != null)
					{
						Parent.PerformLayout();
					}
				}
			}
		}

		private PushButtonState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (!_state.Equals(value))
				{
					_state = value;
					Invalidate();
				}
			}
		}

		#endregion Properties

		/// <inheritdoc/>
		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData.Equals(Keys.Down) && _showSplit)
			{
				return true;
			}

			return base.IsInputKey(keyData);
		}

		/// <inheritdoc/>
		protected override void OnGotFocus(EventArgs e)
		{
			if (!_showSplit)
			{
				base.OnGotFocus(e);
				return;
			}

			if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
			{
				State = PushButtonState.Default;
			}
		}

		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if (_showSplit)
			{
				if (kevent.KeyCode.Equals(Keys.Down) && !_isSplitMenuVisible)
				{
					ShowContextMenuStrip();
				}

				else if (kevent.KeyCode.Equals(Keys.Space) && kevent.Modifiers == Keys.None)
				{
					State = PushButtonState.Pressed;
				}
			}

			base.OnKeyDown(kevent);
		}

		/// <inheritdoc/>
		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			if (kevent.KeyCode.Equals(Keys.Space))
			{
				if (MouseButtons == MouseButtons.None)
				{
					State = PushButtonState.Normal;
				}
			}
			else if (kevent.KeyCode.Equals(Keys.Apps))
			{
				if (MouseButtons == MouseButtons.None && !_isSplitMenuVisible)
				{
					ShowContextMenuStrip();
				}
			}

			base.OnKeyUp(kevent);
		}

		/// <inheritdoc/>
		protected override void OnEnabledChanged(EventArgs e)
		{
			State = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;

			base.OnEnabledChanged(e);
		}

		/// <inheritdoc/>
		protected override void OnLostFocus(EventArgs e)
		{
			if (!_showSplit)
			{
				base.OnLostFocus(e);
				return;
			}

			if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
			{
				State = PushButtonState.Normal;
			}
		}

		/// <inheritdoc/>
		protected override void OnMouseEnter(EventArgs e)
		{
			if (!_showSplit)
			{
				base.OnMouseEnter(e);
				return;
			}

			_isMouseEntered = true;

			if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
			{
				State = PushButtonState.Hot;
			}

		}

		/// <inheritdoc/>
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!_showSplit)
			{
				base.OnMouseLeave(e);
				return;
			}

			_isMouseEntered = false;

			if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
			{
				State = Focused ? PushButtonState.Default : PushButtonState.Normal;
			}
		}

		/// <inheritdoc/>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!_showSplit)
			{
				base.OnMouseDown(e);
				return;
			}

#if !NET5_0_OR_GREATER
			//handle ContextMenu re-clicking the drop-down region to close the menu
			if (_splitMenu != null && e.Button == MouseButtons.Left && !_isMouseEntered)
				_skipNextOpen = true;
#endif // !NET5_0_OR_GREATER

			if (_dropDownRectangle.Contains(e.Location) && !_isSplitMenuVisible && e.Button == MouseButtons.Left)
			{
				ShowContextMenuStrip();
			}
			else
			{
				State = PushButtonState.Pressed;
			}
		}

		/// <inheritdoc/>
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (!_showSplit)
			{
				base.OnMouseUp(mevent);
				return;
			}

			// if the right button was released inside the button
			if (mevent.Button == MouseButtons.Right && ClientRectangle.Contains(mevent.Location) && !_isSplitMenuVisible)
			{
				ShowContextMenuStrip();
			}
#if !NET5_0_OR_GREATER
			else if (_splitMenuStrip == null && _splitMenu == null || !_isSplitMenuVisible)
#else
			else if (_splitMenuStrip == null || !_isSplitMenuVisible)
#endif // !NET5_0_OR_GREATER
			{
				SetButtonDrawState();

				if (ClientRectangle.Contains(mevent.Location) && !_dropDownRectangle.Contains(mevent.Location))
				{
					OnClick(new EventArgs());
				}
			}
		}

		/// <inheritdoc/>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			if (!_showSplit)
			{
				return;
			}

			Graphics g = pevent.Graphics;
			Rectangle bounds = ClientRectangle;

			// draw the button background as according to the current state.
			if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles)
			{
				Rectangle backgroundBounds = bounds;
				backgroundBounds.Inflate(-1, -1);
				ButtonRenderer.DrawButton(g, backgroundBounds, State);

				// button renderer doesn't draw the black frame when themes are off
				g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);
			}
			else
			{
				ButtonRenderer.DrawButton(g, bounds, State);
			}

			// calculate the current dropdown rectangle.
			_dropDownRectangle = new Rectangle(bounds.Right - SplitSectionWidth, 0, SplitSectionWidth, bounds.Height);

			int internalBorder = BorderSize;
			Rectangle focusRect =
				new Rectangle(internalBorder - 1,
							  internalBorder - 1,
							  bounds.Width - _dropDownRectangle.Width - internalBorder,
							  bounds.Height - (internalBorder * 2) + 2);

			bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles);


			if (RightToLeft == RightToLeft.Yes)
			{
				_dropDownRectangle.X = bounds.Left + 1;
				focusRect.X = _dropDownRectangle.Right;

				if (drawSplitLine)
				{
					// draw two lines at the edge of the dropdown button
					g.DrawLine(SystemPens.ButtonShadow, bounds.Left + SplitSectionWidth, BorderSize, bounds.Left + SplitSectionWidth, bounds.Bottom - BorderSize);
					g.DrawLine(SystemPens.ButtonFace, bounds.Left + SplitSectionWidth + 1, BorderSize, bounds.Left + SplitSectionWidth + 1, bounds.Bottom - BorderSize);
				}
			}
			else
			{
				if (drawSplitLine)
				{
					// draw two lines at the edge of the dropdown button
					g.DrawLine(SystemPens.ButtonShadow, bounds.Right - SplitSectionWidth, BorderSize, bounds.Right - SplitSectionWidth, bounds.Bottom - BorderSize);
					g.DrawLine(SystemPens.ButtonFace, bounds.Right - SplitSectionWidth - 1, BorderSize, bounds.Right - SplitSectionWidth - 1, bounds.Bottom - BorderSize);
				}
			}

			// Draw an arrow in the correct location
			PaintArrow(g, _dropDownRectangle);

			//paint the image and text in the "button" part of the splitButton
			PaintTextandImage(g, new Rectangle(0, 0, ClientRectangle.Width - SplitSectionWidth, ClientRectangle.Height));

			// draw the focus rectangle.
			if (State != PushButtonState.Pressed && Focused && ShowFocusCues)
			{
				ControlPaint.DrawFocusRectangle(g, focusRect);
			}
		}

		private void PaintTextandImage(Graphics g, Rectangle bounds)
		{
			// Figure out where our text and image should go
			Rectangle text_rectangle;
			Rectangle image_rectangle;

			CalculateButtonTextAndImageLayout(ref bounds, out text_rectangle, out image_rectangle);

			//draw the image
			if (Image != null)
			{
				if (Enabled)
				{
					g.DrawImage(Image, image_rectangle.X, image_rectangle.Y, Image.Width, Image.Height);
				}
				else
				{
					ControlPaint.DrawImageDisabled(g, Image, image_rectangle.X, image_rectangle.Y, BackColor);
				}
			}

			// If we dont' use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
			if (!UseMnemonic)
			{
				_textFormatFlags = _textFormatFlags | TextFormatFlags.NoPrefix;
			}
			else if (!ShowKeyboardCues)
			{
				_textFormatFlags = _textFormatFlags | TextFormatFlags.HidePrefix;
			}

			//draw the text
			if (!string.IsNullOrEmpty(Text))
			{
				if (Enabled)
				{
					TextRenderer.DrawText(g, Text, Font, text_rectangle, ForeColor, _textFormatFlags);
				}
				else
				{
					ControlPaint.DrawStringDisabled(g, Text, Font, BackColor, text_rectangle, _textFormatFlags);
				}
			}
		}

		private void PaintArrow(Graphics g, Rectangle dropDownRect)
		{
			Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

			//if the width is odd - favor pushing it over one pixel right.
			middle.X += (dropDownRect.Width % 2);

			Point[] arrow = new[] { new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1), new Point(middle.X, middle.Y + 2) };

			if (Enabled)
			{
				g.FillPolygon(SystemBrushes.ControlText, arrow);
			}
			else
			{
				g.FillPolygon(SystemBrushes.ButtonShadow, arrow);
			}
		}

		/// <inheritdoc/>
		public override Size GetPreferredSize(Size proposedSize)
		{
			Size preferredSize = base.GetPreferredSize(proposedSize);

			//autosize correctly for splitbuttons
			if (_showSplit)
			{
				if (AutoSize)
				{
					return CalculateButtonAutoSize();
				}

				if (!string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + SplitSectionWidth > preferredSize.Width)
				{
					return preferredSize + new Size(SplitSectionWidth + BorderSize * 2, 0);
				}
			}

			return preferredSize;
		}

		private Size CalculateButtonAutoSize()
		{
			Size ret_size = Size.Empty;
			Size text_size = TextRenderer.MeasureText(Text, Font);
			Size image_size = Image == null ? Size.Empty : Image.Size;

			// Pad the text size
			if (Text.Length != 0)
			{
				text_size.Height += 4;
				text_size.Width += 4;
			}

			switch (TextImageRelation)
			{
				case TextImageRelation.Overlay:
					ret_size.Height = SMath.Max(Text.Length == 0 ? 0 : text_size.Height, image_size.Height);
					ret_size.Width = SMath.Max(text_size.Width, image_size.Width);
					break;
				case TextImageRelation.ImageAboveText:
				case TextImageRelation.TextAboveImage:
					ret_size.Height = text_size.Height + image_size.Height;
					ret_size.Width = SMath.Max(text_size.Width, image_size.Width);
					break;
				case TextImageRelation.ImageBeforeText:
				case TextImageRelation.TextBeforeImage:
					ret_size.Height = SMath.Max(text_size.Height, image_size.Height);
					ret_size.Width = text_size.Width + image_size.Width;
					break;
			}

			// Pad the result
			ret_size.Height += (Padding.Vertical + 6);
			ret_size.Width += (Padding.Horizontal + 6);

			//pad the splitButton arrow region
			if (_showSplit)
			{
				ret_size.Width += SplitSectionWidth;
			}

			return ret_size;
		}

		#region Button Layout Calculations

		//The following layout functions were taken from Mono's Windows.Forms
		//implementation, specifically "ThemeWin32Classic.cs",
		//then modified to fit the context of this splitButton

		private void CalculateButtonTextAndImageLayout(ref Rectangle content_rect, out Rectangle textRectangle, out Rectangle imageRectangle)
		{
			Size text_size = TextRenderer.MeasureText(Text, Font, content_rect.Size, _textFormatFlags);
			Size image_size = Image == null ? Size.Empty : Image.Size;

			textRectangle = Rectangle.Empty;
			imageRectangle = Rectangle.Empty;

			switch (TextImageRelation)
			{
				case TextImageRelation.Overlay:
					// Overlay is easy, text always goes here
					textRectangle = OverlayObjectRect(ref content_rect, ref text_size, TextAlign); // Rectangle.Inflate(content_rect, -4, -4);

					//Offset on Windows 98 style when button is pressed
					if (_state == PushButtonState.Pressed && !Application.RenderWithVisualStyles)
					{
						textRectangle.Offset(1, 1);
					}

					// Image is dependent on ImageAlign
					if (Image != null)
					{
						imageRectangle = OverlayObjectRect(ref content_rect, ref image_size, ImageAlign);
					}

					break;
				case TextImageRelation.ImageAboveText:
					content_rect.Inflate(-4, -4);
					LayoutTextAboveOrBelowImage(content_rect, false, text_size, image_size, out textRectangle, out imageRectangle);
					break;
				case TextImageRelation.TextAboveImage:
					content_rect.Inflate(-4, -4);
					LayoutTextAboveOrBelowImage(content_rect, true, text_size, image_size, out textRectangle, out imageRectangle);
					break;
				case TextImageRelation.ImageBeforeText:
					content_rect.Inflate(-4, -4);
					LayoutTextBeforeOrAfterImage(content_rect, false, text_size, image_size, out textRectangle, out imageRectangle);
					break;
				case TextImageRelation.TextBeforeImage:
					content_rect.Inflate(-4, -4);
					LayoutTextBeforeOrAfterImage(content_rect, true, text_size, image_size, out textRectangle, out imageRectangle);
					break;
			}
		}

		private static Rectangle OverlayObjectRect(ref Rectangle container, ref Size sizeOfObject, System.Drawing.ContentAlignment alignment)
		{
			int x, y;

			switch (alignment)
			{
				case System.Drawing.ContentAlignment.TopLeft:
					x = 4;
					y = 4;
					break;
				case System.Drawing.ContentAlignment.TopCenter:
					x = (container.Width - sizeOfObject.Width) / 2;
					y = 4;
					break;
				case System.Drawing.ContentAlignment.TopRight:
					x = container.Width - sizeOfObject.Width - 4;
					y = 4;
					break;
				case System.Drawing.ContentAlignment.MiddleLeft:
					x = 4;
					y = (container.Height - sizeOfObject.Height) / 2;
					break;
				case System.Drawing.ContentAlignment.MiddleCenter:
					x = (container.Width - sizeOfObject.Width) / 2;
					y = (container.Height - sizeOfObject.Height) / 2;
					break;
				case System.Drawing.ContentAlignment.MiddleRight:
					x = container.Width - sizeOfObject.Width - 4;
					y = (container.Height - sizeOfObject.Height) / 2;
					break;
				case System.Drawing.ContentAlignment.BottomLeft:
					x = 4;
					y = container.Height - sizeOfObject.Height - 4;
					break;
				case System.Drawing.ContentAlignment.BottomCenter:
					x = (container.Width - sizeOfObject.Width) / 2;
					y = container.Height - sizeOfObject.Height - 4;
					break;
				case System.Drawing.ContentAlignment.BottomRight:
					x = container.Width - sizeOfObject.Width - 4;
					y = container.Height - sizeOfObject.Height - 4;
					break;
				default:
					x = 4;
					y = 4;
					break;
			}

			return new Rectangle(x, y, sizeOfObject.Width, sizeOfObject.Height);
		}

		private void LayoutTextBeforeOrAfterImage(Rectangle totalArea, bool textFirst, Size textSize, Size imageSize, out Rectangle textRect, out Rectangle imageRect)
		{
			int element_spacing = 0;    // Spacing between the Text and the Image
			int total_width = textSize.Width + element_spacing + imageSize.Width;

			if (!textFirst)
			{
				element_spacing += 2;
			}

			// If the text is too big, chop it down to the size we have available to it
			if (total_width > totalArea.Width)
			{
				textSize.Width = totalArea.Width - element_spacing - imageSize.Width;
				total_width = totalArea.Width;
			}

			int excess_width = totalArea.Width - total_width;
			int offset = 0;

			Rectangle final_text_rect;
			Rectangle final_image_rect;

			HorizontalAlignment h_text = TextAlign.ToAlignments().Horizontal;
			HorizontalAlignment h_image = ImageAlign.ToAlignments().Horizontal;

			if (h_image == HorizontalAlignment.Left)
			{
				offset = 0;
			}
			else if (h_image == HorizontalAlignment.Right && h_text == HorizontalAlignment.Right)
			{
				offset = excess_width;
			}
			else if (h_image == HorizontalAlignment.Center && (h_text == HorizontalAlignment.Left || h_text == HorizontalAlignment.Center))
			{
				offset += excess_width / 3;
			}
			else
			{
				offset += 2 * (excess_width / 3);
			}

			if (textFirst)
			{
				final_text_rect = new Rectangle(totalArea.Left + offset, textSize.CalculateAlignedRect(totalArea, TextAlign).Top, textSize.Width, textSize.Height);
				final_image_rect = new Rectangle(final_text_rect.Right + element_spacing, imageSize.CalculateAlignedRect(totalArea, ImageAlign).Top, imageSize.Width, imageSize.Height);
			}
			else
			{
				final_image_rect = new Rectangle(totalArea.Left + offset, imageSize.CalculateAlignedRect(totalArea, ImageAlign).Top, imageSize.Width, imageSize.Height);
				final_text_rect = new Rectangle(final_image_rect.Right + element_spacing, textSize.CalculateAlignedRect(totalArea, TextAlign).Top, textSize.Width, textSize.Height);
			}

			textRect = final_text_rect;
			imageRect = final_image_rect;
		}

		private void LayoutTextAboveOrBelowImage(Rectangle totalArea, bool textFirst, Size textSize, Size imageSize, out Rectangle textRect, out Rectangle imageRect)
		{
			int element_spacing = 0;    // Spacing between the Text and the Image
			int total_height = textSize.Height + element_spacing + imageSize.Height;

			if (textFirst)
			{
				element_spacing += 2;
			}

			if (textSize.Width > totalArea.Width)
			{
				textSize.Width = totalArea.Width;
			}

			// If the there isn't enough room and we're text first, cut out the image
			if (total_height > totalArea.Height && textFirst)
			{
				imageSize = Size.Empty;
				total_height = totalArea.Height;
			}

			int excess_height = totalArea.Height - total_height;
			int offset = 0;

			Rectangle final_text_rect;
			Rectangle final_image_rect;

			VerticalAlignment v_text = TextAlign.ToAlignments().Vertical;
			VerticalAlignment v_image = ImageAlign.ToAlignments().Vertical;

			if (v_image == VerticalAlignment.Top)
			{
				offset = 0;
			}
			else if (v_image == VerticalAlignment.Bottom && v_text == VerticalAlignment.Bottom)
			{
				offset = excess_height;
			}
			else if (v_image == VerticalAlignment.Center && (v_text == VerticalAlignment.Top || v_text == VerticalAlignment.Center))
			{
				offset += excess_height / 3;
			}
			else
			{
				offset += 2 * (excess_height / 3);
			}

			if (textFirst)
			{
				final_text_rect = new Rectangle(textSize.CalculateAlignedRect(totalArea, TextAlign).Left, totalArea.Top + offset, textSize.Width, textSize.Height);
				final_image_rect = new Rectangle(imageSize.CalculateAlignedRect(totalArea, ImageAlign).Left, final_text_rect.Bottom + element_spacing, imageSize.Width, imageSize.Height);
			}
			else
			{
				final_image_rect = new Rectangle(imageSize.CalculateAlignedRect(totalArea, ImageAlign).Left, totalArea.Top + offset, imageSize.Width, imageSize.Height);
				final_text_rect = new Rectangle(textSize.CalculateAlignedRect(totalArea, TextAlign).Left, final_image_rect.Bottom + element_spacing, textSize.Width, textSize.Height);

				if (final_text_rect.Bottom > totalArea.Bottom)
				{
					final_text_rect.Y = totalArea.Top;
				}
			}

			textRect = final_text_rect;
			imageRect = final_image_rect;
		}

		#endregion Button Layout Calculations


		private void ShowContextMenuStrip()
		{
			if (_skipNextOpen)
			{
				// we were called because we're closing the context menu strip
				// when clicking the dropdown button.
				_skipNextOpen = false;
				return;
			}

			State = PushButtonState.Pressed;


#if !NET5_0_OR_GREATER
			_splitMenu?.Show(this, new Point(0, Height));
#endif // !NET5_0_OR_GREATER
			_splitMenuStrip?.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
		}

		private void SplitMenuStrip_Opening(object sender, CancelEventArgs e)
			=> _isSplitMenuVisible = true;

		private void SplitMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			_isSplitMenuVisible = false;

			SetButtonDrawState();

			if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
			{
				_skipNextOpen = _dropDownRectangle.Contains(PointToClient(SWFCursor.Position)) && MouseButtons == MouseButtons.Left;
			}
		}

		private void SplitMenu_Popup(object sender, EventArgs e)
			=> _isSplitMenuVisible = true;

		/// <inheritdoc/>
		protected override void WndProc(ref Message m)
		{
			//0x0212 == WM_EXITMENULOOP
			if (m.Msg == 0x0212)
			{
				//this message is only sent when a ContextMenu is closed (not a ContextMenuStrip)
				_isSplitMenuVisible = false;
				SetButtonDrawState();
			}

			base.WndProc(ref m);
		}

		private void SetButtonDrawState()
		{
			if (Bounds.Contains(Parent.PointToClient(SWFCursor.Position)))
			{
				State = PushButtonState.Hot;
			}
			else if (Focused)
			{
				State = PushButtonState.Default;
			}
			else if (!Enabled)
			{
				State = PushButtonState.Disabled;
			}
			else
			{
				State = PushButtonState.Normal;
			}
		}
	}
}
