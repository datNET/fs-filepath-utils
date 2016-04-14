#r @"./._fake/packages/FAKE/tools/FakeLib.dll"
#r @"./._fake/packages/FSharp.FakeTargets/tools/FSharpFakeTargets.dll"

#load @"./._fake/loader.fsx"

open Fake
open NuGetHelper
open RestorePackageHelper
open FSharpUtils.Fake.Config

DatNET.Targets.Initialize id

Target "RestorePackages" (fun _ ->
  Source.SolutionFile
  |> Seq.head
  |> RestoreMSSolutionPackages (fun p ->
      { p with
          Sources = [ "https://nuget.org/api/v2"; ]
          OutputPath = "packages"
          Retries = 4 })
)

Target "Test" (fun _ ->
  let setParams = (fun p ->
    { p with DisableShadowCopy = true; ErrorLevel = DontFailBuild; Framework = Build.DotNetVersion; })

  Build.TestAssemblies |> NUnit setParams
)

Target "CreateNugetPackageDirPath" (fun _ ->
  CreateDir Nuget.PackageDirName
)

Target "PackageAndPublish" (fun _ ->
  Release.Nuspec
    |> NuGet (fun p ->
        { p with
            Version     = Release.Version
            Project     = Release.Project
            Authors     = Release.Authors
            Description = Release.Description
            OutputPath  = Release.OutputPath
            WorkingDir  = Release.WorkingDir
            Publish     = true
            AccessKey   = Nuget.ApiKey
        })
)

"MSBuild"           <== [ "Clean"; "RestorePackages" ]
"Test"              <== [ "MSBuild" ]
"PackageAndPublish" <== [ "MSBuild"; "CreateNugetPackageDirPath" ]

RunTargetOrDefault "MSBuild"
