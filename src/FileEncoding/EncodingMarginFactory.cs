﻿using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace FileEncoding
{
    /// <summary>
    /// Export a <see cref="IWpfTextViewMarginProvider"/>, which returns an instance of the margin for the editor to use.
    /// </summary>
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(EncodingMargin.MarginName)]
    [Order(After = PredefinedMarginNames.BottomRightCorner)]  // Ensure that the margin occurs below the horizontal scrollbar
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]             // Set the container to the bottom of the editor window
    [ContentType("text")]                                       // Show this margin for all text-based types
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class EncodingMarginFactory : IWpfTextViewMarginProvider
    {
        #region IWpfTextViewMarginProvider

        /// <summary>
        /// Creates an <see cref="IWpfTextViewMargin"/> for the given <see cref="IWpfTextViewHost"/>.
        /// </summary>
        /// <param name="wpfTextViewHost">The <see cref="IWpfTextViewHost"/> for which to create the <see cref="IWpfTextViewMargin"/>.</param>
        /// <param name="marginContainer">The margin that will contain the newly-created margin.</param>
        /// <returns>The <see cref="IWpfTextViewMargin"/>.
        /// The value may be null if this <see cref="IWpfTextViewMarginProvider"/> does not participate for this context.
        /// </returns>
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {

            //return new EncodingMargin(wpfTextViewHost.TextView);
            ITextViewRoleSet roles = wpfTextViewHost.TextView.Roles;
            if (roles.Contains("DOCUMENT")
                || roles.Contains("EMBEDDED_PEEK_TEXT_VIEW"))
            {
                return new EncodingMargin(wpfTextViewHost.TextView, marginContainer);
            }
            return null;
        }

        #endregion
    }
}
