using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System.Windows.Input;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace FileEncoding
{
    /// <summary>
    /// Margin's canvas and visual definition including both size and content
    /// </summary>
    public partial class EncodingMargin : Button, IWpfTextViewMargin
    {
        /// <summary>
        /// Margin name.
        /// </summary>
        public const string MarginName = "FileEncodeMargin";

        /// <summary>
        /// A value indicating whether the object is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The document of textview.
        /// </summary>
        private readonly ITextDocument document = null;

        /// <summary>
        /// The document encoding name.
        /// </summary>
        private string documentEncodingName;

        private static bool HasBom(Encoding encoding)
        {
            return encoding.GetPreamble().Length != 0;
        }

        /// <summary>
        /// Rewrite some encoding name.
        /// </summary>
        /// <param name="document">The document to get encoding.</param>
        /// <returns>Encoding name</returns>
        private static string GetDocumentEncoding(ITextDocument document)
        {
            int codepage = document.Encoding.CodePage;
            if (codepage == Encoding.UTF8.CodePage) {
                return HasBom(document.Encoding) ? "UTF-8 (BOM)" : "UTF-8";
            } else if (codepage == Encoding.Unicode.CodePage) {
                return "Unicode";
            } else if (codepage == Encoding.BigEndianUnicode.CodePage) {
                // default name is too long
                return "Unicode BE";
            } else {
                return document.Encoding.EncodingName;
            }
        }

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
                EncodingMargin _this = parameter as EncodingMargin;
                if (_this.document.Encoding.CodePage != encoding.CodePage ||
                    (HasBom(_this.document.Encoding) != HasBom(encoding))) {
                    _this.document.Encoding = encoding;
                    _this.document.UpdateDirtyState(true, DateTime.Now);
                    _this.UpdateContent();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="textView">The textView to attach the margin to.</param>
        public EncodingMargin(IWpfTextView textView, IWpfTextViewMargin marginContainer)
        {
            InitializeComponent();
            // display
            Focusable = false;

            // Text
            if (!textView.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document)) {
                textView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document);
            }
            UpdateContent();
            // Menu
            Encoding[] encodings = {
                Encoding.Unicode, Encoding.BigEndianUnicode,
                Encoding.UTF8, new UTF8Encoding(false), Encoding.Default
            };
            for (int i = 0; i < ContextMenu.Items.Count; ++i)
            {
                MenuItem item = ContextMenu.Items[i] as MenuItem;
                item.Command = new ConvertCommand(encodings[i]);
                item.CommandParameter = this;
                if (i == 4) item.Header = "SYS - " + Encoding.Default.EncodingName;
            }
            ContextMenu.PlacementTarget = this;
            ContextMenu.Placement = PlacementMode.Custom;
            ContextMenu.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
            ContextMenu.Opened += ContextMenuOpened;
            document.FileActionOccurred += (sender, e) => UpdateContent();
        }

        private void ContextMenuOpened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = (ContextMenu)e.Source;
            MenuItem item = (MenuItem)menu.Items[0];
            item.Focus();
        }

        private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            Debug.WriteLine("placement");
            CustomPopupPlacement[] result = new CustomPopupPlacement[1];
            result[0] = new CustomPopupPlacement{
                Point = new Point(targetSize.Width - popupSize.Width, -popupSize.Height),
                PrimaryAxis = PopupPrimaryAxis.Horizontal
            };
            return result;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            string[] names = { "Unicode", "Unicode BE", "UTF-8 (BOM)", "UTF-8", Encoding.Default.EncodingName };
            for (int i = 0; i < ContextMenu.Items.Count; ++i)
            {
                MenuItem item = ContextMenu.Items[i] as MenuItem;
                item.IsChecked = documentEncodingName.Equals(names[i]);
            }
            ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Update content with document.
        /// </summary>
        private void UpdateContent()
        {
            documentEncodingName = GetDocumentEncoding(document);
            if (documentEncodingName.Equals(Encoding.Default.EncodingName)) {
                Content = "SYS";
            } else {
                Content = documentEncodingName;
            }
        }

        #region AutoGenerate
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();

                // Since this is a horizontal margin, its width will be bound to the width of the text view.
                // Therefore, its size is its height.
                return ActualHeight;
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
                ThrowIfDisposed();

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
            return string.Equals(marginName, MarginName, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        /// <summary>
        /// Disposes an instance of <see cref="FileEncodeMargin"/> class.
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed) {
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Checks and throws <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (isDisposed) {
                throw new ObjectDisposedException(MarginName);
            }
        }
        #endregion
    }
}

