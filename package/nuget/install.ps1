param($rootPath, $toolsPath, $package, $project)

if ($project) {
	
	$src = Join-Path $rootPath "files"
	$dst = Split-Path $project.FullName -Parent
		
	robocopy $src $dst /s

}