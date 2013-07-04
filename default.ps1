properties {
    $base_dir  = resolve-path .
    $lib_dir = "$base_dir\SharpFE.Core\lib"
    $build_dir = "$base_dir\build"
    $packages_dir = "$base_dir\packages"
    $buildartifacts_dir = "$build_dir\"
    $sln_file = "$base_dir\SharpFE.sln"
    $global:configuration = "Debug"
}

task default -depends dev

task dev -depends Compile

task CleanBuildDirectory { 
	Remove-Item $build_dir -Recurse -Force -ErrorAction SilentlyContinue
}

task Compile -depends CleanBuildDirectory {
    $v4_net_version = (ls "$env:windir\Microsoft.NET\Framework\v4.0*").Name
    
    Write-Host "Compiling with '$global:configuration' configuration" -ForegroundColor Yellow
    exec { &"C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" "$sln_file" /p:OutDir="$buildartifacts_dir\" /p:Configuration=$global:configuration }
}