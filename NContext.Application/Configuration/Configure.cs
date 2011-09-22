// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureContainer.cs">
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
//
// <summary>
//   Defines a static class for fluid-interface application configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NContext.Application.Configuration
{
    /// <summary>
    /// Defines a static class for fluid-interface application configuration.
    /// </summary>
    /// <remarks></remarks>
    public static class Configure
    {
        /// <summary>
        /// Configures the application using a custom <see cref="IApplicationConfiguration"/>.
        /// </summary>
        /// <typeparam name="TApplicationConfiguration">The type of the application configuration.</typeparam>
        /// <param name="applicationConfiguration">The application manager instance.</param>
        /// <remarks></remarks>
        public static void Using<TApplicationConfiguration>(TApplicationConfiguration applicationConfiguration) 
            where TApplicationConfiguration : IApplicationConfiguration
        {
            applicationConfiguration.Setup();
        }
    }
}