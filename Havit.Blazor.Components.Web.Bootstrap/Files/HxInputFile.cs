﻿using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Wraps <see cref="HxInputFileCore"/> as Bootstrap form control (incl. <code>Label</code> etc.)
	/// </summary>
	public partial class HxInputFile : ComponentBase, ICascadeEnabledComponent, IFormValueComponent
	{
		/// <summary>
		/// URL of the server endpoint receiving the files.
		/// </summary>
		[Parameter] public string UploadUrl { get; set; }

		/// <summary>
		/// Gets or sets the event callback that will be invoked when the collection of selected files changes.
		/// </summary>
		[Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

		/// <summary>
		/// Raised during running file upload (the frequency depends on browser implementation).
		/// </summary>
		[Parameter] public EventCallback<UploadProgressEventArgs> OnProgress { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<FileUploadedEventArgs> OnFileUploaded { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<UploadCompletedEventArgs> OnUploadCompleted { get; set; }

		/// <summary>
		/// Single <c>false</c> or multiple <c>true</c> files upload.
		/// </summary>
		[Parameter] public bool Multiple { get; set; }

		/// <summary>
		/// Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow.
		/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">MDN Web Docs - HTML attribute: accept</see>.
		/// </summary>
		[Parameter] public string Accept { get; set; }

		/// <summary>
		/// The maximum files size in bytes.
		/// When exceeded, the <see cref="OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
		/// Default is <c>null</c> (unlimited).
		/// </summary>
		[Parameter] public long? MaxFileSize { get; set; }

		#region IFormValueComponent public properties
		/// <summary>
		/// Label to render before input (or after input for Checkbox).		
		/// </summary>
		[Parameter] public string Label { get; set; }

		/// <summary>
		/// Label to render before input (or after input for Checkbox).
		/// </summary>
		[Parameter] public RenderFragment LabelTemplate { get; set; }

		/// <summary>
		/// Hint to render after input as form-text.
		/// </summary>
		[Parameter] public string Hint { get; set; }

		/// <summary>
		/// Hint to render after input as form-text.
		/// </summary>
		[Parameter] public RenderFragment HintTemplate { get; set; }

		/// <summary>
		/// Custom CSS class to render with wrapping div.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the label.
		/// </summary>
		[Parameter] public string LabelCssClass { get; set; }
		#endregion

		/// <summary>
		/// Custom CSS class to render with the input element.
		/// </summary>
		[Parameter] public string InputCssClass { get; set; }

		/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
		[Parameter] public bool? Enabled { get; set; }

		/// <inheritdoc cref="Web.FormState" />
		[CascadingParameter] protected FormState FormState { get; set; }
		FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

		/// <summary>
		/// Last known count of associated files.
		/// </summary>
		public int FileCount => hxInputFileCoreComponentReference.FileCount;

		/// <summary>
		/// ID if the input element. Autogenerated when used with label.
		/// </summary>
		protected string InputId { get; private set; } = "hx" + Guid.NewGuid().ToString("N");

		/// <summary>
		/// CSS class to be rendered with the input element.
		/// </summary>
		private protected virtual string CoreInputCssClass => "form-control";

		string IFormValueComponent.LabelFor => this.InputId;
		string IFormValueComponent.CoreCssClass => "";
		string IFormValueComponent.CoreLabelCssClass => "form-label";
		string IFormValueComponent.CoreHintCssClass => "form-text";

		private protected HxInputFileCore hxInputFileCoreComponentReference;

		/// <summary>
		/// Gets list of files chosen.
		/// </summary>
		public Task<FileInfo[]> GetFilesAsync() => hxInputFileCoreComponentReference.GetFilesAsync();

		/// <summary>
		/// Clears associated input-file element and resets component to initial state.
		/// </summary>
		public Task ResetAsync() => hxInputFileCoreComponentReference.ResetAsync();

		/// <summary>
		/// Starts the upload.
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		/// <remarks>
		/// We do not want to make the Havit.Blazor library dependant on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
		/// </remarks>
		public Task StartUploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.StartUploadAsync(accessToken);

		/// <summary>
		/// Uploads the file(s).
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		public Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.UploadAsync(accessToken);

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenRegion(0);
			base.BuildRenderTree(builder);
			builder.CloseRegion();

			HxFormValueRenderer.Current.Render(1, builder, this);
		}

		void IFormValueComponent.RenderValue(RenderTreeBuilder builder)
		{
			builder.OpenComponent<HxInputFileCore>(1);
			builder.AddAttribute(1001, nameof(HxInputFileCore.Id), this.InputId);
			builder.AddAttribute(1002, nameof(HxInputFileCore.UploadUrl), this.UploadUrl);
			builder.AddAttribute(1003, nameof(HxInputFileCore.Multiple), this.Multiple);
			builder.AddAttribute(1004, nameof(HxInputFileCore.OnChange), this.OnChange);
			builder.AddAttribute(1005, nameof(HxInputFileCore.OnProgress), this.OnProgress);
			builder.AddAttribute(1006, nameof(HxInputFileCore.OnFileUploaded), this.OnFileUploaded);
			builder.AddAttribute(1007, nameof(HxInputFileCore.OnUploadCompleted), this.OnUploadCompleted);
			builder.AddAttribute(1007, nameof(HxInputFileCore.Accept), this.Accept);
			builder.AddAttribute(1007, nameof(HxInputFileCore.MaxFileSize), this.MaxFileSize);
			builder.AddAttribute(1008, "class", CssClassHelper.Combine(this.CoreInputCssClass, this.InputCssClass));
			builder.AddAttribute(1009, "disabled", !CascadeEnabledComponent.EnabledEffective(this));
			builder.AddComponentReferenceCapture(1010, r => hxInputFileCoreComponentReference = (HxInputFileCore)r);
			builder.CloseComponent();
		}
	}
}
