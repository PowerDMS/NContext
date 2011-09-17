// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRunWhenApplicationConfigurationIsComplete.cs">
//   This file is part of NContext.
//
//   NContext is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or any later version.
//
//   NContext is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with NContext.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>

// <summary>
//   Defines a role-interface which allows implementors to run when IApplicationConfiguration.Setup() has completed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines a role-interface which allows implementors to run 
    /// when <see cref="IApplicationConfiguration.Setup"/> has completed.
    /// </summary>
    /// <remarks></remarks>
    [InheritedExport]
    public interface IRunWhenApplicationConfigurationIsComplete
    {
        /// <summary>
        /// Runs when the <see cref="IApplicationConfiguration.Setup"/> has completed.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        /// <remarks></remarks>
        void Run(IApplicationConfiguration applicationConfiguration);
    }
}