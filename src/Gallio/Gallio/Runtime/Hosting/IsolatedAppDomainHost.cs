// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using Gallio.Runtime.Logging;
using Gallio.Runtime;
using Gallio.Reflection;
using Gallio.Utilities;

namespace Gallio.Runtime.Hosting
{
    /// <summary>
    /// <para>
    /// An isolated app domain host is a <see cref="IHost"/> the runs code within an
    /// isolated <see cref="AppDomain" /> of this process.  Communication with the
    /// <see cref="AppDomain" /> occurs over an inter-<see cref="AppDomain" /> .Net
    /// remoting channel.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="IsolatedAppDomainHost" /> does not support the
    /// <see cref="HostConfiguration.LegacyUnhandledExceptionPolicyEnabled" />
    /// option because it seems to be ignored by the .Net runtime except when set on
    /// the default AppDomain of the process.
    /// </para>
    /// </remarks>
    public class IsolatedAppDomainHost : RemoteHost
    {
        private AppDomain appDomain;
        private CurrentDirectorySwitcher currentDirectorySwitcher;
        private string temporaryConfigurationFilePath;

        /// <summary>
        /// Creates an uninitialized host.
        /// </summary>
        /// <param name="hostSetup">The host setup</param>
        /// <param name="logger">The logger for host message output</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hostSetup"/>
        /// or <paramref name="logger"/> is null</exception>
        public IsolatedAppDomainHost(HostSetup hostSetup, ILogger logger)
            : base(hostSetup, logger)
        {
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                FreeResources();
        }

        /// <inheritdoc />
        protected override void InitializeImpl()
        {
            try
            {
                SetWorkingDirectory();
                CreateTemporaryConfigurationFile();
                CreateAppDomain();

                InitializeHostService();
            }
            catch (Exception)
            {
                FreeResources();
                throw;
            }
        }

        private void FreeResources()
        {
            UnloadAppDomain();
            ResetWorkingDirectory();
            DeleteTemporaryConfigurationFile();
        }

        private void SetWorkingDirectory()
        {
            currentDirectorySwitcher = new CurrentDirectorySwitcher(HostSetup.WorkingDirectory);
        }

        private void CreateTemporaryConfigurationFile()
        {
            HostConfiguration configuration = HostSetup.Configuration;
            if (HostSetup.ApplicationBaseDirectory != RuntimeAccessor.InstallationPath)
            {
                configuration = configuration.Copy();
                configuration.AddAssemblyBinding(GetType().Assembly, false);
            }

            string configurationXml = configuration.ToString();

            temporaryConfigurationFilePath = Path.GetTempFileName();
            File.WriteAllText(temporaryConfigurationFilePath, configurationXml);
        }

        private void CreateAppDomain()
        {
            try
            {
                AppDomainSetup appDomainSetup = new AppDomainSetup();

                appDomainSetup.ApplicationBase = HostSetup.ApplicationBaseDirectory;
                appDomainSetup.ApplicationName = @"IsolatedAppDomainHost";
                appDomainSetup.ConfigurationFile = temporaryConfigurationFilePath;

                if (HostSetup.ShadowCopy)
                {
                    appDomainSetup.ShadowCopyFiles = @"true";
                    appDomainSetup.ShadowCopyDirectories = null;
                }

                // TODO: Might need to be more careful about how the Evidence is derived.
                Evidence evidence = AppDomain.CurrentDomain.Evidence;
                PermissionSet defaultPermissionSet = new PermissionSet(PermissionState.Unrestricted);
                StrongName[] fullTrustAssemblies = new StrongName[0];
                appDomain = AppDomain.CreateDomain(appDomainSetup.ApplicationName, evidence, appDomainSetup, defaultPermissionSet, fullTrustAssemblies);
            }
            catch (Exception ex)
            {
                throw new HostException("Could not create the isolated AppDomain.", ex);
            }
        }

        private static T CreateRemoteInstance<T>(AppDomain appDomain, params object[] args)
        {
            Type type = typeof(T);
            string assemblyFile = AssemblyUtils.GetFriendlyAssemblyLocation(type.Assembly);
            return (T)appDomain.CreateInstanceFromAndUnwrap(assemblyFile, type.FullName, false,
                BindingFlags.Default, null, args, null, null, null);
        }

        private void InitializeHostService()
        {
            IHostService hostService = CreateRemoteInstance<RemoteHostService>(appDomain, (TimeSpan?)null);
            ConfigureHostService(hostService, null);
        }

        private void UnloadAppDomain()
        {
            try
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);
            }
            catch (Exception ex)
            {
                UnhandledExceptionPolicy.Report("Could not unload AppDomain.", ex);
            }
            finally
            {
                appDomain = null;
            }
        }

        private void ResetWorkingDirectory()
        {
            try
            {
                if (currentDirectorySwitcher != null)
                    currentDirectorySwitcher.Dispose();
            }
            catch (Exception ex)
            {
                UnhandledExceptionPolicy.Report("Could not reset working directory.", ex);
            }
            finally
            {
                currentDirectorySwitcher = null;
            }
        }

        private void DeleteTemporaryConfigurationFile()
        {
            try
            {
                if (temporaryConfigurationFilePath != null && File.Exists(temporaryConfigurationFilePath))
                    File.Delete(temporaryConfigurationFilePath);
            }
            catch (Exception ex)
            {
                UnhandledExceptionPolicy.Report("Could not delete temporary configuration file.", ex);
            }
            finally
            {
                temporaryConfigurationFilePath = null;
            }
        }
    }
}