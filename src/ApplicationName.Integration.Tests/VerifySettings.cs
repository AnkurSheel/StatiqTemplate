﻿using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DiffEngine;
using VerifyTests;

namespace Bookland.Integration.Tests
{
    public static class VerifySettings
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifierSettings.DerivePathInfo(
                (sourceFile, projectDirectory, type, method) => new PathInfo(
                    directory: Path.Combine(projectDirectory, "Snapshots"),
                    typeName: type.Name,
                    methodName: method.Name));

            DiffRunner.Disabled = true;
            VerifierSettings.OnFirstVerify(
                filePair =>
                {
                    AutoApproveFile(filePair);
                    return Task.CompletedTask;
                });
            VerifierSettings.OnVerifyMismatch(
                (filePair, message) =>
                {
                    AutoApproveFile(filePair);
                    return Task.CompletedTask;
                });
        }

        private static void AutoApproveFile(FilePair filePair)
        {
            if(File.Exists(filePair.Verified))
            {
                File.Delete(filePair.Verified);
            }
            File.Copy(filePair.Received, filePair.Verified);
            // File.Delete(filePair.Received);
        }
    }
}
