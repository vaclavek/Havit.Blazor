﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider (delegate) for <see cref="HxInputTags" />.
	/// </summary>
	public delegate Task<InputTagsDataProviderResult> InputTagsDataProviderDelegate(InputTagsDataProviderRequest request);
}
