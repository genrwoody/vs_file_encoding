using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace FileEncoding
{
    /// <summary>
    /// Margin's canvas and visual definition including both size and content
    /// </summary>
    internal class EncodingMargin : Button, IWpfTextViewMargin
    {
        /// <summary>
        /// Margin name.
        /// </summary>
        public const string MarginName = "EncodingMargin";

        /// <summary>
        /// A value indicating whether the object is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The document of textview.
        /// </summary>
        private readonly ITextDocument document = null;

        internal class ConvertCommand : ICommand
        {
            private readonly Encoding encoding;
            public ConvertCommand(Encoding encoding)
            {
                this.encoding = encoding;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var margin = parameter as EncodingMargin;
                if (margin == null) return;

                if (margin.document.Encoding.DisplayName() != encoding.DisplayName())
                {
                    var isDirty = margin.document.IsDirty;

                    margin.document.Encoding = encoding;
                    margin.document.UpdateDirtyState(true, DateTime.UtcNow);
                    if (!isDirty)
                    {
                        margin.document.Save();
                        margin.document.UpdateDirtyState(false, DateTime.UtcNow);
                    }

                    margin.Content = margin.document.Encoding.ShortName();
                    margin.ToolTip = margin.document.Encoding.DisplayName();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="textView">The textView to attach the margin to.</param>
        public EncodingMargin(IWpfTextView textView, IWpfTextViewMargin marginContainer)
        {
            this.BorderThickness = new Thickness(0);
            this.ClipToBounds = true;
            this.Background = new SolidColorBrush(Colors.Transparent);

            // Text
            if (!textView.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document))
            {
                textView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document);
            }
            Content = document.Encoding.ShortName();
            ToolTip = document.Encoding.DisplayName();

            // Menu
            ContextMenu = new ContextMenu();
            var candidateEncodings = new[] { Encoding.Default, new UTF8Encoding(false, true), new UTF8Encoding(true, true) };
            foreach (var encoding in candidateEncodings)
            {
                var text = "Convert to " + encoding.DisplayName();
                _ = ContextMenu.Items.Add(new MenuItem
                {
                    Header = text,
                    Command = new ConvertCommand(encoding),
                    CommandParameter = this
                });
            }
            ContextMenu.PlacementTarget = this;
            ContextMenu.Placement = PlacementMode.Top;

            Click += (sender, e) =>
            {
                for (var i = 0; i < ContextMenu.Items.Count; ++i)
                {
                    var item = ContextMenu.Items[i] as MenuItem;
                    item.IsChecked = Content.Equals(candidateEncodings[i].ShortName());
                }
                ContextMenu.IsOpen = true;
            };

            document.FileActionOccurred += (sender, e) =>
            {
                Content = document.Encoding.ShortName();
                ToolTip = document.Encoding.DisplayName();
            };
        }

        #region IWpfTextViewMargin

        /// <summary>
        /// Gets the <see cref="Sytem.Windows.FrameworkElement"/> that implements the visual representation of the margin.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public FrameworkElement VisualElement
        {
            // Since this margin implements Canvas, this is the object which renders
            // the margin.
            get
            {
                this.ThrowIfDisposed();
                return this;
            }
        }

        #endregion

        #region ITextViewMargin

        /// <summary>
        /// Gets the size of the margin.
        /// </summary>
        /// <remarks>
        /// For a horizontal margin this is the height of the margin,
        /// since the width will be determined by the <see cref="ITextView"/>.
        /// For a vertical margin this is the width of the margin,
        /// since the height will be determined by the <see cref="ITextView"/>.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public double MarginSize
        {
            get
            {
                this.ThrowIfDisposed();

                // Since this is a horizontal margin, its width will be bound to the width of the text view.
                // Therefore, its size is its height.
                return this.ActualHeight;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the margin is enabled.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The margin is disposed.</exception>
        public bool Enabled
        {
            get
            {
                this.ThrowIfDisposed();

                // The margin should always be enabled
                return true;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITextViewMargin"/> with the given <paramref name="marginName"/> or null if no match is found
        /// </summary>
        /// <param name="marginName">The name of the <see cref="ITextViewMargin"/></param>
        /// <returns>The <see cref="ITextViewMargin"/> named <paramref name="marginName"/>, or null if no match is found.</returns>
        /// <remarks>
        /// A margin returns itself if it is passed its own name. If the name does not match and it is a container margin, it
        /// forwards the call to its children. Margin name comparisons are case-insensitive.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="marginName"/> is null.</exception>
        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return string.Equals(marginName, EncodingMargin.MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        /// <summary>
        /// Disposes an instance of <see cref="EncodingMargin"/> class.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                GC.SuppressFinalize(this);
                this.isDisposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Checks and throws <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(MarginName);
            }
        }
    }
}
