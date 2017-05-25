//  ---------------------------------------------------------------------------------
//  Copyright (c) Yodo1,LTD.  All rights reserved.
//  ---------------------------------------------------------------------------------

using ContosoModels;
using Yodo1ServiceModels;
using System.Text.RegularExpressions;
using Telerik.Core;

namespace Yodo1APICaller.ViewModels
{
    /// <summary>
    /// Wrapper for the ConfigBody model in the master/details Customers page.
    /// </summary>
    public class ConfigBodyViewModel : ValidateViewModelBase
    {
        /// <summary>
        /// Creates a new ConfigBody model.
        /// </summary>
        public ConfigBodyViewModel(ConfigBody model)
        {
            Model = model ?? new ConfigBody();
        }

        /// <summary>
        /// The underlying ConfigBody model.
        /// </summary>
        internal ConfigBody Model { get; set; }

        /// <summary>
        /// Gets or sets whether the underlying model has been modified. 
        /// This is used when sync'ing with the server to reduce load
        /// and only upload the models that changed.
        /// </summary>
        internal bool IsModified { get; set; }

        /// <summary>
        /// Gets or sets whether to validate model data. 
        /// </summary>
        internal bool Validate { get; set; }

        /// <summary>
        /// Gets or sets the Config Body's key.
        /// </summary>
        public string Key
        {
            get { return Model.Key; }
            set
            {
                if (value != Model.Key)
                {
                    Model.Key = value;
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Config Body's value.
        /// </summary>
        public string Value
        {
            get { return Model.Value; }
            set
            {
                if (value != Model.Value)
                {
                    Model.Value = value;
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Config Body's type.
        /// </summary>
        public string Type
        {
            get { return Model.Type; }
            set
            {
                if (value != Model.Type)
                {
                    Model.Type = value;
                    IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Config Body's descripation.
        /// </summary>
        public string Description
        {
            get { return Model.Des; }
            set
            {
                if (value != Model.Des)
                {
                    Model.Des = value;
                    IsModified = true;
                }
            }
        }
    }
}