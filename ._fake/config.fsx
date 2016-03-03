namespace FSharpVersionUtils.Fake.Config

#r @"./packages/FAKE/tools/FakeLib.dll"

open Fake
open System.IO

module Common =
  let RootDir = Directory.GetCurrentDirectory()

module Source =
  open Common

  let SolutionFile = !! (Path.Combine(RootDir, "*.sln"))

module Build =
  let TestAssemblies = !! "tests/**/*.Tests.dll" -- "**/obj/**/*.Tests.dll"
  let DotNetVersion = "4.5"
  let MSBuildArtifacts = !! "**/bin/**.*" ++ "**/obj/**/*.*"
