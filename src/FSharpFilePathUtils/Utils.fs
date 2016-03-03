namespace FSharpFilePathUtils

module Utils =
    open System.IO
    open Fake.FileUtils
    open System

    let (+/) p1 p2 = Path.Combine(p1, p2)

    // Splits a path to a tuple of the parent path and the leaf
    let (!/.) path = Path.GetDirectoryName(path), Path.GetFileName(path)

    // Converts relative path to absolute
    let (~+.) rel = Path.GetFullPath(Directory.GetCurrentDirectory() +/ rel)

    // Takes a function fn, runs it w/ working directory dir, returing the
    // result, and alos ensurinng that the caller's working directory remains
    // unchanged.
    let (=>/) fn dir =
        pushd dir
        let result = fn()
        popd()
        result

    let ChangeExtension filePath extension =
        let path, file = !/. filePath
        let name = Path.GetFileNameWithoutExtension(file)
        path +/ (String.Join("", name, extension))

    let private _invalidFileChars = Path.GetInvalidFileNameChars()
    let private _invalidPathChars = Path.GetInvalidPathChars()

    let private _StripChars stripChars toStrip =
        stripChars
        |> Seq.fold (fun (str: string) ch -> str.Replace(ch, '_')) toStrip

    let StripInvalidFileChars path = _StripChars _invalidFileChars path
    let StripInvalidPathChars path = _StripChars _invalidPathChars path

    let StripAllInvalidFilesystemChars path =
        path
        |> StripInvalidFileChars
        |> StripInvalidPathChars